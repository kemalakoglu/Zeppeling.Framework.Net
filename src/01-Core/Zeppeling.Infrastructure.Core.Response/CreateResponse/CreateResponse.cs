using Zeppeling.Framework.Abstactions.Response;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Zeppeling.Infrastructure.Core.Response.CreateResponse
{
    public static class CreateResponse
    {

        /// <summary>
        /// Returns the specified entity.
        /// </summary>
        /// <param name="entity">if set to <c>true</c> [entity].</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public static async Task<ResponseDTO> Return(bool entity)
        {

            string message = string.Empty;
            if (entity == true)
                message = GetResponseCode.GetDescription(ResponseCodes.Success);
            else
                message = GetResponseCode.GetDescription(ResponseCodes.Failed);
            ResponseDTO response = new ResponseDTO
            {
                Data = entity,
                Message = message,
                Information = new Information
                {
                    ResponseDate = DateTime.Now,
                    TrackId = Guid.NewGuid().ToString()
                },
                RC = entity == false ? ResponseCodes.Failed : ResponseCodes.Success
            };
            Log.Write(LogEventLevel.Information, message, response);
            return response;
        }
    }
}
