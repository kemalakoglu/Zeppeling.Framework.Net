using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Abstactions.Response;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using Zeppeling.Infrastructure.Application.Contract.Events;
using MediatR;

namespace Zeppeling.Infrastructure.Application.Commands
{
    public partial class ApplicationCommandQuery: IApplicationCommandQuery
    {
        private readonly IMediator mediator;

        public ApplicationCommandQuery(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
