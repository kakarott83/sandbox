using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public abstract class TreeDto : EntityDto
    {
        public int children { get; set; }
        public long nodeId 
        {
            get { return getNodeId(); }
            set { }
        }

        virtual public long getNodeId()
        {
            return getEntityId();
        }
    }
}