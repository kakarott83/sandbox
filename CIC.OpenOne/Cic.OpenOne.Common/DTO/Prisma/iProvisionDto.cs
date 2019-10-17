using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    

    /// <summary>
    /// Holds all input  values to calculate a provision
    /// </summary>
    public class iProvisionDto
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
        public long sysprfld
        {
            get;
            set;
        }

        /// <summary>
        /// reference to UST
        /// </summary>
        public long sysmwst
        {
            get;
            set;
        }

        /// <summary>
        /// The value corresponding to prparam
        /// </summary>
        public double provisionInputValue
        {
            get;
            set;
        }

        /// <summary>
        /// The X-input for a vg step into the vg table
        /// </summary>
        public String vgXValue
        {
            get;
            set;
        }
        /// <summary>
        /// The Y-input for a vg step into the vg table
        /// </summary>
        public String vgYValue
        {
            get;
            set;
        }
        private VGInterpolationMode _interpolationMode = VGInterpolationMode.LINEAR;
        public VGInterpolationMode interpolationMode
        {
            get { return _interpolationMode; }
            set {_interpolationMode = value;}
        }
        private plSQLVersion _vgQueryMode = plSQLVersion.V2;
        public plSQLVersion vgQueryMode
        {
            get { return _vgQueryMode; }
            set { _vgQueryMode = value; }
        }
        /// <summary>
        /// Overwrites the value from the Provisionsteps
        /// </summary>
        public double provisionOverwriteValue
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if to use the overwrite value
        /// </summary>
        public bool useProvisionOverwriteValue
        {
            get;
            set;
        }
        /// <summary>
        /// The selected provision tarif, optional
        /// </summary>
        public long sysProvTarif
        {
            get;
            set;
        }
        /// <summary>
        /// Default Distribution Role Type - if set everything goes to this person type
        /// </summary>
        public int defRoleType
        {
            get;
            set;
        }

        /// <summary>
        /// User input for provision value, optional
        /// </summary>
        public double Wunschprovision
        {
            get;
            set;
        }


        /// <summary>
        /// Instructs the calculator to calculate a zero Provision
        /// May be used in conjunction with customer specific logic (e.g. internal users always have zero provision when condition xy is met)
        /// optional
        /// </summary>
        public bool setZeroFlag
        {
            get;
            set;
        }

        /// <summary>
        /// optional additional inputvalues needed for some provision calculations
        /// </summary>
        public ProvisionCalcComponent[] additionalInputValues { get; set; }
            

    }
}
