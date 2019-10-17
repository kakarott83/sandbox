using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// IZekBo interface for ZEK Webservice methods EC1 - EC7
    /// </summary>
    public interface IZekBo
    {
        /// <summary>
        /// Interface method to execute ZEK EC1 Service and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto kreditgesuchNeu(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC2 Service and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto informativabfrage(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC3 Service and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto kreditgesuchAblehnen(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerBardarlehen(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerFestkredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerKontokorrentkredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerLeasingMietvertrag(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerTeilzahlungskredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto registerTeilzahlungsvertrag(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 additional Service (Saldomeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto meldungKartenengagement(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC4 additional Service (Saldomeldung) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto meldungUeberziehungskredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC6 Service (Mutation der Grunddaten) and save input and output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateAddress(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateBardarlehen(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateFestkredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateKontokorrentkredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateLeasingMietvertrag(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateTeilzahlungskredit(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and input and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto updateTeilzahlungsvertrag(ZekInDto inDto);
        /// <summary>
        /// Interface method to execute ZEK EC1 Service and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto kreditgesuchNeu(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC2 Service and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto informativabfrage(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC3 Service and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto kreditgesuchAblehnen(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto registerBardarlehen(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto registerFestkredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto registerKontokorrentkredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto registerLeasingMietvertrag(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto registerTeilzahlungskredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 main Service (Vertragsanmeldung) and saveoutput to database
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto registerTeilzahlungsvertrag(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 additional Service (Saldomeldung) and output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto meldungKartenengagement(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC4 additional Service (Saldomeldung) and output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto meldungUeberziehungskredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC6 Service (Mutation der Grunddaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateAddress(long sysAuskunft);
        /// <summary>
        ///  Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateBardarlehen(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateFestkredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateKontokorrentkredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateLeasingMietvertrag(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto updateTeilzahlungskredit(long sysAuskunft);
        /// <summary>
        /// Interface method to execute ZEK EC7 Service (Mutation der Vertragsdaten) and save output to database
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto updateTeilzahlungsvertrag(long sysAuskunft);
        /// <summary>
        /// closeBardarlehen
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeBardarlehen(ZekInDto inDto);

        /// <summary>
        /// closeBardarlehen
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeBardarlehen(long sysAuskunft);

        /// <summary>
        /// closeLeasingMietvertrag
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeLeasingMietvertrag(ZekInDto inDto);

        /// <summary>
        /// closeLeasingMietvertrag
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeLeasingMietvertrag(long sysAuskunft);

        /// <summary>
        /// closeFestkredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeFestkredit(ZekInDto inDto);

        /// <summary>
        /// closeFestkredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeFestkredit(long sysAuskunft);

        /// <summary>
        /// closeTeilzahlungskredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeTeilzahlungskredit(ZekInDto inDto);

        /// <summary>
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        AuskunftDto closeTeilzahlungsvertrag(ZekInDto inDto);

        /// <summary>
        /// closeTeilzahlungskredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeTeilzahlungskredit(long sysAuskunft);

        /// <summary>
        /// closeTeilzahlungsvertrag
        /// /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto closeTeilzahlungsvertrag(long sysAuskunft);

        /// <summary>
        /// closeKontokorrentkredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto closeKontokorrentkredit(ZekInDto inDto);

        /// <summary>
        /// closeKontokorrentkredit
        /// Interface method to execute ZEK EC5 Service (Vertragsabmeldung) and save output to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto closeKontokorrentkredit(long sysAuskunft);

        /// <summary>
        /// eCode178Anmelden
        /// Interface method to execute eCode178 anmelden
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        AuskunftDto eCode178Anmelden(ZekInDto inDto);

        /// <summary>
        /// eCode178Anmelden
        /// Interface method to execute eCode178 anmelden
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto eCode178Anmelden(long sysAuskunft);

        /// <summary>
        /// eCode178Mutieren
        /// Interface method to execute eCode178Mutieren
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        AuskunftDto eCode178Mutieren(ZekInDto inDto);

        /// <summary>
        /// eCode178Mutieren
        /// Interface method to execute eCode178Mutieren
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto eCode178Mutieren(long sysAuskunft);

        /// <summary>
        ///  eCode178Abmelden
        /// Interface method to execute eCode178Abmelden
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto eCode178Abmelden(ZekInDto inDto);

        /// <summary>
        /// eCode178Abmelden
        /// Interface method to execute eCode178Abmelden
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto eCode178Abmelden(long sysAuskunft);

        /// <summary>
        /// eCode178Abfrage
        /// Interface method to execute eCode178Abfrage
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto eCode178Abfrage(ZekInDto inDto);

        /// <summary>
        /// eCode178Abfrage
        /// Interface method to execute eCode178Abfrage
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns>sysAuskunft</returns>
        AuskunftDto eCode178Abfrage(long sysAuskunft);

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="inDto">inDto</param>
        /// <returns></returns>
        AuskunftDto getARMs(ZekInDto inDto);

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        AuskunftDto getARMs(long sysAuskunft);

        /// <summary>
        /// informativabfrageLogDump
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="area"></param>
        /// <param name="sysAreaid"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        AuskunftDto informativabfrageLogDump(ZekInDto inDto, string area, long sysAreaid, long syswfuser);

        /// <summary>
        /// informativabfrageOL
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftOLDto informativabfrageOL(AuskunftOLDto inDto);
    }
}