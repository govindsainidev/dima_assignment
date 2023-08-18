using Lib.Services.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Lib.Services
{
    public interface IBaseServices
    {
        void Initilize();
        void Commit();
        void Rollback();
        void DisposeConnection();

    }
    public class BaseServices : IBaseServices
    {
        protected internal string _connectionString;
        protected internal AdminSettings _adminSettings;
        protected internal AppSettings _appSettings;
        protected static internal IDbConnection _idbConnection;
        protected static internal IDbTransaction _idbTransaction;

        public BaseServices()
        {
            _adminSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AdminSettings>>().Value;
            _appSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AppSettings>>().Value;
            _connectionString = _appSettings.ConnectionString;
        }

        private void OpenConnection()
        {
            lock (_idbConnection)
            {
                if (_idbConnection.State != ConnectionState.Open)
                    _idbConnection?.Open();
            }
        }
        public void Initilize()
        {
            _idbConnection = _idbConnection ?? new SqlConnection(_connectionString);
            OpenConnection();
            _idbTransaction = _idbTransaction ?? _idbConnection.BeginTransaction();

        }
        public void Commit()
        {
            _idbTransaction?.Commit();

        }
        public void Rollback()
        {
            _idbTransaction?.Rollback();

        }
        public void DisposeConnection()
        {
            _idbConnection = null;
            _idbTransaction = null;
        }

    }
}
