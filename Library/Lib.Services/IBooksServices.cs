using Dapper;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Lib.Services
{
    public interface IBooksServices : IDisposable
    {
        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(string search = "", int page = 1, int pageSize = 10);
    }

    public class BooksServices : BaseServices, IBooksServices
    {
        public BooksServices(IDbConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {

        }

        //public BooksServices(string connectionString) : base(connectionString)
        //{

        //}

        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(string search, int page = 1, int pageSize = 10)
        {
            try
            {
                
                int maxPagSize = 50;
                pageSize = (pageSize > 0 && pageSize <= maxPagSize) ? pageSize : maxPagSize;

                int skip = (page - 1) * pageSize;
                int take = pageSize;

                string whereClause = "";
                if (string.IsNullOrEmpty(search))
                {
                    whereClause = $" where TRIM(LOWER(b.Title)) = TRIM(LOWER('{search}')) ";
                }
                string query = $@"SELECT 
                            COUNT(*)
                            FROM Books b
                            LEFT JOIN Geners ge ON b.GenereId = ge.Id 
                            {whereClause}
 
                            SELECT  b.*, ge.Name as Genere,ge.Id as GenereId FROM Books b
                            LEFT JOIN Geners ge ON b.GenereId = ge.Id 
                            {whereClause}

                            ORDER BY b.UpdatedAt
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";


                var reader = _idbConnection.QueryMultiple(query, new { Skip = skip, Take = take }, _idbTransaction);

                int count = reader.Read<int>().FirstOrDefault();
                List<BooksDto> allTodos = reader.Read<BooksDto>().ToList();

                return ServicesResponse<PagedResults<List<BooksDto>>>.Success(new PagedResults<List<BooksDto>>(allTodos, count, page, pageSize));



            }
            catch (Exception ex)
            {
                return ServicesResponse<PagedResults<List<BooksDto>>>.Error(ex.GetActualError());
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
