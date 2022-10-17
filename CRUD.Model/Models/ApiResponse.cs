using System.Net;
using System.Runtime.Serialization;

namespace CRUD.Model.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }

        public string? ErrorMessage { get; set; }

        public T? Result { get; set; }
    }
}
