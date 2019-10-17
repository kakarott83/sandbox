using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Cic.OpenOne.Common.Util.Collection;

using Cic.OpenOne.Common.Util.Logging;

using Cic.OpenLease.Service.Versicherung;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.ServiceAccess.Merge.MembershipProvider;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using Cic.OpenOne.Common.Model.DdEurotax;

namespace Cic.OpenLease.Service.Services.DdOl
{
    /// <summary>
    /// Manages all Vehicle-Specific data like
    /// *tech data
    /// *vehicle context data
    /// *categories
    /// *kinds
    /// *prices
    /// </summary>
    public class VehicleDao
    {
        private const string CnstEtgTypeImportTableName = "ETGTYPE";
        private const string CnstExcelTypeImportTableName = "EXCEL";
        
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, OBARTDto[]> obartcache = CacheFactory<String, OBARTDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, OBKATDto[]> obkatcache = CacheFactory<String, OBKATDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, String> herstellerCache = CacheFactory<long, String>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, OBTYPDto> obTypCache2 = CacheFactory<long, OBTYPDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, OBTYPDto> obTypCache = CacheFactory<String, OBTYPDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private OBARTDto[] MyDeliverObArts(DdOlExtended context, bool isFromSa3, string hersteller, bool isMotorrad, int contractext)
        {
            String key = hersteller + "_" + isFromSa3 + "_" + isMotorrad+"_"+contractext;
            if (!obartcache.ContainsKey(key))
            {
                obartcache[key] = MyDeliverObArtsInternal(context, isFromSa3, hersteller, isMotorrad, contractext);
            }
            return obartcache[key];
        }

        private OBARTDto[] MyDeliverObArtsInternal(DdOlExtended context, bool isFromSa3, string hersteller, bool isMotorrad, int contractext)
        {
            List<OBARTDto> ObArtDtoList;

            try
            {

               /* hersteller = hersteller.ToLower();
                if (!"bmw".Equals(hersteller) && !"mini".Equals(hersteller))
                    hersteller = "fremdmarke";

                String motfilter = " and lower(trim(id)) not like '%motorrad%' ";
                if (isMotorrad)
                    motfilter = " and lower(trim(id))  like '%motorrad%' ";

                string query = "select distinct sysobart name from bmwkurz left outer join obart on kennung=name where bereichid='OB_KATEGORIE'  and lower(trim(id)) like lower(trim('" + hersteller + "'))||'%' " + motfilter;

                List<long> sysobarts = context.ExecuteStoreQuery<long>(query, null).ToList<long>();
                */
                // Declare the query result
                IQueryable<OBART> ObArts;

                // Check if it's for SA3
                //if (isFromSa3)
                {
                    // Query OBART
                    ObArts = from ObArt in context.OBART
                             where (ObArt.TYP == OBARTHelper.CnstObArtNew || ObArt.TYP == OBARTHelper.CnstObArtVorfuehr || ObArt.TYP == OBARTHelper.CnstObArtUsed) 
                             //&& sysobarts.Contains(ObArt.SYSOBART)
                             orderby ObArt.RANK
                             select ObArt;
                }
                /*else
                {
                    // Query OBART
                    ObArts = from ObArt in context.OBART
                             where sysobarts.Contains(ObArt.SYSOBART)
                             orderby ObArt.RANK
                             select ObArt;
                }*/


                // Create the result list
                ObArtDtoList = new List<OBARTDto>();

                // New OBARTAssembler
                OBARTAssembler Assembler = new OBARTAssembler();

                // Loop through all ObArts found
                foreach (OBART LoopObArt in ObArts)
                {
                    if (contractext > 0)//Filter Für Verlängerungsprodukte
                    {
                        if (LoopObArt.TYP.Value != 1) continue;//Nur Gebrauchte
                        
                    }
                    OBARTDto dto = Assembler.ConvertToDto(LoopObArt);
                    // Convert and add to the list
                    ObArtDtoList.Add(dto);
                    // Get ObKat
                    dto.ObjectCategories = MyDeliverObKats(context, hersteller, dto.NAME, isMotorrad,contractext);
                }


            }
            catch (System.Exception e)
            {
                // Throw an exception
                throw new System.ApplicationException("Could not get ObArt list", e);
            }

            // Return the list as an array
            return ObArtDtoList.ToArray();
        }

        
        private OBKATDto[] MyDeliverObKats(DdOlExtended context, string hersteller, string obart, bool isMotorrad, int contractext)
        {
            String key = hersteller + "_" + obart + "_" + isMotorrad+"_"+contractext;
            if (!obkatcache.ContainsKey(key))
            {
                obkatcache[key] = MyDeliverObKatsInternal(context, hersteller, obart, isMotorrad, contractext);
            }
            return obkatcache[key];
        }

