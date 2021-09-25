using System.Threading;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using AutoMapper;
using MediatR;

namespace Zeppeling.Infrastructure.Domain.Handlers
{
    public class RCCommandHandler : IRequestHandler<InsertIRCRequestDTO, ResponseDTO>
    {
        private readonly IMapper mapper;
        private readonly IRCRepository rcRepository;
        public RCCommandHandler(IMapper mapper, IRCRepository rcRepository)
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
        public async Task<ResponseDTO> Handle(InsertIRCRequestDTO request, CancellationToken cancellationToken)
        {
            return await this.rcRepository.InsertRC(request);
        }
    }
}
