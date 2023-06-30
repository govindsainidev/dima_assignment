﻿using Lib.Services;
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
            return View();
        }

        [HttpPost]
        [Route("GetPaging")]
        public IActionResult GetPaging([FromForm] DataTableRequest request )
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IBooksServices bookSerivce = new BooksServices(connection, transaction))
                {
                    string searchValue = request?.search?.value??"";
                    int pageSize = Convert.ToInt32(request.length);
                    int page = Convert.ToInt32(request.start) / pageSize + 1;
                    var result = bookSerivce.BookPaging(searchValue, page, pageSize);
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
                        
                }
            }
            return BadRequest();

        }
    }
}
