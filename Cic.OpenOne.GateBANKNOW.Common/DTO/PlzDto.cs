namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Postleitzahldto mit Zusatzdaten wie Land Kanton und Ort
    /// </summary>
    public class PlzDto
    {
        private string _land;

        /// <summary>
        /// Postleitzahl
        /// </summary>
        public string plz { get; set; }

        /// <summary>
        /// Kanton
        /// </summary>
        public string bezirk { get; set; }

        /// <summary>
        /// Ort
        /// </summary>
        public string ort { get; set; }

        /// <summary>
        /// Land
        /// </summary>
        public string land
        {
            get { return _land; }
            set
            {
                _land = value;
                _land = _land[0] + _land.Substring(1).ToLower();
            }
        }

        /// <summary>
        /// sysland
        /// </summary>
        public long sysland
        {
            get;
            set;
        }
    }
}