using System.Globalization;

namespace NailDesignerAPI.Models {
    public class ServiceResult<T> {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public static ServiceResult<T> Ok( T data ) => new() {
            Success = true,
            StatusCode = 200,
            Data = data
        };

        public static ServiceResult<T> Created( T data ) => new() {
            Success = true,
            StatusCode = 201,
            Data = data
        };

        public static ServiceResult<T> NotFound( string message ) => new() {
            Success = false,
            StatusCode = 404,
            ErrorMessage = message
        };

        public static ServiceResult<T> Conflict( string message ) => new() {
            Success = false,
            StatusCode = 409,
            ErrorMessage = message
        };
    }
}
