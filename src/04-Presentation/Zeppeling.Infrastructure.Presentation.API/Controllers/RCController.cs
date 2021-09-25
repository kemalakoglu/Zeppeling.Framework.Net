using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Request;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Infrastructure.Application.Contract.Events;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;

namespace Zeppeling.Infrastructure.Presentation.API.Controllers
{
    public class RCController : Controller
    {
        private readonly IApplicationCommandQuery applicationCommandQuery;
        private readonly IMapper mapper;
        public RCController(IApplicationCommandQuery applicationCommandQuery, IMapper mapper)
        {
            this.applicationCommandQuery = applicationCommandQuery;
            this.mapper = mapper;
        }

        /// <summary>
        /// Inserts the dealer.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("api/RC/Insert")]
        [HttpPost]
        public async Task<ResponseDTO> InsertRC([FromBody] RequestDTO<InsertIRCRequestDTO> request)
        {
            return await this.applicationCommandQuery.InsertRC(request);
        }

        /// <summary>
        /// Gets the rc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("api/RC/GetById")]
        [HttpPost]
        public async Task<ResponseDTO<GetRCByIdResponseDTO>> GetRC([FromBody] RequestDTO<GetRCByIdRequestDTO> request)
        {
            return await this.applicationCommandQuery.GetRCById(request);
        }
    }
}
