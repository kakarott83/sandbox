// OWNER JJ, 30-11-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class ITAssembler
    {
        #region Private variables
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private long? _SysPEROLE;
        private long? _SysPUSER;
        #endregion

        #region Enums
        public enum Gender : int
        {
            Unknown = 0,
            Herr = 1,
            Frau = 2
        }
        #endregion

        #region Constructors
        public ITAssembler(long? sysPEROLE, long? SysPUSER)
        {
            _SysPEROLE = sysPEROLE;
            _SysPUSER = SysPUSER;
            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region IDtoAssembler<ITDto,IT> Members (Methods)
        public bool IsValid(ITDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            // Otymistic
            bool IsValid = true;

          
            // DdOl
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Check if exists
                if (dto.SYSIT.HasValue && !RestOfTheHelpers.ContainsIT(Context,dto.SYSIT.Value))
                {
                    _Errors.Add("SYSIT", "Can not be found.");
                    IsValid = false;
                }

                // Check if _SysPERSONInPEROLE is not null
                if (IsValid && !_SysPEROLE.HasValue)
                {
                    _Errors.Add("SYSIT", "Not exists in sight field. SysPERSONInPEROLE is null.");
                    IsValid = false;
                }

                // Check SYSIT
                if (IsValid && dto.SYSIT.HasValue)
                {
                   

                    // Check sight field
                    if (IsValid && !Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, (long)_SysPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.IT, (long)dto.SYSIT))
                    {
                        _Errors.Add("SYSIT", "Not exists in sight field.");
                        IsValid = false;
                    }
                }

                // Check Ids
                /* Ticket #3380 Sysperson nicht prüfen               
                 * if (IsValid && dto.SYSPERSON.HasValue)
                                {
                                    if (! PersonHelper.Contains(Context, (long)dto.SYSPERSON))
                                    {
                                        _Errors.Add( IT.FieldNames.SYSPERSON.ToString(), "Can not be found.");
                                        IsValid = false;
                                    }
                                }
                                */
                if (IsValid && dto.SYSLAND.HasValue && dto.SYSLAND.Value>0)
                {
                    if (! RestOfTheHelpers.ContainsLAND(Context, (long)dto.SYSLAND))
                    {
                        _Errors.Add("SYSLAND", "Can not be found.");
                        IsValid = false;
                    }
                }

                if (IsValid && dto.SYSSTAAT.HasValue)
                {
                    if (! RestOfTheHelpers.ContainsSTAAT(Context, (long)dto.SYSSTAAT))
                    {
                        _Errors.Add( "SYSSTAAT", "Can not be found.");
                        IsValid = false;
                    }
                }

              /*  if (IsValid && dto.SYSLANDNAT.HasValue)
                {
                    if (! LANDHelper.Contains(Context, (long)dto.SYSLANDNAT))
                    {
                        _Errors.Add( IT.FieldNames.SYSLANDNAT.ToString(), "Can not be found.");
                        IsValid = false;
                    }
                }

                if (IsValid && dto.SYSBRANCHE.HasValue)
                {
                    if (! BRANCHEHelper.Contains(Context, (long)dto.SYSBRANCHE))
                    {
                        _Errors.Add( IT.FieldNames.SYSBRANCHE.ToString(), "Can not be found.");
                        IsValid = false;
                    }
                }*/
            }
            // DdOw
            using (DdOwExtended Context = new DdOwExtended())
            {


                // Check Ids
                if (IsValid && dto.SYSCTLANG.HasValue)
                {
                    if (!RestOfTheHelpers.ContainsCTLANG(Context, (long)dto.SYSCTLANG))
                    {
                        _Errors.Add("SYSCTLANG", "Can not be found.");
                        IsValid = false;
                    }
                }
            }



            return IsValid;
        }

        public IT Create(ITDto dto)
        {
            IT NewIT;
            

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            NewIT = new IT();



            using (DdOlExtended Context = new DdOlExtended())
            {
                Context.IT.Add(NewIT);
                // Map
                MyMap(dto, NewIT, Context);
                //TRANSACTIONS
                // using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
                {
                    //Create ITOPTION
                   // NewITOPTION = new ITOPTION();


                    // Insert (with Save changes)
                  //  Context.Insert<IT>(NewIT);
                    //Context.AddToIT(NewIT);
                    Context.SaveChanges();//save it generate sysit
                   // NewITOPTION.IT = NewIT;
                  //  NewITOPTION.OPTION1 = dto.VERTRETUNGSBERECHTIGUNG;
                    //Insert ITOPTION
                  //  Context.ITOPTION.Add(NewITOPTION);

                    // Create peunis
                    if (_SysPEROLE.HasValue)
                    {
                         PEUNIHelper.ConnectNodes(Context,  PEUNIHelper.Areas.IT, NewIT.SYSIT, _SysPEROLE.Value);
                    }

                    // Save changes
                    Context.SaveChanges();//true);//System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);

                    //TRANSACTIONS
                    // Set transaction complete
                    //TransactionScope.Complete();
                }
            }

            return NewIT;
        }


        public static ITDto CreateITDto(ITConfiguration it)
        {
            // Create new IT
            ITDto ItDto = new ITDto();

            try
            {

                ItDto.ITCONFIGSOURCE = (Cic.OpenLease.ServiceAccess.ITTypeConstants)it.Type;
                ItDto.ITCONFIGID = it.ITId;

                ItDto.ANREDE = it.Salutation;
                ItDto.ANREDECODE = "0";
                // Set the properties
                ItDto.GESCHLECHT = ITDto.Sex.Unknown;
                if (it.Gender != null)
                {
                    ItDto.GESCHLECHT = Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Unknown;
                    try
                    {
                        Gender itGender = (Gender)EnumUtil.DeliverDefinedOrDefault(typeof(Gender), Enum.Parse(typeof(Gender), it.Gender), Gender.Unknown);
                        if (itGender == Gender.Frau)
                        {
                            ItDto.GESCHLECHT = Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Female;
                            ItDto.ANREDECODE = "1";
                        }
                        else if (itGender == Gender.Herr)
                        {
                            ItDto.GESCHLECHT = Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Male;
                            //ItDto.ANREDE = "Herrn";
                            ItDto.ANREDECODE = "2";
                        }
                        else if (itGender == Gender.Unknown)
                        {
                            ItDto.GESCHLECHT = Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Unknown;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }


                //ItDto.? = itDto.PartnerTyp;
                //ItDto.? = itDto.Flag;
                //IT: 1=Private, 2=Company, wenn Anrede Firma, dann Einzelunternehmen (kdtyp = 2)
                //3=Unternehmen 1 = Privat
                int kdtyp = it.PartnerTyp == 2 ? 3 : 1;
                if ("Firma".Equals(it.Gender))//#3884
                {
                    it.PartnerTyp = 2;
                    kdtyp = 2;
                    ItDto.ANREDECODE = "3";
                }
                ItDto.SYSLAND = 13;//Default german
                using ( DdOlExtended ctx = new  DdOlExtended())
                {

                    var q = from kd in ctx.KDTYP
                            where kd.TYP == kdtyp
                            select kd.SYSKDTYP;
                    ItDto.SYSKDTYP = q.FirstOrDefault();
                    if (it.Country != null && it.Country.Length > 0)
                    {
                        long syslandinput =-1;
                        try
                        {
                            syslandinput = long.Parse(it.Country);
                        }catch(Exception )
                        {

                        }

                        var q2 = from ld in ctx.LAND
                                 where ld.COUNTRYNAME == it.Country || ld.SYSLAND == syslandinput
                                 select ld.SYSLAND;
                        long sysld = q2.FirstOrDefault();
                        if (sysld > 0)
                            ItDto.SYSLAND = sysld;
                    }

                }


                
                ItDto.TITEL = it.Title;
                if(it.Title!=null)
                {
                    if(it.Title.ToUpper().IndexOf("DR")>-1)
                        ItDto.TITELCODE = "1";
                    if (it.Title.ToUpper().IndexOf("PROF") > -1)
                        ItDto.TITELCODE = "2";
                    if (it.Title.ToUpper().IndexOf("DR") > -1 && it.Title.ToUpper().IndexOf("PROF") > -1)
                        ItDto.TITELCODE = "3";
                }
                ItDto.NAME = it.Surname;
                ItDto.NAMEAG1 = it.Surname2;
                ItDto.VORNAME = it.Prename;
                ItDto.UIDNUMMER = it.VATRegNumber;
                if (it.Street == null) it.Street = "";
                ItDto.STRASSE = it.Street;
                /*+" " + it.StreetNumber;
                if (it.AddressField2 != null && it.AddressField2.Length > 0)//#3802
                    ItDto.STRASSE += " " + it.AddressField2;*/
                ItDto.HSNR = it.StreetNumber;

                ItDto.PLZ = it.Zip;
                ItDto.ORT = it.City;
                ItDto.FAX = it.Fax;
                ItDto.EMAIL = it.Email;

                /*
                ItDto.TELEFON = it.Phone2;//Geschäftlich
                ItDto.PTELEFON = it.Phone1;//identisch mit Phone! == Privat
                ItDto.PTELEFON = it.Phone;
                ItDto.HANDY = it.Phone3;
                */

                ItDto.PTELEFON = it.Phone;//Privat
                ItDto.TELEFON = it.Phone1;//Geschäftlich
                ItDto.HANDY = it.Phone2;//Mobil

                if (it.gebDatum.HasValue)
                    ItDto.GEBDATUM = it.gebDatum;
                else { 
                    try
                    {
                        ItDto.GEBDATUM = new System.DateTime(it.BirthYear, it.BirthMonth, it.BirthDay);
                    }
                    catch (Exception) { }
                }

                ItDto.HREGISTER = it.VATRegNumber;
                
                //ItDto.? = iit.VATRegNumberInternational;
                //ItDto.? = it.VATGroupKey;
                //ItDto.? = it.PoBox;
                //ItDto.SYSLAND = it.Country???????;
                //ItDto.? = it.County;

                ItDto.TELEFONKONT = it.Phone2;

                //ItDto.? = it.Phone1;
                //ItDto.? = it.Phone2;
                //ItDto.? = it.Phone3;
                //ItDto.? = it.CustomerLanguage;
                //ItDto.? = it.AddressTyp;
                //ItDto.? = it.Salutation;
                //ItDto.SYSBRANCHE= it.Branch;

                //set default to Germany
                if (!ItDto.SYSLANDNAT.HasValue || ItDto.SYSLANDNAT.Value == 0)
                    ItDto.SYSLANDNAT = 13;

            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("ITDto couldn not be created", e);
            }

            // Return angebot
            return ItDto;
        }

      
        /// <summary>
        /// Update the compliance-Data
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateCompliance(ITDto dto)
        {

            using (DdOlExtended Context = new DdOlExtended())
            {
                //Read compliance-Data
                long syscompliance = 0;
                if(dto.SYSIT.HasValue && dto.SYSIT.Value>0)
                    syscompliance = Context.ExecuteStoreQuery<long>("select compliance.syscompliance from compliance where compliance.area='IT' and compliance.sysid=" + dto.SYSIT.Value).FirstOrDefault();

                if (syscompliance == 0 && !dto.FLAGAKTIV)//kein Eintrag bisher und FLAGAKTIV nicht gesetzt -> raus
                {
                    return;
                }
                if(syscompliance>0 && !dto.FLAGAKTIV)//bisherigen Eintrag entfernen HCERZWEI-2059
                {
                    Context.ExecuteStoreCommand("delete from compliance where compliance.area='IT' and compliance.sysid=" + dto.SYSIT.Value);
                    return;
                }
                if (syscompliance==0)
                {
                    Context.ExecuteStoreCommand("insert into compliance(area,sysid,syscompliancetype) values('IT',"+dto.SYSIT.Value+",1)");
                    Context.SaveChanges();
                    syscompliance = Context.ExecuteStoreQuery<long>("select compliance.syscompliance from compliance where compliance.area='IT' and compliance.sysid=" + dto.SYSIT.Value).FirstOrDefault();
                }

                List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = dto.AMTBEZ });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = dto.FLAGAKTIV });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = dto.AMTSEIT });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = dto.AMTBIS });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p5", Value = dto.AMTLAND });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p6", Value = syscompliance });
                Context.ExecuteStoreCommand("update compliance set bezeichnung=:p1, flagaktiv=:p2, beginn=:p3, ende=:p4, sysland=:p5 where syscompliance=:p6", parameters2.ToArray());
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Update the KNE Link
        /// </summary>
        /// <param name="dto"></param>
        public static void UpdateKNE(ITDto dto)
        {
            //vorher: SYSOBER == Hauptkunde
            //dto.SYSIT= IH/WB/etc == SYSUNTER

            //neu:SYSUNTER==Hauptkunde
            //dto.SYSIT= IH/WB/etc == SYSOBER
            if (dto.KNE == null || dto.KNE.SYSUNTER == 0) return;
            using (DdOlExtended Context = new DdOlExtended())
            {

                String[] knes = { "GESELLS", "KOMPL", "PARTNER", "VORSTAND", "STIFTUNGSV", "STIFTB" };
                if (dto.KNE.relateTypeCode.Equals("INH"))//convert INH to one of the above
                {

                    String domainid = Context.ExecuteStoreQuery<String>("SELECT domainid FROM ddlkppos WHERE code = 'RECHTSFORMCODE' AND ID=(select rechtsformcode from it where sysit=" + dto.KNE.SYSUNTER+")" ).FirstOrDefault();
                    /*GbR
                    GmbH
                    KG
                    OHG
                    Partnerschaftsges.
                    Sonstige
                    Stiftung
                    UG (Unternehmerges.)
                    e.K. (eingetr. Kfm)
                    e.V. (Verein)*/
                    if(domainid!=null)
                    {
                        if(domainid.IndexOf("e.K.")>-1)
                            dto.KNE.relateTypeCode = "INH";
                        else if (domainid.IndexOf("KG") > -1)
                            dto.KNE.relateTypeCode = "KOMPL";
                        else if (domainid.IndexOf("OHG") > -1)
                            dto.KNE.relateTypeCode = "GESELLS";
                        else if (domainid.IndexOf("GbR") > -1)
                            dto.KNE.relateTypeCode = "PARTNER";
                        else if (domainid.IndexOf("Partnerschaftsges") > -1)
                            dto.KNE.relateTypeCode = "PARTNER";
                        else if (domainid.IndexOf("UG") > -1)
                            dto.KNE.relateTypeCode = "GESELLS";
                        else if (domainid.IndexOf("e.V.") > -1)
                            dto.KNE.relateTypeCode = "VORSTAND";
                        else if (domainid.IndexOf("Stiftung") > -1)
                            dto.KNE.relateTypeCode = "STIFTUNGSVORST";
                        else if (domainid.IndexOf("GmbH") > -1)
                            dto.KNE.relateTypeCode = "GESELLS";
                        else if (domainid.IndexOf("Sonstige") > -1)
                            dto.KNE.relateTypeCode = "GESELLS";
                    }
                    
                   
                }
               

                List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = dto.KNE.SYSUNTER });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = dto.SYSIT });                
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = dto.KNE.relateTypeCode });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = dto.KNE.SYSANGEBOT });
                //Read KNE
                long sysitkne = Context.ExecuteStoreQuery<long>("select sysitkne from itkne where sysunter=:p1 and sysober=:p2 and relatetypecode=:p3 and sysarea=:p4 and area='ANGEBOT'", parameters2.ToArray()).FirstOrDefault();

                if (sysitkne == 0)
                {
                    parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = dto.KNE.SYSUNTER });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = dto.SYSIT });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = dto.KNE.relateTypeCode });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = dto.KNE.SYSANGEBOT });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p5", Value = dto.KNE.QUOTE });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p6", Value = dto.KNE.codeRelateKind });
                    Context.ExecuteStoreCommand("insert into itkne(sysunter,sysober,relatetypecode,sysarea,area,quote,coderelatekind) values(:p1,:p2,:p3,:p4,'ANGEBOT',:p5,:p6)", parameters2.ToArray());
                    Context.SaveChanges();
                    
                }//else already there, update quote
                else
                {
                    parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = dto.KNE.QUOTE });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = sysitkne });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = dto.KNE.codeRelateKind });
                    Context.ExecuteStoreCommand("update itkne set quote=:p1,coderelatekind=:p3 where sysitkne=:p2", parameters2.ToArray());
                    Context.SaveChanges();
                }
                //Delete OLD INH Entries if available
               /* parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = dto.KNE.SYSUNTER });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = dto.SYSIT });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = "INH" });
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = dto.KNE.SYSANGEBOT });
                //Read KNE
                Context.ExecuteStoreCommand ("delete from itkne where sysunter=:p1 and sysober=:p2 and relatetypecode=:p3 and sysarea=:p4 and area='ANGEBOT'", parameters2.ToArray());
                Context.SaveChanges();*/
            }
        }

        public IT Update(ITDto dto)
        {
            IT OriginalIT;
            IT ModifiedIT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            if (dto.SYSIT == null)
            {
                throw new ArgumentException("dto.SYSIT");
            }

            using (DdOlExtended Context = new DdOlExtended())
            {
                OriginalIT = (from c in Context.IT
                              where c.SYSIT == dto.SYSIT
                              select c).FirstOrDefault();//Context.SelectById<IT>((long)dto.SYSIT);

                // Map
                MyMap(dto, OriginalIT, Context);

             /*   long sysitoption = Context.ExecuteStoreQuery<long>("select sysitoption from itoption where sysit="+dto.SYSIT,null).FirstOrDefault();

                ITOPTION OriginalITOpt = (from t in Context.ITOPTION
                                          where t.SYSITOPTION == sysitoption
                                          select t).FirstOrDefault();

                if (OriginalITOpt == null)
                {
                    //Create ITOPTION
                    //OriginalITOpt = new ITOPTION();


                    OriginalITOpt.IT = OriginalIT;
                    //Insert ITOPTION
                   // OriginalITOpt.OPTION1 = dto.VERTRETUNGSBERECHTIGUNG;
                    //Context.ITOPTION.Add(OriginalITOpt);
                }
                else
                { 
                   // OriginalITOpt.OPTION1 = dto.VERTRETUNGSBERECHTIGUNG;
                   // Context.Update<ITOPTION>(OriginalITOpt, null);
                }
              * */
                    // Update (with Save changes)
                //    ModifiedIT = Context.Update<IT>(OriginalIT, null);
                Context.SaveChanges();
                ModifiedIT = OriginalIT;
            }

            return ModifiedIT;
        }

        public ITDto ConvertToDto(IT domain)
        {
            ITDto ITDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ITDto = new ITDto();
            MyMap(domain, ITDto);

            return ITDto;
        }
        public ITDto ValidateLegit(ITDto ITDto)
        {
            if(true)
                return ITDto;
            /*
            //Disable for now HCEB-3838
            if (ITDto.LEGITDATUM.HasValue)
            {
                DateTime now = System.DateTime.Now;
                TimeSpan ts = now.Subtract(ITDto.LEGITDATUM.Value);
                if (ts.Days > (30 * 6)) return ITDto;
                //Einkommen
                ITDto.EINKNETTO = null;
                ITDto.NEBENEINKNETTO = null;
                ITDto.ZEINKNETTO = null;
                ITDto.SONSTVERM = null;
                ITDto.ARTSONSTVERM = null;
                //Ausgaben
                ITDto.MIETE = null;
                ITDto.AUSLAGEN = null;
                ITDto.KREDRATE1 = null;
                ITDto.UNTERHALT = null;
                ITDto.MIETNEBEN = null;

                /* //#3501
                ITDto.LEGITDATUM = null;
                ITDto.LEGITABNEHMER = null;
                ITDto.FAMILIENSTAND = null;
                ITDto.WOHNUNGART = null;
                ITDto.KINDERIMHAUS = null;
                ITDto.WEHRDIENST = null;
                ITDto.NAMEAG = null;
                ITDto.STRASSEAG = null;
                ITDto.PLZAG = null;
                ITDto.ORTAG = null;
                ITDto.SYSLANDAG = null;
                ITDto.SYSBRANCHE = null;
                ITDto.BERUF = null;
                ITDto.BESCHSEITAG = null;
                ITDto.BESCHBISAG = null;
                ITDto.NAMEAG1 = null;
                ITDto.NAMEAG2 = null;
                ITDto.NAMEAG3 = null;

                ITDto.BESCHSEITAG1 = null;
                ITDto.BESCHSEITAG2 = null;
                ITDto.BESCHSEITAG3 = null;
                ITDto.BESCHBISAG1 = null;
                ITDto.BESCHBISAG2 = null;
                ITDto.BESCHBISAG3 = null;
               //end of old comment

            }
            return ITDto;*/
        }
        public IT ConvertToDomain(ITDto dto, DdOlExtended context)
        {
            IT IT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            IT = new IT();
            MyMap(dto, IT, context);

            return IT;
        }
        #endregion

        #region IDtoAssembler<ITDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                return _Errors;
            }
        }
        #endregion

        #region My methods
        //used for create and update
        private void MyMap(ITDto fromITDto, IT toIT, DdOlExtended context)
        {
            // Mapping
            // Ids
            // NOTE JJ, The value < 1 is converted to the null
            toIT.SYSPERSON = (fromITDto.SYSPERSON.HasValue && fromITDto.SYSPERSON.Value < 1L ? null : fromITDto.SYSPERSON);
            if (fromITDto.SYSLAND.HasValue && fromITDto.SYSLAND.Value>0)
                toIT.SYSLAND= (long)fromITDto.SYSLAND;

            

            toIT.SYSSTAAT = (fromITDto.SYSSTAAT.HasValue && fromITDto.SYSSTAAT.Value < 1L ? null : fromITDto.SYSSTAAT);
            
            toIT.SYSCTLANG = (fromITDto.SYSCTLANG.HasValue && fromITDto.SYSCTLANG.Value < 1L ? null : fromITDto.SYSCTLANG);
            toIT.SYSBRANCHE = (fromITDto.SYSBRANCHE.HasValue && fromITDto.SYSBRANCHE.Value < 1L ? null : fromITDto.SYSBRANCHE);
            toIT.SYSLANDAG = (fromITDto.SYSLANDAG.HasValue && fromITDto.SYSLANDAG.Value < 1L ? null : fromITDto.SYSLANDAG);
            toIT.SYSKDTYP = (fromITDto.SYSKDTYP.HasValue && fromITDto.SYSKDTYP.Value < 1L ? null : fromITDto.SYSKDTYP);
            //HCEDEV new
            toIT.GEBORT = fromITDto.GEBORT;
            toIT.HREGISTERORT = fromITDto.HREGISTERORT;

            //Voradresse
            if (fromITDto.SYSLAND2.HasValue)
                toIT.SYSLAND2 = (long)fromITDto.SYSLAND2;
            toIT.STRASSE2 = fromITDto.STRASSE2;
            toIT.ORT2 = fromITDto.ORT2;
            toIT.PLZ2 = fromITDto.PLZ2;
            toIT.HSNR2 = fromITDto.HSNR2;
            if(toIT.ORT2==null||toIT.ORT2.Length==0)
            {
                toIT.SYSLAND2 = null;
                toIT.STRASSE2 = null;
                toIT.ORT2 = null;
                toIT.PLZ2 = null;
                toIT.HSNR2 = null;
            }

            // Flags
            toIT.PRIVATFLAG = (fromITDto.PRIVATFLAG ? 1 : 0);
            toIT.GESCHLECHT = (int)fromITDto.GESCHLECHT;
            toIT.TITELCODE = fromITDto.TITELCODE;
            toIT.ANREDECODE = fromITDto.ANREDECODE;
            toIT.WOHNUNGART = fromITDto.WOHNUNGART;
            
            if (toIT.SYSKDTYP.HasValue && toIT.SYSKDTYP > 0)
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    KDTYP kd = (from c in ctx.KDTYP
                                where c.SYSKDTYP == toIT.SYSKDTYP
                                select c).FirstOrDefault();//ctx.SelectById<KDTYP>((long)toIT.SYSKDTYP);
                    if (kd != null)
                    {
                        if (kd.TYP == 1)//privat
                        {
                            toIT.PRIVATFLAG = 1;
                            toIT.SYSBRANCHE = 99;//immer branche
                        }
                        else if (kd.TYP == 2)//Einzelunternehmen
                        {
                            toIT.PRIVATFLAG = 0;
                        }
                        else
                        {
                            toIT.PRIVATFLAG = 0;
                            toIT.GESCHLECHT = 0;
                            //anredecode für Unternehmen immer Firma
                            toIT.ANREDECODE = "3";
                            //landnat=land weil kein pflichtfeld und nicht in gui
                            fromITDto.SYSLANDNAT = fromITDto.SYSLAND;
                        }
                    }


                }
            }

            toIT.SYSLANDNAT = (fromITDto.SYSLANDNAT.HasValue && fromITDto.SYSLANDNAT.Value < 1L ? null : fromITDto.SYSLANDNAT);


            // Properties
            // Base
            toIT.ANREDE = fromITDto.ANREDE;
            toIT.TITEL = (fromITDto.TITEL == "0" ? null : fromITDto.TITEL);
            toIT.SUFFIX = (fromITDto.SUFFIX == "0" ? null : fromITDto.SUFFIX);
            toIT.VORNAME = fromITDto.VORNAME;
            toIT.NAME = fromITDto.NAME;
            toIT.ZUSATZ = fromITDto.ZUSATZ;
            if (toIT.NAME != null)
                toIT.NAME = toIT.NAME.Trim();

            toIT.GEBDATUM = fromITDto.GEBDATUM;

            // Additional copmany data
            toIT.RECHTSFORM = fromITDto.RECHTSFORM;
            toIT.RECHTSFORMCODE = fromITDto.RECHTSFORMCODE;
            toIT.GRUENDUNG = fromITDto.GRUENDUNG;
            toIT.UIDNUMMER = fromITDto.UIDNUMMER;
            toIT.HREGISTER = fromITDto.HREGISTER;
			toIT.PREVNAME = fromITDto.PREVNAME;

			//TODO HCE new field
			//toIT.AMTSGERICHT = fromITDto.AMTSGERICHT;

			// Address
			toIT.STRASSE = fromITDto.STRASSE;
            if (toIT.STRASSE != null)
            {
                toIT.STRASSE = toIT.STRASSE.Trim();
                if (toIT.STRASSE.Length > 50)
                    toIT.STRASSE = toIT.STRASSE.Substring(0, 50);
            }

            toIT.HSNR = fromITDto.HSNR;
            toIT.PLZ = fromITDto.PLZ;
            toIT.ORT = fromITDto.ORT;
            if (toIT.ORT != null)
                toIT.ORT = toIT.ORT.Trim();

            // Identification
            toIT.AUSWEISART = fromITDto.AUSWEISART;
            toIT.AUSWEISNR = fromITDto.AUSWEISNR;
            if (toIT.AUSWEISNR != null)
            {
                toIT.AUSWEISNR = toIT.AUSWEISNR.Trim();
                if (toIT.AUSWEISNR.Length > 30)
                    toIT.AUSWEISNR = toIT.AUSWEISNR.Substring(0, 30);
            }
            
            toIT.AUSWEISBEHOERDE = fromITDto.AUSWEISBEHOERDE;
            toIT.AUSWEISORT = fromITDto.AUSWEISORT;
            toIT.AUSWEISDATUM = fromITDto.AUSWEISDATUM;
            toIT.AUSWEISABLAUF = fromITDto.AUSWEISABLAUF;
            toIT.AUSWEISGUELTIG = fromITDto.AUSWEISGUELTIG;
            toIT.AUSLAUSWEISCODE = fromITDto.AUSLAUSWEISCODE;
            toIT.LEGITDATUM = fromITDto.LEGITDATUM;
            toIT.LEGITABNEHMER = fromITDto.LEGITABNEHMER;
            toIT.SVNR = fromITDto.SVNR;
			toIT.IDENTEG = fromITDto.IDENTEG;
            //toIT.LEI = fromITDto.LEI;
            toIT.IDENTUST = fromITDto.IDENTUST;
            toIT.HREGISTERPLZ = fromITDto.HREGISTERPLZ;
            toIT.HREGISTERART = fromITDto.HREGISTERART;

            // Contact
            toIT.TELEFON = fromITDto.TELEFON;
            toIT.PTELEFON = fromITDto.PTELEFON;
            toIT.HANDY = fromITDto.HANDY;
            toIT.FAX = fromITDto.FAX;
            toIT.EMAIL = fromITDto.EMAIL;
            toIT.URL = fromITDto.URL;
            toIT.ERREICHBTREL = fromITDto.ERREICHBTREL;

            // Bank accounts
            toIT.KONTOINHABER = fromITDto.KONTOINHABER;
            toIT.BLZ = fromITDto.BLZ;
            toIT.KONTONR = fromITDto.KONTONR;
            toIT.BANKNAME = fromITDto.BANKNAME;
            toIT.IBAN = fromITDto.IBAN;
            if (toIT.IBAN != null)
            { 
                toIT.IBAN = toIT.IBAN.Replace(" ", "");
                toIT.KONTOINHABER = fromITDto.NAME != null ? fromITDto.NAME : "" +" "+ fromITDto.VORNAME != null ? fromITDto.VORNAME : "";
            }
            toIT.BIC = fromITDto.BIC;
            if (toIT.BIC != null)
                toIT.BIC = toIT.BIC.Replace(" ","");

            
            // Contact person
            toIT.ANREDEKONT = fromITDto.ANREDEKONT;
            toIT.TITELKONT = fromITDto.TITELKONT;
            // TODO JJ 0 JJ, Add SUFFIXKONT
            toIT.VORNAMEKONT = fromITDto.VORNAMEKONT;
            toIT.NAMEKONT = fromITDto.NAMEKONT;
            toIT.TELEFONKONT = fromITDto.TELEFONKONT;
            toIT.EMAILKONT = fromITDto.EMAILKONT;

            // Household
            toIT.FAMILIENSTAND = fromITDto.FAMILIENSTAND;
            toIT.KINDERIMHAUS = fromITDto.KINDERIMHAUS;
            toIT.WOHNUNGART = fromITDto.WOHNUNGART;
            toIT.WEHRDIENST = fromITDto.WEHRDIENST;

            // Income
            toIT.EINKNETTO = fromITDto.EINKNETTO;
            toIT.NEBENEINKNETTO = fromITDto.NEBENEINKNETTO;
            toIT.ZEINKNETTO = fromITDto.ZEINKNETTO;
            toIT.SONSTVERM = fromITDto.SONSTVERM;
            toIT.ARTSONSTVERM = fromITDto.ARTSONSTVERM;

            // Costs
            toIT.MIETE = fromITDto.MIETE;
            toIT.AUSLAGEN = fromITDto.AUSLAGEN;
            toIT.KREDRATE1 = fromITDto.KREDRATE1;
            toIT.UNTERHALT = fromITDto.UNTERHALT;
            toIT.MIETNEBEN = fromITDto.MIETNEBEN;

            // Occupation
            toIT.BERUF = fromITDto.BERUF;
            toIT.BESCHSEITAG = fromITDto.BESCHSEITAG;
            toIT.BESCHBISAG = fromITDto.BESCHBISAG;

            // Employer
            toIT.NAMEAG = fromITDto.NAMEAG;
            toIT.STRASSEAG = fromITDto.STRASSEAG;
            toIT.HSNRAG = fromITDto.HSNRAG;
            toIT.PLZAG = fromITDto.PLZAG;
            toIT.ORTAG = fromITDto.ORTAG;

            // Employer 1
            toIT.NAMEAG1 = fromITDto.NAMEAG1;
            toIT.BESCHSEITAG1 = fromITDto.BESCHSEITAG1;
            toIT.BESCHBISAG1 = fromITDto.BESCHBISAG1;

            // Employer 2
            toIT.NAMEAG2 = fromITDto.NAMEAG2;
            toIT.BESCHSEITAG2 = fromITDto.BESCHSEITAG2;
            toIT.BESCHBISAG2 = fromITDto.BESCHBISAG2;

            // Employer 3
            toIT.NAMEAG3 = fromITDto.NAMEAG3;
            toIT.BESCHSEITAG3 = fromITDto.BESCHSEITAG3;
            toIT.BESCHBISAG3 = fromITDto.BESCHBISAG3;

            // Zusatzinformationen für ausländische Staatsbürger (bei Privatperson und Einzelunternehmer)
            toIT.MELDEDATUM = fromITDto.MELDEDATUM;
            toIT.AHBEWILLIGUNG = fromITDto.AHBEWILLIGUNG;
            toIT.AHBEWILLIGUNGBIS = fromITDto.AHBEWILLIGUNGBIS;
            toIT.AHGUELTIG = fromITDto.AHGUELTIG;
            toIT.AHBEWILLDURCH = fromITDto.AHBEWILLDURCH;
            toIT.ABBEWILLIGUNG = fromITDto.ABBEWILLIGUNG;
            toIT.ABBEWILLIGUNGBIS = fromITDto.ABBEWILLIGUNGBIS;
            toIT.ABGUELTIG = fromITDto.ABGUELTIG;
            toIT.ABBEWILLDURCH = fromITDto.ABBEWILLDURCH;
            toIT.WOHNSEIT = fromITDto.WOHNSEIT;


            toIT.URL = fromITDto.URL;
            toIT.SUFFIXKONT = fromITDto.SUFFIXKONT;
            toIT.WBEGUENST = fromITDto.WBEGUENST;

            toIT.USTABZUG = fromITDto.USTABZUG;

            toIT.KUNDENGRUPPE = "Einzelkunde";
            //TODO SEPA
            toIT.PAYART = fromITDto.PAYART;
            toIT.WOHNVERH = fromITDto.WOHNVERH;

        }
        /// <summary>
        /// used for deliver
        /// </summary>
        /// <param name="fromIT"></param>
        /// <param name="toITDto"></param>
        private void MyMap(IT fromIT, ITDto toITDto)
        {
            // Mapping
            // Id
            toITDto.SYSIT = fromIT.SYSIT;

            // Ids
            // NOTE JJ, The value < 1 is converted to the null
            toITDto.SYSPERSON = (fromIT.SYSPERSON.HasValue && fromIT.SYSPERSON.Value < 1L ? null : fromIT.SYSPERSON);
            toITDto.SYSLAND = fromIT.SYSLAND.GetValueOrDefault();
            toITDto.SYSSTAAT = (fromIT.SYSSTAAT.HasValue && fromIT.SYSSTAAT.Value < 1L ? null : fromIT.SYSSTAAT);
            toITDto.SYSLANDNAT = (fromIT.SYSLANDNAT.HasValue && fromIT.SYSLANDNAT.Value < 1L ? null : fromIT.SYSLANDNAT);
            
            toITDto.SYSCTLANG = (fromIT.SYSCTLANG.HasValue && fromIT.SYSCTLANG.Value < 1L ? null : fromIT.SYSCTLANG);
            toITDto.SYSBRANCHE = (fromIT.SYSBRANCHE.HasValue && fromIT.SYSBRANCHE.Value < 1L ? null : fromIT.SYSBRANCHE);
            toITDto.SYSLANDAG = (fromIT.SYSLANDAG.HasValue && fromIT.SYSLANDAG.Value < 1L ? null : fromIT.SYSLANDAG);
            toITDto.SYSKDTYP = (fromIT.SYSKDTYP.HasValue && fromIT.SYSKDTYP.Value < 1L ? null : fromIT.SYSKDTYP);
            //TODO SEPA            
            toITDto.PAYART = (fromIT.PAYART.HasValue && fromIT.PAYART.Value >= 1 ? fromIT.PAYART.Value : 0);
            //HCE NEW
            //toITDto.GEBORT = fromIT.GEBORT;

            // Flags
            toITDto.PRIVATFLAG = (fromIT.PRIVATFLAG.HasValue && fromIT.PRIVATFLAG == 1 ? true : false);

            //Voradresse
            toITDto.SYSLAND2 = (fromIT.SYSLAND2.HasValue && fromIT.SYSLAND2.Value < 1L ? null : fromIT.SYSLAND2);
            toITDto.STRASSE2 = fromIT.STRASSE2;
            toITDto.ORT2 = fromIT.ORT2;
            toITDto.PLZ2 = fromIT.PLZ2;
            toITDto.HSNR2 = fromIT.HSNR2;


            // Properties
            // Base
            toITDto.ANREDE = fromIT.ANREDE;
            toITDto.TITEL = (fromIT.TITEL == null ? "0" : fromIT.TITEL);
            toITDto.TITELCODE = fromIT.TITELCODE;
            toITDto.ANREDECODE = fromIT.ANREDECODE;
            toITDto.WOHNUNGART = fromIT.WOHNUNGART;
            toITDto.RECHTSFORMCODE = fromIT.RECHTSFORMCODE;
            toITDto.SUFFIX = (fromIT.SUFFIX == null ? "0" : fromIT.SUFFIX);
            toITDto.VORNAME = fromIT.VORNAME;
            if (toITDto.VORNAME != null)
                toITDto.VORNAME = toITDto.VORNAME.Trim();
            toITDto.NAME = fromIT.NAME;
            if (toITDto.NAME != null)
                toITDto.NAME = toITDto.NAME.Trim();

            toITDto.ZUSATZ = fromIT.ZUSATZ;
            try
            {
                toITDto.GESCHLECHT = (Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex)EnumUtil.DeliverDefinedOrDefault(typeof(Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex), fromIT.GESCHLECHT, Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Unknown);
            }
            catch (Exception)
            {
            }
            toITDto.GEBDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.GEBDATUM);

            // Additional copmany data
            toITDto.RECHTSFORM = fromIT.RECHTSFORM;
            toITDto.GRUENDUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.GRUENDUNG);
            toITDto.UIDNUMMER = fromIT.UIDNUMMER;
            toITDto.HREGISTER = fromIT.HREGISTER;
			toITDto.PREVNAME = fromIT.PREVNAME;
			//HCE new
			toITDto.HREGISTERORT = fromIT.HREGISTERORT;
            toITDto.GEBORT = fromIT.GEBORT;

            // Address
            toITDto.STRASSE = fromIT.STRASSE;
            toITDto.HSNR = fromIT.HSNR;
            toITDto.PLZ = fromIT.PLZ;
            toITDto.ORT = fromIT.ORT;
            if (toITDto.STRASSE != null)
                toITDto.STRASSE = toITDto.STRASSE.Trim();
            if (toITDto.ORT != null)
                toITDto.ORT = toITDto.ORT.Trim();

            // Identification
            toITDto.AUSWEISART = fromIT.AUSWEISART;
            toITDto.AUSWEISNR = fromIT.AUSWEISNR;
            if (toITDto.AUSWEISNR != null)
                toITDto.AUSWEISNR = toITDto.AUSWEISNR.Trim();
            toITDto.AUSWEISBEHOERDE = fromIT.AUSWEISBEHOERDE;
            toITDto.AUSLAUSWEISCODE = fromIT.AUSLAUSWEISCODE;
            toITDto.AUSWEISORT = fromIT.AUSWEISORT;
            toITDto.AUSWEISDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.AUSWEISDATUM);
            toITDto.AUSWEISABLAUF = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.AUSWEISABLAUF);
            toITDto.AUSWEISGUELTIG = fromIT.AUSWEISGUELTIG;
            toITDto.WOHNSEIT = fromIT.WOHNSEIT;
            toITDto.LEGITDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.LEGITDATUM);
            toITDto.LEGITABNEHMER = fromIT.LEGITABNEHMER;
            toITDto.SVNR = fromIT.SVNR;
			toITDto.IDENTEG = fromIT.IDENTEG;
            toITDto.IDENTUST = fromIT.IDENTUST;
            //toITDto.LEI = fromIT.LEI;
            toITDto.HREGISTERART = fromIT.HREGISTERART;
            toITDto.HREGISTERPLZ = fromIT.HREGISTERPLZ;


            // Contact
            toITDto.TELEFON = fromIT.TELEFON;
            toITDto.PTELEFON = fromIT.PTELEFON;
            toITDto.HANDY = fromIT.HANDY;
            toITDto.FAX = fromIT.FAX;
            toITDto.EMAIL = fromIT.EMAIL;
            // TODO JJ 0 JJ, Add URL
            toITDto.ERREICHBTREL = fromIT.ERREICHBTREL;

            // Bank accounts
            toITDto.KONTOINHABER = fromIT.KONTOINHABER;
            toITDto.BLZ = fromIT.BLZ;
            toITDto.KONTONR = fromIT.KONTONR;
            toITDto.BANKNAME = fromIT.BANKNAME;
            toITDto.IBAN = fromIT.IBAN;
            if (toITDto.IBAN != null)
                toITDto.IBAN = toITDto.IBAN.Trim().Replace(" ", "");
            toITDto.BIC = fromIT.BIC;
            if (toITDto.BIC != null)
                toITDto.BIC = toITDto.BIC.Trim().Replace(" ","");

            // Contact person
            toITDto.ANREDEKONT = fromIT.ANREDEKONT;
            toITDto.TITELKONT = fromIT.TITELKONT;
            // TODO JJ 0 JJ, Add SUFFIXKONT
            toITDto.VORNAMEKONT = fromIT.VORNAMEKONT;
            toITDto.NAMEKONT = fromIT.NAMEKONT;
            toITDto.TELEFONKONT = fromIT.TELEFONKONT;
            toITDto.EMAILKONT = fromIT.EMAILKONT;

            // Household
            toITDto.FAMILIENSTAND = fromIT.FAMILIENSTAND;
            toITDto.KINDERIMHAUS = fromIT.KINDERIMHAUS;
            toITDto.WOHNUNGART = fromIT.WOHNUNGART;
            toITDto.WEHRDIENST = fromIT.WEHRDIENST;

            // Income
            toITDto.EINKNETTO = fromIT.EINKNETTO;
            toITDto.NEBENEINKNETTO = fromIT.NEBENEINKNETTO;
            toITDto.ZEINKNETTO = fromIT.ZEINKNETTO;
            toITDto.SONSTVERM = fromIT.SONSTVERM;
            toITDto.ARTSONSTVERM = fromIT.ARTSONSTVERM;

            // Costs
            toITDto.MIETE = fromIT.MIETE;
            toITDto.AUSLAGEN = fromIT.AUSLAGEN;
            toITDto.KREDRATE1 = fromIT.KREDRATE1;
            toITDto.UNTERHALT = fromIT.UNTERHALT;
            toITDto.MIETNEBEN = fromIT.MIETNEBEN;

            // Occupation
            toITDto.BERUF = fromIT.BERUF;
            toITDto.BESCHSEITAG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHSEITAG);
            toITDto.BESCHBISAG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHBISAG);

            // Employer
            toITDto.NAMEAG = fromIT.NAMEAG;
            toITDto.STRASSEAG = fromIT.STRASSEAG;
            toITDto.HSNRAG = fromIT.HSNRAG;
            toITDto.PLZAG = fromIT.PLZAG;
            toITDto.ORTAG = fromIT.ORTAG;

            // Employer 1
            toITDto.NAMEAG1 = fromIT.NAMEAG1;
            toITDto.BESCHSEITAG1 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHSEITAG1);
            toITDto.BESCHBISAG1 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHBISAG1);

            // Employer 2
            toITDto.NAMEAG2 = fromIT.NAMEAG2;
            toITDto.BESCHSEITAG2 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHSEITAG2);
            toITDto.BESCHBISAG2 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHBISAG2);

            // Employer 3
            toITDto.NAMEAG3 = fromIT.NAMEAG3;
            toITDto.BESCHSEITAG3 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHSEITAG3);
            toITDto.BESCHBISAG3 = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.BESCHBISAG3);

            // NOTE JJ, Extended properties are created in model, only GET allowed
            // Extended properties
           /* toITDto.ExtTitle = fromIT.ExtTitle;
            toITDto.ExtCompleteName = fromIT.ExtCompleteName;
            toITDto.ExtZipCodeCity = fromIT.ExtZipCodeCity;
            toITDto.ExtBankAccountCompleteName = fromIT.ExtBankAccountCompleteName;
            toITDto.ExtBankCompleteName = fromIT.ExtBankCompleteName;
            toITDto.ExtContactCompleteName = fromIT.ExtContactCompleteName;*/

            // Zusatzinformationen für ausländische Staatsbürger (bei Privatperson und Einzelunternehmer)
            toITDto.MELDEDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.MELDEDATUM);
            toITDto.AHBEWILLIGUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.AHBEWILLIGUNG);
            toITDto.AHBEWILLIGUNGBIS = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.AHBEWILLIGUNGBIS);
            toITDto.AHGUELTIG = fromIT.AHGUELTIG;
            toITDto.AHBEWILLDURCH = fromIT.AHBEWILLDURCH;
            toITDto.ABBEWILLIGUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.ABBEWILLIGUNG);
            toITDto.ABBEWILLIGUNGBIS = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.ABBEWILLIGUNGBIS);
            toITDto.ABGUELTIG = fromIT.ABGUELTIG;
            toITDto.ABBEWILLDURCH = fromIT.ABBEWILLDURCH;

            toITDto.URL = fromIT.URL;
            toITDto.SUFFIXKONT = fromIT.SUFFIXKONT;
            toITDto.WBEGUENST = fromIT.WBEGUENST;

            toITDto.USTABZUG = fromIT.USTABZUG;
            toITDto.KUNDENGRUPPE = fromIT.KUNDENGRUPPE;
            toITDto.WOHNVERH = fromIT.WOHNVERH;

        }
        #endregion
    }
}