        private OBKATDto[] MyDeliverObKatsInternal(DdOlExtended context, string hersteller, string obart, bool isMotorrad, int contractext)
        {
            // Create the list for ObKats
            List<OBKATDto> ObKatDtoList = new List<OBKATDto>();

            try
            {
                /*hersteller = hersteller.ToLower();
                if (!"bmw".Equals(hersteller) && !"mini".Equals(hersteller))
                    hersteller = "fremdmarke";

                String motfilter = " and lower(trim(id)) not like '%motorrad%' ";
                if (isMotorrad)
                    motfilter = " and lower(trim(id))  like '%motorrad%' ";

                string query = "select distinct sysobkat from bmwkurz left outer join obkat on (lower(trim(bezeichnung)) like '%'||lower(trim(name))||'%'   ) where kennung='" + obart + "' and  bereichid='OB_KATEGORIE'  and lower(trim(id)) like lower(trim('" + hersteller + "'))||'%' " + motfilter;

                List<long> sysobkats = context.ExecuteStoreQuery<long>(query, null).ToList<long>();
                */
                // Query OBKAT
                var Kats = from Kat in context.OBKAT
                           //where sysobkats.Contains(Kat.SYSOBKAT)
                           select Kat;

                // Loop through all selected Kats
                foreach (var LoopKat in Kats)
                {
                    if (contractext > 0)//Filter Für Verlängerungsprodukte
                    {
                        if (LoopKat.NAME.ToLower().Contains("premium selection ex")) continue; //Keine Premium Selection
                    }
                    // Create new OBKATDto
                    OBKATDto NewObKatDto = new OBKATDto();
                    NewObKatDto.NAME = LoopKat.NAME;
                    NewObKatDto.DESCRIPTION = LoopKat.DESCRIPTION;
                    NewObKatDto.SYSOBKAT = LoopKat.SYSOBKAT;

                    // Add ObKat to the list
                    ObKatDtoList.Add(NewObKatDto);
                }
            }
            catch (System.Exception e)
            {
                // Throw an exception
                throw new System.ApplicationException("ObKat list could not be delivered", e);
            }

            // Return the list as an array
            return ObKatDtoList.ToArray();
        }
        
        public string MyDeliverHersteller(DdOlExtended context, long sysObTyp, string name)
        {
            if (!herstellerCache.ContainsKey(sysObTyp))
            {
                herstellerCache[sysObTyp] = MyDeliverHerstellerInternal(  context, sysObTyp, name);
                if(herstellerCache[sysObTyp]==null) herstellerCache[sysObTyp]="";
            }
            return herstellerCache[sysObTyp];
        }
        
        private string MyDeliverHerstellerInternal(DdOlExtended context, long sysObTyp, string name)
        {
            var Query = from obtyp in context.OBTYP
                        where obtyp.SYSOBTYP == sysObTyp
                        select obtyp;

            OBTYP ObTyp = Query.FirstOrDefault<OBTYP>();

            if (ObTyp != null)
            {
                if (ObTyp.SYSOBTYPP.HasValue)
                {
                    return MyDeliverHerstellerInternal(context, ObTyp.SYSOBTYPP.GetValueOrDefault(0), ObTyp.BEZEICHNUNG);
                }
            }

            return name;
        }
        
       
        /// <summary>
        /// sets Objektarten in objectContext
        /// </summary>
        /// <param name="checkMultifranchise">when users'brand has more than one brand, hide the corresponding other brand</param>
        /// <param name="context">db context</param>
        /// <param name="ObjectContextDto"></param>
        /// <param name="syspuser">needed for multifranchise check only</param>
        /// <param name="contractext">extended contracts have other obarts</param>
        public void getObArts(bool checkMultifranchise, DdOlExtended context, ObjectContextDto ObjectContextDto, long syspuser, int contractext)
        {
            //string[] showonly = null;
            //string[] hide = null;
            /*if (checkMultifranchise)
            {
                Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[] userBrands = Cic.OpenLease.Common.MembershipProvider.listBrands(syspuser);
                if (userBrands != null && userBrands.Count() > 1)
                {
                    BRAND brand = BRANDHelper.DeliverBRAND(context, ObjectContextDto.sysBrand);
                    if (brand.NAME.ToLower().IndexOf("bmw") > -1)
                    {
                        showonly = new string[2];
                        showonly[0] = "bmw";
                        showonly[1] = "mini";
                    }
                    else if (brand.NAME.ToLower().IndexOf("alphera") > -1)
                    {
                        hide = new string[2];
                        hide[0] = "bmw";
                        hide[1] = "mini";
                    }

                }
            }*/
            ObjectContextDto.isMotorrad = HaftpflichtCalculator.isMotorrad(ObjectContextDto.ObjectType.SYSOBTYP, context);
            ObjectContextDto.ObjectArts = MyDeliverObArts(context, ObjectContextDto.isFromSa3, ObjectContextDto.Hersteller, ObjectContextDto.isMotorrad, contractext);
           /* if (checkMultifranchise)
            {
                if (showonly != null)
                {
                    if (ObjectContextDto.Hersteller.ToLower().IndexOf(showonly[0]) < 0 && ObjectContextDto.Hersteller.ToLower().IndexOf(showonly[1]) < 0)
                    {
                        ObjectContextDto.IsEurotaxNrValid = false;
                    }
                }
                else if (hide != null)
                {
                    if (ObjectContextDto.Hersteller.ToLower().IndexOf(hide[0]) > -1 || ObjectContextDto.Hersteller.ToLower().IndexOf(hide[1]) > -1)
                    {
                        ObjectContextDto.IsEurotaxNrValid = false;
                    }
                }
            }*/
        }
        
        private OBTYPDto MyDeliverObTyp(DdOlExtended context, long sysObTyp)
        {
            if (!obTypCache2.ContainsKey(sysObTyp))
            {
                obTypCache2[sysObTyp] = MyDeliverObTypInternal(context, sysObTyp);
            }
            return obTypCache2[sysObTyp];
        }

