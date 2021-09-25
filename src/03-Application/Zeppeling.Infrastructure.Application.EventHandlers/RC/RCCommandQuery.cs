using Zeppeling.Framework.Abstactions.Response;
using System.Threading.Tasks;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using Zeppeling.Framework.Abstactions.Request;

namespace Zeppeling.Infrastructure.Application.Commands
{
    public partial class ApplicationCommandQuery
    {
        /// <summary>
        /// Inserts the rc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> InsertRC(RequestDTO<InsertIRCRequestDTO> request) => await this.mediator.Send(request.RequestBody.Data);

        /// <summary>
        /// Gets the rc by identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ResponseDTO<GetRCByIdResponseDTO>> GetRCById(RequestDTO<GetRCByIdRequestDTO> request) => await this.mediator.Send(request.RequestBody.Data);
    }
}
