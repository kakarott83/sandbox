// OWNER MK, 02-07-2009
namespace Cic.OpenLease.Service.Merge.Dictionary
{
    #region Using
    using Cic.One.Utils.Util.Exceptions;
    using Cic.OpenLease.Service.Services.DdOl;
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using Cic.OpenOne.Common.Model.DdCt;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OIQUEUE.EF6.Model;
    using CIC.Database.OL.EF6.Model;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.DictionaryService")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class DictionaryService : Cic.OpenLease.ServiceAccess.Merge.Dictionary.IDictionaryService
    {
        #region Private constants
        private const string CnstDictionaryCfg = "AUSWAHLLISTEN";
        #endregion

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region IDictionaryService Members
        public DictionaryDto[] DeliverDictionary(string dictionaryName)
        {
            try
            {
                return DictionaryDao.getDictionaryValues(dictionaryName);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDictionaryFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                DictionaryDto[] rval = new DictionaryDto[0];
                return rval.ToArray();
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> DeliverLANDDtoList()        
        {

            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {

                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> LANDDtoList = DictionaryDao.getLaender();
                // Translate
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ServiceValidator.ISOLanguageCode) && LANDDtoList != null && LANDDtoList.Count > 0)
                {
                    try
                    {
                        using (DdCtExtended context = new DdCtExtended())
                        {
                            if ( CTTWEBHelper.hasTranslations(context))
                            foreach (LANDDto LANDDtoLoop in LANDDtoList)
                            {
                                LANDDtoLoop.COUNTRYNAME =  CTTWEBHelper.DeliverTranslation(context, LANDDtoLoop.COUNTRYNAME, ServiceValidator.ISOLanguageCode);
                            }
                        }
                    }
                    catch (System.Exception Exception)
                    {
                        throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
                    }
                }

                return LANDDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverLANDDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> DeliverAddressLand()
        {
            List<LAND> LANDList = null;
            List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> LANDDtoList = null;

            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {

                    LANDDtoList= context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto>("select * from land order by countryname").ToList();

                    //LANDList = MyFilterLandAddressList(context.Select<LAND>(null, null, LAND.FieldNames.COUNTRYNAME, 0, 0));
                }

                /*
                if (LANDList != null)
                {
                    // New list
                    LANDDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto>();
                    // New assembler
                    LANDAssembler LANDAssembler = new LANDAssembler();

                    foreach (LAND LANDLoop in LANDList)
                    {
                        // Convert and add to the list
                        LANDDtoList.Add(LANDAssembler.ConvertToDto(LANDLoop));
                    }
                }*/

                // Translate
                if (   !Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ServiceValidator.ISOLanguageCode) && LANDDtoList != null && LANDDtoList.Count > 0)
                {
                    try
                    {
                        using (DdCtExtended context = new DdCtExtended())
                        {
                            if(CTTWEBHelper.hasTranslations(context))
                            foreach (LANDDto LANDDtoLoop in LANDDtoList)
                            {
                                LANDDtoLoop.COUNTRYNAME = CTTWEBHelper.DeliverTranslation(context, LANDDtoLoop.COUNTRYNAME, ServiceValidator.ISOLanguageCode);
                            }
                        }
                    }
                    catch (System.Exception Exception)
                    {
                        throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
                    }
                }

                return LANDDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverAddressLandFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto> DeliverSTAATDtoList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<STAAT> STAATList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto> STAATDtoList = null;

                using (DdOlExtended context = new DdOlExtended())
                {
                    STAATDtoList = context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto>("select * from staat order by staat").ToList();

                   /* STAATList = MyFilterStaatList(context.Select<STAAT>(null, null, STAAT.FieldNames.STAAT1, 0, 0));

                    if (STAATList != null)
                    {
                        foreach (STAAT STAATLoop in STAATList)
                        {
                            if (STAATLoop.LANDReference.IsLoaded == false)
                            {
                                // Load reference
                                STAATLoop.LANDReference.Load();
                            }
                        }
                    }*/
                }


               /* if (STAATList != null)
                {
                    // New list
                    STAATDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto>();
                    // New assembler
                    STAATAssembler STAATAssembler = new STAATAssembler();

                    foreach (STAAT STAATLoop in STAATList)
                    {
                        Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto TempSTAATDto = STAATAssembler.ConvertToDto(STAATLoop);

                        if (STAATLoop.LAND != null)
                        {
                            // Set SYSLAND
                            TempSTAATDto.SYSLAND = STAATLoop.LAND.SYSLAND;
                        }

                        // Convert and add to the list
                        STAATDtoList.Add(TempSTAATDto);
                    }
                }*/

                // Translate
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ServiceValidator.ISOLanguageCode) && STAATDtoList != null && STAATDtoList.Count > 0)
                {
                    try
                    {
                        using (DdCtExtended context = new DdCtExtended())
                        {
                            if ( CTTWEBHelper.hasTranslations(context))
                            foreach (STAATDto STAATDtoLoop in STAATDtoList)
                            {
                                STAATDtoLoop.STAAT1 =  CTTWEBHelper.DeliverTranslation(context, STAATDtoLoop.STAAT1, ServiceValidator.ISOLanguageCode);
                            }
                        }
                    }
                    catch (System.Exception Exception)
                    {
                        throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
                    }
                }

                return STAATDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverSTAATDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto> DeliverKDTYPDtoList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<KDTYP> KDTYPList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto> KDTYPDtoList = null;

                using (DdOlExtended context = new DdOlExtended())
                {

                    KDTYPDtoList = context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto>("select * from kdtyp order by name").ToList();

                    //KDTYPList = MyFilterKDTYPList(context.Select<KDTYP>(null, null, KDTYP.FieldNames.NAME, 0, 0));


                }

                if (KDTYPList != null)
                {
                    // New list
                    KDTYPDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto>();
                    // New assembler
                    KDTYPAssembler KDTYPAssembler = new KDTYPAssembler();

                    foreach (KDTYP KDTYPLoop in KDTYPList)
                    {
                        Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto TempKDTYPDto = KDTYPAssembler.ConvertToDto(KDTYPLoop);

                        //if (KDTYPLoop != null)
                        //{
                        // Set SYSLAND
                        //     TempKDTYPDto = KDTYPLoop.LAND.SYSLAND;
                        // }

                        // Convert and add to the list
                        KDTYPDtoList.Add(TempKDTYPDto);
                    }
                }

                // Translate
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ServiceValidator.ISOLanguageCode) && KDTYPDtoList != null && KDTYPDtoList.Count > 0)
                {
                    try
                    {
                        using (DdCtExtended context = new DdCtExtended())
                        {
                            if ( CTTWEBHelper.hasTranslations(context))
                            foreach (KDTYPDto KDTYPDtoLoop in KDTYPDtoList)
                            {
                                KDTYPDtoLoop.NAME =  CTTWEBHelper.DeliverTranslation(context, KDTYPDtoLoop.NAME, ServiceValidator.ISOLanguageCode);
                            }
                        }
                    }
                    catch (System.Exception Exception)
                    {
                        throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
                    }
                }

                return KDTYPDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverKDTYPDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto> DeliverCTLANGDtoList()
        {
            List<CTLANG> CTLANGList = null;
            List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto> CTLANGDtoList = null;

            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto>("select * from ctlang order by languagename").ToList();
                    //CTLANGList = context.Select<Cic.OpenLease.Model.DdOw.CTLANG>(null, null, Cic.OpenLease.Model.DdOw.CTLANG.FieldNames.LANGUAGENAME, 0, 0);
                }

               /* if (CTLANGList != null)
                {
                    // New list
                    CTLANGDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto>();
                    // New assembler
                    CTLANGAssembler CTLANGAssembler = new CTLANGAssembler();

                    foreach (Cic.OpenLease.Model.DdOw.CTLANG CTLANGLoop in CTLANGList)
                    {
                        // Convert and add to the list
                        CTLANGDtoList.Add(CTLANGAssembler.ConvertToDto(CTLANGLoop));
                    }
                }*/

                //return CTLANGDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverCTLANGDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto> DeliverBRANCHEDtoList()
        {
            List<BRANCHE> BRANCHEList = null;
            List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto> BRANCHEDtoList = null;

            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    //BRANCHEList = context.Select<BRANCHE>(null, null, BRANCHE.FieldNames.BEZEICHNUNG, 0, 0);
                    String filter = AppConfig.getValueFromDb("SETUP.NET", "SEARCH","FILTER_BRANCHE", "1=1");// schluessel not in ('97A', '97B')
                    BRANCHEDtoList = context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto>("select * from branche where "+filter+" order by bezeichnung").ToList();
                }

                /*
                if (BRANCHEList != null)
                {
                    // New list
                    BRANCHEDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto>();
                    // New assembler
                    BRANCHEAssembler BRANCHEAssembler = new BRANCHEAssembler();

                    foreach (BRANCHE BRANCHELoop in BRANCHEList)
                    {
                        // Convert and add to the list
                        BRANCHEDtoList.Add(BRANCHEAssembler.ConvertToDto(BRANCHELoop));
                    }
                }*/

                // Translate
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ServiceValidator.ISOLanguageCode) && BRANCHEDtoList != null && BRANCHEDtoList.Count > 0)
                {
                    try
                    {
                        using (DdCtExtended context = new DdCtExtended())
                        {
                            if ( CTTWEBHelper.hasTranslations(context))
                            foreach (BRANCHEDto BRANCHEDtoLoop in BRANCHEDtoList)
                            {
                                BRANCHEDtoLoop.BEZEICHNUNG = CTTWEBHelper.DeliverTranslation(context, BRANCHEDtoLoop.BEZEICHNUNG, ServiceValidator.ISOLanguageCode);
                            }
                        }
                    }
                    catch (System.Exception Exception)
                    {
                        throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, ExceptionUtil.DeliverFlatExceptionMessage(Exception));
                    }
                }

                return BRANCHEDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBRANCHEDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> DeliverPLZDtoList()
        {

            try
            {
                List<PLZ> PLZList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> PLZDtoList = null;

                using (DdOlExtended context = new DdOlExtended())
                {
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto>("select * from plz").ToList();
                    //PLZList = context.Select<PLZ>(null, null, null, 0, 0);
                }

               /* if (PLZList != null)
                {
                    // New list
                    PLZDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto>();
                    // New assembler
                    PLZAssembler PLZAssembler = new PLZAssembler();

                    foreach (PLZ PLZLoop in PLZList)
                    {
                        // Convert and add to the list
                        PLZDtoList.Add(PLZAssembler.ConvertToDto(PLZLoop));
                    }
                }

                return PLZDtoList;*/
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPLZDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> DeliverBLZDtoList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<BLZ> BLZList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> BLZDtoList = null;

                using (DdOlExtended context = new DdOlExtended())
                {
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto>("select * from blz order by sysblz").ToList();
                    //BLZList = context.Select<BLZ>(null, null, BLZ.FieldNames.SYSBLZ, 0, 0);
                }

               /* if (BLZList != null)
                {
                    // New list
                    BLZDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto>();
                    // New assembler
                    BLZAssembler BLZAssembler = new BLZAssembler();

                    foreach (BLZ PLZLoop in BLZList)
                    {
                        // Convert and add to the list
                        BLZDtoList.Add(BLZAssembler.ConvertToDto(PLZLoop));
                    }
                }

                return BLZDtoList;*/
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBLZDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto> DeliverBERUFDtoList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<VC_BERUF> VC_BERUFList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto> BERUFDtoList = null;

                using (DdOlExtended context = new DdOlExtended())
                {
                   // VC_BERUFList = context.Select < VC_BERUF>(null, null, VC_BERUF.FieldNames.RANK, 0, 0);
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto>("select * from CIC.VC_BERUF order by rank").ToList();
                }

              /*  if (VC_BERUFList != null)
                {
                    // New list
                    BERUFDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto>();
                    // New assembler
                    BERUFAssembler BERUFAssembler = new BERUFAssembler();

                    foreach (VC_BERUF VC_BERUFLoop in VC_BERUFList)
                    {
                        // Convert and add to the list
                        BERUFDtoList.Add(BERUFAssembler.ConvertToDto(VC_BERUFLoop));
                    }
                }

                return BERUFDtoList;*/
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBERUFDtoListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }


        /// <summary>
        /// Delivers all RECHTSFORMCODE entries
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverRECHTSFORMList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto>("select * from CIC.DDLKPPOS where code='RECHTSFORMCODE' order by domainid,rank").ToList();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDictionaryFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Delivers all HREGISTERART entries
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverHREGISTERARTList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto>("select p1.ID, p2.domainid, p1.code, p1.value,p1.tooltip from ddlkppos p1, ddlkppos p2 where p1.code='REGISTERART' and p2.code='REGISTERART_REFO' and p2.value=p1.value order by p1.rank").ToList();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDictionaryFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Generic ddlkppos list get method
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverDDLKPPOSList(String code, String domainid)
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    String query = "select* from CIC.DDLKPPOS where code =:p1 order by domainid, rank";
                    List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                    
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = code });
                    if (domainid != null)
                    {
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = domainid });
                        query = "select* from CIC.DDLKPPOS where code =:p1 and domainid=:p2 order by domainid, rank";
                    }

                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto>(query,par.ToArray()).ToList();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDictionaryFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// Delivers all VARTs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverVARTList()
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                 
                    return context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto>("select sysvart ID, code, bezeichnung value from vart").ToList();
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDictionaryFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> SearchORT(string plz)
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<PLZ> PLZList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> PLZDtoList = null;

                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(plz))
                {
                    throw new Exception("plz");
                }

                using (DdOlExtended context = new DdOlExtended())
                {
                    PLZList = RestOfTheHelpers.SearchPlz(context, plz);
                }

                if (PLZList != null)
                {
                    // New list
                    PLZDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto>();
                    // New assembler
                    PLZAssembler PLZAssembler = new PLZAssembler();

                    foreach (PLZ PlzLoop in PLZList)
                    {
                        // Add
                        PLZDtoList.Add(PLZAssembler.ConvertToDto(PlzLoop));
                    }
                }

                return PLZDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SearchORTFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> SearchBANKNAME(string blz, string bic)
        {
            // No validateion Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, false);

            try
            {
                List<BLZ> BLZList = null;
                List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> BLZDtoList = null;

                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(blz) && Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(bic))
                {
                    throw new Exception("blz, bic");
                }

                using (DdOlExtended context = new DdOlExtended())
                {
                    BLZList = RestOfTheHelpers.SearchBankname(context, blz, bic);
                }

                if (BLZList != null)
                {
                    // New list
                    BLZDtoList = new List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto>();
                    // New assembler
                    BLZAssembler BLZAssembler = new BLZAssembler();

                    foreach (BLZ BlzLoop in BLZList)
                    {
                        // Add
                        BLZDtoList.Add(BLZAssembler.ConvertToDto(BlzLoop));
                    }
                }

                return BLZDtoList;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SearchBANKNAMEFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }
        #endregion

        #region My methods
        private System.Collections.Generic.List<LAND> MyFilterLandList(System.Collections.Generic.List<LAND> list)
        {
            System.Collections.Generic.List<LAND> LandList = new System.Collections.Generic.List<LAND>();

            foreach (LAND LandLoop in list)
            {
                // Filter out emptys
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(LandLoop.COUNTRYNAME))
                {
                    // filter out languages dummies
                    if (!LandLoop.COUNTRYNAME.StartsWith(" "))
                    {
                        LandList.Add(LandLoop);
                    }
                }
            }

            return LandList;
        }

        private System.Collections.Generic.List<LAND> MyFilterLandAddressList(System.Collections.Generic.List<LAND> list)
        {
            System.Collections.Generic.List<LAND> LandList = new System.Collections.Generic.List<LAND>();

            foreach (LAND LandLoop in list)
            {
                // Filter out emptys
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(LandLoop.COUNTRYNAME))
                {
                    // filter out languages dummies
                    if (LandLoop.NOTADDRESSRELEVANT != 1)
                    {
                        LandList.Add(LandLoop);
                    }
                }
            }

            return LandList;
        }

        private System.Collections.Generic.List<KDTYP> MyFilterKDTYPList(System.Collections.Generic.List<KDTYP> list)
        {
            System.Collections.Generic.List<KDTYP> KdtypList = new System.Collections.Generic.List<KDTYP>();

            foreach (KDTYP KDTYPLoop in list)
            {
                // Filter out emptys
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(KDTYPLoop.NAME))
                {
                    if(KDTYPLoop.ACTIVEFLAG != 0)
                        KdtypList.Add(KDTYPLoop);
                }
            }

            return KdtypList;
        }

        private System.Collections.Generic.List<STAAT> MyFilterStaatList(System.Collections.Generic.List<STAAT> list)
        {
            System.Collections.Generic.List<STAAT> StaatList = new System.Collections.Generic.List<STAAT>();

            foreach (STAAT StaatLoop in list)
            {
                // Filter out emptys
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(StaatLoop.STAAT1))
                {
                    StaatList.Add(StaatLoop);
                }
            }

            return StaatList;
        }

        private string MyGetCfgEntryValue(string entryCode, List<CFGVAR> entries, bool optional)
        {
            // Query entries
            var CurrentEntry = (from Entry in entries
                                where Entry.CODE == entryCode
                                select Entry).FirstOrDefault();

            // Check if the entry exists
            if (CurrentEntry == null || string.IsNullOrEmpty(CurrentEntry.WERT))
            {
                // Check if the value is optional
                if (optional)
                {
                    // Return null value
                    return null;
                }

                // Throw an exception
                throw new Exception("Value for \"" + entryCode + "\" not found.");
            }

            // Return the value
            return CurrentEntry.WERT;
        }
        #endregion
    }
}
