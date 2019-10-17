using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.DTO.Prisma
{
  

    /// <summary>
    /// Holds all input and output values to calculate a provision
    /// </summary>
    public class oProvisionDto
    {
        /// <summary>
        /// Type of Provision
        /// </summary>
        public long sysprprovtype
        {
            get;
            set;
        }

        /// <summary>
        /// Type of provision (VP,HG or both)
        /// </summary>
        public ProvGroupAssignType provType
        {
            get;
            set;
        }

        /// <summary>
        /// Prisma Field Information, connected to the field the provision is calced for
        /// </summary>
        public ParamDto prparam
        {
            get;
            set;
        }

        /// <summary>
        /// The value corresponding to prparam
        /// </summary>
        public double provisionOutputValue
        {
            get;
            set;
        }


        //old:

        /// <summary>
        /// The selected provision, may change during calculation!, optional
        /// </summary>
        public long sysProvTarif
        {
            get;
            set;
        }


        /// <summary>
        /// Ergebnis-Provision
        /// </summary>
        public double provision
        {
            get;
            set;
        }

        /// <summary>
        /// Zusatzergebnis bei Abschlussprovision
        /// </summary>
        public double aufschlag
        {
            get;
            set;
        }

        /// <summary>
        /// difference between default and selected tarif
        /// </summary>
        public double deltaStandard
        {
            get;
            set;
        }

        /// <summary>
        /// Zusatzergebnis bei Berechnung Abschlussprovision
        /// </summary>
        public double zugangsProvision
        {
            get;
            set;
        }


    }
}
