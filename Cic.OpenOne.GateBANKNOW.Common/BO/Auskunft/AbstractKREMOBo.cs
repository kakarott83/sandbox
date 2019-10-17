using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstarct Class: KREMO Business Object
    /// </summary>
    public abstract class AbstractKREMOBo : IKREMOBo
    {
        /// <summary>
        /// KREMO Web Service Data Access Object
        /// </summary>
        protected IKREMOWSDao kremoWSDao;

        /// <summary>
        /// KREMO DB Data Access Object
        /// </summary>
        protected IKREMODBDao kremoDBDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kremoWSDao">KREMO Web Service Data Access Object</param>
        /// <param name="kremoDBDao">KREMO DB Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public AbstractKREMOBo(IKREMOWSDao kremoWSDao, IKREMODBDao kremoDBDao, IAuskunftDao auskunftDao)
        {
            this.kremoWSDao = kremoWSDao;
            this.kremoDBDao = kremoDBDao;
            this.auskunftDao = auskunftDao;            
        }

        /// <summary>
        /// abstract method to fill input values for KREMO Webservice by InDto and call KREMOWSDao method CallKremoByValues()  
        /// </summary>
        /// <param name="InDto">Input Data</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto callByValues(KREMOInDto InDto);

        /// <summary>
        /// abstract method to fill input values for KREMO Webservice by Information ID and call KREMOWSDao method CallKremoByID()  
        /// </summary>
        /// <param name="sysAuskunft">information ID</param>
        /// <returns>Information Data</returns>
        public abstract AuskunftDto callByValues(long sysAuskunft);

        /// <summary>
        /// abstract method to call KREMOWSDao method getVersion() 
        /// </summary>
        /// <param name="InDto">Input Data</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract KREMOOutDto getVersion(KREMOInDto InDto);

        /// <summary>
        /// abstract method to call KREMOWSDao method getVersion()
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Information Data</returns>
        public abstract AuskunftDto getVersion(long sysAuskunft);

        /// <summary>
        /// gets the available budget for the given data
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract double getBudget(KREMOInDto inDto);

        /// <summary>
        /// gets the Kremo values for the input 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        abstract
        public KREMOOutDto getKremoValues(KREMOInDto inDto);

        /// <summary>
        /// Gets the budget-Value from the KREMO result
        /// </summary>
        /// <param name="outDto"></param>
        /// <returns></returns>
        abstract
        public double getBudgetValue(KREMOOutDto outDto);

        /// <summary>
        /// Calculates the Budgetüberschuss for Budgetcalculator 
        /// considers AS1 and AS2 
        /// uses Ruleenine "ANTRAGSASSISTENT", "RULEENGINE_BUDGET", "USE_RULESET_B2B"
        /// calls Kremo-Interface before calling ruleengine
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="budget1"></param>
        /// <param name="budget2"></param>
        /// <returns></returns>
        abstract public DTO.ogetKremoBudget getKremoBudget(long syswfuser, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget1, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget2, long sysprproduct);
    }
    
}