        private OBTYPDto MyDeliverObTypInternal(DdOlExtended context, long sysObTyp)
        {
            try
            {
                // Query OBTYP
                var ObTyp = (from Typ in context.OBTYP
                             where Typ.SYSOBTYP == sysObTyp
                             select Typ).FirstOrDefault();

                // Check if ObTyp is null
                if (ObTyp != null)
                {
                    // Create an assembler for ObTyp
                    OBTYPAssembler ObTypAssember = new OBTYPAssembler();

                    // Return converted ObTyp
                    OBTYPDto rval = ObTypAssember.ConvertToDto(ObTyp);

                    rval.FABRIKAT = context.ExecuteStoreQuery<String>("select bezeichnung from obtyp where importtable='ETGMODEL' start with sysobtyp=" + sysObTyp + "  connect by prior sysobtypp=sysobtyp", null).FirstOrDefault();
                    if (rval.FABRIKAT == null)
                    {
                        List<String> bezeichnungen = context.ExecuteStoreQuery<String>("select bezeichnung from obtyp  start with sysobtyp=" + sysObTyp + "  connect by prior sysobtypp=sysobtyp", null).ToList<String>();
                        if (bezeichnungen.Count > 1)
                            rval.FABRIKAT = bezeichnungen[1];
                    }

                    if (ObTyp.FZTYP == null)
                        context.Entry(ObTyp).Reference(f => f.FZTYP).Load();
                   
                    // Check if FzTyp exists
                    if (ObTyp.FZTYP != null)
                    {

                        if (ObTyp.FZTYP.LEISTUNG != null)
                            rval.NAME += ", " + ObTyp.FZTYP.LEISTUNG + " kW";
                    }

                    //select bezeichnung from obtyp where importtable='ETGMODEL' start with sysobtyp=119429  connect by prior sysobtypp=sysobtyp;
                    return rval;
                }
                else
                {
                    return null;
                }


            }
            catch (System.Exception e)
            {
                // Throw an exception adding inner exception
                throw new System.ApplicationException("ObTyp could not be delivered.", e);
            }

        }
        
        public OBTYPDto MyDeliverObTyp(DdOlExtended context, string eurotaxNr)
        {
            if (!obTypCache.ContainsKey(eurotaxNr))
            {
                obTypCache[eurotaxNr] = MyDeliverObTypInternal(context, eurotaxNr);
            }
            return obTypCache[eurotaxNr];
        }
        
        private OBTYPDto MyDeliverObTypInternal(DdOlExtended context, string eurotaxNr)
        {
            try
            {
                //manual obtyp: && Typ.NOEXTID ==1
                //Typ.IMPORTTABLE == CnstEtgTypeImportTableName (from eurotax)
                // Query OBTYP
                var obTyp = (from Typ in context.OBTYP
                             where Typ.SCHWACKE != null && Typ.SCHWACKE == eurotaxNr
                             && (Typ.IMPORTTABLE == CnstEtgTypeImportTableName || Typ.IMPORTTABLE == CnstExcelTypeImportTableName || Typ.NOEXTID == 1)
                             && (Typ.IMPORTSOURCE == null || Typ.IMPORTSOURCE == 0 || Typ.IMPORTSOURCE == 2)
                             //&& Typ.ACTIVEFLAG == 1
                             select Typ).FirstOrDefault();

                OBTYPDto rval = null;

                // Check if ObTyp is null
                if (obTyp != null)
                {
                    // Create an assembler for ObTyp
                    OBTYPAssembler ObTypAssember = new OBTYPAssembler();

                    // Return converted ObTyp
                    rval = ObTypAssember.ConvertToDto(obTyp);
                }
                 else
                 {
                     rval = new OBTYPDto();
                 }


                using (DdEurotaxExtended context2 = new DdEurotaxExtended())
                {
                    // Query ETGTYPE
                    var Types = from Type in context2.ETGTYPE
                                join Price in context2.ETGPRICE
                                on Type.NATCODE equals Price.NATCODE
                                join Make in context2.ETGMAKE
                                on Type.MAKCD equals Make.NATCODE
                                where Type.NATCODE == eurotaxNr &&
                                Make.VEHTYPE == Type.VEHTYPE
                                orderby Type.NAME, Type.IMPBEGIN descending
                                select new { Type, Price, Make };
                    var extInfo = Types.FirstOrDefault();
                    if (extInfo != null)
                    {
                        string Years = extInfo.Type.IMPBEGIN.Substring(0, 4);
                        if (extInfo.Type.IMPEND != null && extInfo.Type.IMPEND.Length >= 4)
                            Years += " - " + extInfo.Type.IMPEND.Substring(0, 4);
                        else
                            Years += "+";
                        rval.NAME = extInfo.Type.NAME + ", " + extInfo.Type.KW + " kW (" + Years + ")";
                    }

                    String reihe = (from Model in context2.ETGMODEL
                                    join t in context2.ETGTYPE
                                    on Model.NATCODE equals t.MODCD
                                    where t.NATCODE == eurotaxNr
                                    select Model.NAME).FirstOrDefault();
                    if (reihe != null)
                        rval.FABRIKAT = reihe;

                    if (reihe == null || extInfo == null)//eurotax not yet available, get from obtyps
                    {
                        // Ticket#2012120610000087 NullReferenceException
                        if (obTyp != null)
                        {
                            OBTYPDto tmpDto = MyDeliverObTyp(context, obTyp.SYSOBTYP);
                            if (tmpDto != null)
                            {
                                rval.FABRIKAT = tmpDto.FABRIKAT;
                                rval.NAME = tmpDto.NAME;
                            }
                        }
                    }
                }
                return rval;
            }
            catch (System.Exception e)
            {
                // Throw an exception adding inner exception
                throw new System.ApplicationException("ObTyp could not be delivered.", e);
            }
        }

