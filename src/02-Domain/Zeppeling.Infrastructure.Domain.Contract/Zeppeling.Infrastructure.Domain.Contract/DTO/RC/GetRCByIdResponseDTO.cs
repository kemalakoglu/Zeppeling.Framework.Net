using System;

namespace Zeppeling.Infrastructure.Domain.Contract.DTO.RC
{
    public class GetRCByIdResponseDTO
    {
        public long Id { get; set; }
        public string ResponseCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MessageEN { get; set; }
        public string MessageTR { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}