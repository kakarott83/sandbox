using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    using AutoMapper;

    using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF;
    using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

    using DTO.Auskunft.Schufa;
    using DTO.Auskunft.Schufa.Type;

    public abstract class AbstractSchufaBo : AbstractAuskunftBo<SchufaInDto, AuskunftDto>
    {
        protected SchufaOutDto outDto = new SchufaOutDto();

        protected readonly ISchufaWSDao schufaWSDao;
        protected readonly ISchufaDBDao schufaDBDao;
        protected readonly IAuskunftDao auskunftDao;
        protected readonly string auskunfttyp;

        private const string CFGCONFIG = "MO_PARAMETER";
        private const string CFGSECTION = "SCHUFA";

        public enum SchufaCodes
        {
            CicTechnischerFehler = -2,
            CicServiceException = -1,

            OK = 0,
            FachlichUndTechnischeFehler = 1,
            FachlicherFehler = 2,
            TechnischerFehler = 3,
            ElektronischeAuskunftNichtMöglich = 4,

            KeineNachrichtenVerfügbar = 10,
            ManuelleWeiterverarbeitung = 11,
            ManuelleWeiterverarbeitungNichtGewünscht = 12,
            ManuelleWeiterverarbeitungNochNichtErfolgt = 13,

            UnbekannterFehler = 20,
            SchufaConnectionError = 19
        }

        protected AbstractSchufaBo(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao, string auskunfttyp)
        {
            this.schufaWSDao = schufaWsDao;
            this.schufaDBDao = schufaDbDao;
            this.auskunftDao = auskunftDao;
            this.auskunfttyp = auskunfttyp;
        }

        public override AuskunftDto doAuskunft(SchufaInDto inDto)
        {
            return PrepareRequest(auskunfttyp, inDto, doAuskunft);
        }

        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return PrepareRequest(auskunfttyp, sysAuskunft, doAuskunft);
        }

        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        protected long doAuskunft(long sysAuskunft, SchufaInDto inDto)
        {
            AktionType aktionType = new AktionType();
            FillHeader(inDto, aktionType);
            FillRequest(sysAuskunft, inDto, aktionType);

            var request = new executeRequest()
            {
                schufaSiml2Request = new SchufaSiml2Request()
                {
                    data = new DataType()
                    {
                        aktion = new AktionType[]
                        {
                            aktionType
                        }
                    },
                    service = new ServiceType()
                    {
                        online = new OnlineServiceProtocolType()
                        {
                            request = ""
                        }
                    }
                }
            };

            return Execute(sysAuskunft, request);
        }

        private long Execute(long sysAuskunft, executeRequest request)
        {
            SchufaCodes code = SchufaCodes.OK;
            try
            {
                var result = schufaWSDao.execute(request);
                if (result.schufaSiml2Response.data == null)
                {
                    CreateError(result.schufaSiml2Response.service.error);

                    if (outDto.Error != null)
                    {
                        code = (SchufaCodes)this.outDto.Error.Code;
                    }

                    return (int)code;
                }

                var firstReaction = result.schufaSiml2Response.data.reaktion.FirstOrDefault();
                if (firstReaction != null)
                {
                    MapToOutput(sysAuskunft, outDto, firstReaction);
                    var fehler = firstReaction.fehlermeldung;
                    CreateError(fehler);

                    if (outDto.Error != null)
                    {
                        code = (SchufaCodes)this.outDto.Error.Code;
                    }
                    else if (firstReaction.keineNachrichtenVerfuegbar != null)
                    {
                        code = SchufaCodes.KeineNachrichtenVerfügbar;
                    }
                    else if (firstReaction.elektronischeAuskunftNichtMoeglich != null)
                    {
                        code = SchufaCodes.ElektronischeAuskunftNichtMöglich;
                    }
                    else if (firstReaction.manuelleWeiterverarbeitungNichtGewuenscht != null)
                    {
                        code = SchufaCodes.ManuelleWeiterverarbeitungNichtGewünscht;
                    }
                    else if (firstReaction.manuelleWeiterverarbeitungNochNichtErfolgt != null)
                    {
                        code = SchufaCodes.ManuelleWeiterverarbeitungNochNichtErfolgt;
                    }
                    else if (firstReaction.manuelleWeiterverarbeitung != null)
                    {
                        code = SchufaCodes.ManuelleWeiterverarbeitung;
                    }
                }
            }
            catch (FaultException ex)
            {
                CreateError(ex);
                if (outDto.Error != null)
                {
                    return outDto.Error.Code;
                }
            }
            return (int)code;
        }


        private void FillHeader(SchufaInDto inDto, AktionType aktionType)
        {
            var aktionsHeader = GetAktionsHeader(inDto);
            Mapper.Map(aktionsHeader, aktionType);
        }

        protected abstract SchufaTAktionsHeader GetAktionsHeader(SchufaInDto inDto);

        protected abstract void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion);

        protected abstract void MapToOutput(long sysAuskunft, SchufaOutDto schufaOutDto, ReaktionType reaktion);

        #region Helpers
        protected long CreateError(FaultException ex)
        {
            outDto.Error = Mapper.Map(ex, new SchufaTAusnahmeDto());
            outDto.Error.Code = (int)SchufaCodes.CicServiceException;
            return outDto.Error.Code;
        }

        private long CreateError(SimpleErrorResponseType ausnahme)
        {
            if (ausnahme == null)
            {
                return 0;
            }

            outDto.Error = new SchufaTAusnahmeDto()
            {
                Code = (int)SchufaCodes.SchufaConnectionError,
                Message = ausnahme.errorcode + ": " + ausnahme.errormessage
            };
            return outDto.Error.Code;
        }

        protected long CreateError(FehlermeldungType ausnahme)
        {
            if (ausnahme == null)
            {
                return 0;
            }

            outDto.Error = Mapper.Map(ausnahme, new SchufaTAusnahmeDto());
            var fachlicheFehler = ausnahme.fehlermeldung.Select(a => a.fachlicherFehler).Where(a => a != null);
            var technischeFehler = ausnahme.fehlermeldung.Select(a => a.technischerFehler).Where(a => a != null);
            SetErrorCode(fachlicheFehler, technischeFehler);
            return outDto.Error.Code;
        }

        private void SetErrorCode(IEnumerable<FachlicherFehlerType> FehlerlisteFachlich, IEnumerable<TechnischerFehlerType> FehlerlisteTechnisch)
        {
            if (outDto.Error == null || outDto.Error.Code != 0)
            {
                return;
            }

            if (outDto.Error.Code == 0)
            {
                bool hatFachlicheFehler = FehlerlisteFachlich != null && FehlerlisteFachlich.Any();
                bool hatTechnischeFehler = FehlerlisteTechnisch != null && FehlerlisteTechnisch.Any();
                if (hatFachlicheFehler && hatTechnischeFehler)
                {
                    outDto.Error.Code = (int)SchufaCodes.FachlichUndTechnischeFehler;
                }
                else if (hatFachlicheFehler)
                {
                    outDto.Error.Code = (int)SchufaCodes.FachlicherFehler;
                }
                else if (hatTechnischeFehler)
                {
                    outDto.Error.Code = (int)SchufaCodes.TechnischerFehler;
                }
            }
        }

        /// <summary>
        /// Prepariert den Request, falls nur die sysauskunft bekannt ist (lädt das Objekt aus der Datenbank)
        /// </summary>
        /// <param name="auskunfttyp">Typ der Auskunft</param>
        /// <param name="sysAuskunft">ID</param>
        /// <param name="action">Aktion, welche aufgerufen werden soll, sobald InDto und SysAuskunft bekannt sind</param>
        /// <returns>AuskunftDto</returns>
        private AuskunftDto PrepareRequest(string auskunfttyp, long sysAuskunft, Func<long, SchufaInDto, long> action)
        {
            SchufaCodes code = SchufaCodes.CicTechnischerFehler;

            // Get AuskunftDto
            AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                SchufaInDto inDto = schufaDBDao.FindBySysId(sysAuskunft, auskunfttyp);
                inDto.SysId = auskunftDto.sysid;

                code = SchufaCodes.CicServiceException;

                WrapRequest(auskunfttyp, sysAuskunft, () => action(sysAuskunft, inDto));

                auskunftDto.SchufaInDto = inDto;
                auskunftDto.SchufaOutDto = outDto;
                var soap = schufaWSDao.getSoapXMLDto();
                auskunftDto.requestXML = soap.requestXML;
                auskunftDto.responseXML = soap.responseXML;

                return auskunftDto;
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, (int)code);
                throw new ApplicationException(string.Format("Unexpected Exception in AbstractSchufaBo.{0} (with sysAuskunft: {1}).", auskunfttyp, sysAuskunft), ex);
            }
        }

        /// <summary>
        /// Prepariert den Request, falls nur das InDto bekannt ist (speichert das InDto in der Datenbank)
        /// </summary>
        /// <param name="auskunfttyp">Typ der Auskunft</param>
        /// <param name="inDto">InDto</param>
        /// <param name="action">Aktion, welche aufgerufen werden soll, sobald InDto und SysAuskunft bekannt sind</param>
        /// <returns>AuskunftDto</returns>
        private AuskunftDto PrepareRequest(string auskunfttyp, SchufaInDto inDto, Func<long, SchufaInDto, long> action)
        {
            SchufaCodes code = SchufaCodes.CicTechnischerFehler;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(auskunfttyp);
            try
            {
                schufaDBDao.SaveInput(sysAuskunft, inDto, auskunfttyp);

                code = SchufaCodes.CicServiceException;

                WrapRequest(auskunfttyp, sysAuskunft, () => action(sysAuskunft, inDto));

                AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftDto.SchufaInDto = inDto;
                auskunftDto.SchufaOutDto = outDto;
                var soap = schufaWSDao.getSoapXMLDto();
                auskunftDto.requestXML = soap.requestXML;
                auskunftDto.responseXML = soap.responseXML;

                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, (int)code);
                throw new ApplicationException("Unexpected Exception in AbstractSchufaBo." + auskunfttyp + " :", e);
            }
        }

        /// <summary>
        /// jetzt ist schon die sysauskunft und das InDto bekannt und das WSDao wird prepariert, WS-Aufruf getätigt, Output gespeichert
        /// und falls ein Fehler entstanden ist der Code an die Auskunft geupdatet.
        /// </summary>
        /// <param name="auskunfttyp">Typ der Auskunft</param>
        /// <param name="sysAuskunft">Id</param>
        /// <param name="action">Aktion, welche ausgeführt werden soll um das OutDto zu füllen.</param>
        public void WrapRequest(string auskunfttyp, long sysAuskunft, Func<long> action)
        {
            //For report
            schufaWSDao.setCredentials(schufaDBDao.GetCredentials());

            var code = action();

            // Save Output
            schufaDBDao.SaveOutput(sysAuskunft, outDto, auskunfttyp);

            auskunftDao.UpdateAuskunft(sysAuskunft, code);
        }

        #endregion
    }
}