// OWNER WB, 10-03-2019
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoAssemblerAntrag<T, D, A1, A2, A3, K>
    {
        #region Methods
        bool IsValid(T dto);
        void Create(T dto);
        void Update(T dto);
        T ConvertToDto(D domain, A1 a1, A2 a2, A3 a3, K kdtyp, DdOlExtended context);
        D ConvertToDomain(T dto, A1 a1, A2 a2, A3 a3);
        #endregion

        #region Properties
        System.Collections.Generic.Dictionary<string, string> Errors { get; }
        #endregion
    }
    #endregion
}