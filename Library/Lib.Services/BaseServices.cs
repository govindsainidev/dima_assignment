using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services
{
    public class BaseServices
    {
        public BaseServices(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; private set; }
    }
}
