using Lib.Services.Core;
using Microsoft.Extensions.Options;
using System.Data;

namespace Lib.Services
{
    public class BaseServices
    {
        protected internal IDbConnection _idbConnection;
        protected internal IDbTransaction _idbTransaction;
        protected internal string _connectionString;
        
        public BaseServices(IDbConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _idbConnection = sqlConnection;
            _idbTransaction = dbTransaction;
        }

        //public BaseServices(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
    }
}
