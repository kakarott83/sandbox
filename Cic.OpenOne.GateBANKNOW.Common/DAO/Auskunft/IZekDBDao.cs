using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEK Data Access Object Interface
    /// </summary>
    public interface IZekDBDao
    {
        /// <summary>
        /// Gets Username and Password
        /// </summary>
        /// <returns>IdentityDescriptor</returns>
        DAO.Auskunft.ZEKRef.IdentityDescriptor GetIdentityDescriptor();

        /// <summary>
        /// Saves KreditgesuchNeuInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveKreditgesuchNeuInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves InformativanfrageInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveInformativanfrageInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves KreditgesuchAblehnenInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveKreditgesuchAblehnenInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves VertragsanmeldungInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveVertragsanmeldungInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves KreditgesuchNeuOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveKreditgesuchNeuOutput(long sysAuskunft, ZekOutDto outDto);
        /// <summary>
        /// Saves InformativanfrageOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveInformativanfrageOutput(long sysAuskunft, ZekOutDto outDto);
        /// <summary>
        /// Saves InformativanfrabeBemerkung / Loggin BNR11 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="bemerkung"></param>
        void SaveBemerkungInformativabfrage(long sysAuskunft, string bemerkung, string vertragnummer, string antragnummer, string vpnummer);
        /// <summary>
        /// Saves KreditgesuchAblehnenOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveKreditgesuchAblehnenOutput(long sysAuskunft, ZekOutDto outDto);
        /// <summary>
        /// Saves VertragsanmeldungOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveVertragsanmeldungOutput(long sysAuskunft, ZekOutDto outDto);
        /// <summary>
        /// Saves Addressänderung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveUpdateAddressInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves Addressänderung Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveUpdateAddressOutput(long sysAuskunft, ZekOutDto outDto);
        /// <summary>
        /// Saves Vertragsänderung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveUpdateVertragInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves Vertragsänderung Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveUpdateVertragOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// Saves Vertrags-Abmeldung Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveVertragsabmeldungInput(long sysAuskunft, ZekInDto inDto);
        /// <summary>
        /// Saves Vertragsabmeldung Output
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        void SaveVertragsabmeldungOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// SaveeCode178AnmeldenInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">inDto</param>
        void SaveeCode178AnmeldenInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// SaveeCode178AnmeldenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        void SaveeCode178AnmeldenOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// SaveeCode178MutierenInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        void SaveeCode178MutierenInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// SaveeCode178MutierenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        void SaveeCode178MutierenOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// SaveeCode178AbmeldenInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        void SaveeCode178AbmeldenInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// SaveeCode178AbmeldenOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        void SaveeCode178AbmeldenOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// SaveeCode178AbfrageInput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="inDto">inDto</param>
        void SaveeCode178AbfrageInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// SaveeCode178AbfrageOutput
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <param name="outDto">outDto</param>
        void SaveeCode178AbfrageOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// SaveGetARMsInput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        void SaveGetARMsInput(long sysAuskunft, ZekInDto inDto);

        /// <summary>
        /// SaveGetARMsOutput
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        void SaveGetARMsOutput(long sysAuskunft, ZekOutDto outDto);

        /// <summary>
        /// Finds ZEK Input data by sysAuskunft and maps it to ZekInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>ZekInDto, filled with input for ZEK WS</returns>
        ZekInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// getDBInformativanfrage
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        ZekOutDto getDBInformativanfrageOutput(long sysAuskunft);

    }
}