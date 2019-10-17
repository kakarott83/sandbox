namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using BO.Auskunft.CRIF;

    using CIC.Database.CRIF.EF6.Model;

    using CrifSoapService;
    using DTO.Auskunft.Crif;

    using OpenOne.Common.DAO;
    using OpenOne.Common.Util.Logging;
    using Cic.OpenOne.Common.Util.Config;

    public interface ICrifDBDao
    {
        CrifInDto FindBySysId(long sysAuskunft, string auskunfttyp);
        void SaveInput(long sysAuskunft, CrifInDto inDto, string auskunfttyp);
        void SaveOutput(long sysAuskunft, CrifOutDto outDto, string auskunfttyp);
        void FillHeader(long sysCfHeader, TypeBaseRequest request, long sysAuskunft, string cfgKey = "CRIF_AUTO");
    }

    public class CrifDBDao : ICrifDBDao
    {
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ContextFactory contextFactory = new ContextFactory();

        public CrifInDto FindBySysId(long sysAuskunft, string auskunfttyp)
        {
            CrifInDto crifInDto = new CrifInDto();
            try
            {
                using (var context = contextFactory.Create<CRIFContext>())
                {
                    if (auskunfttyp == AuskunfttypDao.CrifIdentifyAddress)
                    {
                        var input = context.CFINPIDENTADDR.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.IdentifyAddress = Mapper.Map<CFINPIDENTADDR, CrifIdentifyAddressInDto>(input);
                    }
                    //###
                    else if (auskunfttyp == AuskunfttypDao.CrifGetArchivedReport)
                    {
                        var input = context.CFINPARCHDREP.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.GetArchivedReport = Mapper.Map<CFINPARCHDREP, CrifGetArchivedReportInDto>(input);
                    }
                    else if (auskunfttyp == AuskunfttypDao.CrifGetListOfReadyOfflineReports)
                    {
                        var input = context.CFINPLISTOFFRE.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.GetListOfReadyOfflineReports = Mapper.Map<CFINPLISTOFFRE, CrifGetListOfReadyOfflineReportsInDto>(input);
                    }
                    else if (auskunfttyp == AuskunfttypDao.CrifPollOfflineReport)
                    {
                        var input = context.CFINPPOLLOFF.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.PollOfflineReport = Mapper.Map<CFINPPOLLOFF, CrifPollOfflineReportInDto>(input);
                    }
                    else if (auskunfttyp == AuskunfttypDao.CrifGetOfflineReport)
                    {
                        var input = context.CFINPORDEROFF.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.OrderOfflineReport = Mapper.Map<CFINPORDEROFF, CrifOrderOfflineReportInDto>(input);
                    }
                    else if (auskunfttyp == AuskunfttypDao.CrifGetDebtDetail)
                    {
                        var input = context.CFINPDEBTDET.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.GetDebtDetails = Mapper.Map<CFINPDEBTDET, CrifGetDebtDetailsInDto>(input);
                    }
                    else if (auskunfttyp == AuskunfttypDao.CrifGetReport || auskunfttyp == AuskunfttypDao.CrifGetReportArb || auskunfttyp == AuskunfttypDao.CrifKontrollinhaber)
                    {
                        var input = context.CFINPGETREPORT.Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);
                        crifInDto.GetReport = Mapper.Map<CFINPGETREPORT, CrifGetReportInDto>(input);
                    }
                }
                return crifInDto;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von Crif Input Daten in FindBySysId. Error Message. ", ex);
                throw ex;
            }
        }

        public void SaveInput(long sysAuskunft, CrifInDto inDto, string auskunftTyp)
        {
            try
            {
                if (auskunftTyp == AuskunfttypDao.CrifIdentifyAddress)
                {
                    StoreInput(sysAuskunft, inDto.IdentifyAddress, new CFINPIDENTADDR());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetArchivedReport)
                {
                    StoreInput(sysAuskunft, inDto.GetArchivedReport, new CFINPARCHDREP());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetListOfReadyOfflineReports)
                {
                    StoreInput(sysAuskunft, inDto.GetListOfReadyOfflineReports, new CFINPLISTOFFRE());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifPollOfflineReport)
                {
                    StoreInput(sysAuskunft, inDto.PollOfflineReport, new CFINPPOLLOFF());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetOfflineReport)
                {
                    StoreInput(sysAuskunft, inDto.OrderOfflineReport, new CFINPORDEROFF());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetDebtDetail)
                {
                    StoreInput(sysAuskunft, inDto.GetDebtDetails, new CFINPDEBTDET());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetReport || auskunftTyp == AuskunfttypDao.CrifGetReportArb || auskunftTyp == AuskunfttypDao.CrifKontrollinhaber)
                {
                    StoreInput(sysAuskunft, inDto.GetReport, new CFINPGETREPORT());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler beim Speichern von Crif Input (CrifDBDao.{0}) : ", auskunftTyp), ex);
                throw ex;
            }
        }

        public void SaveOutput(long sysAuskunft, CrifOutDto outDto, string auskunftTyp)
        {
            try
            {
                if (auskunftTyp == AuskunfttypDao.CrifIdentifyAddress)
                {
                    Store(sysAuskunft, outDto, outDto.IdentifyAddress, new CFOUTIDENTADDR());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetArchivedReport)
                {
                    Store(sysAuskunft, outDto, outDto.GetArchivedReport, new CFOUTARCHREP());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetListOfReadyOfflineReports)
                {
                    Store(sysAuskunft, outDto, outDto.GetListOfReadyOfflineReports, new CFOUTLISTOFFRE());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifPollOfflineReport)
                {
                    Store(sysAuskunft, outDto, outDto.PollOfflineReport, new CFOUTPOLLOFF());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetOfflineReport)
                {
                    Store(sysAuskunft, outDto, outDto.OrderOfflineReport, new CFOUTORDEROFF());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetDebtDetail)
                {
                    Store(sysAuskunft, outDto, outDto.GetDebtDetails, new CFOUTDEBTDET());
                }
                else if (auskunftTyp == AuskunfttypDao.CrifGetReport || auskunftTyp == AuskunfttypDao.CrifGetReportArb || auskunftTyp == AuskunfttypDao.CrifKontrollinhaber)
                {
                    Store(sysAuskunft, outDto, outDto.GetReport, new CFOUTGETREPORT());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler beim Speichern von Crif Output (CrifDBDao.{0}) : ", auskunftTyp), ex);
                throw ex;
            }
        }
        
        private void StoreInput<TElement, TEntity>(long sysAuskunft, TElement element, TEntity entity)
        {
            using (OpenOne.Common.Model.CRIF.CRIFExtended context = new OpenOne.Common.Model.CRIF.CRIFExtended())
            {
                var cfout = Mapper.Map(element, entity);
                ((dynamic)entity).SYSAUSKUNFT = sysAuskunft;
                context.getObjectContext().AddObject(typeof(TEntity).Name, entity);
                
                context.SaveChanges();
            }
        }

        private void Store<TElement, TEntity>(long sysAuskunft, CrifOutDto outDto, TElement element, TEntity entity)
        {
            using (OpenOne.Common.Model.CRIF.CRIFExtended context = new OpenOne.Common.Model.CRIF.CRIFExtended())
            {
                if (outDto.Error == null)
                {
                    if (element == null)
                    {
                        throw new ArgumentException("The out element can't be null.", "element");
                    }

                    var cfout = Mapper.Map(element, entity);
                    ((dynamic) entity).SYSAUSKUNFT = sysAuskunft;
                    context.getObjectContext().AddObject(typeof(TEntity).Name, entity);
                }
                else
                {
                    var error = Mapper.Map(outDto.Error, new CFERROR());
                    error.SYSAUSKUNFT = sysAuskunft;
                    context.CFERROR.Add(error);
                }

                context.SaveChanges();

                if (outDto.Error == null)
                {
                    context.Detach(entity);
                    AuskunftModelCrifProfile.ReportPropertyChanged(entity, "SYS" + typeof(TEntity).Name);
                }
            }
        }
        
        public void FillHeader(long sysCfHeader, TypeBaseRequest request, long sysAuskunft, string cfgKey = "CRIF_AUTO")
        {
            using (var context = contextFactory.Create<CRIFContext>())
            {
                if (sysCfHeader != 0)
                {
                    var header = context.CFHEADER.FirstOrDefault(a => a.SYSCFHEADER == sysCfHeader) ?? new CFHEADER();
                    Mapper.Map(header, request);
                }

                if (request.identityDescriptor == null)
                {
                    request.identityDescriptor = new IdentityDescriptor();
                }

                String minor = AppConfig.Instance.GetEntry("CRIF", "OVERRIDEMINORVERSION", "13", "ANTRAGSASSISTENT");
                String major = AppConfig.Instance.GetEntry("CRIF", "OVERRIDEMAJORVERSION", "1", "ANTRAGSASSISTENT");
                
                if (request.control == null || request.control.majorVersion == 0)
                    request.control = new Control()
                    {
                        majorVersion = int.Parse(major),
                        minorVersion = int.Parse(minor)
                    };

                if (string.IsNullOrEmpty(request.identityDescriptor.userName))
                {
                    try
                    {
                        AUSKUNFTCFG auskunftCfg = null;
                        if (sysAuskunft != 0)
                        {
                            auskunftCfg = context.AUSKUNFT.Include("AUSKUNFTTYP.AUSKUNFTCFG").Where(a => a.SYSAUSKUNFT == sysAuskunft).Select(a => a.AUSKUNFTTYP.AUSKUNFTCFG).FirstOrDefault();
                            if (auskunftCfg != null)
                            {
                                request.identityDescriptor.userName = auskunftCfg.USERNAME;
                                request.identityDescriptor.password = auskunftCfg.KEYVALUE;
                            }
                        }

                        if(auskunftCfg == null)
                        {
                            auskunftCfg = context.AUSKUNFTCFG.First(par => par.BEZEICHNUNG.ToUpper() == cfgKey);
                            request.identityDescriptor.userName = auskunftCfg.USERNAME;
                            request.identityDescriptor.password = auskunftCfg.KEYVALUE;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not load username and password for CRIF SysAuskunft: "+sysAuskunft, ex);
                        throw ex;
                    }
                }
            }
        }
    }
}