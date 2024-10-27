using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layer_Entities.Wrappers
{
    public class JsonResponse<T>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public JsonResponse()
        {
            Errors = new List<string>();
        }

        public JsonResponse(T data, string message = null)
        {
            Succeeded = true;
            Data = data;
            Message = message;
            Errors = new List<string>();
        }

        public JsonResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string>();
        }
    }
}
