using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.Utils;
using Microsoft.AspNetCore.Http;
using Vans_SRMS_API.Models;

namespace Vans_SRMS_API.Filters
{
    /// <summary>
    /// Validate device key from header value
    /// </summary>
    public class DeviceAuthorizedAttribute : TypeFilterAttribute
    {
        public DeviceAuthorizedAttribute() : base(typeof(DeviceKeyFilterImpl))
        {
        }

        private class DeviceKeyFilterImpl : IActionFilter
        {
            private IAuthenticationRepository _authRepo;
            private IStoreRepository _storeRepo;
            public DeviceKeyFilterImpl(IAuthenticationRepository authRepo, IStoreRepository storeRepo)
            {
                _authRepo = authRepo;
                _storeRepo = storeRepo;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (!headerKeyIsValid())
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = ("Device Unauthorized")
                    };
                }

                if (!defaultStoreIsSet())
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = ("Default store has not been set")
                    };
                }

                bool headerKeyIsValid()
                {
                    if (!context.HttpContext.Request.Headers.ContainsKey(Util._DEVICE_ID_HEADER_FIELD))
                        return false;

                    string deviceKey = context.HttpContext.Request.Headers[Util._DEVICE_ID_HEADER_FIELD].ToString();

                    // check the session for valid key
                    int? deviceId = context.HttpContext.Session.GetInt32(deviceKey);
                    if (deviceId == null)
                    {
                        // check the database for valid key
                        deviceId = _authRepo.DeviceKeyIsValid(deviceKey);
                        if (deviceId == null)
                            return false;
                    }

                    // set the device ID on the context so it can be used within any Action method
                    context.HttpContext.Items[Util._DEVICE_ID_HEADER_FIELD] = deviceId;

                    // store the device key in session for quicker lookup next time
                    context.HttpContext.Session.SetInt32(deviceKey, deviceId.Value);

                    return true;
                }

                bool defaultStoreIsSet()
                {
                    Store defaultStore = _storeRepo.GetDefault();

                    return (defaultStore != null && defaultStore.StoreNumber != "-1");
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }

    /// <summary>
    /// Swagger documentation helper
    /// </summary>
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "DeviceKey",
                In = "header",
                Description = "Registered Device Key",
                Required = false,
                Type = "string"
            });
        }
    }
}