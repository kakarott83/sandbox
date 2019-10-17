using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Affinity
{
    class VKInfoDto
    {
        public String email { get; set; }
        public String hdname { get; set; }
        public String vkname { get; set; }
        public String isocode { get; set; }
        public long sysperson { get; set; }
    }
    class ITNatInfoDto
    {
        public String lang { get; set; }
        public String nat { get; set; }
        public String staat { get; set; }

    }

    /// <summary>
    /// Handles external insurance deeplink to Zurich Versicherung
    /// </summary>
    public class VSLinkBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Calls the external insurance service to get a link for signing an insurance
        /// also creates a disclaimermemo
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="extvscode"></param>
        /// <param name="sysperolevk"></param>
        /// <param name="isocode"></param>
        /// <returns></returns>
        public String getVSLink(long sysantrag, String extvscode, long sysperolevk, String isocode)
        {
            AffinityIntegrationInterfacev1_0Client client = null;
            try
            {
                client = new AffinityIntegrationInterfacev1_0Client();

            }catch(Exception e)
            {
                _log.Error("Verbindung zu VSLink-Service nicht möglich", e);
                String infoString = @"web.config auf Vorhandensein von <endpoint address='http://<host>/services/AffinityIntegration-v1' binding ='basicHttpBinding' bindingConfiguration ='ServiceSoap' contract ='AffinityIntegrationInterfacev1_0' name='AffinityIntegrationInterfacev1_0' /> prüfen";
                _log.Info(infoString);

                LogUtil.addWFLog("ANTRAG", "Verbindung zu VSLink-Service nicht möglich "+ infoString + " Details: " + e.Message, 2);



                throw new ServiceBaseException("Verbindung zu externem Service momentan nicht möglich");
            }

            
            IAngAntBo angAntBo = BOFactory.getInstance().createAngAntBo();
            AntragDto antrag = angAntBo.getAntrag(sysantrag, sysperolevk);

            IKundeDao kundeDao = CommonDaoFactory.getInstance().getKundeDao();
            KundeDto kunde = kundeDao.getKundeViaAntragID(antrag.kunde.sysit,sysantrag);

            int debugService = 0;
            
            MotorInsuranceInput mii = new MotorInsuranceInput();

            long sysperson = 0;
            ITNatInfoDto natinfo = null;
            using (PrismaExtended ctx = new PrismaExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = extvscode });

                mii.affinityID = ctx.ExecuteStoreQuery<String>("select bezeichnung from vstyp where code=:code", parameters.ToArray()).FirstOrDefault();
                debugService = ctx.ExecuteStoreQuery<int>("select debugflag from auskunfttyp where bezeichnung='getVSLink'").FirstOrDefault();
                natinfo = ctx.ExecuteStoreQuery < ITNatInfoDto >(@"select isocode lang, land.iso nat, staat.code staat from it, land, ctlang,staat where it.sysctlang=ctlang.sysctlang(+) and  it.syslandnat=land.sysland(+) and it.sysstaat=staat.sysstaat(+) and it.sysit=" + antrag.kunde.sysit).FirstOrDefault();
                VKInfoDto vki = ctx.ExecuteStoreQuery<VKInfoDto>(@"select vk.sysperson,vk.name||' '||vk.vorname vkname, vk.email, ctlang.isocode, hd.name hdname from perole, person vk, person hd, perole pp, ctlang where perole.sysparent=pp.sysperole and perole.sysperson=vk.sysperson and pp.sysperson=hd.sysperson and vk.sysctlang=ctlang.sysctlang and perole.sysperole=" + sysperolevk).FirstOrDefault();
                if (vki != null)
                {
                    sysperson = vki.sysperson;
                    mii.dealerName = vki.vkname;
                    mii.dealerEmail = vki.email;
                    if (vki.isocode != null)
                    {
                        String lang = vki.isocode.Substring(0, 2).ToLower();
                        Language dlang;
                        Enum.TryParse(lang, out dlang);
                        mii.dealerLanguage = dlang;
                    }
                }
                mii.branchName = ctx.ExecuteStoreQuery<String>("select name from person,antrag where antrag.sysvm=person.sysperson and antrag.sysid="+ sysantrag).FirstOrDefault();
                antrag.angAntObDto.schwacke = ctx.ExecuteStoreQuery<String>("select schwacke from obtyp where OBTYP.IMPORTSOURCE = 2 and obtyp.sysobtyp = "+antrag.angAntObDto.sysobtyp).FirstOrDefault();
                antrag.angAntObDto.brief.hubraum = ctx.ExecuteStoreQuery<long>("select hubraum from obtypfzadd where obtypfzadd.sysobtyp = " + antrag.angAntObDto.sysobtyp).FirstOrDefault();
            }


            if (natinfo!=null && natinfo.lang != null)
            {
                String lang = natinfo.lang.Substring(0, 2).ToLower();
                Language dlang;
                Enum.TryParse(lang, out dlang);
                mii.clientLanguage = dlang;
            }
            



            mii.clientLastname = antrag.kunde.name;
            
            mii.clientStreetAddress = antrag.kunde.strasse + " " + antrag.kunde.hsnr;
            mii.clientZIP = antrag.kunde.plz;
            mii.clientCity = antrag.kunde.ort;


            mii.clientPrivatTelephone = antrag.kunde.telefon;
            mii.clientMobileTelephone = antrag.kunde.handy;
            mii.clientEmail = antrag.kunde.email;

            if(natinfo!=null)
                mii.cantonOfRegistration = natinfo.staat;


            if (antrag.kunde.syskdtyp == 1)
            {
                mii.clientFirstname = antrag.kunde.vorname;
                if (antrag.kunde.gebdatum.HasValue)
                {
                    mii.clientDateOfBirth = antrag.kunde.gebdatum.Value;
                    mii.clientDateOfBirthSpecified = true;
                }
                try
                {
                    if ("FEMALE".Equals(antrag.kunde.anredeCode)|| "1".Equals(antrag.kunde.anredeCode))
                        mii.clientGender = PersonGender.female;
                    else
                        mii.clientGender = PersonGender.male;
                    mii.clientGenderSpecified = true;
                }
                catch (Exception) { }


                try
                {
                    mii.clientCivilStatus = PersonMaritalStatus.unknown;
                    short famstand = kunde.zusatzdaten[0].pkz[0].familienstand;
                    if(famstand==1)
                        mii.clientCivilStatus = PersonMaritalStatus.single;
                    else if (famstand == 2)
                        mii.clientCivilStatus = PersonMaritalStatus.married;
                    else if (famstand == 3)
                        mii.clientCivilStatus = PersonMaritalStatus.widowed;
                    else if (famstand == 4)
                        mii.clientCivilStatus = PersonMaritalStatus.divorced;
                    else if (famstand == 5)
                        mii.clientCivilStatus = PersonMaritalStatus.married;

                    mii.clientCivilStatusSpecified = true;
                }
                catch (Exception) { }
                if(natinfo!=null && natinfo.nat!=null)
                {
                    Country dnat;
                    try
                    {
                        Enum.TryParse(natinfo.nat, out dnat);
                        mii.clientNationality = dnat;
                        mii.clientNationalitySpecified = true;
                    }
                    catch (Exception) { }//Nat not readable
                }
                
                
            }



            if ("1".Equals(antrag.angAntObDto.fahrerCode) || antrag.angAntObDto.fahrerCode==null || antrag.angAntObDto.fahrerCode.Length==0|| "0".Equals(antrag.angAntObDto.fahrerCode)  )
            {
                mii.isClientTheMainDriver = true;
                mii.isClientTheMainDriverSpecified = true;
            }
            else
            {
                mii.isClientTheMainDriver = false;
                mii.isClientTheMainDriverSpecified = true;
            }

            mii.insuranceExpectedEffectiveFromDateSpecified = true;
            mii.insuranceExpectedEffectiveFromDate = DateTime.Now;
            if (antrag.angAntObDto.liefer.HasValue)
                mii.insuranceExpectedEffectiveFromDate = antrag.angAntObDto.liefer.Value;


            mii.clientPartyTypeSpecified = true;
            mii.clientPartyType = PartyType.naturalPerson;
            if(antrag.kunde.syskdtyp>1)
                mii.clientPartyType = PartyType.legalEntity;

            if (antrag.angAntObDto.erstzulassung.HasValue)
            {
                mii.vehicleRegistrationDate = antrag.angAntObDto.erstzulassung.Value;
                mii.vehicleRegistrationDateSpecified = true;
            }

            
            if (antrag.angAntObDto.schwacke != null)
                try
                {
                    mii.eurotaxTypeTag = int.Parse(antrag.angAntObDto.schwacke);
                    mii.eurotaxTypeTagSpecified = true;
                }
                catch (Exception) { }

            if(antrag.angAntObDto.hersteller!=null)
                mii.make = antrag.angAntObDto.hersteller.Trim();
            if(antrag.angAntObDto.baugruppe!=null)
                mii.model = antrag.angAntObDto.baugruppe.Trim();
            mii.enginePowerKW = antrag.angAntObDto.brief.kw;
            mii.enginePowerKWSpecified = true;
            mii.engineSize = (int)antrag.angAntObDto.brief.hubraum;
            mii.engineSizeSpecified = true;
            mii.catalogPrice = (decimal)antrag.angAntObDto.grundBrutto;
            mii.catalogPriceSpecified = true;
            mii.accessoriesPrice = (decimal)antrag.angAntObDto.zubehoerBrutto;
            mii.accessoriesPriceSpecified = true;

            String rval = null;

            if(debugService>0)
                LogUtil.addLogDump("ANTRAG", sysantrag, "createOrUpdateAntragService.getVSLink", XMLSerializer.Serialize(mii, "UTF-8"), client.Endpoint.Address.Uri.ToString());
            try
            {
                rval=client.createSalespersonURL(mii);              
            }
            catch (Exception e)
            {
                _log.Error("VSLink-Service lieferte bei Aufruf von createSalespersonURL einen Fehler", e);
                LogUtil.addWFLog("ANTRAG", "VSLink - Service lieferte bei Aufruf von createSalespersonURL einen Fehler "+ e.Message, 2);
                throw new ServiceBaseException("Verarbeitung in externem Service momentan nicht möglich");
            }
            try
            {
                String disclaimerCode = extvscode + "_DISCLAIMER";
                String querydisclaimer = @"select  cttfoid.replaceterm as disclaimer from ctfoid, cttfoid, ctlang WHERE 
                                           ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang 
                                           AND ctlang.isocode= :pisocode 
                                           AND ctlang.flagtranslate=1 
                                           and ctfoid.frontid=:dcode order by ctfoid.sysctfoid asc";
                String disclaimerText = "";
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pisoCode", Value = isocode });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "dcode", Value = disclaimerCode });

                    disclaimerText = ctx.ExecuteStoreQuery<String>(querydisclaimer, parameters.ToArray()).FirstOrDefault();
                }

                createOrUpdateMemo(sysantrag, null, disclaimerText, "ANTRAG", "Disclaimer " + extvscode, "Disclaimer Popup", sysperson);
            }
            catch (Exception e)
            {
                _log.Error("VSLink-Service konnte keinen Disclaimer verspeichern", e);
                
            }

            return rval;
        }

        public static void createOrUpdateMemo(long sysid, String notizmemo, String extInhalt,   String wftableSyscode, String kurzbez, String kategorieBezeichnung, long? sysPerson)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == wftableSyscode
                                 select t).FirstOrDefault();

               
                WFMMKAT kat = (from k in context.WFMMKAT
                                where k.BESCHREIBUNG == kategorieBezeichnung
                                select k).FirstOrDefault();

                WFMMEMO wfmmemo = new WFMMEMO();
                wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                wfmmemo.CREATEUSER = AngAntDao.getSysWfuser(sysPerson);
                wfmmemo.SYSLEASE = sysid;
                wfmmemo.SYSWFMTABLE = table.SYSWFTABLE;
                wfmmemo.SYSWFMMKAT = kat.SYSWFMMKAT;
                context.WFMMEMO.Add(wfmmemo);
               

                wfmmemo.KURZBESCHREIBUNG = kurzbez;
                if (notizmemo != null)
                {
                    if (notizmemo.Length > 4800)
                        wfmmemo.NOTIZMEMO = notizmemo.Substring(0, 4800);
                    else
                        wfmmemo.NOTIZMEMO = notizmemo;
                }
                context.SaveChanges();


                WFMMEMOEXT discExt = new WFMMEMOEXT();
                discExt.SYSWFMMEMO = wfmmemo.SYSWFMMEMO;
                discExt.INHALT = extInhalt;
                discExt.RANK = 1;
                context.WFMMEMOEXT.Add(discExt);

                context.SaveChanges();
            }
        }
    }
}
