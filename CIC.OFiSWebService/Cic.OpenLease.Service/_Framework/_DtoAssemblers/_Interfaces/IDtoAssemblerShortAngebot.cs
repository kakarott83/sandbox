// OWNER WB, 10-03-2019
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoAssemblerShortAngebot<T, D, A1, A2, A3, A4, A5, A6>
    {
        #region Methods
        bool IsValid(T dto);
        D Create(T dto);
        D Update(T dto);
        T ConvertToDto(D domain, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6);
        D ConvertToDomain(T dto, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, DdOlExtended context);
        #endregion

        #region Properties
        System.Collections.Generic.Dictionary<string, string> Errors { get; }
        #endregion
    }
    #endregion
}