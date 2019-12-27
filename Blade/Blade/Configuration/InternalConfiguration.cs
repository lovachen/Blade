﻿using Blade.Configuration.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blade.Configuration
{
    public class InternalConfiguration
    {
        public InternalConfiguration()
        {
            GlobalConfiguration = new FileGlobalConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        public FileGlobalConfiguration GlobalConfiguration { get; set; }
    }
}
