using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class FileattDto : EntityDto
    {

        /*Primärschlüssel */
        public long sysFileAtt { get; set; }
        /*Verweis zur E-Mail */
        public long sysMailMsg { get; set; }
        /*Verweis zum Termin */
        public long sysApptmt { get; set; }
        /*Verweis zur Aufgabe */
        public long sysPtask { get; set; }

        public long? sysId { get; set; }

        public string area { get; set; }

        /*Exchange ID (Attachment) */
        public String attId { get; set; }
        /*Exchange ID (Content) */
        public String contId { get; set; }
        /*Name der angehängten Datei */
        public String fileName { get; set; }
        /*Ort der angehängten Datei */
        public String fileLocation { get; set; }
        /*Grösse der angehängten Datei */
        public long fileSize { get; set; }
        /*Inhalt */
        public byte[] content { get; set; }

        /// <summary>
        /// Active-Flag
        /// </summary>
        public int activeflag { get; set; }

        public String typCode { get; set; }

        override public long getEntityId()
        {
            return sysFileAtt;
        }
    }
}