// OWNER WB, 17-03-2019
namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoAssemblerShortVT<T, D, A1, A2, A3, A4, A5>
    {
        #region Methods
        bool IsValid(T dto);
        D Create(T dto);
        D Update(T dto);
        T ConvertToDto(D domain, A1 a1, A2 a2, A3 a3, A4 a4, A5 a);
        D ConvertToDomain(T dto, A1 a1, A2 a2, A3 a3, A4 a4);
        #endregion

        #region Properties
        System.Collections.Generic.Dictionary<string, string> Errors { get; }
        #endregion
    }
    #endregion
}