using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System.Reflection;
using System.Web;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Config;
using System.Security.Cryptography;
using AutoMapper;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander.Invoice;
using System.Globalization;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.Util;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Santander
{

    /// <summary>
    /// BO handling the Santander Invoice Interface
    /// </summary>
    public class InvoiceBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IEaihotDao dao;

        public InvoiceBo()
        {
            dao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao();
        }


        /// <summary>
        /// Processes invoice-data
        /// </summary>
        /// <param name="invoiceData"></param>
        /// <returns></returns>
        public void processInvoice(string invoiceData)
        {


            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(invoiceData);
            InvoiceNotification data = XMLDeserializer.objectFromXml<InvoiceNotification>(bytes, "UTF-8");

            
            
            EaihotDto eaihotInput = new EaihotDto();
            eaihotInput.CODE = "HEK_KMD_NC_INVOICE_FIN";
            eaihotInput.PROZESSSTATUS = 0;
            eaihotInput.HOSTCOMPUTER = "*";
            eaihotInput.EVE = 0;
            eaihotInput.OLTABLE = "SYSTEM";
            eaihotInput.SYSOLTABLE = 0;
            eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
            eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
            eaihotInput.INPUTPARAMETER2 = data.numberOfRecords;
            eaihotInput.INPUTPARAMETER3 = data.grossSumOfRecords;
            
            //eaihotInput.INPUTPARAMETER1 = data.I_DATUM;
            EAIART art = dao.getEaiArt("HEK_KMD_NC_INVOICE_FIN");
            if (art == null)
            {
                _log.Error("EAIART HEK_KMD_NC_INVOICE_FIN NOT FOUND, cannot proceed with hce invoice");
                throw new Exception("EAIART HEK_KMD_NC_INVOICE_FIN NOT FOUND");
            }
            eaihotInput.SYSEAIART = art.SYSEAIART;

           
            eaihotInput.INPUTPARAMETER1 = data.senderId;
            try
            {
                CredentialContext cctx = new CredentialContext();
                long wfuser = cctx.getMembershipInfo().sysWFUSER;
                eaihotInput.SYSWFUSER = wfuser;
            }
            catch (Exception) { }

            eaihotInput = dao.createEaihot(eaihotInput);
            try
            {
                EaihfileDto eaihfile = new EaihfileDto();
                eaihfile.SYSEAIHOT = eaihotInput.SYSEAIHOT;
                eaihfile.EAIHFILE = bytes;
                dao.createEaihfile(eaihfile);
            }catch(Exception exc)
            {
                _log.Error("Error writing Santander INVOICE DATA", exc);
            }

            foreach (InvoiceNotificationInvoiceList il in data.invoiceList)
            {
                List<EaiqinDto> queueQin = new List<EaiqinDto>();
                String kunnr = il.billToParty.dealerCode;

                //Invoice
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "INVTY", il.invoiceType,"invoiceType"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "INVNO", il.invoiceNo, "invoiceNo"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "INVDT", il.invoiceDate, "invoiceDate"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "INVNOO", il.oldInvoiceNo,"oldInvoiceNo"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "INVDTO", il.oldInvoiceDate,"oldInvoiceDate"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ORDTY", il.orderType,"orderType"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "VBELN", il.orderNo,"orderNo"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "GAODT", il.deliveryDate,"deliveryDate"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "VATCDS", il.sellerVATId,"sellerVATId"));
                
                //Dealer
                if (il.billToParty != null)
                {
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "KUNNR", il.billToParty.dealerCode, "dealerCode"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "VATCD", il.billToParty.dealerVATId, "dealerVATId"));
                    if (il.billToParty.address != null)
                    {
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "KUNN1", il.billToParty.address.dealerName1, "dealerName1"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "KUNN2", il.billToParty.address.dealerName2, "dealerName2"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ADDR1", il.billToParty.address.address1, "address1"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ADDR2", il.billToParty.address.address2, "address2"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "LANDX", il.billToParty.address.countryName, "countryName"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "POSTL", il.billToParty.address.zipCode, "zipCode"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "LAND1", il.billToParty.address.countryCode, "countryCode"));
                    }
                }

                //SubDealer
                if (il.shipToParty != null)
                {
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SUNNR", il.shipToParty.dealerCode, "dealerCode"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SATCD", il.shipToParty.dealerVATId, "dealerVATId"));
                    if (il.shipToParty.address != null)
                    {
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SUNN1", il.shipToParty.address.dealerName1, "dealerName1"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SUNN2", il.shipToParty.address.dealerName2, "dealerName2"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SDDR1", il.shipToParty.address.address1, "address1"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SDDR2", il.shipToParty.address.address2, "address2"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SANDX", il.shipToParty.address.countryName, "countryName"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SOSTL", il.shipToParty.address.zipCode, "zipCode"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "SAND1", il.shipToParty.address.countryCode, "countryCode"));
                    }
                }
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "WAERS", il.currencyCode, "currencyCode"));
                queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ZTERM", il.paymentTerm, "paymentTerm"));

                if (il.productLine != null)
                {
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "CARDOC", il.productLine.carDocNo, "carDocNo"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "TRANS", il.productLine.transporterCode, "transporterCode"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "MODEL", il.productLine.modelCode, "modelCode"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "OCNO", il.productLine.ocnCode, "ocnCode"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "MAKTX", il.productLine.fscDescription, "fscDescription"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "VHVIN", il.productLine.vin, "vin"));
                    if (il.productLine.color != null)
                    {
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ZZEXC", il.productLine.color.colorCode, "colorCode"));
                        queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "ZZEXX", il.productLine.color.colorDescription, "colorDescription"));
                    }
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "MILEAG", il.productLine.mileage, "mileage"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "REGDAT", il.productLine.firstregdate, "firstregdate"));
                }
                if (il.invoiceAmount != null)
                {
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "NETWR", il.invoiceAmount.netAmount, "netAmount"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "MWSBP", il.invoiceAmount.taxAmount, "taxAmount"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "VATRT", il.invoiceAmount.taxRate, "taxRate"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "GROSS", il.invoiceAmount.grossAmount, "grossAmount"));
                    queueQin.Add(getQeueInFromData(eaihotInput.SYSEAIHOT, kunnr, il.invoiceNo, "TIMVAL", il.invoiceAmount.timevalue, "timevalue"));
                }




                dao.createEaiqin(queueQin);
            }
            //activate now
            dao.activateEaihot(eaihotInput, 1);


        }

        /// <summary>
        /// Converts our base error object to the santander one
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string getErrorObject(oBaseDto o)
        {
            InvoiceResponse rvalue = new InvoiceResponse();
            rvalue.Type = "S";
            if (o.message.type != MessageType.None)
            {
                rvalue.Type = "E";
            }
            rvalue.Message = o.message.message;

            return XMLSerializer.SerializeUTF8(rvalue);
        
        }


        private static EaiqinDto getQeueInFromData(long syseaihot, String kdnr, String invno, String fname, String data, String orgName)
        {


            String value = String.Format(CultureInfo.InvariantCulture, "{0}", data);
            EaiqinDto qout = new EaiqinDto();
            qout.F01 = kdnr;
            qout.F02 = invno;
            qout.F03 = fname;
            qout.F04 = value;
            qout.F10 = orgName;
            qout.sysEaihot = syseaihot;
            return qout;

        }


    }
}
