using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;

namespace Zeppeling.Infrastructure.Domain.Contract.DTO.RC
{
    public class GetRCByIdRequestDTO : IRequest<ResponseDTO<GetRCByIdResponseDTO>>
    {
        public GetRCByIdRequestDTO(long id)
        {
            this.Id = id;
        }
        public long Id { get; set; }
    }
}
