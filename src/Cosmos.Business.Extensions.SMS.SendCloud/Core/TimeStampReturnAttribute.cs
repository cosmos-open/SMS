﻿using System;
using System.Threading.Tasks;
using Cosmos.Business.Extensions.SMS.Exceptions;
using Cosmos.Business.Extensions.SMS.SendCloud.Models.Results;
using Newtonsoft.Json.Linq;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace Cosmos.Business.Extensions.SMS.SendCloud.Core
{
    public class TimeStampReturnAttribute : JsonReturnAttribute
    {
        protected override async Task<object> GetTaskResult(ApiActionContext context)
        {
            var response = context.ResponseMessage;
            var s = await response.Content.ReadAsStringAsync();

            try
            {
                var json = JObject.Parse(s);
                if (json.Property("statusCode") != null)
                {
                    return json.ToObject<ResponseData<TimeStampResult>>();
                }

                return new ResponseData<TimeStampResult>
                {
                    StatusCode = 500,
                    Message = s
                };
            }
            catch (Exception e)
            {
                ExceptionHandleResolver.ResolveHandler()?.Invoke(e);
            }

            return new ResponseData<TimeStampResult>
            {
                StatusCode = (int)response.StatusCode,
                Message = "发送失败",
                Result = false
            };
        }
    }
}