        /// <summary>
        /// Liefert Hersteller/Fahrzeugtextinformationen
        /// used in AIDA-Gui:
        ///  * after manual eurotax-number 
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="isFromSa3"></param>
        /// <param name="checkMultifranchise"></param>
        /// <param name="sysBrand"></param>
        /// <param name="MembershipUserValidationInfo"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public ObjectContextDto deliverObjectContext(string eurotaxNr, bool isFromSa3, bool checkMultifranchise, long sysBrand, Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo, int contractext)
        {

            // Declare an object context DTO
            Cic.OpenLease.ServiceAccess.DdOl.ObjectContextDto ObjectContextDto = null;

            try
            {
                // Create the object context DTO
                ObjectContextDto = new ServiceAccess.DdOl.ObjectContextDto();

                eurotaxNr = eurotaxNr.Trim();
                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {




                    // Get the object type
                    ObjectContextDto.ObjectType = MyDeliverObTyp(context, eurotaxNr);
                    ObjectContextDto.isFromSa3 = isFromSa3;
                    ObjectContextDto.IsEurotaxNrValid = false;
                    ObjectContextDto.sysBrand = sysBrand;

                    if (ObjectContextDto.ObjectType != null)
                    {
                        if (ObjectContextDto.ObjectType.FABRIKAT == null)
                        {
                            OBTYPDto tmpDto = MyDeliverObTyp(context, ObjectContextDto.ObjectType.SYSOBTYP);
                            if (tmpDto != null)
                            {
                                ObjectContextDto.ObjectType.FABRIKAT = tmpDto.FABRIKAT;
                                ObjectContextDto.ObjectType.NAME = tmpDto.NAME;
                            }
                        }
                        // Get Fabrikat
                        ObjectContextDto.Fabrikat = ObjectContextDto.ObjectType.FABRIKAT;


                        // Get Hersteller 
                        ObjectContextDto.Hersteller = MyDeliverHersteller(context, ObjectContextDto.ObjectType.SYSOBTYP, ObjectContextDto.ObjectType.DESCRIPTION);
                        ObjectContextDto.IsEurotaxNrValid = true;

                        // Get ObArt
                        if (ObjectContextDto.Hersteller != null)
                            getObArts(checkMultifranchise, context, ObjectContextDto, checkMultifranchise?MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value:0, contractext);

                    }

                     if(ObjectContextDto.ObjectType!=null && ObjectContextDto.ObjectType.SYSOBTYP>0)
                    {
                        long isactive = context.ExecuteStoreQuery<long>("select flagaktiv from obtyp where sysobtyp=" + ObjectContextDto.ObjectType.SYSOBTYP, null).FirstOrDefault();
                         if(isactive==0)
                             ObjectContextDto.IsEurotaxNrValid = false;
                    }
                     try
                     {
                         ObjectContextDto.baujahrVon = context.ExecuteStoreQuery<DateTime>("select to_date(coalesce(baujahr,'1900')||'-'||coalesce(baumonat,'01')||'-01','yyyy-MM-dd') from cic.vc_obtyp5 where id5=" + ObjectContextDto.ObjectType.SYSOBTYP, null).FirstOrDefault();
                         
                     }catch(Exception )
                     {

                     }
                     try
                     {
                         ObjectContextDto.baujahrBis = context.ExecuteStoreQuery<DateTime>("select to_date(coalesce(baubisjahr,'2100')||'-'||coalesce(baubismonat,'12')||'-31','yyyy-MM-dd') from cic.vc_obtyp5 where id5=" + ObjectContextDto.ObjectType.SYSOBTYP, null).FirstOrDefault();
                     }
                     catch (Exception ) { }
                }
                
               

                // Retrn the object context DTO
                return ObjectContextDto;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverObjectContextFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }
        

        /// <summary>
        /// Liefert Hersteller/Fahrzeugtextinformationen
        /// used in AIDA-Gui:
        ///  * fill Kategorie-List (from getObjectArts)
        ///  * after carconfig or when from sa3
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="isFromSa3"></param>
        /// <param name="checkMultifranchise"></param>
        /// <param name="sysBrand"></param>
        /// <param name="info"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        public ObjectContextDto deliverObjectContextFromObTyp(long sysObTyp, bool isFromSa3, bool checkMultifranchise, long sysBrand, MembershipUserValidationInfo info, int contractext)
        {

            
                // Create the object context DTO
                ObjectContextDto ObjectContextDto = new ObjectContextDto();


                // Get database context and search for description
                using (DdOlExtended context = new DdOlExtended())
                {
                    // Get ObTyp
                    var CurrentObTyp = (from ObTyp in context.OBTYP
                                        where ObTyp.SYSOBTYP == sysObTyp
                                        //&& ObTyp.ACTIVEFLAG==1
                                        select ObTyp).FirstOrDefault();

                    // Check if anything was found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new Exception("Type not found: ObTyp: " + sysObTyp + ".");
                    }
                    // If possible get from schwacke
                    if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0 && CurrentObTyp.IMPORTTABLE == CnstEtgTypeImportTableName)
                    {
                        return deliverObjectContext(CurrentObTyp.SCHWACKE, isFromSa3, checkMultifranchise, sysBrand, info, contractext);
                    }


                    // Get the object type
                    ObjectContextDto.ObjectType = MyDeliverObTyp(context, sysObTyp);
                    ObjectContextDto.IsEurotaxNrValid = false;
                    ObjectContextDto.isFromSa3 = isFromSa3;
                    ObjectContextDto.sysBrand = sysBrand;

                    if (ObjectContextDto.ObjectType != null)
                    {
                        // Get Fabrikat
                        ObjectContextDto.Fabrikat = ObjectContextDto.ObjectType.FABRIKAT;

                        // Get Hersteller 
                        ObjectContextDto.Hersteller = MyDeliverHersteller(context, ObjectContextDto.ObjectType.SYSOBTYP, ObjectContextDto.ObjectType.DESCRIPTION);
                        ObjectContextDto.IsEurotaxNrValid = true;

                        // Get ObArt
                        if (ObjectContextDto.Hersteller != null)
                            getObArts(checkMultifranchise, context, ObjectContextDto,checkMultifranchise? info.PUSERDto.SYSPUSER.Value:0, contractext);
                    }

                }

