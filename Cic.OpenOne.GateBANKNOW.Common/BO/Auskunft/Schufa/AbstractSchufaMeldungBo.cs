using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    using AutoMapper;
    using DAO.Auskunft;
    using DTO.Auskunft.Schufa;
    using SchufaSiml2AuskunfteiWorkflow;

    public abstract class AbstractSchufaMeldungBo : AbstractSchufaBo
    {
        protected AbstractSchufaMeldungBo(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao, string auskunfttyp)
            : base(schufaWsDao, schufaDbDao, auskunftDao, auskunfttyp)
        {
        }

        protected override SchufaTAktionsHeader GetAktionsHeader(SchufaInDto inDto)
        {
            return inDto.Meldung.Data.AktionsHeader;
        }

        protected override void MapToOutput(long sysAuskunft, SchufaOutDto schufaOutDto, ReaktionType reaktion)
        {
            var result = new SchufaMeldungOutDto();
            Mapper.Map(reaktion, result);
            Mapper.Map(reaktion.meldungseingangsbestaetigung, result);
            schufaOutDto.Meldung = result;
        }
    }
}