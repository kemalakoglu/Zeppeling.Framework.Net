using Zeppeling.Framework.Abstactions.Response;
using System.Threading.Tasks;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using Zeppeling.Framework.Abstactions.Request;

namespace Zeppeling.Infrastructure.Application.Contract.Events
{
    public interface IApplicationCommandQuery
    {
        Task<ResponseDTO> InsertRC(RequestDTO<InsertIRCRequestDTO> request);
        Task<ResponseDTO<GetRCByIdResponseDTO>> GetRCById(RequestDTO<GetRCByIdRequestDTO> request);
    }
}
