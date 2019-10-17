using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// DMS Interface Configuration Parameters
    /// </summary>
    public class DMSConfig
    {
        public String deepLinkUrl { get; set; }
        public String interfaceUrl { get; set; }
        public String userAgent { get; set; }
        public String mandant { get; set; }
        public String locale { get; set; }
        public String certsubject { get; set; }
        public String certpassword { get; set; }
        public String certpath { get; set; }

        public int timeout { get; set; }
        public int timeoutBatch { get; set; }
    }

    /// <summary>
    /// Bo for outgoing / incoming documents to/from DMS (Aconso)
    /// </summary>
    public class DMSBo : IDMSBo
    {
        private ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod().DeclaringType);
        private IDMSDao dao;
        private DMSConfig config;
        private Dictionary<AconsoImportConfig, List<String>> useCaseFields = new Dictionary<AconsoImportConfig, List<string>>();

        /// <summary>
        /// Construct and initialize a DMS Business Object
        /// </summary>
        public DMSBo()
        {
            //urls/settings from db-config
            config = new DMSConfig();
            config.deepLinkUrl = AppConfig.getValueFromDb("SETUP", "DMS", "DEEPLINKURL", "https://www.aconso-akte.de:611/aconso/framework.servlet?");
			config.interfaceUrl = AppConfig.getValueFromDb ("SETUP", "DMS", "INTERFACEURL", "https://aconso-akte.de:624/aconso/aconsoimport.servlet/bnowv/configApi?");
			config.userAgent = AppConfig.getValueFromDb("SETUP", "DMS", "USERAGENT", "BOSEAI");
            config.mandant = AppConfig.getValueFromDb("SETUP", "DMS", "MANDANT", "olusr");
            config.locale = AppConfig.getValueFromDb("SETUP", "DMS", "LOCALE", "de");

            config.timeout = 1000*int.Parse( AppConfig.getValueFromDb("SETUP", "DMS", "TIMEOUT", "100") );
            config.timeoutBatch = 1000*int.Parse(AppConfig.getValueFromDb("SETUP", "DMS", "TIMEOUTBATCH", "100"));

            config.certsubject = AppConfig.getValueFromDb("SETUP", "DMS", "CERTSUBJECT", "CicOneDMSCertificate");//must be in localMachine Store

            config.certpassword = AppConfig.getValueFromDb("SETUP", "DMS", "CERTPASSWORD");//,"test");
            config.certpath = AppConfig.getValueFromDb("SETUP", "DMS", "CERTPATH", "c:\\temp\\testcert.p12");

            dao = CommonDaoFactory.getInstance().getDMSDao();

            //Map Use-Cases to Aconso-fields
            List<String> cs = new List<String>();
            cs.Add("AKTEID");
            cs.Add("ITID");
            cs.Add("ID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");
            cs.Add("GEB");
            cs.Add("STATUS");
            cs.Add("ZUSATZ");
            cs.Add("RETENTIONDATE");
            useCaseFields.Add(AconsoImportConfig.UpdateStammsatzMitAkteID, cs);

            cs = new List<String>();
            cs.Add("AKTEID");
            cs.Add("ITID");
            cs.Add("ID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");
            cs.Add("GEB");
            cs.Add("STATUS");
            cs.Add("ZUSATZ");
            cs.Add("RETENTIONDATE");
            cs.Add("RECORDID");
            useCaseFields.Add(AconsoImportConfig.BatchUpdateMitAkteID, cs);

            cs = new List<String>();
            cs.Add("PARTNERNR");
            cs.Add("HAENDLNR");
            cs.Add("REFERENZ");
            cs.Add("TYPE");
            cs.Add("AKTIV");
            cs.Add("NACHN");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("RETENTIONDATE");
            cs.Add("RECORDID");
            useCaseFields.Add(AconsoImportConfig.BatchUpdateMitPartnerNr, cs);

            cs = new List<String>();
            cs.Add("AKTEID");
            cs.Add("ITID");
            cs.Add("ID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");
            cs.Add("GEB");
            cs.Add("ZUSATZ");
            cs.Add("RETENTIONDATE");
            useCaseFields.Add(AconsoImportConfig.CreateStammsatzOhneVGNR, cs);
            

            cs = new List<String>();
            cs.Add("ID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");

            cs.Add("GEB");
            cs.Add("ZUSATZ");
            useCaseFields.Add(AconsoImportConfig.UpdateStammsatzMitKNR, cs);

            cs = new List<String>();
            cs.Add("ID");
            cs.Add("ITID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");

            cs.Add("GEB");
            cs.Add("ZUSATZ");
            useCaseFields.Add(AconsoImportConfig.UpdateStammsatzMitITID, cs);


            cs = new List<String>();
            cs.Add("AKTEID");
            cs.Add("ATT_DOC_TYPE");
            cs.Add("ATT_SCAN_DATE");
            cs.Add("ATT_DOC_DATE");
            cs.Add("ATT_PAGE_COUNT");
            cs.Add("ATT_FILE_TYPE");
            /*
            cs.Add("ATT_CHANNEL_TYPE");
            cs.Add("ATT_CHANNEL_INFO");
            cs.Add("ATT_PRODUCT");
            cs.Add("ATT_LANGUAGE");*/
            useCaseFields.Add(AconsoImportConfig.UploadDocumentMitAKTEID, cs);
            useCaseFields.Add(AconsoImportConfig.UploadDokumentMitAKTEID, cs);

			cs = new List<String> ();
			cs.Add ("REFERENZ");            // PERSON:SYSPERSON-reference for UploadDocumentMitREFERENZ (rh 20181016) 
			cs.Add ("ARCHIVE");             // diverted archives for: Vertriebspartner: bnowp.pa; 	MA Vertriebspartner: bnowp.ma 		

			cs.Add ("ATT_DOC_TYPE");		// STD DMS fields:
			cs.Add ("ATT_SCAN_DATE");       // ...
			cs.Add ("ATT_DOC_DATE");        // ...
			cs.Add ("ATT_PAGE_COUNT");      // ...
			cs.Add ("ATT_FILE_TYPE");       // ...
			useCaseFields.Add (AconsoImportConfig.UploadDocumentMitREFERENZ, cs);		// THE "usecase"

		    cs = new List<String>();
            cs.Add("AKTEID");
            cs.Add("VORGNR");
            cs.Add("ITID");
            cs.Add("KNR");
            cs.Add("KONTY");
            cs.Add("KUNTY");
            cs.Add("NAME");
            cs.Add("VORNA");
            cs.Add("PLZ");
            cs.Add("ORT");
            cs.Add("STRNR");
            cs.Add("CSMA");

            cs.Add("GEB");
            cs.Add("STATUS");
            cs.Add("ZUSATZ");
            useCaseFields.Add(AconsoImportConfig.UpdateStammsatzMappeZuAkte, cs);
        }

		/// <summary>
		/// Creates or Updates a dossier (the attributes) in the DMS via the DMS-HTTP-Interface
		/// outgoing
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public DMSAKTE createOrUpdateDMSAkte (icreateOrUpdateDMSAkteDto input)
		{
			_log.Debug ("Called createOrUpdateDMSAkte for sysdmsakte " + input.sysdmsakte);
			DMSAKTE akte = null;
			try
			{
				if (input.sysdmsakte == 0)
				{
					throw new Exception ("createOrUpdateDMSAkte not possible - empty sysdmsakte");
				}

				// use input-data to fetch all needed fields from db for interface of dms
				AconsoHTTPImportBo op = new AconsoHTTPImportBo (config);
                
				// get DMSAkte
				akte = dao.getDMSAkte (input.sysdmsakte);
				if (akte.SYSID == null || !akte.SYSID.HasValue || akte.SYSID.Value == 0)
				{
					throw new Exception ("createOrUpdateDMSAkte failed - empty SYSID for sysdmsakte " + input.sysdmsakte);
				}

				

				// map.Methode to AconsoImportConfig
				AconsoImportConfig icfg = AconsoImportConfig.CreateStammsatzOhneVGNR;//default
				try
				{
					icfg = (AconsoImportConfig) System.Enum.Parse (typeof (AconsoImportConfig), akte.METHOD);
				}
				catch (Exception)
				{
					throw new Exception ("createOrUpdateDMSAkte failed - wrong METHOD " + akte.METHOD + " for sysdmsakte " + input.sysdmsakte);
				}

                //Batch-Verarbeitung-----------------------------------------------------
                if (icfg == AconsoImportConfig.BatchUpdateMitPartnerNr || icfg == AconsoImportConfig.BatchUpdateMitAkteID)
                {

                    throw new Exception("createOrUpdateDMSAkte failed - wrong area in EAIHOT for Batch-Processing: "+input.sysdmsakte);

                }
                else
                {


                    DMSExportDataDto data = dao.getDataForDMS(akte.AREA, akte.SYSID.Value);
                    if (data == null)
                    {
                        throw new Exception("createOrUpdateDMSAkte failed - no data found in Database for AREA " + akte.AREA + " SYSID: " + akte.SYSID + " sysdmsakte " + input.sysdmsakte);
                    }

                    // for this usecase when we have a VGNR change the usecase
                    if (icfg == AconsoImportConfig.CreateStammsatzOhneVGNR && data.VORGNR != null && data.VORGNR.Length > 0)
                    {
                        try
                        {
                            long tvgnr = long.Parse(data.VORGNR);
                            icfg = AconsoImportConfig.UpdateStammsatzMappeZuAkte;
                            akte.METHOD = "UpdateStammsatzMappeZuAkte";
                        }
                        catch (Exception)
                        {
                            // invalid VORGNR ignore
                        }
                    }
                    // Wird ein Interessent bereits als Person (IT.SYSPERSON ist gefüllt) geführt, 
                    // werden die Änderungen zu diesem Interessent ignoriert und sind somit von der Übertragung ausgenommen.
                    if (icfg == AconsoImportConfig.UpdateStammsatzMitITID && data.ID > 0)
                    {
                        // reset errors:
                        akte.ERRCODE = null;
                        akte.ERRMESSAGE = "Skipped because PERSON already available";
                        akte.ERRTYPE = null;
                        akte.REQDATE = DateTime.Now;
                        akte.REQTIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                        akte.RETCODE = "0";
                        akte.RETRYONERR = 0;
                        dao.updateDMSAkte(akte);           // WRITE BACK results to DMSAKTE
                    }

                    _log.Debug("Using config " + icfg);
                    op.setUseCase(icfg);

                    fillFields(op, icfg, data);            // FILL aconso input fields for area/id

                    op.call(akte);                         // CALL Aconso

                    dao.updateDMSAkte(akte);               // WRITE BACK results to DMSAKTE
                }
			}
			catch (Exception e)
			{
				if (akte != null)
				{
					akte.ERRMESSAGE = e.Message;
					akte.ERRCODE = "0";
					akte.ERRTYPE = "permanent";
					// dont throw, error is logged in DMSAKTE
					dao.updateDMSAkte (akte);           // WRITE BACK results to DMSAKTE
				}
				_log.Debug ("createOrUpdateDMSAkte failed " + e.Message);
				throw e;
			}
			return akte;
		}



        /// <summary>
        /// Creates or Updates (the attributes) in the DMS via the DMS-HTTP-Interface for all dmsakte-Entries of the dmsbatch
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateDMSAkteBatchDto createOrUpdateDMSAkteBatch(icreateOrUpdateDMSAkteBatchDto input)
        {
            _log.Debug("Called createOrUpdateDMSAkte for sysdmsbatch " + input.sysdmsbatch);
            ocreateOrUpdateDMSAkteBatchDto rval = new ocreateOrUpdateDMSAkteBatchDto();
            try
            {
                if (input.sysdmsbatch == 0)
                {
                    throw new Exception("createOrUpdateDMSAkte for Batchprocessing not possible - empty sysdmsakte");
                }

                // use input-data to fetch all needed fields from db for interface of dms
                AconsoHTTPImportBo op = new AconsoHTTPImportBo(config);

                String method = "";

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    method = ctx.ExecuteStoreQuery<String>("select method from dmsakte where sysdmsbatch=" + input.sysdmsbatch).FirstOrDefault();
                }

                // map.Methode to AconsoImportConfig
                AconsoImportConfig icfg = AconsoImportConfig.BatchUpdateMitAkteID;//default
                try
                {
                    icfg = (AconsoImportConfig)System.Enum.Parse(typeof(AconsoImportConfig), method);
                }
                catch (Exception)
                {
                    throw new Exception("createOrUpdateDMSAkte for Batchprocessing failed - wrong METHOD " + method + " for sysdmsakte " + input.sysdmsbatch);
                }

                
                //Batch-Verarbeitung-----------------------------------------------------
                if (!(icfg == AconsoImportConfig.BatchUpdateMitPartnerNr || icfg == AconsoImportConfig.BatchUpdateMitAkteID))
                {
                    throw new Exception("createOrUpdateDMSAkte for Batchprocessing failed - Method not supported: "+ method + " for sysdmsbatch " + input.sysdmsbatch);
                }


                if (icfg == AconsoImportConfig.BatchUpdateMitPartnerNr)
                {
                    config.interfaceUrl = AppConfig.getValueFromDb("SETUP", "DMS", "BATCHUPDATEPARTNERURL", config.interfaceUrl);
                }
                else if (icfg == AconsoImportConfig.BatchUpdateMitAkteID)
                {
                    config.interfaceUrl = AppConfig.getValueFromDb("SETUP", "DMS", "BATCHUPDATEAKTEURL", config.interfaceUrl);
                }
                op.setUseCase(icfg);

                int i = 0;
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    //iterate all dmsbatch-akte-entries and append to request
                    IQueryable<DmsBatchInfoDto> dmb = ctx.ExecuteStoreQuery<DmsBatchInfoDto>("select sysdmsakte,area,sysid,method from dmsakte where sysdmsbatch=" + input.sysdmsbatch).AsQueryable(); 
                    foreach(DmsBatchInfoDto bInfo in dmb)
                    {
                        DMSExportDataDto data = dao.getDataForDMS(bInfo.area, bInfo.sysid);
                        fillFields(op, icfg, data);            // FILL aconso input fields for area/id
                        op.addRecord(bInfo.sysdmsakte);
                        i++;
                    }
                }

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rtime", Value = DateTimeHelper.DateTimeNullableToClarionTime(DateTime.Now) });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rdate", Value =DateTime.Now });
                    
                    ctx.ExecuteStoreCommand("update dmsbatch set reqtime=:rtime, reqdate=:rdate where sysdmsbatch=" + input.sysdmsbatch, parameters.ToArray());
                }

                ocreateOrUpdateDMSAkteBatchDto aconsoRval = op.callBatch();

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    ctx.ExecuteStoreCommand("update dmsbatch set error="+long.Parse(aconsoRval.errcode)+" where sysdmsbatch="+input.sysdmsbatch);
                }
                rval.errcode = aconsoRval.errcode;
                rval.retcode = aconsoRval.retcode;
                rval.errmessage = "Processed " + i + " records for DMS Batch";
            }
            catch (Exception e)
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    ctx.ExecuteStoreCommand("update dmsbatch set error=2 where sysdmsbatch=" + input.sysdmsbatch);//some exception - batch failed!
                }
                _log.Debug("createOrUpdateDMSAkte Batchprocessing failed " + e.Message);
                throw e;
            }
            return rval;
        }

        /// <summary>
        /// creates or updates a document (the file) in the DMS via the DMS Documentimport Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DMSAKTE createOrUpdateDMSDokument (icreateOrUpdateDMSDokmentDto input)
		{
			_log.Debug ("Called createOrUpdateDMSDokument for sysdmsakte " + input.sysdmsakte);
			DMSAKTE akte = null;
			try
			{
				if (input.sysdmsakte == 0)
					throw new Exception ("createOrUpdateDMSDokument not possible - empty sysdmsakte");

				AconsoHTTPImportBo op = new AconsoHTTPImportBo (config);
				
				akte = dao.getDMSAkte (input.sysdmsakte);       // get dmsakte

				if (akte.SYSID == null || !akte.SYSID.HasValue || akte.SYSID.Value == 0)
					throw new Exception ("createOrUpdateDMSDokument failed - empty SYSID for sysdmsakte " + input.sysdmsakte);

				DMSExportDataDto data = dao.getDataForDMS (akte.AREA, akte.SYSID.Value);
				if (data == null)
					throw new Exception ("createOrUpdateDMSDokument failed - no data found in Database for AREA " + akte.AREA + " SYSID: " + akte.SYSID + " sysdmsakte " + input.sysdmsakte);

				// map.Methode to "various" AconsoImportConfig's
				AconsoImportConfig icfg;
				try
				{
					icfg = (AconsoImportConfig) System.Enum.Parse (typeof (AconsoImportConfig), akte.METHOD);
				}
				catch (Exception)
				{
					throw new Exception ("createOrUpdateDMSDokument failed - wrong METHOD " + akte.METHOD + " for sysdmsakte " + input.sysdmsakte);
				}

				if (data.AREA == "PEROLE")			// ON Area "PEROLE"
				{
					if (icfg == AconsoImportConfig.UploadDocumentMitREFERENZ)
					{
                        // for debug tests skip overwriting of interfaceUrl (continue with orig. STD-URL) 
						if ( ! System.Diagnostics.Debugger.IsAttached)
						{
							config.interfaceUrl = AppConfig.getValueFromDb ("SETUP", "DMS", "PARTNERREFERENCEURL", "https://aconso-akte.de:624/aconso/aconsoimport.servlet/bnowp/configApi?");
						}
					}
				}
				else
				{// STD-branche
					if (icfg == AconsoImportConfig.UploadDokumentMitAKTEID)
						icfg = AconsoImportConfig.UploadDocumentMitAKTEID;          // REMAP to "one" methode UploadDocumentMitAKTEID (written with "C")
				}

				_log.Debug ("Using config " + icfg);
				op.setUseCase (icfg);

				DMSExportDataDto fileAttributeData = dao.getFileDataForDMS (input.sysdmsdoc);
				op.addFileData (fileAttributeData.FILEDATA, fileAttributeData.ATT_FILE_TYPE, fileAttributeData.FILENAME);

				fillAttrs (op, fileAttributeData, data);              // FILL aconso ATTRs for area/id
				fillFields (op, icfg, data);        // FILL aconso input MASTERDATA FIELDS for area/id

				String docid = op.call (akte);		// CALL Aconso (ON NON-Debug)
				
				dao.updateDMSAkte (akte);           // WRITE BACK RESULTS to DMSAKTE

				if (docid != null)                  
				{
					using (DdOwExtended ctx = new DdOwExtended ())
					{
						List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = akte.SYSDMSAKTE });
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = docid });

						ctx.ExecuteStoreCommand ("UPDATE DMSAKTE set EXTID=:extid where SYSDMSAKTE=:id", parameters.ToArray ());
					}
				}
			}
			catch (Exception e)
			{
				if (akte != null)
				{
					akte.ERRMESSAGE = e.Message;
					akte.ERRCODE = "0";
					akte.ERRTYPE = "permanent";
					//dont throw, error is logged in dmsakte
					dao.updateDMSAkte (akte);       // WRITE BACK RESULTS to DMSAKTE
				}
				_log.Debug ("createOrUpdateDMSDokument failed " + e.Message);
				throw e;
			}
			return akte;
		}

		/// <summary>
		/// Fills the aconso import structure with MASTER-data from the given area/id
		/// using only the fields defined for the usecase in useCaseFields
		/// </summary>
		/// <param name="op"></param>
		/// <param name="uc"></param>
		/// <param name="data"></param>
		private void fillFields(AconsoHTTPImportBo op, AconsoImportConfig uc, DMSExportDataDto data)
        {
            List<String> vStrings = useCaseFields[uc];

            if (vStrings.Contains("AKTEID") && data.AKTEID != null)
            {
                op.addMDField("AKTEID", data.AKTEID);
            }
            if (vStrings.Contains("CSMA"))
            {
                op.addMDField("CSMA", "" + data.CSMA);
            }

            if (vStrings.Contains("GEB") && data.GEB != null && data.GEB.HasValue && data.KUNTY == 1)
            {
                if (data.GEB.Value.Year >= 1754)
                {
                    op.addMDField("GEB", String.Format("{0:yyyyMMdd}", data.GEB));
                }
            }
           
            if (vStrings.Contains("ITID") && data.ITID > 0)
            {
                op.addMDField("ITID", "" + data.ITID);
            }
            if (vStrings.Contains("KNR") && data.KNR > 0)
            {
                op.addMDField("KNR", "" + data.KNR);
            }
            if (vStrings.Contains("KONTY") && data.KONTY != null)
            {
                op.addMDField("KONTY", data.KONTY);
            }
            if (vStrings.Contains("KUNTY"))
            {
                op.addMDField("KUNTY", "" + data.KUNTY);
            }
            if (vStrings.Contains("NAME") && data.NAME != null)
            {
                op.addMDField("NAME", data.NAME);
            }
            else if (vStrings.Contains("NACHN") && data.NAME != null)
            {
                op.addMDField("NACHN", data.NAME);
            }
            if (vStrings.Contains("ORT") && data.ORT != null)
            {
                op.addMDField("ORT", data.ORT);
            }
            if (vStrings.Contains("PLZ") && data.PLZ != null)
            {
                op.addMDField("PLZ", data.PLZ);
            }
            if (vStrings.Contains("VORNA") && data.VORNA != null)
            {
                op.addMDField("VORNA", data.VORNA);
            }
            if (vStrings.Contains("STRNR") && data.STRNR != null)
            {
                op.addMDField("STRNR", data.STRNR);
            }
            if (vStrings.Contains("VORGNR") && data.VORGNR != null)
            {
                op.addMDField("VORGNR", data.VORGNR);
            }
            if (vStrings.Contains("ZUSATZ") && data.ZUSATZ != null)
            {
                op.addMDField("ZUSATZ", data.ZUSATZ);
            }
            if (vStrings.Contains("CHANNELID") && data.CHANNELID != null)
            {
                op.addMDField("CHANNELID", data.CHANNELID);
            }
            if (vStrings.Contains("AS2") && data.AS2 != null)
            {
                op.addMDField("AS2", data.AS2);
            }
            if (vStrings.Contains("PARTNERNR") && data.PARTNERNR != null)
            {
                op.addMDField("PARTNERNR", data.PARTNERNR);
            }
            if (vStrings.Contains("HAENDLNR") && data.HAENDLNR != null)
            {
                op.addMDField("HAENDLNR", data.HAENDLNR);
            }
            if (vStrings.Contains("TYPE") && data.TYPE != null)
            {
                op.addMDField("TYPE", data.TYPE);
            }
            if (vStrings.Contains("AKTIV") && data.AKTIV != null)
            {
                op.addMDField("AKTIV", data.AKTIV);
            }


            if (vStrings.Contains("REFERENZ") && data.REFERENZ != null)
            {
                op.addMDField("REFERENZ", data.REFERENZ);
            }

            bool sendRetention = false;
            if ((uc == AconsoImportConfig.UpdateStammsatzMitAkteID) && "ANTRAG".Equals(data.AREA))
            {
                if (data.ZUSTAND != null && "abgeschlossen".Equals(data.ZUSTAND.ToLower().Trim()))
                {
                    if (data.ATTRIBUT != null)
                    {
                        String att = data.ATTRIBUT.ToLower().Trim();
                        if(att.Equals("abgelehnt")|| att.Equals("gelöscht")|| att.Equals("vertrag aktiviert")|| att.Equals("verzichtet"))
                            sendRetention = true;
                    }
                }

            }
            else
            {
                sendRetention = true;
            }

            if(sendRetention)
            {
                if (vStrings.Contains("RETENTIONDATE") && data.RETENTIONDATE != null && data.RETENTIONDATE.HasValue)
                {
                    op.addMDField("RETENTIONDATE", String.Format("{0:yyyyMMdd}", data.RETENTIONDATE));
                }
            }

            bool sendStatus = false;
            //Sections to decide if we bail out and DO NOT Transmit the Status
            if ((uc == AconsoImportConfig.UpdateStammsatzMitAkteID || uc == AconsoImportConfig.BatchUpdateMitAkteID) && "ANTRAG".Equals(data.AREA))
            {
                if(data.ZUSTAND!=null && "abgeschlossen".Equals(data.ZUSTAND.ToLower().Trim()))
                {
                    sendStatus = true;
                    //for this state transmit STATUS
                }
                
            }
            else if ((uc == AconsoImportConfig.UpdateStammsatzMappeZuAkte || uc == AconsoImportConfig.BatchUpdateMitAkteID) && ("ANTRAG".Equals(data.AREA) || "ANGEBOT".Equals(data.AREA)))
            {
                if (data.ZUSTAND != null && "abgeschlossen".Equals(data.ZUSTAND.ToLower().Trim()))
                {
                    //for this state transmit STATUS
                    sendStatus = true;
                }
                
            }

            if ((uc == AconsoImportConfig.BatchUpdateMitAkteID) && ("VT".Equals(data.AREA)))
            {
                sendStatus = false;
            }

            if (sendStatus)
            {
                if (vStrings.Contains("STATUS") && data.STATUS != null)
                {
                    op.addMDField("STATUS", data.STATUS);
                }
            }


        }

        /// <summary>
        /// Fills the aconso import structure with Attribute-data from the given area/id
        /// using only the fields defined for the usecase in useCaseFields
        /// </summary>
        /// <param name="op"></param>
        /// <param name="fileAttributeData">Data Attributes of the uploaded Document</param>
        /// <param name="data">Account/Contract Attributes</param>
        private void fillAttrs (AconsoHTTPImportBo op, DMSExportDataDto fileAttributeData, DMSExportDataDto data)
        {
            AconsoImportConfig uc = op.getUseCase();
            List<String> vStrings = useCaseFields[uc];

            if (vStrings.Contains("ATT_FILE_TYPE") && fileAttributeData.ATT_FILE_TYPE != null)
            {
                op.addAttrField("ATT_FILE_TYPE", fileAttributeData.ATT_FILE_TYPE);
            }
            if (vStrings.Contains("ATT_SCAN_DATE") && fileAttributeData.ATT_SCAN_DATE != null)
            {
                op.addAttrField("ATT_SCAN_DATE", fileAttributeData.ATT_SCAN_DATE);
            }
            if (vStrings.Contains("ATT_DOC_DATE") && fileAttributeData.ATT_DOC_DATE != null)
            {
                op.addAttrField("ATT_DOC_DATE", fileAttributeData.ATT_DOC_DATE);
            }

            op.addAttrField("ATT_DOC_TYPE", fileAttributeData.ATT_DOC_TYPE);

            if (vStrings.Contains("ATT_PAGE_COUNT") && fileAttributeData.ATT_PAGE_COUNT != null)
            {
                op.addAttrField("ATT_PAGE_COUNT", fileAttributeData.ATT_PAGE_COUNT);
            }
            if (vStrings.Contains("ATT_CHANNEL_TYPE") && fileAttributeData.ATT_CHANNEL_TYPE != null)
            {
                op.addAttrField("ATT_CHANNEL_TYPE", fileAttributeData.ATT_CHANNEL_TYPE);
            }
            if (vStrings.Contains("ATT_CHANNEL_INFO") && fileAttributeData.ATT_CHANNEL_INFO != null)
            {
                op.addAttrField("ATT_CHANNEL_INFO", fileAttributeData.ATT_CHANNEL_INFO);
            }
            if (vStrings.Contains("ATT_PRODUCT") && fileAttributeData.ATT_PRODUCT != null)
            {
                op.addAttrField("ATT_PRODUCT", fileAttributeData.ATT_PRODUCT);
            }
            if (vStrings.Contains("ATT_LANGUAGE") && fileAttributeData.ATT_LANGUAGE != null)
            {
                op.addAttrField("ATT_LANGUAGE", fileAttributeData.ATT_LANGUAGE);
            }

			
			if (vStrings.Contains ("ARCHIVE") && data.ARCHIVE != null)      
			{
				op.addAttrField ("ARCHIVE", data.ARCHIVE);
			}

		}

		/// <summary>
		/// interface from DMS to OL for new incoming Documents
		/// </summary>
		/// <param name="sysperole"></param>
		/// <param name="syswfuser"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public void execDMSUploadTrigger(long sysperole, long syswfuser, iDMSUploadDto input)
        {
            _log.Debug("Received DMS-Upload Trigger case: " + input.caseid + " / version: " + input.interfaceVersion + " / reference:" + input.sysReferenz);

            try
            {
               //determine routing, sysreferenz/referenz not null
               bool routing = !String.IsNullOrEmpty(input.vtNumber);

               String allowedDoctypes = "";
               if (routing)
               {
                   allowedDoctypes = AppConfig.Instance.GetEntry("DMS", "DOCTYPES_ROUTING", "", "SETUP");
                   String tmpdt = AppConfig.Instance.GetEntry("DMS", "DOCTYPES_ROUTING2", "", "SETUP");
                   if(tmpdt!=null && tmpdt.Length>0)
                   {
                       if(!tmpdt.StartsWith(";"))
                       {
                           allowedDoctypes += ";";
                       }
                       allowedDoctypes+=tmpdt;
                   }
               }
               else
               {
                   allowedDoctypes = AppConfig.Instance.GetEntry("DMS", "DOCTYPES_EINGANG", "", "SETUP");
                   String tmpdt = AppConfig.Instance.GetEntry("DMS", "DOCTYPES_EINGANG2", "", "SETUP");
                   if (tmpdt != null && tmpdt.Length > 0)
                   {
                       if (!tmpdt.StartsWith(";"))
                       {
                           allowedDoctypes += ";";
                       }
                       allowedDoctypes += tmpdt;
                   }
               }

                if (allowedDoctypes == null)
                {
                    allowedDoctypes = "";
                }

                List<String> doctypes = allowedDoctypes.Split(';').ToList();

                //write input-data in CIC.DMSUPLOAD
                //write input-data.values into CIC.DMSUPLOADDETAIL
                DMSUPL u = new DMSUPL();
                u.CREDATE = DateTime.Now;
                u.CRETIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                
                
                u.DOCID = "" + input.documentId;//req
                if (input.documentId ==0)
                {
                    throw new Exception("documentId was empty");
                }

                u.DOCTYPE= input.documenttype;//req
                if (input.documenttype == null)
                {
                    throw new Exception("documenttype was empty");
                }

                u.INTERFACEVERS = input.interfaceVersion;//req
                if (input.interfaceVersion == null)
                {
                    throw new Exception("interfaceVersion was empty");
                }

                u.VORGANG = "" + input.caseid;
                if (input.caseid=="")
                {
                    throw new Exception("caseid was empty");
                }

                u.REFERENZ = input.vtNumber;
                u.SYSREFERENZ = input.sysReferenz;
                GebietInfoDto gebiet = null;
                long sysdmsuplinst = 0;
                if (routing)
                {
                    gebiet = dao.getDMSTarget(u.REFERENZ, u.SYSREFERENZ.GetValueOrDefault());
                    sysdmsuplinst = dao.createOrUpdateDmsUplInst(gebiet);
                    u.SYSDMSUPL = dao.getSysDmsupl(input.documentId, sysdmsuplinst);
                }

                u = dao.createOrUpdateDMSUPL(u, sysdmsuplinst);

                List<DMSUPLDETAIL> uplDetails = new List<DMSUPLDETAIL>();
                int i = 0;
                foreach (DMSField f in input.values)
                {
                    DMSUPLDETAIL det = new DMSUPLDETAIL();
                    if (f.name == null)
                    {
                        throw new Exception("Input-Value Fieldname was empty in values[" + i + "]");
                    }
                    if (f.value == null)
                    {
                        throw new Exception("Input-Value Fieldvalue was empty in values[" + i + "]");
                    }
                    det.ATTRIBUTNAME = f.name;
                    det.ATTRIBUTTYP = f.type.ToString();
                    det.ATTRIBUTWERT = f.value.getValue(f.type);
                    uplDetails.Add(det);
                    i++;
                }
                dao.createOrUpdateDmsUpldetails(uplDetails, u.SYSDMSUPL);

                _log.Info("Received DMS Document SYSDMSUPL: " + u.SYSDMSUPL + " VORGANG: " + u.VORGANG + " REFERENZ: " + u.REFERENZ + " SYSREFERENZ: " + u.SYSREFERENZ + " DOCTYPE: " + u.DOCTYPE + " routing: " + routing + " supported Doctypes: " + allowedDoctypes);

				if (!doctypes.Contains(u.DOCTYPE))
                {
                    _log.Warn("Doctype was not supported, no further processing ("+doctypes.ToArray().ToString()+")");
                    return;
                }
            
                if (routing)//link to available ang ant vt
                {
                    
                    if(gebiet==null)
                    {
                        _log.Error("Routing failed - no ANGEBOT/ANTRAG/VT/PERSON found for " + u.REFERENZ + "/" + u.SYSREFERENZ);
                        return;
                    }
                    dao.createOrUpdateDmsUplInst(gebiet);
                    //bpe process start
                    String procCode = AppConfig.Instance.GetEntry("DMS", "PROC_DMS_ROUTING", "", "SETUP");
                    String evtCode = AppConfig.Instance.GetEntry("DMS", "EVT_DMS_ROUTING", "", "SETUP");
                    BPEBo.createBPEProcess(procCode, evtCode, "DMSUPL", u.SYSDMSUPL, syswfuser);
                }
                else //create angebot
                {
                    IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebot = new Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto();
                    angebot.erfassungsclient = AngAntDao.ERFASSUNGSCLIENT_DMR;//Clienttyp (10=B2B, 20=MA, 30=B2C, 50=ONE, 60=DMR)


                    String testWert = AppConfig.Instance.GetEntry("DIGITALE_CHECKLISTE", "ACTIVEFLAG", "0", "ANTRAGSASSISTENT");
                    bool eventtest = "1".Equals(testWert) || "true".Equals(testWert) || "TRUE".Equals(testWert);

                    if (eventtest)
                    {
                        //angebot.sysAbwicklung
                        String cid = getAttValue(uplDetails, "ATT_CHANNEL_ID");
                        if (cid != null)
                        {
                            String cinfo = getAttValue(uplDetails, "ATT_CHANNEL_INFO");
                            if (cinfo != null && cinfo.Length > 0)
                            {
                                String query = "";
                                if ("1".Equals(cid))//EMAIL
                                {
                                    if (cinfo.IndexOf("@") < 0)
                                        cinfo += "@bank-now.ch";
                                    query = @"SELECT t1.sysperole
                                        FROM (SELECT peabwo.sysperole, peabwo.sysperson sp
                                        FROM perelate, perole pevm, perole peabwo
                                        WHERE pevm.sysperson >0
                                        AND perelate.sysperole2 = pevm.sysperole
                                        AND perelate.sysperole1 = peabwo.sysperole                                        
                                        AND pevm.sysperson > 0 AND (perelate.relbeginndate IS NULL
                                        OR perelate.relbeginndate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relbeginndate <= sysdate)
                                        AND (perelate.relenddate IS NULL
                                        OR perelate.relenddate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relenddate >= sysdate)
                                        AND peabwo.sysroletype = 11 
                                        ORDER BY NVL(perelate.flagdefault, 0) DESC, perelate.sysperelate DESC) t1, person where person.sysperson=t1.sp
                                        and (trim(upper(email))) = upper(:p)";
                                }
                                else if ("2".Equals(cid))//FAX
                                {
                                    query = @"SELECT t1.sysperole
                                        FROM (SELECT peabwo.sysperole, peabwo.sysperson sp
                                        FROM perelate, perole pevm, perole peabwo
                                        WHERE pevm.sysperson >0
                                        AND perelate.sysperole2 = pevm.sysperole
                                        AND perelate.sysperole1 = peabwo.sysperole                                        
                                        AND pevm.sysperson > 0 AND (perelate.relbeginndate IS NULL
                                        OR perelate.relbeginndate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relbeginndate <= sysdate)
                                        AND (perelate.relenddate IS NULL
                                        OR perelate.relenddate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relenddate >= sysdate)
                                        AND peabwo.sysroletype = 11 
                                        ORDER BY NVL(perelate.flagdefault, 0) DESC, perelate.sysperelate DESC) t1, person where person.sysperson=t1.sp
                                        and upper(trim(fax))= :p";

                                }
                                using (PrismaExtended ctx = new PrismaExtended())
                                {
                                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p", Value = cinfo });
                                    angebot.sysAbwicklung = ctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();

                                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p", Value = angebot.sysAbwicklung });
                                    angebot.sysVM = ctx.ExecuteStoreQuery<long>("select sysperson from perole where sysperole=:p", parameters.ToArray()).FirstOrDefault();
                                    angebot.vertriebsweg = null;//dann wird berater für abwicklungsort genommen
                                }
                            }
                        }
                    }
                    
                    angebot.angAntVars = new List<OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>();
                    angebot = bo.createOrUpdateAngebot(angebot, sysperole);
                    

                    //update the referenz from the offer to the upload
                    u.REFERENZ = angebot.angebot;
                    dao.updateDMSUPL(u);

                    //now send a trigger to dms

                    //bpe process start
                    String procCode = AppConfig.Instance.GetEntry("DMS", "PROC_DMS_EINGANG", "", "SETUP");
                    String evtCode = AppConfig.Instance.GetEntry("DMS", "EVT_DMS_EINGANG", "", "SETUP");
                    BPEBo.createBPEProcess(procCode, evtCode, "ANGEBOT", angebot.sysid, syswfuser);
                }
            }
            catch (Exception e)
            {
                _log.Error("Failure saving received DMS-Upload-Data: " + e.Message, e);
                throw new Exception("Failure saving received DMS-Upload-Data: " + e.Message);
            }
        }

        /// <summary>
        /// Determine the attribute-value
        /// </summary>
        /// <param name="uplDetails"></param>
        /// <param name="att"></param>
        /// <returns></returns>
        private static String getAttValue (List<DMSUPLDETAIL> uplDetails, String att)
        {
            String rval = (from u in uplDetails
                               where u.ATTRIBUTNAME.Equals(att)
                               select u.ATTRIBUTWERT).FirstOrDefault();
            return rval;
        }
    }


    class DmsBatchInfoDto
    {
        public long sysdmsakte { get;set;}
        public long sysid { get; set; }
        public String area { get; set; }
        public String method { get; set; }
    }
}
