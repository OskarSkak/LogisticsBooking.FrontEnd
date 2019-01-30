using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.Documents
{
    public class Response
    {
        public bool IsSuccesfull { get; private set; }
        public string Message { get; private set; }
        public string Exception { get; private set; }

        public Response(bool isSuccesfull, string message = null, string exception = null)
        {
            IsSuccesfull = isSuccesfull;
            Message = message;
            Exception = exception;
        }

        public static Response Succes()
        {
            return new Response(true);
        }

        public static Response Unsuccesfull()
        {
            return new Response(false);
        }

        public static Response Unsuccesfull(string error)
        {
            return new Response(false, error);
        }
    }
}
