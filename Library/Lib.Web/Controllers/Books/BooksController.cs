using Lib.Services;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lib.Web.Controllers.Books
{
    public class BooksController : BaseController
    {

        public BooksController()
        {
        }

        public IActionResult Index()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    var rsesult = bookSerivce.AddBook(new AddBookDto
                    {
                        Title = "Sample Book1",
                        AuthorName = "sfd",
                        CreatedAt = DateTime.UtcNow,
                        GenereId = 1,
                        UpdatedAt = DateTime.UtcNow
                    });

                    if (rsesult.IsSuccess)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return View();
        }

        [HttpPost]
        [Route("GetPaging")]
        public IActionResult GetPaging([FromForm] DataTableRequest request)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    var result = bookSerivce.BookPaging(request);
                    if (result.IsSuccess)
                    {
                        var jsonData = new
                        {
                            draw = request.draw,
                            recordsFiltered = result.Data.TotalRecords,
                            recordsTotal = result.Data.TotalRecords,
                            data = result.Data.Items
                        };

                        return Ok(jsonData);
                    }
                    else { return BadRequest(result); }

                }
            }
        }
    }
}
