﻿using System;
using System.Threading.Tasks;
using Cosmos.Business.Extensions.SMS.ChuangLan.Models.Results;
using Cosmos.Business.Extensions.SMS.Exceptions;
using Newtonsoft.Json.Linq;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace Cosmos.Business.Extensions.SMS.ChuangLan.Core.Attributes
{
    public class ChuanglanSendVariableReturnAttribute : JsonReturnAttribute
    {
        protected override async Task<object> GetTaskResult(ApiActionContext context)
        {
            var response = context.ResponseMessage;

            var s = await response.Content.ReadAsStringAsync();

            try
            {
                var json = JObject.Parse(s);
                if (json.Property("code") != null)
                {
                    return json.ToObject<VariableResponseData>();
                }

                return new ResponseData()
                {
                    Code = "500",
                    ErrorMsg = s
                };
            }
            catch (Exception e)
            {
                ExceptionHandleResolver.ResolveHandler()?.Invoke(e);
            }

            return new VariableResponseData()
            {
                Code = response.StatusCode.ToString(),
                ErrorMsg = "发送失败",
            };
        }
    }
}