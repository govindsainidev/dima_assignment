using Lib.Services.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;

namespace Lib.Services
{
    public class BaseServices
    {
        protected internal IDbConnection _idbConnection;
        protected internal IDbTransaction _idbTransaction;
        protected internal string _connectionString;
        protected internal AdminSettings _adminSettings;

        public BaseServices(IDbConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _idbConnection = sqlConnection;
            _idbTransaction = dbTransaction;
            _adminSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AdminSettings>>().Value;
        }

        //public BaseServices(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
    }
}
