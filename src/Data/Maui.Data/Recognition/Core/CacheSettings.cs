﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Recognition.Core
{
    public class CacheSettings
    {
        public CacheSettings()
        {
            MaxEntryLiveTime = TimeSpan.FromDays( 1 );
            MaxCacheSizeInKB = 50 * 1024;
        }

        public TimeSpan MaxEntryLiveTime
        {
            get;
            set;
        }

        public int MaxCacheSizeInKB
        {
            get;
            set;
        }
    }
}
