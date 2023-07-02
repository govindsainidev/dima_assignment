using Lib.Services;
using Lib.Services.Core;
using Lib.Services.Dtos;
using Lib.Services.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Lib.Web.Controllers.Subscribers
{
    public class SubscribersController : BaseController
    {
        private readonly ISubscribersServices _subscribersServices;
        private readonly IBooksServices _booksServices;

        public SubscribersController(ISubscribersServices subscribersServices, IBooksServices booksServices)
        {
            _subscribersServices = subscribersServices;
            _booksServices = booksServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetSubscriber/{id?}")]
        public IActionResult GetSubscriber(Guid? id)
        {
            var result = _subscribersServices.GetSubscribers(id ?? Guid.Empty);
            if (result.IsSuccess)
            {
                _subscribersServices.Commit();
                AddUpdateSubscribersDto addDto = _mapper.Map<AddUpdateSubscribersDto, SubscribersDto>(result.Data);
                addDto = addDto ?? new AddUpdateSubscribersDto();

                var bookRes = _booksServices.GetAllBooks();
                if(!bookRes.IsSuccess)
                    return BadRequest(result);

                var selectedbookRes = _booksServices.SubscriberBooks(id ?? Guid.Empty);
                if (!selectedbookRes.IsSuccess)
                    return BadRequest(selectedbookRes);

                addDto.LoanedBooks = selectedbookRes.Data.Select(s => s.Id.Value);

                addDto.AllBooks = bookRes.Data.Where(s=> !addDto.LoanedBooks.Contains(s.Id.Value));
                addDto.SelectedBooks = selectedbookRes.Data;
                

                return PartialView("_AddUpdate", addDto);
            }
            else { return BadRequest(result); }
        }

        [HttpGet]
        [Route("GetDetailSubscriber/{id}")]
        public IActionResult GetDetailSubscriber(Guid id)
        {
            var result = _subscribersServices.GetSubscribers(id);
            if (result.IsSuccess)
            {
                return PartialView("_Detail", result.Data);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, result); }


        }

        [HttpPost]
        [Route("AddSubscriber")]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubscriber(AddUpdateSubscribersDto reqDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var result = _subscribersServices.AddUpdateSubscribers(reqDto);
            if (result.IsSuccess)
            {
                _subscribersServices.Commit();
                return Ok(true);
            }
            else
            {
                _subscribersServices.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }


        }

        [HttpDelete]
        [Route("DeleteSubscriber/{id}")]
        public IActionResult DeleteSubscriber(Guid id)
        {
            var result = _subscribersServices.DeleteSubscribers(id);
            if (result.IsSuccess)
            {
                _subscribersServices.Commit();
                return Ok(true);
            }
            else
            {
                _subscribersServices.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

        }

        [HttpPost]
        [Route("GetSubscriberPaging")]
        public IActionResult GetSubscriberPaging([FromForm] DataTableRequest request)
        {
            var result = _subscribersServices.SubscribersPaging(request);
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
