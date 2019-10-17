using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoMapper<T, D>
    {
        #region Methods
        void mapToDto(T dto, D domain);
        void mapFromDto(T dto, D domain);
        #endregion

    }
    #endregion
}
