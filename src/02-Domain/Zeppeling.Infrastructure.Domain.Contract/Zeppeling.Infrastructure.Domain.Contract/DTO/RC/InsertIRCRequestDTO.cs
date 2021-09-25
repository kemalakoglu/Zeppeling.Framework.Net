using Zeppeling.Framework.Abstactions.Base;
using Zeppeling.Framework.Abstactions.Response;
using MediatR;
using System;

namespace Zeppeling.Infrastructure.Domain.Contract.DTO.RC
{
    [Serializable]
    public class InsertIRCRequestDTO: IRequest<ResponseDTO>
    {
        public string ResponseCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MessageEN { get; set; }
        public string MessageTR { get; set; }
    }
}
