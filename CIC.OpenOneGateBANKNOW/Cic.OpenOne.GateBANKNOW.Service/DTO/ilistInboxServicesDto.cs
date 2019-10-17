namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter
    /// </summary>
    public class ilistInboxServicesDto
    {
        /// <summary>
        /// Dient zum Umschalten zwischen Vertragslisten und Eventualverbindlichkeiten
        /// Mögliche Werte : EvVb (Eventualverbindlichkeiten) und lfVT (laufende Verträge)
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Die sysId vom Vertriebspartner
        /// </summary>
        public long sysPerson { get; set; }
    }
}