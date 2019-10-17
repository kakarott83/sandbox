namespace Cic.One.DTO
{
    /// <summary>
    /// Eingabeparameter für das Finden von Kontakten
    /// </summary>
    public class ifindContact
    {
        /// <summary>
        /// Teil des Namens, welcher gesucht werden soll
        /// </summary>
        public string NameToResolve { get; set; }

        /// <summary>
        /// Legt fest wo gesucht werden soll
        /// </summary>
        public MResolveNameSearchLocationEnum ResolveNameSearchLocation { get; set; }

        /// <summary>
        /// Legt fest, ob die vollen Kontakt-Details zurückgeliefert werden sollen
        /// </summary>
        public bool ReturnContactDetails { get; set; }
    }
}