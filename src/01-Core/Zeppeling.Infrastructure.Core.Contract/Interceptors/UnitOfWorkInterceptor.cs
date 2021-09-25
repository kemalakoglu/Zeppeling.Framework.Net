using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zeppeling.Framework.Core.Abstraction;
using Zeppeling.Infrastructure.Core.Response;
using Zeppeling.Infrastructure.Core.Response.BusinessException;

namespace Zeppeling.Infrastructure.Core.Contract.Interceptors
{
   
    public class UnitOfWorkInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UnitOfWorkInterceptor(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            //Before method execution
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted;
            bool entityLazyLoad = true;

            using (IUnitOfWork uow =
                this.unitOfWorkFactory.Create(ContextKeys.DomainContext, isoLevel, entityLazyLoad))
            {
                try
                {
                    uow.Begin(false);

                    //Executing the actual method
                    invocation.Proceed();

                    //After method execution
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw new BusinessException(ResponseCodes.Failed, ex.Message);
                }
            }
        }
    }

}
