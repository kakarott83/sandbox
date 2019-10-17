using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für ParamDto
    /// </summary>
    public class ParamDto
    {
        /// <summary>
        /// ID des Parameters
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
        
        /// <summary>
        /// Bezeichnung Metaobjekt
        /// </summary>
        public string meta
        {
            get;
            set;
        }
        
        /// <summary>
        /// Bezeichnung des Parameters
        /// </summary>
        public string name
        {
            get;
            set;
        }
        
        /// <summary>
        /// Parameter ist sichtbar
        /// </summary>
        public bool visible
        {
            get;
            set;
        }
        
        /// <summary>
        /// Parameter ist nicht schaltbar
        /// </summary>
        public bool disabled
        {
            get;
            set;
        }
        
        /// <summary>
        /// 0=Numerisch / 1=Prozent
        /// </summary>
        public short type
        {
            get;
            set;
        }
        
        /// <summary>
        /// Numerischer Minimalwert bei Typ = 1
        /// </summary>
        public double minvaln
        {
            get;
            set;
        }
        
        /// <summary>
        /// Numerischer Maximalwert bei Typ = 1
        /// </summary>
        public double maxvaln
        {
            get;
            set;
        }
        
        /// <summary>
        /// Numerischer Defaultwert bei Typ = 1
        /// </summary>
        public double defvaln
        {
            get;
            set;
        }

        /// <summary>
        /// Prozentueller Minimalwert bei Typ = 2
        /// </summary>
        public double minvalp
        {
            get;
            set;
        }
        
        /// <summary>
        /// Prozentueller Maximalwert bei Typ = 2
        /// </summary>
        public double maxvalp
        {
            get;
            set;
        }
        
        /// <summary>
        /// Prozentueller Defaultwert bei Typ = 2
        /// </summary>
        public double defvalp
        {
            get;
            set;
        }
        
        /// <summary>
        /// Schrittlistenobjekt
        /// </summary>
        public SteplistDto[] steplist
        {
            get;
            set;
        }
    }
}
