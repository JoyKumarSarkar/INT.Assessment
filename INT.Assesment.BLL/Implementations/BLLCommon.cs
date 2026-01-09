using INT.Assessment.BLL.Signatures;
using INT.Assessment.ENTITY;
using INT.Assessment.LOGGER;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INT.Assessment.BLL.Implementations
{
    public partial class BLLCommon : IBLLCommon
    {
        private readonly LogAssembly LogAssembly = LogAssembly.BLL;
        private readonly IFileLogger _FileLogger;

        public BLLCommon (IFileLogger FileLogger)
        {
            _FileLogger = FileLogger;
        }
    }
}
