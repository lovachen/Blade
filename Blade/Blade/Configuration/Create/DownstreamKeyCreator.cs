using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration.Create
{
    public class DownstreamKeyCreator : IDownstreamKeyCreator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downstream"></param>
        /// <returns></returns>
        public string Create(FileDownstreamOptions downstream)
        {
            return $"{downstream.ServiceName}|{nameof(downstream)}";
        }
    }
}
