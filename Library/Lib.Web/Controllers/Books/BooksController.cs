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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetBook/{id?}")]
        public IActionResult GetBook(Guid? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    var result = bookSerivce.GetBook(id ?? Guid.Empty);
                    if (result.IsSuccess)
                    {
                        AddUpdateBooksDto addDto = _mapper.Map<AddUpdateBooksDto, BooksDto>(result.Data);
                        return PartialView("_AddUpdate", addDto);
                    }
                    else { return BadRequest(result); }

                }
            }
        }

        [HttpPost]
        [Route("AddBook")]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(AddUpdateBooksDto reqDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(false);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    var result = bookSerivce.AddUpdateBook(reqDto);
                    if (result.IsSuccess)
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest(result);
                    }

                }
            }

        }

        [HttpDelete]
        [Route("DeleteBook")]
        public IActionResult DeleteBook(Guid Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    var result = bookSerivce.DeleteBook(Id);
                    if (result.IsSuccess)
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest(result);
                    }

                }
            }

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
