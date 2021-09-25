using System.Threading;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using AutoMapper;
using MediatR;
using Zeppeling.Infrastructure.Core.Response.CreateResponse;

namespace Zeppeling.Infrastructure.Domain.Handlers
{
    public class RCQueryHandler : IRequestHandler<GetRCByIdRequestDTO, ResponseDTO<GetRCByIdResponseDTO>>
    {
        private readonly IMapper mapper;
        private readonly IRCRepository rcRepository;
        public RCQueryHandler(IMapper mapper, IRCRepository rcRepository)
        {
            this.mapper = mapper;
            this.rcRepository = rcRepository;
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<ResponseDTO<GetRCByIdResponseDTO>> Handle(GetRCByIdRequestDTO request, CancellationToken cancellationToken)
        {
            RC entity = await this.rcRepository.GetRCById(request);
            return await CreateResponseWithData<GetRCByIdResponseDTO>.Return(mapper.Map(entity, new GetRCByIdResponseDTO()));
        }
    }
}
