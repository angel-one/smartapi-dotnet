using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AngelBroking
{
    #region WebSocketv2 
    public class AngelException : Exception
    {
        HttpStatusCode Status;
        public AngelException(string Message, HttpStatusCode HttpStatus, Exception innerException = null) : base(Message, innerException) { Status = HttpStatus; }
    }

    public class GeneralException : AngelException
    {
        public GeneralException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.InternalServerError, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }

    public class TokenException : AngelException
    {
        public TokenException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.Forbidden, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }


    public class PermissionException : AngelException
    {
        public PermissionException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.Forbidden, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }

    public class OrderException : AngelException
    {
        public OrderException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadRequest, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }

    public class InputException : AngelException
    {
        public InputException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadRequest, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }

    public class DataException : AngelException
    {
        public DataException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadGateway, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }

    public class NetworkException : AngelException
    {
        public NetworkException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.ServiceUnavailable, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
    #endregion
}
