using Lib.Services;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Lib.Web.Controllers.Books
{
    public class BooksController : BaseController
    {
        private readonly IBooksServices _bookSerivce;
        private readonly IGenersServices _genersServices;

        public BooksController(IBooksServices bookSerivce, IGenersServices genersServices)
        {
            _bookSerivce = bookSerivce;
            _genersServices = genersServices;
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
           
            var result = _bookSerivce.GetBook(id ?? Guid.Empty);
            if (result.IsSuccess)
            {
                _bookSerivce.Commit();
                AddUpdateBooksDto addDto = _mapper.Map<AddUpdateBooksDto, BooksDto>(result.Data);
                addDto = addDto ?? new AddUpdateBooksDto();
               
                var gener = _genersServices.GetGeneres();
                if (!gener.IsSuccess)
                    return StatusCode(StatusCodes.Status500InternalServerError, gener);

                addDto.Geners = gener.Data;
                return PartialView("_AddUpdate", addDto);
            }
            else { return BadRequest(result); }
        }

        [HttpGet]
        [Route("GetDetailBook/{id}")]
        public IActionResult GetDetailBook(Guid id)
        {
            var result = _bookSerivce.GetBook(id);
            if (result.IsSuccess)
            {
                return PartialView("_Detail", result.Data);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }


        }

        [HttpPost]
        [Route("AddBook")]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(AddUpdateBooksDto reqDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var result = _bookSerivce.AddUpdateBook(reqDto);
            if (result.IsSuccess)
            {
                _bookSerivce.Commit();
                return Ok(true);
            }
            else
            {
                _bookSerivce.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }


        }

        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var result = _bookSerivce.DeleteBook(id);
            if (result.IsSuccess)
            {
                _bookSerivce.Commit();
                return Ok(true);
            }
            else
            {
                _bookSerivce.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

        }

        [HttpPost]
        [Route("GetBookPaging")]
        public IActionResult GetBookPaging([FromForm] DataTableRequest request)
        {
            var result = _bookSerivce.BookPaging(request);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    draw = request.draw,
                    recordsFiltered = result.Data.TotalRecords,
                    recordsTotal = result.Data.TotalRecords,
                    data = result.Data.Items
                });
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }
        }

        [HttpGet]
        [Route("BooksLoan")]
        public IActionResult BooksLoan()
        {
            return View();
        }

        [HttpGet]
        [Route("GetBooksLoan/{id_author_title?}")]
        public IActionResult GetBooksLoan(string? id_author_title)
        {
            var result = _bookSerivce.GetLoanBook(id_author_title);
            if (result.IsSuccess)
            {
                return PartialView("_BooksLoan", result.Data);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }


        }
    }
}
