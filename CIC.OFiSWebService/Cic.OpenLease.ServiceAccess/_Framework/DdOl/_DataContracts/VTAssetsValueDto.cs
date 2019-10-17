// OWNER MK, 15-09-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Verträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class VTAssetsValueDto
    {
        /// <summary>
        /// PDF Dokument
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public byte[] PdfDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Buchwert Datum
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? Date
        {
            get;
            set;
        }

        /// <summary>
        /// Buchwert
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public double? Value
        {
            get;
            set;
        }
    }
}
