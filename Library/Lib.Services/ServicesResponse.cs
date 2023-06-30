using System.Net;

namespace Lib.Services
{
    public class ServicesResponse<T>
    {
        public ServicesResponse()
        {

        }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public int StatusCode { get; private set; }

        public static ServicesResponse<T> Success()
        {
            return new ServicesResponse<T>
            {
                Message = "Success",
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public static ServicesResponse<T> Success(string message)
        {
            return new ServicesResponse<T>
            {
                Message = message,
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public static ServicesResponse<T> Success(T Data)
        {
            return new ServicesResponse<T>
            {
                Message = "Success",
                Data = Data,
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        public static ServicesResponse<T> Success(string message, T Data)
        {
            return new ServicesResponse<T>
            {
                Message = message,
                Data = Data,
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        public static ServicesResponse<T> Error()
        {
            return new ServicesResponse<T>
            {
                Message = "Error!",
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        public static ServicesResponse<T> Error(string message)
        {
            return new ServicesResponse<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        public static ServicesResponse<T> Error(T Data)
        {
            return new ServicesResponse<T>
            {
                Message = "Error",
                Data = Data,
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        public static ServicesResponse<T> Error(string message, T Data)
        {
            return new ServicesResponse<T>
            {
                Message = message,
                Data = Data,
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}

