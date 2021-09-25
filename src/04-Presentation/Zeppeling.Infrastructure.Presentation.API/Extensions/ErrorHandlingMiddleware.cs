using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Error;
using Zeppeling.Framework.Abstactions.Request;
using Zeppeling.Infrastructure.Core.Response;
using Zeppeling.Infrastructure.Core.Response.BusinessException;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;

namespace Zeppeling.Infrastructure.Presentation.API.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            var responseBody = new MemoryStream();
            var sw = Stopwatch.StartNew();
            string bodyStr = string.Empty;
            Stream originalBodyStream = null;
            try
            {
                bodyStr = await FormatRequest(context.Request);
                RequestDTO<object> request = JsonConvert.DeserializeObject<RequestDTO<object>>(bodyStr);
                context.Request.Body.Position = 0;
                originalBodyStream = context.Response.Body;

                if (request != null)
                {
                    //Log.Write(
                    //    LogLevel.Information, 
                    //    MessageTemplate.errorMiddlewareRequestLogTemplate, 
                    //    LogValue.Of("RequestMethod", context.Request.Method), 
                    //    LogValue.Of("RequestPath", context.Request.Path.Value), 
                    //    LogValue.Of("TrackId", request.RequestHeader.TrackId), 
                    //    LogValue.Of("ProcessCode", request.RequestHeader.ProcessCode), 
                    //    LogValue.Of("MessageType", request.RequestHeader.MessageType), 
                    //    LogValue.Of("ClientCode", request.RequestHeader.ClientCode), 
                    //    LogValue.Of("ApplicationId", request.RequestHeader.ApplicationId), 
                    //    LogValue.Of("RequestDate", request.RequestHeader.RequestDate), 
                    //    LogValue.Of("RequestHeader", 
                    //    JsonConvert.SerializeObject(request.RequestHeader)), 
                    //    LogValue.Of("RequestBody", 
                    //    JsonConvert.SerializeObject(request.RequestBody)));
                }

                context.Response.Body = responseBody;
                await next(context);
                sw.Stop();
                if (request != null)
                {
                        //logger.Write(
                        //    LogLevel.Information, 
                        //    MessageTemplate.errorMiddlewareResponseLogTemplate, 
                        //    LogValue.Of("RequestMethod", context.Request.Method), 
                        //    LogValue.Of("RequestPath", context.Request.Path.Value), 
                        //    LogValue.Of("TrackId", request.RequestHeader.TrackId), 
                        //    LogValue.Of("ProcessCode", request.RequestHeader.ProcessCode), 
                        //    LogValue.Of("MessageType", request.RequestHeader.MessageType), 
                        //    LogValue.Of("ClientCode", request.RequestHeader.ClientCode), 
                        //    LogValue.Of("ApplicationId", request.RequestHeader.ApplicationId), 
                        //    LogValue.Of("HttpStatus", context.Response.StatusCode), 
                        //    LogValue.Of("ResponseBody", FormatResponse(context.Response)), 
                        //    LogValue.Of("ElapsedTime", sw.Elapsed.TotalMilliseconds));
                }
         
                await responseBody.CopyToAsync(originalBodyStream);
                responseBody.Dispose();
            }
            catch (HttpRequestException ex)
            {
                //logger.Write(LogLevel.Error, ex.Message, ex, LogValue.Of("ServicePath", context.Request.Path.Value));
                await HandleExceptionAsync(context, ex);
            }
            catch (AuthenticationException ex)
            {
                //logger.Write(LogLevel.Error, ex.Message, ex, LogValue.Of("ServicePath", context.Request.Path.Value));
                await HandleExceptionAsync(context, ex);
            }
            catch (BusinessException ex)
            {
                //logger.Write(LogLevel.Error, ex.Message, ex, LogValue.Of("ServicePath", context.Request.Path.Value));
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                //logger.Write(LogLevel.Error, ex.Message, ex, LogValue.Of("ExceptionSource", ex.Source), LogValue.Of("ExceptionTargetSize", ex.TargetSite));
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, object exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            string message = string.Empty;
            string RC = string.Empty;
            string details = string.Empty;

            if (exception.GetType() == typeof(HttpRequestException))
            {
                code = HttpStatusCode.OK;
                RC = ResponseCodes.Failed;
                message = BusinessException.GetDescription(RC);
                details = ((HttpRequestException)exception).Message;
            }
            else if (exception.GetType() == typeof(AuthenticationException))
            {
                code = HttpStatusCode.OK;
                RC = ResponseCodes.Unauthorized;
                message = BusinessException.GetDescription(RC);
                details = ((AuthenticationException)exception).Message;
            }
            else if (exception.GetType() == typeof(BusinessException))
            {
                var businesException = (BusinessException)exception;
                message = businesException.Message;
                code = HttpStatusCode.OK;

                if (string.IsNullOrEmpty(businesException.RC))
                    RC = ResponseCodes.Failed;
                else
                    RC = businesException.RC;
                details = ((BusinessException)exception).Details;
            }
            else if (exception.GetType() == typeof(Exception))
            {
                code = HttpStatusCode.OK;
                RC = ResponseCodes.BadRequest;
                message = BusinessException.GetDescription(RC);
                details = details = ((Exception)exception).Message;
            }

            var response = new ErrorDTO
            {
                message = message,
                rc = RC,
                details = details,
                trackId = Guid.NewGuid().ToString()
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return bodyAsText;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}