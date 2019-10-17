namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceModel;
    using CrifSoapService;
    using DAO.Auskunft;
    using DTO.Auskunft;
    using DTO.Auskunft.Crif;

    using OpenOne.Common.Util.Config;
    using OpenOne.Common.Util.Logging;

    using SF;

    public interface ICrifBo
    {
    }

    public abstract class AbstractCrifBo<TInput, TOutput> : AbstractAuskunftBo<CrifInDto, AuskunftDto>
        where TInput: TypeBaseRequest
        where TOutput: TypeBaseResponse
    {
        protected CrifOutDto outDto = new CrifOutDto();
        protected readonly ICrifWSDao crifWsDao;
        protected readonly ICrifDBDao crifDbDao;
        protected readonly IAuskunftDao auskunftDao;
        private readonly string auskunftTyp;

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public enum CrifCodes
        {
            CicTechnischerFehler = -2,
            CicServiceException = -1,

            OK = 0,
            Fault = 1,
        }

        public AbstractCrifBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao, string auskunftTyp)
        {
            this.crifWsDao = crifWsDao;
            this.crifDbDao = crifDbDao;
            this.auskunftDao = auskunftDao;
            this.auskunftTyp = auskunftTyp;
        }

        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            CrifCodes code = CrifCodes.CicTechnischerFehler;
            try
            {
                CrifInDto inDto = crifDbDao.FindBySysId(sysAuskunft, auskunftTyp);
                code = CrifCodes.CicServiceException;

                return doAuskunft(sysAuskunft, inDto);
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, (int)code);
                throw new ApplicationException(string.Format("Unexpected Exception in AbstractCrifBo.{0} (with sysAuskunft: {1}).", auskunftTyp, sysAuskunft), ex);
            }
        }
        
        public override AuskunftDto doAuskunft(CrifInDto inDto)
        {
            CrifCodes code = CrifCodes.CicTechnischerFehler;
            long sysAuskunft = auskunftDao.SaveAuskunft(auskunftTyp);
            try
            {
                crifDbDao.SaveInput(sysAuskunft, inDto, auskunftTyp);
                code = CrifCodes.CicServiceException;
                return doAuskunft(sysAuskunft, inDto);
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, (int)code);
                throw new ApplicationException("Unexpected Exception in AbstractCrifBo." + auskunftTyp + " :", e);
            }
        }

        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        protected AuskunftDto doAuskunft(long sysAuskunft, CrifInDto inDto)
        {
            CrifCodes code = CrifCodes.OK;
            try
            {
                var request = MapFromInput(sysAuskunft, inDto);
                crifDbDao.FillHeader(GetSysCfHeader(inDto), request, sysAuskunft);
                crifWsDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                var response = ExecuteRequest(sysAuskunft, request);

                CleanupResponse(response);

                MapToOutput(outDto, response);
            }
            catch (FaultException<Error> exception)
            {
                outDto.Error = new CrifTError()
                {
                    Code = exception.Detail.code,
                    MessageText = exception.Detail.messageText,
                    FaultCode = exception.Code.Name,
                    Message = exception.Message
                };
                code = CrifCodes.Fault;
            }
            catch (FaultException exception)
            {
                outDto.Error = new CrifTError()
                {
                    FaultCode = exception.Code.Name,
                    Message = exception.Message
                };
                code = CrifCodes.Fault;
            }

            // Save Output
            Stopwatch watch = Stopwatch.StartNew();
            crifDbDao.SaveOutput(sysAuskunft, outDto, auskunftTyp);
            Trace.WriteLine("Took: " + watch.ElapsedMilliseconds);
            auskunftDao.UpdateAuskunft(sysAuskunft, (int)code);
            
            AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
            auskunftDto.CrifInDto = inDto;
            auskunftDto.CrifOutDto = outDto;
            var soap = crifWsDao.getSoapXMLDto();
            if (soap != null)
            {
                auskunftDto.requestXML = soap.requestXML;
                auskunftDto.responseXML = soap.responseXML;
            }
            return auskunftDto;
        }

        private void CleanupResponse(TOutput response)
        {
            var paths = AppConfig.Instance.GetEntry("CRIF", this.auskunftTyp.ToUpper() + "_WITHOUT_ELEMENTS", "", "ANTRAGSASSISTENT");
            foreach (var path in paths.Split(';'))
            {
                Type currentType = response.GetType();
                object lastValue = response;
                object currentValue = response;
                PropertyInfo property = null;

                try
                {

                    foreach (string propertyName in path.Split('.'))
                    {
                        property = currentType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        lastValue = currentValue;
                        if (property == null)
                        {
                            lastValue = null;
                            break;
                        }

                        currentValue = property.GetValue(currentValue, null);
                        if (currentValue == null)
                        {
                            break;
                        }
                        currentType = property.PropertyType;
                    }

                    if (lastValue != null && property != null)
                    {
                        property.SetValue(lastValue, null);
                        _log.Info("Removed path from CRIF-Response: " + path);
                    }

                }
                catch (Exception)
                {
                    _log.Error("Could not remove path from CRIF-Response: " + path);
                }
            }
        }


        protected abstract TInput MapFromInput(long sysAuskunft, CrifInDto inDto);
        protected abstract TOutput ExecuteRequest(long sysAuskunft, TInput request);
        protected abstract void MapToOutput(CrifOutDto crifOutDto, TOutput response);
        protected abstract long GetSysCfHeader(CrifInDto inDto);
    }
}
