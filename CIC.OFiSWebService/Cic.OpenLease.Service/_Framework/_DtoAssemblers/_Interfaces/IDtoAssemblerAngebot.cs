// OWNER WB, 10-03-2019
namespace Cic.OpenLease.Service
{
    #region Methods
    [System.CLSCompliant(true)]
    public interface IDtoAssemblerAngebot<T, D, A1, A2, A3, A4, A5, A6,A7>
    {
        #region Methods
        bool IsValid(T dto);
        void Create(T dto, out D domain, out A1 a1, out A2 a2, out A3 a3, out A4 a4, out A6 a6, out A7 a7);
        void Update(T dto, out D domain, out A1 a1, out A2 a2, out A3 a3, out A4 a4, out A6 a6, out A7 a7);
        T ConvertToDto(D domain, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7);
        D ConvertToDomain(T dto, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7);
        #endregion

        #region Properties
        System.Collections.Generic.Dictionary<string, string> Errors { get; }
        #endregion
    }
    #endregion
}