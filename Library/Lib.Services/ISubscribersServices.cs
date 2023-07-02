using Dapper;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Lib.Services
{
    public interface ISubscribersServices : IBaseServices, IDisposable
    {
        public ServicesResponse<PagedResults<List<SubscribersDto>>> SubscribersPaging(string search = "", int page = 1, int pageSize = 10);
        public ServicesResponse<PagedResults<List<SubscribersDto>>> SubscribersPaging(DataTableRequest req);

        public ServicesResponse<SubscribersDto> AddUpdateSubscribers(AddUpdateSubscribersDto req);
        public ServicesResponse<SubscribersDto> GetSubscribers(Guid id);
        public ServicesResponse<bool> DeleteSubscribers(Guid id);

    }

    public class SubscribersServices : BaseServices, ISubscribersServices
    {

        public SubscribersServices()
        {
            Initilize();
        }

        public ServicesResponse<PagedResults<List<SubscribersDto>>> SubscribersPaging(DataTableRequest req)
        {

            string searchValue = req?.search?.value ?? "";
            int pageSize = Convert.ToInt32(req.length);
            int page = Convert.ToInt32(req.start) / pageSize + 1;

            return SubscribersPaging(searchValue, page, pageSize);

        }

        public ServicesResponse<PagedResults<List<SubscribersDto>>> SubscribersPaging(string search, int page = 1, int pageSize = 10)
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
                    whereClause = $@" where TRIM(LOWER(s.Firstname)) LIKE TRIM(LOWER('{search}'))+'%' or TRIM(LOWER(b.Lastname)) LIKE TRIM(LOWER('{search}'))+'%'";
                }
                string query = $@"SELECT count(s.Id) AS Totalbook
                                    FROM Subscribers s
                                    LEFT JOIN BooksLoans bl ON bl.SubscriberId = s.Id 
                                    GROUP BY s.Id 
                            {whereClause}
 
                            SELECT s.Id, s.Firstname, s.Lastname, s.CreatedAt ,s.UpdatedAt, count(bl.Id) AS Totalbook
                            FROM Subscribers s
                            LEFT JOIN BooksLoans bl ON bl.SubscriberId = s.Id 
                            GROUP BY s.Id, s.Firstname, s.Lastname, s.CreatedAt ,s.UpdatedAt 
                            {whereClause}

                            ORDER BY s.UpdatedAt desc
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";


                var reader = _idbConnection.QueryMultiple(query, new { Skip = skip, Take = take }, _idbTransaction);

                int count = reader.Read<int>().FirstOrDefault();
                List<SubscribersDto> allTodos = reader.Read<SubscribersDto>().ToList();

                return ServicesResponse<PagedResults<List<SubscribersDto>>>.Success(new PagedResults<List<SubscribersDto>>(allTodos, count, page, pageSize));



            }
            catch (Exception ex)
            {
                return ServicesResponse<PagedResults<List<SubscribersDto>>>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<SubscribersDto> AddUpdateSubscribers(AddUpdateSubscribersDto req)
        {
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("Firstname", req.Firstname);
                data.Add("Lastname", req.Lastname);
                data.Add("CreatedAt", DateTime.UtcNow);
                data.Add("UpdatedAt", DateTime.UtcNow);

                string query = string.Empty;
                Guid entityId;

                if (_adminSettings.IsUniqueSubscriberName)
                {
                    query = $@"SELECT Count(s.Id) FROM Subscribers s WHERE TRIM(LOWER(s.Firstname)) = TRIM(LOWER('{req.Firstname}')) and TRIM(LOWER(s.Lastname)) = TRIM(LOWER('{req.Lastname}'))";

                    if (!string.IsNullOrEmpty(req.Id?.ToString()))
                        query = query + $" and Id !='{req.Id}'";

                    int model = _idbConnection.QueryFirstOrDefault<int>(query, transaction: _idbTransaction);
                    if (model > 0)
                        return ServicesResponse<SubscribersDto>.Error($"{req.Firstname} {req.Lastname} Subscriber already exist");
                }

                if (string.IsNullOrEmpty(req.Id?.ToString()))
                {
                    query = $@"INSERT INTO Subscribers (Firstname, Lastname,CreatedAt,UpdatedAt) OUTPUT INSERTED.Id VALUES 
                                  (@Firstname, @Lastname, @CreatedAt, @UpdatedAt)";
                    entityId = _idbConnection.QuerySingle<Guid>(query, data, _idbTransaction);


                    
                }
                else
                {

                    query = $@"update Subscribers SET Firstname=@Firstname, Lastname=@Lastname, UpdatedAt=@UpdatedAt 
                                WHERE Id = '{req.Id}'";

                    var rowsAffected = _idbConnection.Execute(query, data, _idbTransaction);
                    entityId = req.Id.Value;

                   



                }

                #region manage subscriber loaned books
                query = $@"DELETE FROM BooksLoans WHERE SubscriberId = '{entityId}'";
                _idbConnection.Execute(query, transaction: _idbTransaction);

                
                if (req.LoanedBooks?.Count() > 0)
                {
                    int maxLoanedAllowed = _adminSettings.MaxLoanedAmount > 0 ? _adminSettings.MaxLoanedAmount : 3;
                    var subscriberBooks = req.LoanedBooks.Select(s => new { SubscriberId = entityId, BookId = s }).Take(maxLoanedAllowed);
                    query = $@"INSERT INTO BooksLoans (SubscriberId, BookId) VALUES (@SubscriberId, @BookId)";
                    var isa = _idbConnection.Execute(query, subscriberBooks, transaction: _idbTransaction);
                }
                #endregion

                return GetSubscribers(entityId);

            }
            catch (Exception ex)
            {
                return ServicesResponse<SubscribersDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<SubscribersDto> GetSubscribers(Guid id)
        {
            try
            {

                string query = $@" SELECT s.Id, s.Firstname, s.Lastname, s.CreatedAt ,s.UpdatedAt, count(bl.Id) AS Totalbook
                            FROM Subscribers s
                            LEFT JOIN BooksLoans bl ON bl.SubscriberId = s.Id 
                            WHERE s.Id = '{id}'
                            GROUP BY s.Id, s.Firstname, s.Lastname, s.CreatedAt ,s.UpdatedAt";

                SubscribersDto smodel = _idbConnection.QueryFirstOrDefault<SubscribersDto>(query, transaction: _idbTransaction);

                return ServicesResponse<SubscribersDto>.Success(smodel);


            }
            catch (Exception ex)
            {
                return ServicesResponse<SubscribersDto>.Error(ex.GetActualError());
            }
        }

        public ServicesResponse<bool> DeleteSubscribers(Guid id)
        {
            try
            {
                string query = $@"DELETE FROM Subscribers WHERE Id = '{id}'";
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
