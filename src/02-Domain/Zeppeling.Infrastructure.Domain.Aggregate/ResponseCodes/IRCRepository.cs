using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Framework.Core.Repository;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;

namespace Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes
{
    public interface IRCRepository: IRepository<RC,long>
    {
        /// <summary>
        /// Inserts the rc.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ResponseDTO> InsertRC(InsertIRCRequestDTO request);

        /// <summary>
        /// Gets the rc by identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<RC> GetRCById(GetRCByIdRequestDTO request);
    }
}
