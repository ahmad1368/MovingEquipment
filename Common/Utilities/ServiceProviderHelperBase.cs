﻿using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class ServiceProviderHelper
    {
        public static IServiceProvider Instance { get; set; } = null!;
    }
}
