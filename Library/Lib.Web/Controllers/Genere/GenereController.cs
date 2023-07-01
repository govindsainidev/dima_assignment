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
using System.Data.SqlClient;

namespace Lib.Web.Controllers.Genere
{
    public class GenereController : BaseController
    {
        private readonly IGenersServices _genersServices;

        public GenereController(IGenersServices genersServices)
        {
            _genersServices = genersServices;
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
            var result = _genersServices.GetGenere(id ?? 0);
            if (result.IsSuccess)
            {
                AddUpdateGenersDto addDto = _mapper.Map<AddUpdateGenersDto, GenersDto>(result.Data);
                return PartialView("_AddUpdate", addDto);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }
        }

        [HttpGet]
        [Route("GetDetailGenere/{id}")]
        public IActionResult GetDetailGenere(int id)
        {
            var result = _genersServices.GetGenere(id);
            if (result.IsSuccess)
            {
                return PartialView("_Detail", result.Data);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }
        }

        [HttpPost]
        [Route("AddGenere")]
        [ValidateAntiForgeryToken]
        public IActionResult AddGenere(AddUpdateGenersDto reqDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var result = _genersServices.AddUpdateGenere(reqDto);
            if (result.IsSuccess)
            {
                _genersServices.Commit();
                return Ok(true);
            }
            else
            {
                _genersServices.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

        }

        [HttpDelete]
        [Route("DeleteGenere/{id}")]
        public IActionResult DeleteGenere(int id)
        {
            var result = _genersServices.DeleteGenere(id);
            if (result.IsSuccess)
            {
                _genersServices.Commit();
                return Ok(true);
            }
            else
            {
                _genersServices.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

        }

        [HttpPost]
        [Route("GetGenerePaging")]
        public IActionResult GetGenerePaging([FromForm] DataTableRequest request)
        {
            var result = _genersServices.GenerePaging(request);
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
    }
}
