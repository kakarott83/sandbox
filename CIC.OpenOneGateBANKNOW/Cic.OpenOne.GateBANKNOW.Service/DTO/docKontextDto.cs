
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für docKontextDto
    /// </summary>
    public class docKontextDto
    {
        /// <summary>
        /// Primary Key of the item to print
        /// </summary>
        public long sysID { get; set; }

        /// <summary>
        /// Type of the item to print (corresponding to the table where to look for sysID)
        /// </summary>
        public DocumentArea area { get; set; }

        /// <summary>
        /// Optional type of a document subset for the area
        /// </summary>
        public DocumentSubArea subarea { get; set; }
    }
}