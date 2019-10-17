// OWNER MK, 23-11-2009
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoAssembler<T, D>
    {
        #region Methods
        bool IsValid(T dto);
        D Create(T dto);
        D Update(T dto);
        T ConvertToDto(D domain);
        D ConvertToDomain(T dto);
        #endregion

        #region Properties
        System.Collections.Generic.Dictionary<string, string> Errors { get; }
        #endregion
    }
    #endregion
}