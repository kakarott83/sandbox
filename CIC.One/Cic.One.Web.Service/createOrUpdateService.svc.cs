using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using System.Reflection;
using Cic.One.Web.Service.DAO;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Utils.BO;
using Cic.One.GateWKT.BO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Util.Logging;



namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class createOrUpdateService : IcreateOrUpdateService
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// delivers Staffelpositionstyp detail
        /// </summary>
        /// <param name="staffelpositionstyp"></param>
        /// <returns></returns>
        public ocreateOrUpdateStaffelpositionstypDto createOrUpdateStaffelpositionstyp(icreateOrUpdateStaffelpositionstypDto staffelpositionstyp)
        {
            ServiceHandler<icreateOrUpdateStaffelpositionstypDto, ocreateOrUpdateStaffelpositionstypDto> ew = new ServiceHandler<icreateOrUpdateStaffelpositionstypDto, ocreateOrUpdateStaffelpositionstypDto>(staffelpositionstyp);
            return ew.process(delegate(icreateOrUpdateStaffelpositionstypDto input, ocreateOrUpdateStaffelpositionstypDto rval,CredentialContext ctx)
            {

                if (input == null || input.staffelpositionstyp == null)
                    throw new ArgumentException("No valid input");

                rval.staffelpositionstyp = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateStaffelpositionstyp(input.staffelpositionstyp);

            });
        }

        /// <summary>
        /// delivers Staffeltyp detail
        /// </summary>
        /// <param name="staffeltyp"></param>
        /// <returns></returns>
        public ocreateOrUpdateStaffeltypDto createOrUpdateStaffeltyp(icreateOrUpdateStaffeltypDto staffeltyp)
        {
            ServiceHandler<icreateOrUpdateStaffeltypDto, ocreateOrUpdateStaffeltypDto> ew = new ServiceHandler<icreateOrUpdateStaffeltypDto, ocreateOrUpdateStaffeltypDto>(staffeltyp);
            return ew.process(delegate(icreateOrUpdateStaffeltypDto input, ocreateOrUpdateStaffeltypDto rval,CredentialContext ctx)
            {

                if (input == null || input.staffeltyp == null)
                    throw new ArgumentException("No valid input");

                rval.staffeltyp = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateStaffeltyp(input.staffeltyp);

            });
        }

        /// <summary>
        /// delivers Rolle detail
        /// </summary>
        /// <param name="rolle"></param>
        /// <returns></returns>
        public ocreateOrUpdateRolleDto createOrUpdateRolle(icreateOrUpdateRolleDto rolle)
        {
            ServiceHandler<icreateOrUpdateRolleDto, ocreateOrUpdateRolleDto> ew = new ServiceHandler<icreateOrUpdateRolleDto, ocreateOrUpdateRolleDto>(rolle);
            return ew.process(delegate(icreateOrUpdateRolleDto input, ocreateOrUpdateRolleDto rval,CredentialContext ctx)
            {

                if (input == null || input.rolle == null)
                    throw new ArgumentException("No valid input");

                rval.rolle = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRolle(input.rolle);

            });
        }

        /// <summary>
        /// delivers Rollentyp detail
        /// </summary>
        /// <param name="rollentyp"></param>
        /// <returns></returns>
        public ocreateOrUpdateRollentypDto createOrUpdateRollentyp(icreateOrUpdateRollentypDto rollentyp)
        {
            ServiceHandler<icreateOrUpdateRollentypDto, ocreateOrUpdateRollentypDto> ew = new ServiceHandler<icreateOrUpdateRollentypDto, ocreateOrUpdateRollentypDto>(rollentyp);
            return ew.process(delegate(icreateOrUpdateRollentypDto input, ocreateOrUpdateRollentypDto rval,CredentialContext ctx)
            {

                if (input == null || input.rollentyp == null)
                    throw new ArgumentException("No valid input");

                rval.rollentyp = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRollentyp(input.rollentyp);

            });
        }

        /// <summary>
        /// delivers Handelsgruppe detail
        /// </summary>
        /// <param name="handelsgruppe"></param>
        /// <returns></returns>
        public ocreateOrUpdateHandelsgruppeDto createOrUpdateHandelsgruppe(icreateOrUpdateHandelsgruppeDto handelsgruppe)
        {
            ServiceHandler<icreateOrUpdateHandelsgruppeDto, ocreateOrUpdateHandelsgruppeDto> ew = new ServiceHandler<icreateOrUpdateHandelsgruppeDto, ocreateOrUpdateHandelsgruppeDto>(handelsgruppe);
            return ew.process(delegate(icreateOrUpdateHandelsgruppeDto input, ocreateOrUpdateHandelsgruppeDto rval,CredentialContext ctx)
            {

                if (input == null || input.handelsgruppe == null)
                    throw new ArgumentException("No valid input");

                rval.handelsgruppe = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateHandelsgruppe(input.handelsgruppe);

            });
        }

        /// <summary>
        /// delivers Vertriebskanal detail
        /// </summary>
        /// <param name="vertriebskanal"></param>
        /// <returns></returns>
        public ocreateOrUpdateVertriebskanalDto createOrUpdateVertriebskanal(icreateOrUpdateVertriebskanalDto vertriebskanal)
        {
            ServiceHandler<icreateOrUpdateVertriebskanalDto, ocreateOrUpdateVertriebskanalDto> ew = new ServiceHandler<icreateOrUpdateVertriebskanalDto, ocreateOrUpdateVertriebskanalDto>(vertriebskanal);
            return ew.process(delegate(icreateOrUpdateVertriebskanalDto input, ocreateOrUpdateVertriebskanalDto rval,CredentialContext ctx)
            {

                if (input == null || input.vertriebskanal == null)
                    throw new ArgumentException("No valid input");

                rval.vertriebskanal = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateVertriebskanal(input.vertriebskanal);

            });
        }

        /// <summary>
        /// delivers Brand detail
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public ocreateOrUpdateBrandDto createOrUpdateBrand(icreateOrUpdateBrandDto brand)
        {
            ServiceHandler<icreateOrUpdateBrandDto, ocreateOrUpdateBrandDto> ew = new ServiceHandler<icreateOrUpdateBrandDto, ocreateOrUpdateBrandDto>(brand);
            return ew.process(delegate(icreateOrUpdateBrandDto input, ocreateOrUpdateBrandDto rval,CredentialContext ctx)
            {

                if (input == null || input.brand == null)
                    throw new ArgumentException("No valid input");

                rval.brand = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateBrand(input.brand);

            });
        }

        /// <summary>
        /// delivers Rechnung detail
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        public ocreateOrUpdateRechnungDto createOrUpdateRechnung(icreateOrUpdateRechnungDto rechnung)
        {
            ServiceHandler<icreateOrUpdateRechnungDto, ocreateOrUpdateRechnungDto> ew = new ServiceHandler<icreateOrUpdateRechnungDto, ocreateOrUpdateRechnungDto>(rechnung);
            return ew.process(delegate(icreateOrUpdateRechnungDto input, ocreateOrUpdateRechnungDto rval,CredentialContext ctx)
            {

                if (input == null || input.rechnung == null)
                    throw new ArgumentException("No valid input");

                rval.rechnung = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRechnung(input.rechnung);

            });
        }

        /// <summary>
        /// delivers Angobbrief detail
        /// </summary>
        /// <param name="angobbrief"></param>
        /// <returns></returns>
        public ocreateOrUpdateAngobbriefDto createOrUpdateAngobbrief(icreateOrUpdateAngobbriefDto angobbrief)
        {
            ServiceHandler<icreateOrUpdateAngobbriefDto, ocreateOrUpdateAngobbriefDto> ew = new ServiceHandler<icreateOrUpdateAngobbriefDto, ocreateOrUpdateAngobbriefDto>(angobbrief);
            return ew.process(delegate(icreateOrUpdateAngobbriefDto input, ocreateOrUpdateAngobbriefDto rval,CredentialContext ctx)
            {

                if (input == null || input.angobbrief == null)
                    throw new ArgumentException("No valid input");

                rval.angobbrief = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngobbrief(input.angobbrief);

            });
        }

        /// <summary>
        /// delivers Zahlungsplan detail
        /// </summary>
        /// <param name="zahlungsplan"></param>
        /// <returns></returns>
        public ocreateOrUpdateZahlungsplanDto createOrUpdateZahlungsplan(icreateOrUpdateZahlungsplanDto zahlungsplan)
        {
            ServiceHandler<icreateOrUpdateZahlungsplanDto, ocreateOrUpdateZahlungsplanDto> ew = new ServiceHandler<icreateOrUpdateZahlungsplanDto, ocreateOrUpdateZahlungsplanDto>(zahlungsplan);
            return ew.process(delegate(icreateOrUpdateZahlungsplanDto input, ocreateOrUpdateZahlungsplanDto rval,CredentialContext ctx)
            {

                if (input == null || input.zahlungsplan == null)
                    throw new ArgumentException("No valid input");

                rval.zahlungsplan = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateZahlungsplan(input.zahlungsplan);

            });
        }

		/// <summary>
		/// delivers Kreditlinie detail
		/// </summary>
		/// <param name="kreditlinie"></param>
		/// <returns></returns>
		public ocreateOrUpdateKreditlinieDto createOrUpdateKreditlinie (icreateOrUpdateKreditlinieDto kreditlinie)
		{
			ServiceHandler<icreateOrUpdateKreditlinieDto, ocreateOrUpdateKreditlinieDto> ew = new ServiceHandler<icreateOrUpdateKreditlinieDto, ocreateOrUpdateKreditlinieDto> (kreditlinie);
			return ew.process (delegate (icreateOrUpdateKreditlinieDto input, ocreateOrUpdateKreditlinieDto rval, CredentialContext ctx)
			{

				if (input == null || input.kreditlinie == null)
					throw new ArgumentException ("No valid input");

				rval.kreditlinie = BOFactoryFactory.getInstance ().getEntityBO (ctx.getMembershipInfo ()).createOrUpdateKreditlinie (input.kreditlinie);

			});
		}

		/// <summary>
        /// delivers Fahrzeugbrief detail
        /// </summary>
        /// <param name="fahrzeugbrief"></param>
        /// <returns></returns>
        public ocreateOrUpdateFahrzeugbriefDto createOrUpdateFahrzeugbrief(icreateOrUpdateFahrzeugbriefDto fahrzeugbrief)
        {
            ServiceHandler<icreateOrUpdateFahrzeugbriefDto, ocreateOrUpdateFahrzeugbriefDto> ew = new ServiceHandler<icreateOrUpdateFahrzeugbriefDto, ocreateOrUpdateFahrzeugbriefDto>(fahrzeugbrief);
            return ew.process(delegate(icreateOrUpdateFahrzeugbriefDto input, ocreateOrUpdateFahrzeugbriefDto rval,CredentialContext ctx)
            {

                if (input == null || input.fahrzeugbrief == null)
                    throw new ArgumentException("No valid input");

                rval.fahrzeugbrief = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateFahrzeugbrief(input.fahrzeugbrief);

            });
        }

        /// <summary>
        /// delivers Kalk detail
        /// </summary>
        /// <param name="kalk"></param>
        /// <returns></returns>
        public ocreateOrUpdateKalkDto createOrUpdateKalk(icreateOrUpdateKalkDto kalk)
        {
            ServiceHandler<icreateOrUpdateKalkDto, ocreateOrUpdateKalkDto> ew = new ServiceHandler<icreateOrUpdateKalkDto, ocreateOrUpdateKalkDto>(kalk);
            return ew.process(delegate(icreateOrUpdateKalkDto input, ocreateOrUpdateKalkDto rval,CredentialContext ctx)
            {

                if (input == null || input.kalk == null)
                    throw new ArgumentException("No valid input");

                rval.kalk = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateKalk(input.kalk);

            });
        }

        /// <summary>
        /// delivers Person detail
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public ocreateOrUpdatePersonDto createOrUpdatePerson(icreateOrUpdatePersonDto person)
        {
            ServiceHandler<icreateOrUpdatePersonDto, ocreateOrUpdatePersonDto> ew = new ServiceHandler<icreateOrUpdatePersonDto, ocreateOrUpdatePersonDto>(person);
            return ew.process(delegate(icreateOrUpdatePersonDto input, ocreateOrUpdatePersonDto rval,CredentialContext ctx)
            {

                if (input == null || input.person == null)
                    throw new ArgumentException("No valid input");

                rval.person = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdatePerson(input.person);

            });
        }

        /// <summary>
        /// delivers Kunde detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateKundeDto createOrUpdateKunde(icreateOrUpdateKundeDto kunde)
        {
            ServiceHandler<icreateOrUpdateKundeDto, ocreateOrUpdateKundeDto> ew = new ServiceHandler<icreateOrUpdateKundeDto, ocreateOrUpdateKundeDto>(kunde);

            return ew.process(delegate(icreateOrUpdateKundeDto input, ocreateOrUpdateKundeDto rval, CredentialContext ctx)
            {

                if (input == null || input.kunde == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.kunde = bo.createOrUpdateKunde(input.kunde);
            });
        }

        /// <summary>
        /// delivers Finanzierungdetail
        /// </summary>
        /// <param name="finanzierung"></param>
        /// <returns></returns>
        public ocreateOrUpdateFinanzierungDto createOrUpdateFinanzierung(icreateOrUpdateFinanzierungDto finanzierung)
        {
            // TODO NKK in edmx
            ServiceHandler<icreateOrUpdateFinanzierungDto, ocreateOrUpdateFinanzierungDto> ew = new ServiceHandler<icreateOrUpdateFinanzierungDto, ocreateOrUpdateFinanzierungDto>(finanzierung);
            return ew.process(delegate(icreateOrUpdateFinanzierungDto input, ocreateOrUpdateFinanzierungDto rval, CredentialContext ctx)
            {

                if (input == null || input.finanzierung == null)
                    throw new ArgumentException("No valid input");


                rval.finanzierung = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateFinanzierung(input.finanzierung, input.saveMode);


            });
        }

        /// <summary>
        /// delivers Rechnung detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateRechnungFaelligDto createOrUpdateRechnungFaellig(icreateOrUpdateRechnungFaelligDto rechnungFaellig)
        {

            ServiceHandler<icreateOrUpdateRechnungFaelligDto, ocreateOrUpdateRechnungFaelligDto> ew = new ServiceHandler<icreateOrUpdateRechnungFaelligDto, ocreateOrUpdateRechnungFaelligDto>(rechnungFaellig);
            return ew.process(delegate(icreateOrUpdateRechnungFaelligDto input, ocreateOrUpdateRechnungFaelligDto rval, CredentialContext ctx)
            {

                if (input == null || input.rechnungFaellig == null)
                    throw new ArgumentException("No valid input");


                rval.rechnungFaellig = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRechnungFaellig(input.rechnungFaellig);

            });
        }

        /// <summary>
        /// delivers Tilgung detail
        /// </summary>
        /// <param name="tilgung"></param>
        /// <returns></returns>
        public ocreateOrUpdateTilgungDto createOrUpdateTilgung(icreateOrUpdateTilgungDto tilgung)
        {

            ServiceHandler<icreateOrUpdateTilgungDto, ocreateOrUpdateTilgungDto> ew = new ServiceHandler<icreateOrUpdateTilgungDto, ocreateOrUpdateTilgungDto>(tilgung);
            return ew.process(delegate(icreateOrUpdateTilgungDto input, ocreateOrUpdateTilgungDto rval, CredentialContext ctx)
            {

                if (input == null || input.tilgung == null)
                    throw new ArgumentException("No valid input");


                rval.tilgung = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateTilgung(input.tilgung);

            });
        }



        /// <summary>
        ///updates Objekt detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateObjektDto createOrUpdateObjekt(icreateOrUpdateObjektDto objekt)
        {

            ServiceHandler<icreateOrUpdateObjektDto, ocreateOrUpdateObjektDto> ew = new ServiceHandler<icreateOrUpdateObjektDto, ocreateOrUpdateObjektDto>(objekt);
            return ew.process(delegate(icreateOrUpdateObjektDto input, ocreateOrUpdateObjektDto rval, CredentialContext ctx)
            {

                if (input == null || input.objekt == null)
                    throw new ArgumentException("No valid input");


                rval.objekt = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateObjekt(input.objekt);

            });
        }

        /// <summary>
        ///updates HEK Objekt detail in eaihot
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateHEKObjektDto createOrUpdateHEKObjekt(icreateOrUpdateHEKObjektDto objekt)
        {

            ServiceHandler<icreateOrUpdateHEKObjektDto, ocreateOrUpdateHEKObjektDto> ew = new ServiceHandler<icreateOrUpdateHEKObjektDto, ocreateOrUpdateHEKObjektDto>(objekt);
            return ew.process(delegate(icreateOrUpdateHEKObjektDto input, ocreateOrUpdateHEKObjektDto rval, CredentialContext ctx)
            {

                if (input == null || input.objekt == null)
                    throw new ArgumentException("No valid input");

                try
                {
                    IEurotaxBo etbo = AuskunftBoFactory.CreateDefaultEurotaxBo();
                    EurotaxInDto etin = new EurotaxInDto();
                    etin.NationalVehicleCode = Convert.ToInt64(input.objekt.schwacke);
                    etin.RegistrationDate = input.objekt.erstzul.Value;
                    etin.Mileage = "" + input.objekt.ubnahmekm;
                    etin.ISOCountryCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcountryType.DE;
                    etin.ISOLanguageCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOlanguageType.DE;
                    etin.ISOCurrencyCodeValuation = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxValuationRef.ISOcurrencyType.EUR;
                    etin.ISOCountryCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOcountryType.DE;
                    etin.ISOCurrencyCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOcurrencyType.EUR;
                    etin.ISOLanguageCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef.ISOlanguageType.DE;

                    _log.Debug("Fetching  TotalValuationTradeAmount for vehicle "+input.objekt.schwacke+"/"+input.objekt.erstzul.Value+"/"+input.objekt.ubnahmekm);
                    EurotaxOutDto etout = etbo.GetValuation(etin);
                    //netto TradeAmount TotalValuation
                    input.objekt.rw = etout.TotalValuationTradeAmount;
                    _log.Debug("Fetched RW from ET Valuation: "+input.objekt.rw);
                    if(etout.ErrorCode>0)
                    {
                        _log.Warn("HEK Price fetched with errorcode "+etout.ErrorCode+"/"+etout.ErrorDescription +" from ET Valuation for HEK_FO_USED_CAR_FIN");
                    }
                }catch(Exception e)
                {
                    _log.Error("HEK Price not fetched from ET Valuation for HEK_FO_USED_CAR_FIN: " + input.objekt.schwacke + ": " + e.Message);
                    input.objekt.rw = -1;
                }
                rval.objekt = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateHEKObjekt(input.objekt);

            });
        }

        /// <summary>
        /// creates/updates Recalc
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateRecalcDto createOrUpdateRecalc(icreateOrUpdateRecalcDto recalc)
        {

            ServiceHandler<icreateOrUpdateRecalcDto, ocreateOrUpdateRecalcDto> ew = new ServiceHandler<icreateOrUpdateRecalcDto, ocreateOrUpdateRecalcDto>(recalc);
            return ew.process(delegate(icreateOrUpdateRecalcDto input, ocreateOrUpdateRecalcDto rval, CredentialContext ctx)
            {

                if (input == null || input.recalc == null)
                    throw new ArgumentException("No valid input");


                rval.recalc = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRecalc(input.recalc);

            });
        }


        /// <summary>
        /// creates/updates Mycalc
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        public ocreateOrUpdateMycalcDto createOrUpdateMycalc(icreateOrUpdateMycalcDto mycalc)
        {

            ServiceHandler<icreateOrUpdateMycalcDto, ocreateOrUpdateMycalcDto> ew = new ServiceHandler<icreateOrUpdateMycalcDto, ocreateOrUpdateMycalcDto>(mycalc);
            return ew.process(delegate(icreateOrUpdateMycalcDto input, ocreateOrUpdateMycalcDto rval, CredentialContext ctx)
            {

                if (input == null || input.mycalc == null)
                    throw new ArgumentException("No valid input");


                rval.mycalc = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateMycalc(input.mycalc);

            });
        }

        /// <summary>
        /// creates/updates Mycalcfs
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        public ocreateOrUpdateMycalcfsDto createOrUpdateMycalcfs(icreateOrUpdateMycalcfsDto mycalcfs)
        {

            ServiceHandler<icreateOrUpdateMycalcfsDto, ocreateOrUpdateMycalcfsDto> ew = new ServiceHandler<icreateOrUpdateMycalcfsDto, ocreateOrUpdateMycalcfsDto>(mycalcfs);
            return ew.process(delegate(icreateOrUpdateMycalcfsDto input, ocreateOrUpdateMycalcfsDto rval, CredentialContext ctx)
            {

                if (input == null || input.mycalcfs == null)
                    throw new ArgumentException("No valid input");


                rval.mycalcfs = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateMycalcfs(input.mycalcfs);

            });
        }


        /// <summary>
        /// delivers Rahmen detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateRahmenDto createOrUpdateRahmen(icreateOrUpdateRahmenDto rahmen)
        {

            ServiceHandler<icreateOrUpdateRahmenDto, ocreateOrUpdateRahmenDto> ew = new ServiceHandler<icreateOrUpdateRahmenDto, ocreateOrUpdateRahmenDto>(rahmen);
            return ew.process(delegate(icreateOrUpdateRahmenDto input, ocreateOrUpdateRahmenDto rval, CredentialContext ctx)
            {

                if (input == null || input.rahmen == null)
                    throw new ArgumentException("No valid input");


                rval.rahmen = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateRahmen(input.rahmen);

            });
        }

        /// <summary>
        /// delivers Haendler detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateHaendlerDto createOrUpdateHaendler(icreateOrUpdateHaendlerDto haendler)
        {

            ServiceHandler<icreateOrUpdateHaendlerDto, ocreateOrUpdateHaendlerDto> ew = new ServiceHandler<icreateOrUpdateHaendlerDto, ocreateOrUpdateHaendlerDto>(haendler);
            return ew.process(delegate(icreateOrUpdateHaendlerDto input, ocreateOrUpdateHaendlerDto rval, CredentialContext ctx)
            {

                if (input == null || input.haendler == null)
                    throw new ArgumentException("No valid input");


                rval.haendler = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateHaendler(input.haendler);

            });
        }

        /// <summary>
        /// delivers It detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateItDto createOrUpdateIt(icreateOrUpdateItDto it)
        {
            ServiceHandler<icreateOrUpdateItDto, ocreateOrUpdateItDto> ew = new ServiceHandler<icreateOrUpdateItDto, ocreateOrUpdateItDto>(it);

            return ew.process(delegate(icreateOrUpdateItDto input, ocreateOrUpdateItDto rval, CredentialContext ctx)
            {

                if (input == null || input.It == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.It = bo.createOrUpdateIt(input.It);
            });
        }

        /// <summary>
        /// delivers Contact detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateContactDto createOrUpdateContact(icreateOrUpdateContactDto contact)
        {

            ServiceHandler<icreateOrUpdateContactDto, ocreateOrUpdateContactDto> ew = new ServiceHandler<icreateOrUpdateContactDto, ocreateOrUpdateContactDto>(contact);
            return ew.process(delegate(icreateOrUpdateContactDto input, ocreateOrUpdateContactDto rval, CredentialContext ctx)
            {

                if (input == null || input.contact == null)
                    throw new ArgumentException("No valid input");

                if (input.forSQL == null || input.forSQL.Length == 0)
                    rval.contact = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateContact(input.contact);
                else
                    BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateContacts(input.contact, input.forSQL);


            });
        }

        /// <summary>
        /// delivers Adresse detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAdresseDto createOrUpdateAdresse(icreateOrUpdateAdresseDto adresse)
        {

            ServiceHandler<icreateOrUpdateAdresseDto, ocreateOrUpdateAdresseDto> ew = new ServiceHandler<icreateOrUpdateAdresseDto, ocreateOrUpdateAdresseDto>(adresse);
            return ew.process(delegate(icreateOrUpdateAdresseDto input, ocreateOrUpdateAdresseDto rval, CredentialContext ctx)
            {

                if (input == null || input.adresse == null)
                    throw new ArgumentException("No valid input");

                int? temp = input.adresse.rang;
                try
                {
                    int newrang;
                    Int32.TryParse(input.adresse.sysAdrTp.ToString(), out newrang);
                    input.adresse.rang = newrang;
                }
                catch
                {
                    input.adresse.rang = temp;
                }

                rval.adresse = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAdresse(input.adresse);



            });
        }

        /// <summary>
        /// delivers Camp detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateCampDto createOrUpdateCamp(icreateOrUpdateCampDto camp)
        {

            ServiceHandler<icreateOrUpdateCampDto, ocreateOrUpdateCampDto> ew = new ServiceHandler<icreateOrUpdateCampDto, ocreateOrUpdateCampDto>(camp);
            return ew.process(delegate(icreateOrUpdateCampDto input, ocreateOrUpdateCampDto rval, CredentialContext ctx)
            {

                if (input == null || input.camp == null)
                    throw new ArgumentException("No valid input");

                rval.camp = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateCamp(input.camp);


            });
        }

        /// <summary>
        /// updates Oppo detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateOppoDto createOrUpdateOppo(icreateOrUpdateOppoDto oppo)
        {

            ServiceHandler<icreateOrUpdateOppoDto, ocreateOrUpdateOppoDto> ew = new ServiceHandler<icreateOrUpdateOppoDto, ocreateOrUpdateOppoDto>(oppo);
            return ew.process(delegate(icreateOrUpdateOppoDto input, ocreateOrUpdateOppoDto rval, CredentialContext ctx)
            {

                if (input == null || input.oppo == null)
                    throw new ArgumentException("No valid input");

                rval.oppo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateOppo(input.oppo);



            });
        }

        /// <summary>
        /// updates Oppotask detail
        /// </summary>
        /// <param name="oppotask"></param>
        /// <returns></returns>
        public ocreateOrUpdateOppotaskDto createOrUpdateOppotask(icreateOrUpdateOppotaskDto oppotask)
        {

            ServiceHandler<icreateOrUpdateOppotaskDto, ocreateOrUpdateOppotaskDto> ew = new ServiceHandler<icreateOrUpdateOppotaskDto, ocreateOrUpdateOppotaskDto>(oppotask);
            return ew.process(delegate(icreateOrUpdateOppotaskDto input, ocreateOrUpdateOppotaskDto rval, CredentialContext ctx)
            {

                if (input == null || input.oppotask == null)
                    throw new ArgumentException("No valid input");

                rval.oppotask = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateOppotask(input.oppotask);



            });
        }

        /// <summary>
        /// delivers Account detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAccountDto createOrUpdateAccount(icreateOrUpdateAccountDto account)
        {
            ServiceHandler<icreateOrUpdateAccountDto, ocreateOrUpdateAccountDto> ew = new ServiceHandler<icreateOrUpdateAccountDto, ocreateOrUpdateAccountDto>(account);
            return ew.process(delegate(icreateOrUpdateAccountDto input, ocreateOrUpdateAccountDto rval, CredentialContext ctx)
            {

                if (input == null || input.account == null)
                    throw new ArgumentException("No valid input");

                rval.account = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAccount(input.account);

            });
        }

        /// <summary>
        /// creates/updates WktAccount detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateWktAccountDto createOrUpdateWktAccount(icreateOrUpdateWktAccountDto account)
        {
            ServiceHandler<icreateOrUpdateWktAccountDto, ocreateOrUpdateWktAccountDto> ew = new ServiceHandler<icreateOrUpdateWktAccountDto, ocreateOrUpdateWktAccountDto>(account);
            return ew.process(delegate(icreateOrUpdateWktAccountDto input, ocreateOrUpdateWktAccountDto rval, CredentialContext ctx)
            {

                if (input == null || input.wktaccount == null)
                    throw new ArgumentException("No valid input");

                rval.wktaccount = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateWktAccount(input.wktaccount);

            });
        }

        /// <summary>
        /// delivers Partner detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePartnerDto createOrUpdatePartner(icreateOrUpdatePartnerDto partner)
        {

            ServiceHandler<icreateOrUpdatePartnerDto, ocreateOrUpdatePartnerDto> ew = new ServiceHandler<icreateOrUpdatePartnerDto, ocreateOrUpdatePartnerDto>(partner);
            return ew.process(delegate(icreateOrUpdatePartnerDto input, ocreateOrUpdatePartnerDto rval, CredentialContext ctx)
            {

                if (input == null || input.partner == null)
                    throw new ArgumentException("No valid input");


                rval.partner = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdatePartner(input.partner);

            });
        }

        /// <summary>
        /// delivers Beteiligter detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateBeteiligterDto createOrUpdateBeteiligter(icreateOrUpdateBeteiligterDto beteiligter)
        {

            ServiceHandler<icreateOrUpdateBeteiligterDto, ocreateOrUpdateBeteiligterDto> ew = new ServiceHandler<icreateOrUpdateBeteiligterDto, ocreateOrUpdateBeteiligterDto>(beteiligter);
            return ew.process(delegate(icreateOrUpdateBeteiligterDto input, ocreateOrUpdateBeteiligterDto rval, CredentialContext ctx)
            {

                if (input == null || input.beteiligter == null)
                    throw new ArgumentException("No valid input");


                rval.beteiligter = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateBeteiligter(input.beteiligter);

            });
        }


        /// <summary>
        /// delivers Konto detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateKontoDto createOrUpdateKonto(icreateOrUpdateKontoDto konto)
        {

            ServiceHandler<icreateOrUpdateKontoDto, ocreateOrUpdateKontoDto> ew = new ServiceHandler<icreateOrUpdateKontoDto, ocreateOrUpdateKontoDto>(konto);
            return ew.process(delegate(icreateOrUpdateKontoDto input, ocreateOrUpdateKontoDto rval, CredentialContext ctx)
            {

                if (input == null || input.konto == null)
                    throw new ArgumentException("No valid input");


                int? temp = input.konto.rang;
                try
                {
                    int newrang;
                    Int32.TryParse(input.konto.syskontoTp.ToString(), out newrang);
                    input.konto.rang = newrang;
                }
                catch
                {
                    input.konto.rang = temp;
                }

                rval.konto = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateKonto(input.konto);



            });
        }


        /// <summary>
        /// delivers Itkonto detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateItkontoDto createOrUpdateItkonto(icreateOrUpdateItkontoDto itkonto)
        {

            ServiceHandler<icreateOrUpdateItkontoDto, ocreateOrUpdateItkontoDto> ew = new ServiceHandler<icreateOrUpdateItkontoDto, ocreateOrUpdateItkontoDto>(itkonto);
            return ew.process(delegate(icreateOrUpdateItkontoDto input, ocreateOrUpdateItkontoDto rval, CredentialContext ctx)
            {

                if (input == null || input.itkonto == null)
                    throw new ArgumentException("No valid input");

                rval.itkonto = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateItkonto(input.itkonto);

            });
        }


        /// <summary>
        /// delivers Itadresse detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateItadresseDto createOrUpdateItadresse(icreateOrUpdateItadresseDto itadresse)
        {

            ServiceHandler<icreateOrUpdateItadresseDto, ocreateOrUpdateItadresseDto> ew = new ServiceHandler<icreateOrUpdateItadresseDto, ocreateOrUpdateItadresseDto>(itadresse);
            return ew.process(delegate(icreateOrUpdateItadresseDto input, ocreateOrUpdateItadresseDto rval, CredentialContext ctx)
            {

                if (input == null || input.itadresse == null)
                    throw new ArgumentException("No valid input");


                rval.itadresse = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateItadresse(input.itadresse);

            });
        }

        /// <summary>
        /// delivers Angebot detail
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        public ocreateOrUpdateAngebotDto createOrUpdateAngebot(icreateOrUpdateAngebotDto angebot)
        {

            ServiceHandler<icreateOrUpdateAngebotDto, ocreateOrUpdateAngebotDto> ew = new ServiceHandler<icreateOrUpdateAngebotDto, ocreateOrUpdateAngebotDto>(angebot);
            return ew.process(delegate(icreateOrUpdateAngebotDto input, ocreateOrUpdateAngebotDto rval, CredentialContext ctx)
            {

                if (input == null || input.angebot == null)
                    throw new ArgumentException("No valid input");


                rval.angebot = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngebot(input.angebot);

            });
        }

        /// <summary>
        /// delivers Antrag detail
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public ocreateOrUpdateAntragDto createOrUpdateAntrag(icreateOrUpdateAntragDto antrag)
        {

            ServiceHandler<icreateOrUpdateAntragDto, ocreateOrUpdateAntragDto> ew = new ServiceHandler<icreateOrUpdateAntragDto, ocreateOrUpdateAntragDto>(antrag);
            return ew.process(delegate(icreateOrUpdateAntragDto input, ocreateOrUpdateAntragDto rval, CredentialContext ctx)
            {

                if (input == null || input.antrag == null)
                    throw new ArgumentException("No valid input");


                rval.antrag = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAntrag(input.antrag);

            });
        }

        /// <summary>
        /// creates or updates an offer from the external application data structure
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        public ocreateOrUpdateExAngebotDto createOrUpdateExAngebot(ExcalcDto angebot)
        {

            ServiceHandler<ExcalcDto, ocreateOrUpdateExAngebotDto> ew = new ServiceHandler<ExcalcDto, ocreateOrUpdateExAngebotDto>(angebot);
            return ew.process(delegate(ExcalcDto input, ocreateOrUpdateExAngebotDto rval, CredentialContext ctx)
            {

                if (input == null || input.calculation == null || input.customer == null)
                    throw new ArgumentException("No valid input");

                AngebotDto mappedAngebot = new AngebotDto();
                mappedAngebot.varianten = new List<AngvarDto>();
                AngvarDto var = new AngvarDto();
                var.angobList = new List<AngobDto>();
                mappedAngebot.varianten.Add(var);

                if (input.obj == null)
                { 
                    input.obj = new ExobjectDto();
                    input.obj.Description = "Kredit";
                    input.obj.Price = input.calculation.Price;
                }
                
                AngobDto  mappedObj= Mapper.Map<ExobjectDto, AngobDto>(input.obj);
                mappedObj.bestellung = input.calculation.Date;
                mappedObj.lieferung = input.calculation.Date;
                mappedObj.erstzul = input.calculation.Date;
                mappedObj.zustand = "Neu";

                var.angobList.Add(mappedObj);

                ItDto mappedIt = Mapper.Map<ExcustomerDto, ItDto>(input.customer);
                AngkalkDto mappedKalk = Mapper.Map<ExcalculationDto, AngkalkDto>(input.calculation);

                mappedAngebot.options = new AngAntOptionDto();

                //IT
                mappedIt = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateIt(mappedIt);
                mappedAngebot.sysIt = mappedIt.sysit;
                if (input.calculation.Date.HasValue)
                    mappedAngebot.beginn = input.calculation.Date.Value;
                else
                    mappedAngebot.beginn = DateTime.Today;
                mappedAngebot.ende = mappedAngebot.beginn.AddMonths(3);
                mappedAngebot.zustand = "Neu";
                mappedAngebot.objektVT = input.calculation.Description;
                //ANGEBOT
                mappedAngebot = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngebot(mappedAngebot);

                //KALKULATION
                mappedKalk.sysangebot = mappedAngebot.sysID;
                mappedKalk.sysangvar = mappedAngebot.varianten[0].sysAngvar;
                mappedKalk = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngkalk(mappedKalk);
                
                rval.angebot = mappedAngebot.angebot;

            });
        }

        /// <summary>
        /// delivers Angob detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAngobDto createOrUpdateAngob(icreateOrUpdateAngobDto angob)
        {

            ServiceHandler<icreateOrUpdateAngobDto, ocreateOrUpdateAngobDto> ew = new ServiceHandler<icreateOrUpdateAngobDto, ocreateOrUpdateAngobDto>(angob);
            return ew.process(delegate(icreateOrUpdateAngobDto input, ocreateOrUpdateAngobDto rval, CredentialContext ctx)
            {

                if (input == null || input.angob == null)
                    throw new ArgumentException("No valid input");


                rval.angob = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngob(input.angob);

            });
        }

        /// <summary>
        /// delivers Antob detail
        /// </summary>
        /// <param name="antob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAntobDto createOrUpdateAntob(icreateOrUpdateAntobDto antob)
        {

            ServiceHandler<icreateOrUpdateAntobDto, ocreateOrUpdateAntobDto> ew = new ServiceHandler<icreateOrUpdateAntobDto, ocreateOrUpdateAntobDto>(antob);
            return ew.process(delegate(icreateOrUpdateAntobDto input, ocreateOrUpdateAntobDto rval, CredentialContext ctx)
            {

                if (input == null || input.antob == null)
                    throw new ArgumentException("No valid input");


                rval.antob = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAntob(input.antob);

            });
        }

        /// <summary>
        /// delivers Angvar detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAngvarDto createOrUpdateAngvar(icreateOrUpdateAngvarDto angvar)
        {

            ServiceHandler<icreateOrUpdateAngvarDto, ocreateOrUpdateAngvarDto> ew = new ServiceHandler<icreateOrUpdateAngvarDto, ocreateOrUpdateAngvarDto>(angvar);
            return ew.process(delegate(icreateOrUpdateAngvarDto input, ocreateOrUpdateAngvarDto rval, CredentialContext ctx)
            {

                if (input == null || input.angvar == null)
                    throw new ArgumentException("No valid input");


                rval.angvar = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngvar(input.angvar);

            });
        }

        /// <summary>
        /// delivers Angkalk detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateAngkalkDto createOrUpdateAngkalk(icreateOrUpdateAngkalkDto angkalk)
        {

            ServiceHandler<icreateOrUpdateAngkalkDto, ocreateOrUpdateAngkalkDto> ew = new ServiceHandler<icreateOrUpdateAngkalkDto, ocreateOrUpdateAngkalkDto>(angkalk);
            return ew.process(delegate(icreateOrUpdateAngkalkDto input, ocreateOrUpdateAngkalkDto rval, CredentialContext ctx)
            {

                if (input == null || input.angkalk == null)
                    throw new ArgumentException("No valid input");


                rval.angkalk = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAngkalk(input.angkalk);

            });
        }

        /// <summary>
        /// delivers Antkalk detail
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        public ocreateOrUpdateAntkalkDto createOrUpdateAntkalk(icreateOrUpdateAntkalkDto antkalk)
        {

            ServiceHandler<icreateOrUpdateAntkalkDto, ocreateOrUpdateAntkalkDto> ew = new ServiceHandler<icreateOrUpdateAntkalkDto, ocreateOrUpdateAntkalkDto>(antkalk);
            return ew.process(delegate(icreateOrUpdateAntkalkDto input, ocreateOrUpdateAntkalkDto rval, CredentialContext ctx)
            {

                if (input == null || input.antkalk == null)
                    throw new ArgumentException("No valid input");


                rval.antkalk = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateAntkalk(input.antkalk);

            });
        }

        /// <summary>
        /// delivers Ptrelate detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePtrelateDto createOrUpdatePtrelate(icreateOrUpdatePtrelateDto ptrelate)
        {
            ServiceHandler<icreateOrUpdatePtrelateDto, ocreateOrUpdatePtrelateDto> ew = new ServiceHandler<icreateOrUpdatePtrelateDto, ocreateOrUpdatePtrelateDto>(ptrelate);
            return ew.process(delegate(icreateOrUpdatePtrelateDto input, ocreateOrUpdatePtrelateDto rval, CredentialContext ctx)
            {

                if (input == null || input.ptrelate == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ptrelate = bo.createOrUpdatePtrelate(input.ptrelate);

            });
        }

        /// <summary>
        /// delivers Crmnm detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateCrmnmDto createOrUpdateCrmnm(icreateOrUpdateCrmnmDto Crmnm)
        {
            ServiceHandler<icreateOrUpdateCrmnmDto, ocreateOrUpdateCrmnmDto> ew = new ServiceHandler<icreateOrUpdateCrmnmDto, ocreateOrUpdateCrmnmDto>(Crmnm);
            return ew.process(delegate(icreateOrUpdateCrmnmDto input, ocreateOrUpdateCrmnmDto rval, CredentialContext ctx)
            {

                if (input == null || input.crmnm == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.crmnm = bo.createOrUpdateCrmnm(input.crmnm);

            });
        }

        /// <summary>
        /// delivers CrmProdukte detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateCrmprDto createOrUpdateCrmProdukte(icreateOrUpdateCrmprDto crmpr)
        {
            ServiceHandler<icreateOrUpdateCrmprDto, ocreateOrUpdateCrmprDto> ew = new ServiceHandler<icreateOrUpdateCrmprDto, ocreateOrUpdateCrmprDto>(crmpr);
            return ew.process(delegate(icreateOrUpdateCrmprDto input, ocreateOrUpdateCrmprDto rval, CredentialContext ctx)
            {

                if (input == null || input.crmpr == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.crmpr = bo.createOrUpdateCrmProdukte(input.crmpr);

            });
        }

        /// <summary>
        /// delivers Checklist detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePrunDto createOrUpdatePrun(icreateOrUpdatePrunDto prun)
        {
            ServiceHandler<icreateOrUpdatePrunDto, ocreateOrUpdatePrunDto> ew = new ServiceHandler<icreateOrUpdatePrunDto, ocreateOrUpdatePrunDto>(prun);
            return ew.process(delegate(icreateOrUpdatePrunDto input, ocreateOrUpdatePrunDto rval, CredentialContext ctx)
            {

                if (input == null || input.prun == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prun = bo.createOrUpdatePrun(input.prun);

            });
        }

        /// <summary>
        /// delivers Checklisttype detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePtypeDto createOrUpdatePtype(icreateOrUpdatePtypeDto ptype)
        {
            ServiceHandler<icreateOrUpdatePtypeDto, ocreateOrUpdatePtypeDto> ew = new ServiceHandler<icreateOrUpdatePtypeDto, ocreateOrUpdatePtypeDto>(ptype);
            return ew.process(delegate(icreateOrUpdatePtypeDto input, ocreateOrUpdatePtypeDto rval, CredentialContext ctx)
            {

                if (input == null || input.ptype == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ptype = bo.createOrUpdatePtype(input.ptype);

            });
        }

        /// <summary>
        /// delivers Check detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePrunstepDto createOrUpdatePrunstep(icreateOrUpdatePrunstepDto prunstep)
        {
            ServiceHandler<icreateOrUpdatePrunstepDto, ocreateOrUpdatePrunstepDto> ew = new ServiceHandler<icreateOrUpdatePrunstepDto, ocreateOrUpdatePrunstepDto>(prunstep);
            return ew.process(delegate(icreateOrUpdatePrunstepDto input, ocreateOrUpdatePrunstepDto rval, CredentialContext ctx)
            {

                if (input == null || input.prunstep == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prunstep = bo.createOrUpdatePrunstep(input.prunstep);

            });
        }

        /// <summary>
        /// delivers Checktype detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdatePstepDto createOrUpdatePstep(icreateOrUpdatePstepDto pstep)
        {
            ServiceHandler<icreateOrUpdatePstepDto, ocreateOrUpdatePstepDto> ew = new ServiceHandler<icreateOrUpdatePstepDto, ocreateOrUpdatePstepDto>(pstep);
            return ew.process(delegate(icreateOrUpdatePstepDto input, ocreateOrUpdatePstepDto rval, CredentialContext ctx)
            {

                if (input == null || input.pstep == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.pstep = bo.createOrUpdatePstep(input.pstep);

            });
        }

        /// <summary>
        /// delivers Segment detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateSegDto createOrUpdateSeg(icreateOrUpdateSegDto seg)
        {
            ServiceHandler<icreateOrUpdateSegDto, ocreateOrUpdateSegDto> ew = new ServiceHandler<icreateOrUpdateSegDto, ocreateOrUpdateSegDto>(seg);
            return ew.process(delegate(icreateOrUpdateSegDto input, ocreateOrUpdateSegDto rval, CredentialContext ctx)
            {

                if (input == null || input.seg == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.seg = bo.createOrUpdateSeg(input.seg);

            });
        }

        /// <summary>
        /// delivers SegmentKampagne detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateSegcDto createOrUpdateSegc(icreateOrUpdateSegcDto segc)
        {
            ServiceHandler<icreateOrUpdateSegcDto, ocreateOrUpdateSegcDto> ew = new ServiceHandler<icreateOrUpdateSegcDto, ocreateOrUpdateSegcDto>(segc);
            return ew.process(delegate(icreateOrUpdateSegcDto input, ocreateOrUpdateSegcDto rval, CredentialContext ctx)
            {

                if (input == null || input.segc == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.segc = bo.createOrUpdateSegc(input.segc);

            });
        }

        /// <summary>
        /// delivers prkgroup detail
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public ocreateOrUpdatePrkgroupDto createOrUpdatePrkgroup(icreateOrUpdatePrkgroupDto prkgroup)
        {

            ServiceHandler<icreateOrUpdatePrkgroupDto, ocreateOrUpdatePrkgroupDto> ew = new ServiceHandler<icreateOrUpdatePrkgroupDto, ocreateOrUpdatePrkgroupDto>(prkgroup);
            return ew.process(delegate(icreateOrUpdatePrkgroupDto input, ocreateOrUpdatePrkgroupDto rval, CredentialContext ctx)
            {

                if (input == null || input.prkgroup == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroup = bo.createOrUpdatePrkgroup(input.prkgroup);

            });
        }

        /// <summary>
        /// delivers prkgroup detail
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public ocreateOrUpdatePrkgroupmDto createOrUpdatePrkgroupm(icreateOrUpdatePrkgroupmDto prkgroup)
        {

            ServiceHandler<icreateOrUpdatePrkgroupmDto, ocreateOrUpdatePrkgroupmDto> ew = new ServiceHandler<icreateOrUpdatePrkgroupmDto, ocreateOrUpdatePrkgroupmDto>(prkgroup);
            return ew.process(delegate(icreateOrUpdatePrkgroupmDto input, ocreateOrUpdatePrkgroupmDto rval, CredentialContext ctx)
            {

                if (input == null || input.prkgroupm == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroupm = bo.createOrUpdatePrkgroupm(input.prkgroupm);

            });
        }

        /// <summary>
        /// saves/updates Dddlkppos
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public ocreateOrUpdateDdlkpsposDto createOrUpdateDddlkppos(icreateOrUpdateDdlkpsposDto ddlkpspos)
        {

            ServiceHandler<icreateOrUpdateDdlkpsposDto, ocreateOrUpdateDdlkpsposDto> ew = new ServiceHandler<icreateOrUpdateDdlkpsposDto, ocreateOrUpdateDdlkpsposDto>(ddlkpspos);
            return ew.process(delegate(icreateOrUpdateDdlkpsposDto input, ocreateOrUpdateDdlkpsposDto rval, CredentialContext ctx)
            {

                if (input == null || input.ddlkpspos == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ddlkpspos = bo.createOrUpdateDdlkpspos(input.ddlkpspos);

            });
        }

        /// <summary>
        /// delivers Stickynote detail
        /// </summary>
        /// <param name="stickynote"></param>
        /// <returns></returns>
        public ocreateOrUpdateStickynoteDto createOrUpdateStickynote(icreateOrUpdateStickynoteDto stickynote)
        {

            ServiceHandler<icreateOrUpdateStickynoteDto, ocreateOrUpdateStickynoteDto> ew = new ServiceHandler<icreateOrUpdateStickynoteDto, ocreateOrUpdateStickynoteDto>(stickynote);
            return ew.process(delegate(icreateOrUpdateStickynoteDto input, ocreateOrUpdateStickynoteDto rval, CredentialContext ctx)
            {

                if (input == null || input.stickynotes == null)
                    throw new ArgumentException("No valid input");

                rval.stickynotes = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateStickynotes(input.stickynotes);

            });
        }


        /// <summary>
        /// saves/updates stickytype
        /// </summary>
        /// <param name="stickytype"></param>
        /// <returns></returns>
        public ocreateOrUpdateStickytypeDto createOrUpdateStickytype(icreateOrUpdateStickytypeDto stickytype)
        {

            ServiceHandler<icreateOrUpdateStickytypeDto, ocreateOrUpdateStickytypeDto> ew = new ServiceHandler<icreateOrUpdateStickytypeDto, ocreateOrUpdateStickytypeDto>(stickytype);
            return ew.process(delegate(icreateOrUpdateStickytypeDto input, ocreateOrUpdateStickytypeDto rval, CredentialContext ctx)
            {

                if (input == null || input.stickytype == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.stickytype = bo.createOrUpdateStickytype(input.stickytype);

            });
        }

        /// <summary>
        /// saves/updates besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        public ocreateOrUpdateBesuchsberichtDto createOrUpdateBesuchsbericht(icreateOrUpdateBesuchsberichtDto besuchsbericht)
        {
            ServiceHandler<icreateOrUpdateBesuchsberichtDto, ocreateOrUpdateBesuchsberichtDto> ew = new ServiceHandler<icreateOrUpdateBesuchsberichtDto, ocreateOrUpdateBesuchsberichtDto>(besuchsbericht);
            return ew.process(delegate(icreateOrUpdateBesuchsberichtDto input, ocreateOrUpdateBesuchsberichtDto rval, CredentialContext ctx)
            {

                if (input == null || input.besuchsbericht == null)
                    throw new ArgumentException("No valid input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.besuchsbericht = bo.createOrUpdateBesuchsbericht(input.besuchsbericht);

            });
        }

       

        /// <summary>
        /// Solve a calculation
        /// </summary>
        /// <param name="ipar"></param>
        /// <returns></returns>
        public osolveKalkulationDto solveKalkulation(isolveKalkulationDto ipar)
        {
            ServiceHandler<isolveKalkulationDto, osolveKalkulationDto> ew = new ServiceHandler<isolveKalkulationDto, osolveKalkulationDto>(ipar);
            return ew.process(delegate(isolveKalkulationDto input, osolveKalkulationDto rval, CredentialContext ctx)
            {
                input.isoLanguageCode = ctx.getMembershipInfo().ISOLanguageCode;

                BOFactoryFactory.getInstance().getCalculationBo().solveKalkulation(input, rval);
            });
        }

        #region Mail


        /// <summary>
        /// delivers Kategorien detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateItemcatDto createOrUpdateItemcat(icreateOrUpdateItemcatDto itemcat)
        {
            ServiceHandler<icreateOrUpdateItemcatDto, ocreateOrUpdateItemcatDto> ew = new ServiceHandler<icreateOrUpdateItemcatDto, ocreateOrUpdateItemcatDto>(itemcat);
            return ew.process(delegate(icreateOrUpdateItemcatDto input, ocreateOrUpdateItemcatDto rval, CredentialContext ctx)
            {

                if (input == null || input.itemcat == null)
                    throw new ArgumentException("No valid input");



                rval.itemcat = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateItemcat(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }

        /// <summary>
        /// delivers ItemKategorien detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateItemcatmDto createOrUpdateItemcatm(icreateOrUpdateItemcatmDto itemcatm)
        {
            ServiceHandler<icreateOrUpdateItemcatmDto, ocreateOrUpdateItemcatmDto> ew = new ServiceHandler<icreateOrUpdateItemcatmDto, ocreateOrUpdateItemcatmDto>(itemcatm);
            return ew.process(delegate(icreateOrUpdateItemcatmDto input, ocreateOrUpdateItemcatmDto rval, CredentialContext ctx)
            {

                if (input == null || input.itemcatm == null)
                    throw new ArgumentException("No valid input");

                rval.itemcatm = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateItemcatm(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }

        /// <summary>
        /// delivers Attachement detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateFileattDto createOrUpdateFileatt(icreateOrUpdateFileattDto fileatt)
        {
            ServiceHandler<icreateOrUpdateFileattDto, ocreateOrUpdateFileattDto> ew = new ServiceHandler<icreateOrUpdateFileattDto, ocreateOrUpdateFileattDto>(fileatt);
            return ew.process(delegate(icreateOrUpdateFileattDto input, ocreateOrUpdateFileattDto rval, CredentialContext ctx)
            {

                if (input == null || input.fileatt == null)
                    throw new ArgumentException("No valid input");

                rval.fileatt = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateFileatt(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }

        /// <summary>
        /// creates or updates Dmsdoc detail
        /// </summary>
        /// <param name="dmsdoc"></param>
        /// <returns></returns>
        public ocreateOrUpdateDmsdocDto createOrUpdateDmsdoc(icreateOrUpdateDmsdocDto dmsdoc)
        {
            ServiceHandler<icreateOrUpdateDmsdocDto, ocreateOrUpdateDmsdocDto> ew = new ServiceHandler<icreateOrUpdateDmsdocDto, ocreateOrUpdateDmsdocDto>(dmsdoc);
            return ew.process(delegate(icreateOrUpdateDmsdocDto input, ocreateOrUpdateDmsdocDto rval, CredentialContext ctx)
            {

                if (input == null || input.dmsdoc == null)
                    throw new ArgumentException("No valid input");

                rval.dmsdoc = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateDmsdoc(input.dmsdoc);
                
            });
        }

        /// <summary>
        /// delivers Reminder detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateReminderDto createOrUpdateReminder(icreateOrUpdateReminderDto reminder)
        {
            ServiceHandler<icreateOrUpdateReminderDto, ocreateOrUpdateReminderDto> ew = new ServiceHandler<icreateOrUpdateReminderDto, ocreateOrUpdateReminderDto>(reminder);
            return ew.process(delegate(icreateOrUpdateReminderDto input, ocreateOrUpdateReminderDto rval, CredentialContext ctx)
            {

                if (input == null || input.reminder == null)
                    throw new ArgumentException("No valid input");

                rval.reminder = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateReminder(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }

        /// <summary>
        /// delivers Recurrence detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ocreateOrUpdateRecurrDto createOrUpdateRecurr(icreateOrUpdateRecurrDto recurr)
        {
            ServiceHandler<icreateOrUpdateRecurrDto, ocreateOrUpdateRecurrDto> ew = new ServiceHandler<icreateOrUpdateRecurrDto, ocreateOrUpdateRecurrDto>(recurr);
            return ew.process(delegate(icreateOrUpdateRecurrDto input, ocreateOrUpdateRecurrDto rval, CredentialContext ctx)
            {

                if (input == null || input.recurr == null)
                    throw new ArgumentException("No valid input");
                rval.recurr = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateRecurr(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }

        /// <summary>
        /// delivers Mailmsg detail
        /// </summary>
        /// <param name="mailmsg"></param>
        /// <returns></returns>
        public ocreateOrUpdateMailmsgDto createOrUpdateMailmsg(icreateOrUpdateMailmsgDto mailmsg)
        {
            ServiceHandler<icreateOrUpdateMailmsgDto, ocreateOrUpdateMailmsgDto> ew = new ServiceHandler<icreateOrUpdateMailmsgDto, ocreateOrUpdateMailmsgDto>(mailmsg);
            return ew.process(delegate(icreateOrUpdateMailmsgDto input, ocreateOrUpdateMailmsgDto rval, CredentialContext ctx)
            {

                if (input == null || input.mailmsg == null)
                    throw new ArgumentException("No valid input");

                rval.mailmsg = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateMailmsg(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: "+input.error.detail,input.error.type);
                }
            });
        }


        /// <summary>
        /// delivers Appointment detail
        /// </summary>
        /// <param name="apptm"></param>
        /// <returns></returns>
        public ocreateOrUpdateApptmtDto createOrUpdateApptmt(icreateOrUpdateApptmtDto apptm)
        {
            ServiceHandler<icreateOrUpdateApptmtDto, ocreateOrUpdateApptmtDto> ew = new ServiceHandler<icreateOrUpdateApptmtDto, ocreateOrUpdateApptmtDto>(apptm);
            return ew.process(delegate(icreateOrUpdateApptmtDto input, ocreateOrUpdateApptmtDto rval, CredentialContext ctx)
            {

                if (input == null || input.apptmt == null)
                    throw new ArgumentException("No valid input");

                rval.apptmt = BOFactoryFactory.getInstance().getEntityMailBO(input.apptmt.sysOwner).createOrUpdateApptmt(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }


            });
        }

        /// <summary>
        /// delivers Task detail
        /// </summary>
        /// <param name="ptask"></param>
        /// <returns></returns>
        public ocreateOrUpdatePtaskDto createOrUpdatePtask(icreateOrUpdatePtaskDto ptask)
        {
            ServiceHandler<icreateOrUpdatePtaskDto, ocreateOrUpdatePtaskDto> ew = new ServiceHandler<icreateOrUpdatePtaskDto, ocreateOrUpdatePtaskDto>(ptask);
            return ew.process(delegate(icreateOrUpdatePtaskDto input, ocreateOrUpdatePtaskDto rval, CredentialContext ctx)
            {

                if (input == null || input.ptask == null)
                    throw new ArgumentException("No valid input");

                rval.ptask = BOFactoryFactory.getInstance().getEntityMailBO(input.ptask.sysOwner).createOrUpdatePtask(input);
                if (input.error != null)
                {
                    throw new ServiceBaseException(input.error.code, "Senden fehlgeschlagen: " + input.error.detail, input.error.type);
                }
            });
        }



        /// <summary>
        /// delivers Wfsignature detail
        /// </summary>
        /// <param name="ptask"></param>
        /// <returns></returns>
        public ocreateOrUpdateWfsignatureDto createOrUpdateWfsignature(icreateOrUpdateWfsignatureDto wfsignature)
        {
            ServiceHandler<icreateOrUpdateWfsignatureDto, ocreateOrUpdateWfsignatureDto> ew = new ServiceHandler<icreateOrUpdateWfsignatureDto, ocreateOrUpdateWfsignatureDto>(wfsignature);
            return ew.process(delegate(icreateOrUpdateWfsignatureDto input, ocreateOrUpdateWfsignatureDto rval, CredentialContext ctx)
            {

                if (input == null || input.wfsignature == null)
                    throw new ArgumentException("No valid input");

                rval.wfsignature = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).createOrUpdateWfsignature(input);

            });
        }





        /// <summary>
        /// Überprüft und aktuallisiert die Exchange Subscription
        /// </summary>
        /// <returns></returns>
        public ocheckCreateSubscriptionDto CheckCreateSubscription()
        {
            ServiceHandler<long, ocheckCreateSubscriptionDto> ew = new ServiceHandler<long, ocheckCreateSubscriptionDto>();
            return ew.process(delegate(long input, ocheckCreateSubscriptionDto rval, CredentialContext ctx)
            {

                Cic.One.Web.DAO.Mail.MailDaoFactory.getInstance().CheckCreateSubscriptionAsync(ctx.getMembershipInfo().sysWFUSER);
            });
        }

        /// <summary>
        /// Leitet eine Mail weiter und gibt die neue zurück
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public oforwardMail forwardMail(iforwardMail inp)
        {
            ServiceHandler<iforwardMail, oforwardMail> ew = new ServiceHandler<iforwardMail, oforwardMail>(inp);
            return ew.process(delegate(iforwardMail input, oforwardMail rval, CredentialContext ctx)
            {

                if (input.sysid == 0)
                    throw new ArgumentException("No valid input");



                rval.mailmsg = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).forwardMail(input.sysid);

                
            });
        }

        /// <summary>
        /// Antwortet auf eine Mail und gibt die neue zurück
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public oreplyMail replyMail(ireplyMail inp)
        {
            ServiceHandler<ireplyMail, oreplyMail> ew = new ServiceHandler<ireplyMail, oreplyMail>(inp);
            return ew.process(delegate(ireplyMail input, oreplyMail rval, CredentialContext ctx)
            {

                if (input.sysid == 0)
                    throw new ArgumentException("No valid input");

                rval.mailmsg = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).replyMail(input.sysid, input.replyAll);

            });
        }

        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        public ocreateOrUpdateMemoDto createOrUpdateMemo(icreateOrUpdateMemoDto memo)
        {
            ServiceHandler<icreateOrUpdateMemoDto, ocreateOrUpdateMemoDto> ew = new ServiceHandler<icreateOrUpdateMemoDto, ocreateOrUpdateMemoDto>(memo);
            return ew.process(delegate(icreateOrUpdateMemoDto input, ocreateOrUpdateMemoDto rval, CredentialContext ctx)
            {

                if (input.memo == null)
                    throw new ArgumentException("No valid input");

                rval.memo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateMemo(input.memo, input.refTable);

            });
        }

        /// <summary>
        /// Aktualisiert oder erstellt einen PUser
        /// </summary>
        /// <param name="puser"></param>
        public ocreateOrUpdatePuserDto createOrUpdatePuser(icreateOrUpdatePuserDto puser)
        {
            ServiceHandler<icreateOrUpdatePuserDto, ocreateOrUpdatePuserDto> ew = new ServiceHandler<icreateOrUpdatePuserDto, ocreateOrUpdatePuserDto>(puser);
            return ew.process(delegate(icreateOrUpdatePuserDto input, ocreateOrUpdatePuserDto rval, CredentialContext ctx)
            {

                if (input.puser == null)
                    throw new ArgumentException("No valid input");

                rval.puser = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdatePuser(input.puser);

            });
        }

        /// <summary>
        /// Aktualisiert oder erstellt eine Abklärung
        /// </summary>
        /// <param name="clarification"></param>
        public ocreateOrUpdateClarificationDto createOrUpdateClarification(icreateOrUpdateClarificationDto clarification)
        {
            ServiceHandler<icreateOrUpdateClarificationDto, ocreateOrUpdateClarificationDto> ew = new ServiceHandler<icreateOrUpdateClarificationDto, ocreateOrUpdateClarificationDto>(clarification);
            return ew.process(delegate(icreateOrUpdateClarificationDto input, ocreateOrUpdateClarificationDto rval, CredentialContext ctx)
            {

                if (input.clarification == null)
                    throw new ArgumentException("No valid input");

                rval.clarification = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateClarification(input.clarification);

            });
        }

        #endregion


        /// <summary>
        /// deletes an Entity
        /// </summary>
        /// <param name="inp">Entity welche gelöscht werden soll</param>
        /// <returns></returns>
        public odeleteEntity deleteEntity(ideleteEntity inp)
        {
            ServiceHandler<ideleteEntity, odeleteEntity> ew = new ServiceHandler<ideleteEntity, odeleteEntity>(inp);
            return ew.process(delegate(ideleteEntity input, odeleteEntity rval, CredentialContext ctx)
            {

                if (input.sysid == 0 || string.IsNullOrEmpty(input.area))
                    throw new ArgumentException("No valid input");

                BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).deleteEntity(input.area, input.sysid);

            });
        }

        /// <summary>
        /// creates or updates a generic view entity data
        /// </summary>
        /// <param name="stickynote"></param>
        /// <returns></returns>
        public ocreateOrUpdateGviewDto createOrUpdateGview(icreateOrUpdateGviewDto gviewdata)
        {

            ServiceHandler<icreateOrUpdateGviewDto, ocreateOrUpdateGviewDto> ew = new ServiceHandler<icreateOrUpdateGviewDto, ocreateOrUpdateGviewDto>(gviewdata);
            return ew.process(delegate(icreateOrUpdateGviewDto input, ocreateOrUpdateGviewDto rval, CredentialContext ctx)
            {

                if (input == null || input.gview == null)
                    throw new ArgumentException("No valid input");

                rval.gview = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateGview(input.gview);

            });
        }

        /// <summary>
        /// creates or updates a generic Pread entity data
        /// </summary>
        /// <param name="stickynote"></param>
        /// <returns></returns>
        public ocreateOrUpdatePreadDto createOrUpdatePread (icreateOrUpdatePreadDto preaddata)
        {

            ServiceHandler<icreateOrUpdatePreadDto, ocreateOrUpdatePreadDto> ew = new ServiceHandler <icreateOrUpdatePreadDto, ocreateOrUpdatePreadDto> (preaddata);
            return ew.process (delegate (icreateOrUpdatePreadDto input, ocreateOrUpdatePreadDto rval, CredentialContext ctx)
            {

                if (input == null || input.pread == null)
                    throw new ArgumentException("No valid input");

				rval.pread = BOFactoryFactory.getInstance ().getEntityBO (ctx.getMembershipInfo ()).createOrUpdatePread (input.pread);

            });
        }


        /// <summary>
        /// Creates or Updates the ZEK data
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        public ocreateOrUpdateZekDto createOrUpdateZek(icreateOrUpdateZekDto zek)
        {
            ServiceHandler<icreateOrUpdateZekDto, ocreateOrUpdateZekDto> ew = new ServiceHandler<icreateOrUpdateZekDto, ocreateOrUpdateZekDto>(zek);
            return ew.process(delegate(icreateOrUpdateZekDto input, ocreateOrUpdateZekDto rval, CredentialContext ctx)
            {

                if (input==null ||input.zek==null)
                    throw new ArgumentException("No valid input");

                rval.zek = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateZek(input.zek);
            });
        }

        /// <summary>
        /// Creates or updates the Dok validation
        /// </summary>
        /// <param name="dok"></param>
        /// <returns></returns>
        public ocreateOrUpdateDokvalDto createOrUpdateDokval(icreateOrUpdateDokvalDto dok)
        {
            ServiceHandler<icreateOrUpdateDokvalDto, ocreateOrUpdateDokvalDto> ew = new ServiceHandler<icreateOrUpdateDokvalDto, ocreateOrUpdateDokvalDto>(dok);
            return ew.process(delegate(icreateOrUpdateDokvalDto input, ocreateOrUpdateDokvalDto rval, CredentialContext ctx)
            {

                if (input == null || input.dokval == null)
                    throw new ArgumentException("No valid input");

                rval.dokval = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateDokval(input.dokval);
            });
        }

        /// <summary>
        /// Creates or updates the checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        public ocreateOrUpdateChklistDto createOrUpdateChklist(icreateOrUpdateChklistDto chklist)
        {
            ServiceHandler<icreateOrUpdateChklistDto, ocreateOrUpdateChklistDto> ew = new ServiceHandler<icreateOrUpdateChklistDto, ocreateOrUpdateChklistDto>(chklist);
            return ew.process(delegate(icreateOrUpdateChklistDto input, ocreateOrUpdateChklistDto rval, CredentialContext ctx)
            {

                if (input == null || input.chklist == null)
                    throw new ArgumentException("No valid input");

                rval.chklist = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdateChklist(input.chklist);
            });
        }


        /// <summary>
        /// Creates or updates the prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        public ocreateOrUpdatePrunartDto createOrUpdatePrunart(icreateOrUpdatePrunartDto prunart)
        {
            ServiceHandler<icreateOrUpdatePrunartDto, ocreateOrUpdatePrunartDto> ew = new ServiceHandler<icreateOrUpdatePrunartDto, ocreateOrUpdatePrunartDto>(prunart);
            return ew.process(delegate(icreateOrUpdatePrunartDto input, ocreateOrUpdatePrunartDto rval, CredentialContext ctx)
            {

                if (input == null || input.prunart == null)
                    throw new ArgumentException("No valid input");

                rval.prunart = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).createOrUpdatePrunart(input.prunart);
            });
        }



        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="ipar"></param>
        /// <returns></returns>
        public ofourEyesDto fourEyesPrinciple(ifourEyesDto ipar)
        {
            ServiceHandler<ifourEyesDto, ofourEyesDto> ew = new ServiceHandler<ifourEyesDto, ofourEyesDto>(ipar);
            return ew.process(delegate(ifourEyesDto input, ofourEyesDto rval, CredentialContext ctx)
            {
                input.syswfuser = ctx.getMembershipInfo().sysWFUSER;

                BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).fourEyesPrinciple(input, rval);
            });
        }
    }
}
