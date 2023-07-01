using Dapper;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Services
{
    public interface IGenersServices : IDisposable
    {
        public ServicesResponse<PagedResults<List<GenersDto>>> GenerePaging(string search = "", int page = 1, int pageSize = 10);
        public ServicesResponse<PagedResults<List<GenersDto>>> GenerePaging(DataTableRequest req);

        public ServicesResponse<GenersDto> AddUpdateGenere(AddUpdateGenersDto req);
        public ServicesResponse<GenersDto> GetGenere(int id);
        public ServicesResponse<bool> DeleteGenere(int id);
    }

    public class GenersServices : BaseServices, IGenersServices
    {
        public GenersServices(IDbConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {

        }


        public ServicesResponse<PagedResults<List<GenersDto>>> GenerePaging(DataTableRequest req)
        {

            string searchValue = req?.search?.value ?? "";
            int pageSize = Convert.ToInt32(req.length);
            int page = Convert.ToInt32(req.start) / pageSize + 1;
            return GenerePaging(searchValue, page, pageSize);

        }

        public ServicesResponse<PagedResults<List<GenersDto>>> GenerePaging(string search, int page = 1, int pageSize = 10)
        {
            try
            {

                int maxPagSize = 50;
                pageSize = (pageSize > 0 && pageSize <= maxPagSize) ? pageSize : maxPagSize;

                int skip = (page - 1) * pageSize;
                int take = pageSize;

                string whereClause = "";
                if (!string.IsNullOrEmpty(search))
                {
                    whereClause = $@" where TRIM(LOWER(b.Name)) LIKE TRIM(LOWER('{search}'))+'%'";
                }
                string query = $@"SELECT 
                            COUNT(*)
                            FROM Geners g
                            {whereClause}
 
                            SELECT  * FROM Geners g
                            {whereClause}

                            ORDER BY b.Name
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";


                var reader = _idbConnection.QueryMultiple(query, new { Skip = skip, Take = take }, _idbTransaction);

                int count = reader.Read<int>().FirstOrDefault();
                List<GenersDto> allTodos = reader.Read<GenersDto>().ToList();

                return ServicesResponse<PagedResults<List<GenersDto>>>.Success(new PagedResults<List<GenersDto>>(allTodos, count, page, pageSize));



            }
            catch (Exception ex)
            {
                return ServicesResponse<PagedResults<List<GenersDto>>>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<GenersDto> AddUpdateGenere(AddUpdateGenersDto req)
        {
            try
            {
                string query = string.Empty;
                int entityId;

                query = $@"SELECT Count(g.Name) FROM Geners g WHERE TRIM(LOWER(b.Name)) = TRIM(LOWER('{req.Name}'))";

                int model = _idbConnection.QueryFirstOrDefault<int>(query, transaction: _idbTransaction);
                if (model > 0)
                    return ServicesResponse<GenersDto>.Error($"{req.Name} Gener name already exist");


                if (req.Id>0)
                {
                    query = $@"INSERT INTO Geners (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)";
                    entityId = _idbConnection.QuerySingle<int>(query, req, _idbTransaction);
                }
                else
                {
                    query = $@"UPDATE Geners SET Name=@Name, Description=@Description WHERE Id = '{req.Id}'";
                    int rowsAffected = _idbConnection.Execute(query, req, _idbTransaction);
                    entityId = req.Id;
                }

                return GetGenere(entityId);

            }
            catch (Exception ex)
            {
                return ServicesResponse<GenersDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<GenersDto> GetGenere(int id)
        {
            try
            {
                string query = $@"SELECT * FROM Geners WHERE Id = '{id}'";
                GenersDto smodel = _idbConnection.QueryFirstOrDefault<GenersDto>(query, transaction: _idbTransaction);
                return ServicesResponse<GenersDto>.Success(smodel);

            }
            catch (Exception ex)
            {
                return ServicesResponse<GenersDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<bool> DeleteGenere(int id)
        {
            try
            {
                string query = $@"DELETE FROM Geners WHERE Id = '{id}'";
                var smodel = _idbConnection.Execute(query, transaction: _idbTransaction);
                return ServicesResponse<bool>.Success(smodel > 0);

            }
            catch (Exception ex)
            {
                return ServicesResponse<bool>.Error(ex.GetActualError());
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
