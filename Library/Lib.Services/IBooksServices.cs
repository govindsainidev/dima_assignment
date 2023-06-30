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
using System.Threading.Tasks;

namespace Lib.Services
{
    public interface IBooksServices : IDisposable
    {
        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(string search = "", int page = 1, int pageSize = 10);
        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(DataTableRequest req);

        public ServicesResponse<BooksDto> AddBook(AddBookDto req);
    }

    public class BooksServices : BaseServices, IBooksServices
    {
        public BooksServices(IDbConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {

        }

        //public BooksServices(string connectionString) : base(connectionString)
        //{

        //}

        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(DataTableRequest req)
        {
            try
            {
                string searchValue = req?.search?.value ?? "";
                int pageSize = Convert.ToInt32(req.length);
                int page = Convert.ToInt32(req.start) / pageSize + 1;
                
                int skip = (page - 1) * pageSize;
                int take = pageSize;

                string whereClause = "";
                if (!string.IsNullOrEmpty(searchValue))
                {
                    whereClause = $@" where TRIM(LOWER(b.Title)) LIKE TRIM(LOWER('{searchValue}'))+'%' and TRIM(LOWER(b.AuthorName)) LIKE TRIM(LOWER('{searchValue}'))+'%'";
                }

                //string orderBy = "";
                //if (req.order?.Count>0)
                //{
                //    var orderByColumn = req.columns[req.order[0].column];
                //    orderBy = $@" ORDER BY b.{orderByColumn.data} {req.order[0].dir}";
                //}

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

        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(string search, int page = 1, int pageSize = 10)
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
                    whereClause = $@" where TRIM(LOWER(b.Title)) LIKE TRIM(LOWER('{search}'))+'%' and TRIM(LOWER(b.AuthorName)) LIKE TRIM(LOWER('{search}'))+'%'";
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

        public ServicesResponse<BooksDto> AddBook(AddBookDto req)
        {
            try
            {
                string query = string.Empty;
                if (_adminSettings.IsUniqueBookName)
                {
                    query = $@"SELECT Count(b.Title) FROM Books b WHERE TRIM(LOWER(b.Title)) = TRIM(LOWER('{req.Title}'))";

                    int model = _idbConnection.QueryFirstOrDefault<int>(query, transaction: _idbTransaction);
                    if (model>0)
                        return ServicesResponse<BooksDto>.Error($"{req.Title} Book name already exist");
                }

                query = $@"INSERT INTO Books (Title, AuthorName,GenereId, CreatedAt, UpdatedAt) OUTPUT INSERTED.Id VALUES 
                                  (@Title, @AuthorName,@GenereId, @CreatedAt, @UpdatedAt)";

                var added = _idbConnection.QuerySingle<Guid>(query, req, _idbTransaction);

                query = $@"SELECT * FROM Books WHERE Id = '{added}'";

                BooksDto smodel = _idbConnection.QueryFirstOrDefault<BooksDto>(query, transaction: _idbTransaction);

                return ServicesResponse<BooksDto>.Success(new BooksDto());



            }
            catch (Exception ex)
            {
                return ServicesResponse<BooksDto>.Error(ex.GetActualError());
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
