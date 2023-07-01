﻿using Dapper;
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
    public interface IBooksServices : IBaseServices, IDisposable
    {
        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(string search = "", int page = 1, int pageSize = 10);
        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(DataTableRequest req);

        public ServicesResponse<BooksDto> AddUpdateBook(AddUpdateBooksDto req);
        public ServicesResponse<BooksDto> GetBook(Guid id);
        public ServicesResponse<bool> DeleteBook(Guid id);
    }

    public class BooksServices : BaseServices, IBooksServices
    {

        public BooksServices()
        {
            Initilize();
        }

        public ServicesResponse<PagedResults<List<BooksDto>>> BookPaging(DataTableRequest req)
        {

            string searchValue = req?.search?.value ?? "";
            int pageSize = Convert.ToInt32(req.length);
            int page = Convert.ToInt32(req.start) / pageSize + 1;

            return BookPaging(searchValue, page, pageSize);

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

                            ORDER BY b.UpdatedAt desc
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

        public ServicesResponse<BooksDto> AddUpdateBook(AddUpdateBooksDto req)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(req));
                data.Add("CreatedAt", DateTime.UtcNow);
                data.Add("UpdatedAt", DateTime.UtcNow);
                string query = string.Empty;
                Guid entityId;
                if (_adminSettings.IsUniqueBookName)
                {
                    query = $@"SELECT Count(b.Title) FROM Books b WHERE TRIM(LOWER(b.Title)) = TRIM(LOWER('{req.Title}'))";
                    if (!string.IsNullOrEmpty(req.Id?.ToString()))
                        query = query + $" and Id !='{req.Id}'";

                    int model = _idbConnection.QueryFirstOrDefault<int>(query, transaction: _idbTransaction);
                    if (model > 0)
                        return ServicesResponse<BooksDto>.Error($"{req.Title} Book name already exist");
                }

                if (string.IsNullOrEmpty(req.Id?.ToString()))
                {
                    query = $@"INSERT INTO Books (Title, AuthorName,GenereId, CreatedAt, UpdatedAt) OUTPUT INSERTED.Id VALUES 
                                  (@Title, @AuthorName,@GenereId, @CreatedAt, @UpdatedAt)";
                    entityId = _idbConnection.QuerySingle<Guid>(query, data, _idbTransaction);
                }
                else
                {
                    query = $@"update Books SET Title=@Title, AuthorName=@AuthorName,GenereId=@GenereId, UpdatedAt=@UpdatedAt 
                                WHERE Id = '{req.Id}'";

                    int rowsAffected = _idbConnection.Execute(query, data, _idbTransaction);
                    entityId = req.Id.Value;
                }

                return GetBook(entityId);

            }
            catch (Exception ex)
            {
                return ServicesResponse<BooksDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<BooksDto> GetBook(Guid id)
        {
            try
            {

                string query = $@"SELECT * FROM Books WHERE Id = '{id}'";
                BooksDto smodel = _idbConnection.QueryFirstOrDefault<BooksDto>(query, transaction: _idbTransaction);

                return ServicesResponse<BooksDto>.Success(smodel);


            }
            catch (Exception ex)
            {
                return ServicesResponse<BooksDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<bool> DeleteBook(Guid id)
        {
            try
            {
                string query = $@"DELETE FROM Books WHERE Id = '{id}'";
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
            DisposeConnection();
        }


    }
}
