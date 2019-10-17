using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateOEM.Service.Contract;
using Cic.OpenOne.GateOEM.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateOEM.Service
{
    /// <summary>
    /// Endpoint for HCE VAP Frontend Interfaces 
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateOEM")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class oemService : IoemService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Processes an invoice from external provider
        /// </summary>
        /// <param name="sXmlInputDataSet"></param>
        /// <returns></returns>
        public string sendInvoice(string sXmlInputDataSet)
        {
            
            ServiceHandler<string, oBaseDto> ew = new ServiceHandler<string, oBaseDto>(sXmlInputDataSet);
            oBaseDto ret= ew.process(delegate(string inputdata, oBaseDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");

                new InvoiceBo().processInvoice(inputdata);

                rval.success();
            },true);

            return InvoiceBo.getErrorObject(ret);
        }

        /// <summary>
        /// returns a list of saldo information for all dealers
        /// </summary>
        /// <returns></returns>
        public ogetSaldoListDto getSaldolist()
        {
            ogetSaldoListDto rval = new ogetSaldoListDto();
            rval.salden = new List<SaldoInfo>();
            SaldoInfo si = new SaldoInfo();
            si.changeDate = DateTime.Now;
            si.conditionKey = "X";
            si.createDate = DateTime.Now;
            si.dealerNr = "1000000000";
            si.fznr = "XSHA39202093AA";
            si.brief = "xxxyyy";
            si.invoiceAmount = 39039.33M;
            si.invoiceDate = DateTime.Now;
            si.invoiceNr = "93932";
            si.saldo_current = 587864.22M;
            si.saldo_debit = 3829393.33M;
            rval.salden.Add(si);
            si = new SaldoInfo();
            si.changeDate = DateTime.Now;
            si.conditionKey = "X";
            si.createDate = DateTime.Now;
            si.dealerNr = "1000030000";
            si.brief = "xxxyyy";
            si.fznr = "XSHA39233093AA";
            si.invoiceAmount = 34221.33M;
            si.invoiceDate = DateTime.Now;
            si.invoiceNr = "2354";
            si.saldo_current = 5864.22M;
            si.saldo_debit = 329393.33M;
            rval.salden.Add(si);
            si = new SaldoInfo();
            si.changeDate = DateTime.Now;
            si.conditionKey = "Z";
            si.createDate = DateTime.Now;
            si.dealerNr = "1000400000";
            si.fznr = "XXHA39202093AA";
            si.invoiceAmount = 19039.33M;
            si.invoiceDate = DateTime.Now;
            si.invoiceNr = "66";
            si.saldo_current = 587864.22M;
            si.saldo_debit = 29393.33M;
            rval.salden.Add(si);

            rval.success();
            return rval;
        }

        /// <summary>
        /// returns the data for a credit line report of all active credit lines per boundary contract
        /// </summary>
        /// <returns></returns>
        public ogetCreditLimitDto getCreditLimits()
        {
            ogetCreditLimitDto rval = new ogetCreditLimitDto();
            rval.creditLimits = new List<CreditLimit>();
            CreditLimit cl = new CreditLimit();
            cl.dealerNr = "1000000000";
            cl.perDate = DateTime.Now;
            cl.saldo = 3829393.24M;
            cl.dealerName = "Testhändler 1";
            cl.divisionCode = 10;
            cl.distributionChannel = 10;
            rval.creditLimits.Add(cl);
            cl = new CreditLimit();
            cl.dealerNr = "1000003000";
            cl.perDate = DateTime.Now;
            cl.saldo = 234443.43M;
            cl.dealerName = "Testhändler 2";
            cl.divisionCode = 10;
            cl.distributionChannel = 30;
            rval.creditLimits.Add(cl);
            cl = new CreditLimit();
            cl.dealerNr = "1000010000";
            cl.perDate = DateTime.Now;
            cl.saldo = 4493.99M;
            cl.dealerName = "Testhändler 3";
            cl.divisionCode = 30;
            cl.distributionChannel = 10;
            rval.creditLimits.Add(cl);
            rval.success();
            return rval;
        }
    }
}
