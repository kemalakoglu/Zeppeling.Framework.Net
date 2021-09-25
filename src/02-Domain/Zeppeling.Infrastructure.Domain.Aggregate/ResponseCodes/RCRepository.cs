using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Framework.Core;
using Zeppeling.Framework.Core.Abstraction;
using Zeppeling.Infrastructure.Core.Response.CreateResponse;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using AutoMapper;

namespace Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes
{
    public class RCRepository : Repository<RC, long>, IRCRepository
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMapper mapper;
        public RCRepository(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets the rc by identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<RC> GetRCById(GetRCByIdRequestDTO request)
        {
            return await base.GetByKey(request.Id);
        }

        /// <summary>
        /// Inserts the rc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> InsertRC(InsertIRCRequestDTO request)
        {
            RC entity = new RC();
            entity.Insert(request.ResponseCode, request.Name, request.Description, request.MessageEN, request.MessageTR);
            entity.Activate();
            base.Insert(entity);
            return await CreateResponse.Return(true);
        }
    }
}
