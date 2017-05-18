using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Vans_SRMS_API.ViewModels
{
    public class RepoResponse<T>
    {

        public HttpStatusCode responseCode { get; set; }
        public T responseObject { get; set; }
        public string responseMessage { get; set; }
        public bool isSuccess
        {
            get
            {
                int code = (int)responseCode;
                return (code >= 200 && code <= 299);
            }
        }
        public string notes { get; set; }
        public string _BROADCAST_ORDER_UPDATES
        {
            get
            {
                return "BROADCAST ORDER UPDATES";
            }
        }

        public RepoResponse(HttpStatusCode code, T obj)
        {
            responseCode = code;
            responseObject = obj;
        }
        public RepoResponse(HttpStatusCode code, string message = "")
        {
            responseCode = code;
            responseMessage = message;
        }

        public IActionResult respond()
        {
            if (isSuccess)
            {
                if (responseObject == null)
                {
                    // return new OkObjectResult(responseMessage);
                    ContentResult res = new ContentResult();
                    res.StatusCode = (int)responseCode;
                    res.Content = responseMessage;
                    return res;
                }
                else
                {
                    return new OkObjectResult(responseObject);
                }
            }
            else
            {
                ObjectResult result = new ObjectResult(responseMessage);
                result.StatusCode = (int)responseCode;
                return result;
            }
        }
    }
}
