using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.Web.BO
{
    public class EnityComparator<T> : IEqualityComparer<T> where T : EntityDto
    {
        public bool Equals(T p1, T p2)
        {
            return p1.entityId == p2.entityId;
        }

        public int GetHashCode(T p)
        {
            return (int)p.entityId;
        }
    }
}
