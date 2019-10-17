using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Parameterklasse für DropListDto
    /// </summary>
    /// <remarks>
    /// Alle einfachen Listen leiten ihre Outputobjekte aus dem allgemeinen DropListDto ab. Das Gebiet ergibt sich aus der konrekt verwendeten Unterklasse.
    /// </remarks>
    public class DropListDto
    {

        /// <summary>
        /// ID des Eintrags aus dem Gebiet
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Code for grouping the product to different GUI's
        /// </summary>
        public String code
        {
            get;
            set;
        }

        /// <summary>
        /// Bezeichung des Eintrags aus Gebiet
        /// </summary>
        public string bezeichnung
        {
            get;
            set;
        }

        /// <summary>
        /// Beschreibung des Eintrags aus Gebiet
        /// </summary>
        public string beschreibung
        {
            get;
            set;
        }
    }
}