                // Retrn the object context DTO
                return ObjectContextDto;
         
        }

        /// <summary>
        /// Used from AIDA Gui to determine technical Data, called once per vehicle
        /// vehicle may be selected through cc, manual et or sa3
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="technicalDataDto"></param>
        /// <returns></returns>
        public TechnicalDataDto deliverTechnicalDataExtendedFromObTyp(long sysObTyp, long sysObArt, Cic.OpenLease.ServiceAccess.DdOl.TechnicalDataDto technicalDataDto, long sysperole)
        {
           
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Get ObTyp
                    var CurrentObTyp = (from ObTyp in Context.OBTYP
                                        where ObTyp.SYSOBTYP == sysObTyp
                                        select ObTyp).FirstOrDefault();

                    // Check if anything was found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new Exception("Type not found: ObTyp: " + sysObTyp + ".");
                    }
                    
                    NoVA nv = new NoVA(Context);
                    // If possible get from schwacke
                    if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0 && CurrentObTyp.IMPORTTABLE == CnstEtgTypeImportTableName)
                    {
                        try
                        {
                            // Fix for SA3 Bugs fetch in Netvision BmwTechnicalDataDto.fetchListenpreis = true;//Fix #4103
                            technicalDataDto.sysobtyp = sysObTyp;
                            nv.fetchTechnicalDataFromEurotax(CurrentObTyp.SCHWACKE, sysObArt, technicalDataDto, sysperole);
                        }
                        catch (Exception e)
                        {
                            _Log.Warn("Schwacke-Values for obtyp not found, using FS-Tables: OBtyp:" + sysObTyp, e);
                            nv.fetchTechnicalDataFromFzTyp(technicalDataDto, CurrentObTyp, sysperole,Context);
                        }
                    }
                    else nv.fetchTechnicalDataFromFzTyp(technicalDataDto, CurrentObTyp, sysperole,Context);

                    decimal Ust = LsAddHelper.getGlobalUst(sysperole);

                    //Gebrauchtwagen überschreibt immer Novabefreiungs-Flag für die Berechnung
                  //  bool nonova = OBARTHelper.isOfType(Context, (long)sysObArt, OBARTHelper.CnstObArtUsed);
                   // nv.calculateNova(technicalDataDto, Ust, nonova,DateTime.Now);//TODO maybe missing lieferdatum, depends on test

                    technicalDataDto.isMotorrad = HaftpflichtCalculator.isMotorrad(CurrentObTyp.SYSOBTYP, Context);
                    technicalDataDto.SYSMART = nv.getAntriebsartId(CurrentObTyp.SYSOBTYP);
                    technicalDataDto.isBMWi = false;
                    /*if (technicalDataDto.Antriebsart == FuelTypeConstants.Electricity)
                    {
                         BRAND brand = BRANDHelper.DeliverBRAND(Context, DdOlExtended.getKey(CurrentObTyp.BRANDReference.EntityKey));
                         if (brand.NAME.ToLower().IndexOf("bmw") > -1)
                         {
                             Cic.OpenLease.Model.DdOiqueue.CfgSingleton CfgSingleton = Cic.OpenLease.Model.DdOiqueue.CfgSingleton.Instance;
                             String regex = CfgSingleton.GetEntry("GENERAL", "BMWIREGEX", "i3", "AIDA");
                             Regex iDrive = new Regex(regex);
                             if(iDrive.IsMatch(CurrentObTyp.BEZEICHNUNG))
                                technicalDataDto.isBMWi = true;//Electric and BMW -> BMWi
                         }
                    }*/
                    return technicalDataDto;
                }
         
        }

        private bool checkExtraSum(decimal maxadd, PurchasePriceDto purchasePriceDto)
        {
            
            decimal curSumExtras =  purchasePriceDto.HerstellerzubehorBrutto + purchasePriceDto.HandlerzubehorBrutto + purchasePriceDto.ueberfuehrungskostenBrutto + purchasePriceDto.zulassungskostenBrutto;
            decimal maxLP = purchasePriceDto.PaketeBrutto + purchasePriceDto.SonderausstattungExternBrutto + purchasePriceDto.ListenpreisBrutto;
            decimal maxSumExtras = maxLP / 100.0M * maxadd;
            if (curSumExtras > maxSumExtras)
                return false;
            return true;
        }
		/// <summary>
		/// Delivers and calculates all values for the price-gui
		/// </summary>
		/// <param name="purchasePriceDto"></param>
		/// <param name="sysperole"></param>
		/// <returns></returns>
		public PurchasePriceDto deliverPurchasePrice(PurchasePriceDto purchasePriceDto, long sysperole)
        {
            // Get the tax rate
            decimal Ust = LsAddHelper.getGlobalUst(sysperole);
            decimal Ust2 = Ust;
            if (purchasePriceDto.ANGOBERINKLMWST.HasValue && purchasePriceDto.ANGOBERINKLMWST == 0)
                Ust2 = 0;

            decimal maxadd = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_B2B_MAX_ADD_Listenpreis);
               
            //always fetch original lp(netto) and calculate brutto, then assign netto from erinklmwst flag
            if(purchasePriceDto.ListenpreisBrutto<=0 && purchasePriceDto.CONFIGSOURCE!=ServiceAccess.OfferTypeConstants.VAP)
            { 
                decimal grund = 0;
                decimal sonderminderung = 0;
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    grund = ctx.ExecuteStoreQuery<decimal>("select etgprice.np2 from etgprice,obtyp where obtyp.schwacke=etgprice.natcode and obtyp.sysobtyp=" + purchasePriceDto.SysObTyp, null).FirstOrDefault();

                    if (grund == 0)//FZTYP
                        grund = ctx.ExecuteStoreQuery<decimal>("select fztyp.grund from fztyp,obtyp where fztyp.sysfztyp=obtyp.sysfztyp and obtyp.sysobtyp=" + purchasePriceDto.SysObTyp, null).FirstOrDefault();
                    purchasePriceDto.ListenpreisNettoNetto = grund;
                           
                }
                    
                NovaType nt = new NovaType(Ust, purchasePriceDto.NovaSatz, NovaType.fetchNovaQuote(), sonderminderung);
                nt.setNetto(grund);
                purchasePriceDto.ListenpreisBrutto = nt.bruttoInklNova;
                purchasePriceDto.ListenpreisBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.ListenpreisBrutto);
                //bei gebraucht initial den rabatt auf 100 stellen
                if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.Initialize && purchasePriceDto.SysObArt == 13
                    && purchasePriceDto.HerstellerzubehorBrutto == 0 && purchasePriceDto.RabattfaehigbetragRabattP==0)
                {
                    purchasePriceDto.RabattfaehigbetragRabattP = 100;
                    purchasePriceDto.RabattfaehigbetragRabattBrutto = purchasePriceDto.ListenpreisBrutto;
                }
            }
            else if (purchasePriceDto.CONFIGSOURCE == ServiceAccess.OfferTypeConstants.CarConfigurator)
            {
                if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.Initialize && purchasePriceDto.SysObArt == 13
                    && purchasePriceDto.HerstellerzubehorBrutto == 0 && purchasePriceDto.RabattfaehigbetragRabattP == 0)
                {
                    purchasePriceDto.RabattfaehigbetragRabattP = 100;
                    purchasePriceDto.RabattfaehigbetragRabattBrutto = purchasePriceDto.ListenpreisBrutto;
                }
            }

            if (purchasePriceDto.CONFIGSOURCE!=ServiceAccess.OfferTypeConstants.VAP && purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.KaufpreisExternBrutto && purchasePriceDto.SysObArt == 13)
            {
                purchasePriceDto.HerstellerzubehorBrutto = 0;
                if (purchasePriceDto.KaufpreisExternBrutto > purchasePriceDto.ListenpreisBrutto)
                {
                    purchasePriceDto.HerstellerzubehorBrutto = purchasePriceDto.KaufpreisExternBrutto - purchasePriceDto.ListenpreisBrutto;
                }
                if(purchasePriceDto.KaufpreisExternBrutto<1)
                {
                    purchasePriceDto.Message += "Erfassen Sie einen Kaufpreis" + System.Environment.NewLine;
                    purchasePriceDto.KaufpreisExternBrutto = purchasePriceDto.ListenpreisBrutto;
                }
            }
            


            NovaType nt2 = new NovaType(Ust2, 0, 0, 0);
            nt2.setBruttoInklNova(purchasePriceDto.ListenpreisBrutto);
            purchasePriceDto.ListenpreisNettoNetto = nt2.netto;


			// Reject negative values ONLY on "in-field" edit in "Nettobeträge linke Spalte"	(rh: 20180626)
			if (purchasePriceDto.CalculationSource == PurchasePriceDto.CalculationSources.PaketeBrutto)             // ONLY on "in-field" edit
			{
				if (purchasePriceDto.PaketeBrutto < 0)
					purchasePriceDto.PaketeBrutto = 0;
				if (purchasePriceDto.ueberfuehrungskostenBrutto < 0)
					purchasePriceDto.ueberfuehrungskostenBrutto = 0;
				if (purchasePriceDto.zulassungskostenBrutto < 0)
					purchasePriceDto.zulassungskostenBrutto = 0;
			}
			if (purchasePriceDto.CalculationSource == PurchasePriceDto.CalculationSources.HerstellerzubehorBrutto)  // ONLY on "in-field" edit
			{
				if (purchasePriceDto.HerstellerzubehorBrutto < 0)
					purchasePriceDto.HerstellerzubehorBrutto = 0;
			}
			if (purchasePriceDto.CalculationSource == PurchasePriceDto.CalculationSources.HandlerzubehorBrutto)     // ONLY on "in-field" edit
			{
				if (purchasePriceDto.HandlerzubehorBrutto < 0)
					purchasePriceDto.HandlerzubehorBrutto = 0;
			}

			//Sonderausstattung
			if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.Initialize || purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.SonderausstattungBrutto)
            {
                if (purchasePriceDto.SonderausstattungBrutto < 0) purchasePriceDto.SonderausstattungBrutto = 0;
                purchasePriceDto.SonderausstattungBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.SonderausstattungBrutto);

                if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.Initialize)
                    purchasePriceDto.SonderausstattungRabattOP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(KalkulationHelper.CalculateRabattoP(purchasePriceDto.SonderausstattungBrutto, purchasePriceDto.SonderausstattungRabattO));

                //Check SonderausstattungBrutto
                if (purchasePriceDto.SonderausstattungBrutto < 0)
                {
                    purchasePriceDto.Message += "SonderausstattungBrutto is below 0." + System.Environment.NewLine;
                    purchasePriceDto.SonderausstattungBrutto = 0;
                    //return purchasePriceDto;
                }

                if (purchasePriceDto.SonderausstattungBrutto < purchasePriceDto.SonderausstattungUser)
                    purchasePriceDto.SonderausstattungBrutto = purchasePriceDto.SonderausstattungUser;

                purchasePriceDto.SonderausstattungDefault = purchasePriceDto.SonderausstattungBrutto - purchasePriceDto.SonderausstattungUser;



                purchasePriceDto.SonderausstattungRabattO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(KalkulationHelper.CalculateRabattO(purchasePriceDto.SonderausstattungBrutto, purchasePriceDto.SonderausstattungRabattOP));
                purchasePriceDto.SonderausstattungExternBrutto = KalkulationHelper.CalculateExternBrutto(purchasePriceDto.SonderausstattungBrutto, purchasePriceDto.SonderausstattungRabattO);

            }
            if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.SonderausstattungUser)
            {
                purchasePriceDto.SonderausstattungBrutto = purchasePriceDto.SonderausstattungDefault + purchasePriceDto.SonderausstattungUser;
                //Check SonderausstattungBrutto
                if (purchasePriceDto.SonderausstattungBrutto < 0)
                {
                    purchasePriceDto.Message += "SonderausstattungBrutto is below 0." + System.Environment.NewLine;
                    purchasePriceDto.SonderausstattungBrutto = 0;
                    //return purchasePriceDto;
                }
                purchasePriceDto.SonderausstattungRabattO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(KalkulationHelper.CalculateRabattO(purchasePriceDto.SonderausstattungBrutto, purchasePriceDto.SonderausstattungRabattOP));
                purchasePriceDto.SonderausstattungExternBrutto = KalkulationHelper.CalculateExternBrutto(purchasePriceDto.SonderausstattungBrutto, purchasePriceDto.SonderausstattungRabattO);
            }

            if (!checkExtraSum(maxadd, purchasePriceDto))
            {
                if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.HerstellerzubehorBrutto)
                {
                    purchasePriceDto.HerstellerzubehorBrutto = 0;
                    purchasePriceDto.HerstellerzubehorExternBrutto = 0;
                }
                else if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.KaufpreisExternBrutto&&purchasePriceDto.SysObArt==13)
                {
                    purchasePriceDto.HerstellerzubehorBrutto = 0;
                    purchasePriceDto.HerstellerzubehorExternBrutto = 0;
                }
                else if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.HandlerzubehorBrutto)
                {
                    purchasePriceDto.HandlerzubehorBrutto = 0;
                    purchasePriceDto.HandlerzubehorExternBrutto = 0;
                }
                else 
                {
                    if (purchasePriceDto.zulassungskostenBrutto > purchasePriceDto.ueberfuehrungskostenBrutto)
                    {
                        purchasePriceDto.zulassungskostenBrutto = 0;
                           
                    }
                    else purchasePriceDto.ueberfuehrungskostenBrutto = 0;
                }
                purchasePriceDto.Message += "Die Summe aller Zusatzkosten übersteigt " + maxadd + "% von Listenpreis+Pakete+Sonderausstattung" + System.Environment.NewLine;
                    
            }

            //Nettobeträge linke Spalte
            decimal paketeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.PaketeBrutto, Ust2);
            decimal HerstellerzubehorNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.HerstellerzubehorBrutto, Ust2);
            decimal ListenpreisNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.ListenpreisBrutto, Ust2);
            decimal SonderausstattungNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.SonderausstattungBrutto, Ust2);
            decimal zulassungskostenNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.zulassungskostenBrutto, Ust);
            decimal ueberfuehrungskostenNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.ueberfuehrungskostenBrutto, Ust);
            decimal HandlerzubehorNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.HandlerzubehorBrutto, Ust);

            //Rabattierfähiger Betrag
            purchasePriceDto.RabattfaehigbetragBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.ListenpreisBrutto + purchasePriceDto.PaketeBrutto + purchasePriceDto.SonderausstattungBrutto);

            //Summe linke Spalte
            purchasePriceDto.KaufpreisBrutto = (purchasePriceDto.HandlerzubehorBrutto + purchasePriceDto.HerstellerzubehorBrutto + purchasePriceDto.ListenpreisBrutto + purchasePriceDto.PaketeBrutto + purchasePriceDto.SonderausstattungBrutto  + purchasePriceDto.zulassungskostenBrutto + purchasePriceDto.ueberfuehrungskostenBrutto);
            purchasePriceDto.Kaufpreis = (paketeNetto + HerstellerzubehorNetto + ListenpreisNetto + SonderausstattungNetto + zulassungskostenNetto + ueberfuehrungskostenNetto + HandlerzubehorNetto);

            //die prozent immer aktualisieren, es könnte sich ja sonderzub oder paket geändert und damit rabattfähig geändert haben, ausser die p werden direkt getippt
            if (purchasePriceDto.CalculationSource != ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.RabattfaehigBetragRabattP)
                purchasePriceDto.RabattfaehigbetragRabattP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(KalkulationHelper.CalculateRabattoP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattBrutto));

            //wenn änderung rabattierfähigbetrag/prozent
            if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.RabattfaehigBetragRabattP || purchasePriceDto.RabattfaehigbetragRabattP < 0 || purchasePriceDto.RabattfaehigbetragRabattP>100)
            {
                if (purchasePriceDto.RabattfaehigbetragRabattP > 100) purchasePriceDto.RabattfaehigbetragRabattP = 100;
                if (purchasePriceDto.RabattfaehigbetragRabattP < 0) purchasePriceDto.RabattfaehigbetragRabattP = 0;
                purchasePriceDto.RabattfaehigbetragRabattP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(purchasePriceDto.RabattfaehigbetragRabattP);


                purchasePriceDto.RabattfaehigbetragExtern = KalkulationHelper.CalculateBruttoFromRabattOP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattP);
                purchasePriceDto.RabattfaehigbetragRabattBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(KalkulationHelper.CalculateRabattO(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattP));
            }
                
            //wenn änderung rabattierfähigbetrag
            if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.RabattfaehigBetragRabatt)
            {
                if (purchasePriceDto.RabattfaehigbetragRabattBrutto < 0) purchasePriceDto.RabattfaehigbetragRabattBrutto = 0;
                if (purchasePriceDto.RabattfaehigbetragRabattBrutto > purchasePriceDto.RabattfaehigbetragBrutto) purchasePriceDto.RabattfaehigbetragRabattBrutto = purchasePriceDto.RabattfaehigbetragBrutto;
                purchasePriceDto.RabattfaehigbetragRabattBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.RabattfaehigbetragRabattBrutto);

                purchasePriceDto.RabattfaehigbetragRabattP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(KalkulationHelper.CalculateRabattoP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattBrutto));
                purchasePriceDto.RabattfaehigbetragExtern = KalkulationHelper.CalculateBruttoFromRabattOP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattP);
            }
            //wenn änderung endpreis
            else if (purchasePriceDto.CalculationSource == ServiceAccess.DdOl.PurchasePriceDto.CalculationSources.KaufpreisExternBrutto)
            {
                    
                decimal gesamtrabatt = purchasePriceDto.KaufpreisBrutto - purchasePriceDto.KaufpreisExternBrutto;
                if (gesamtrabatt < 0) gesamtrabatt = 0;
                if (gesamtrabatt > purchasePriceDto.RabattfaehigbetragBrutto) gesamtrabatt = purchasePriceDto.RabattfaehigbetragBrutto;
                purchasePriceDto.RabattfaehigbetragRabattBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(gesamtrabatt);

                purchasePriceDto.RabattfaehigbetragRabattP = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundInterest(KalkulationHelper.CalculateRabattoP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattBrutto));
                purchasePriceDto.RabattfaehigbetragExtern = KalkulationHelper.CalculateBruttoFromRabattOP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattP);
            }
            purchasePriceDto.RabattfaehigbetragExtern = KalkulationHelper.CalculateBruttoFromRabattOP(purchasePriceDto.RabattfaehigbetragBrutto, purchasePriceDto.RabattfaehigbetragRabattP);

            //summe unten rechts (endpreis nach rabatt)
            purchasePriceDto.RabattfaehigbetragRabatt = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(purchasePriceDto.RabattfaehigbetragRabattBrutto, Ust2);
            purchasePriceDto.KaufpreisExtern = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.Kaufpreis - purchasePriceDto.RabattfaehigbetragRabatt);
            purchasePriceDto.KaufpreisExternBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.KaufpreisBrutto - purchasePriceDto.RabattfaehigbetragRabattBrutto);
            purchasePriceDto.KaufpreisExternUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.KaufpreisExternBrutto - purchasePriceDto.KaufpreisExtern);

            purchasePriceDto.KaufpreisBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.KaufpreisBrutto);
            purchasePriceDto.Kaufpreis = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(purchasePriceDto.Kaufpreis);


            decimal kpB = purchasePriceDto.HandlerzubehorBrutto + purchasePriceDto.HerstellerzubehorBrutto + purchasePriceDto.ListenpreisBrutto + purchasePriceDto.PaketeBrutto + purchasePriceDto.SonderausstattungBrutto;


            NovaType nt3 = new NovaType(Ust2, 0, 0, 0);
            nt3.setBruttoInklNova(purchasePriceDto.ListenpreisBrutto);
            purchasePriceDto.ListenpreisNettoNetto = nt3.netto;







            return purchasePriceDto;
            
        }
    }
}