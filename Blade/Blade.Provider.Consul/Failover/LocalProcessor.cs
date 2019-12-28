using Grpc.Blade.Values;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Grpc.Blade.Provider.Consul.Failover
{
    public class LocalProcessor : ILocalProcessor
    {
        private readonly static ReaderWriterLockSlim writerLockSlim = new ReaderWriterLockSlim();
        private const string CONFIG_BASE_DIR = "consul-config";
        private readonly static ConcurrentDictionary<string, List<Service>> _cache = new ConcurrentDictionary<string, List<Service>>();
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        public LocalProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ILocalProcessor>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task<List<Service>> GetConfigAsync(string host, int port, string serviceName)
        {
            try
            { 
                string config = String.Empty;
                string key = GetCacheKey(host, port, serviceName);
                if (!_cache.TryGetValue(key, out var services))
                {
                    string file_dir = GetFilePath(host, port, serviceName);
                    var file = new FileInfo(file_dir + host);
                    if (!file.Exists)
                        return null;
                    config = File.ReadAllText(file.FullName);
                    services = JsonSerializer.Deserialize<List<Service>>(config);
                }
                return await Task.FromResult(services);
            }
            catch (Exception ex)
            {
                _logger.LogError($"读取本地存储配置错误，host:{host},port:{port},servicename:{serviceName},描述=${ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task RemoveConfigAsync(string host, int port, string serviceName)
        {
            try
            {
                writerLockSlim.EnterWriteLock();
                string key = GetCacheKey(host, port, serviceName);
                //首次移除
                _cache.Remove(key, out var _);

                string file_dir = GetFilePath(host, port, serviceName);
                var file = new FileInfo(file_dir);

                if (file.Exists)
                    File.Delete(file.FullName);
                //再次移除
                _cache.Remove(key, out var _);
            }
            catch (Exception ex)
            {
                _logger.LogError($"移除配置时错误，host:{host},port:{port},servicename:{serviceName},描述={ex.Message}");
            }
            finally
            {
                if (writerLockSlim.IsWriteLockHeld)
                    writerLockSlim.ExitWriteLock();
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task SaveConfigAsync(string host, int port, string serviceName, List<Service> services)
        {
            try
            {
                if (services == null && !services.Any())
                {
                    return;
                }
                var config = JsonSerializer.Serialize(services);

                writerLockSlim.EnterWriteLock();

                string key = GetCacheKey(host, port, serviceName);
                //首次更新或写入缓存
                _cache.AddOrUpdate(key, services, (k, v) => services);

                string file_dir = GetFilePath(host, port, serviceName);
                var file = new FileInfo(file_dir);

                //如果我目录不存在则创建
                if (file.Directory != null && !file.Directory.Exists)
                    file.Directory.Create();
                //写入内容
                File.WriteAllText(file.FullName, config);
                //再次更新或写入缓存
                _cache.AddOrUpdate(key, services, (k, v) => services);
            }
            catch (Exception ex)
            {
                _logger.LogError($"写入配置文件时错误，host:{host},port:{port},servicename:{serviceName},描述={ex.Message}");
            }
            finally
            {
                if (writerLockSlim.IsWriteLockHeld)
                    writerLockSlim.ExitWriteLock();
            }
            await Task.CompletedTask;
        }

        #region 私有方法

        /// <summary>
        /// 获取配置文件目录
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private string GetFilePath(string host, int port, string serviceName)
        {
            string file_dir = Path.Combine(Directory.GetCurrentDirectory(), CONFIG_BASE_DIR, port.ToString(), host);
            if (!String.IsNullOrEmpty(serviceName))
                file_dir = Path.Combine(Directory.GetCurrentDirectory(), CONFIG_BASE_DIR, serviceName, port.ToString(), host);
            return file_dir;
        }

        /// <summary>
        /// 获取缓存的key
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="group"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private string GetCacheKey(string host, int port, string serviceName)
        {
            return $"{host}-{port}-{serviceName}".ToLower();
        }


        #endregion
    }
}
