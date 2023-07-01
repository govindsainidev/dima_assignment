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

namespace Lib.Web.Controllers.Genere
{
    public class GenereController : BaseController
    {

        public GenereController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetGenere/{id?}")]
        public IActionResult GetGenere(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IGenersServices Genereerivce = new GenersServices(connection, transaction))
                {
                    var result = Genereerivce.GetGenere(id ?? 0);
                    if (result.IsSuccess)
                    {
                        AddUpdateGenersDto addDto = _mapper.Map<AddUpdateGenersDto, GenersDto>(result.Data);
                        return PartialView("_AddUpdate", addDto);
                    }
                    else { return BadRequest(result); }

                }
            }
        }

        [HttpGet]
        [Route("GetDetailGenere/{id}")]
        public IActionResult GetDetailGenere(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IGenersServices Genereerivce = new GenersServices(connection, transaction))
                {
                    var result = Genereerivce.GetGenere(id);
                    if (result.IsSuccess)
                    {
                        return PartialView("_Detail", result.Data);
                    }
                    else { return BadRequest(result); }

                }
            }
        }

        [HttpPost]
        [Route("AddGenere")]
        [ValidateAntiForgeryToken]
        public IActionResult AddGenere(AddUpdateGenersDto reqDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(false);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IGenersServices Genereerivce = new GenersServices(connection, transaction))
                {
                    var result = Genereerivce.AddUpdateGenere(reqDto);
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
        [Route("DeleteGenere/{id}")]
        public IActionResult DeleteGenere(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                using (IGenersServices Genereerivce = new GenersServices(connection, transaction))
                {
                    var result = Genereerivce.DeleteGenere(id);
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
                using (IGenersServices Genereerivce = new GenersServices(connection, transaction))
                {
                    var result = Genereerivce.GenerePaging(request);
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
