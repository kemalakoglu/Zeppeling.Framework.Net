using System;
using System.Collections.Generic;
using System.Text;
using Zeppeling.Framework.Entity;

namespace Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes
{
    public class RC: EntityBase<long>
    {
        public string ResponseCode { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string MessageEN { get; protected set; }
        public string MessageTR { get; protected set; }


        public void Update(string responseCode, string name, string description, string messageEN, string messageTR)
        {
            this.ResponseCode = responseCode;
            this.Name = name;
            this.Description = description;
            this.MessageEN = messageEN;
            this.MessageTR = messageTR;
            base.Update();
        }

        public void Insert(string responseCode, string name, string description, string messageEN, string messageTR)
        {
            this.ResponseCode = responseCode;
            this.Name = name;
            this.Description = description;
            this.MessageEN = messageEN;
            this.MessageTR = messageTR;
            base.Activate();
        }
    }
}
