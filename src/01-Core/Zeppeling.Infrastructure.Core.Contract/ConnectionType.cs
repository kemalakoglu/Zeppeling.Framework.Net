using Zeppeling.Infrastructure.Core.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zeppeling.Infrastructure.Core.Contract
{
    public class ConnectionType
    {
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
    }
}
