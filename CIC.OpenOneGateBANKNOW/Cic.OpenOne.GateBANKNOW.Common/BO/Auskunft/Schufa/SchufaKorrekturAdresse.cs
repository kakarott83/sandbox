namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    using AutoMapper;

    using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
    using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

    using DTO.Auskunft.Schufa;

    public class SchufaKorrekturAdresse : AbstractSchufaMeldungBo
    {
        public SchufaKorrekturAdresse(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao)
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaKorrekturAdresse)
        {
        }
        
        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.korrekturAdresse = Mapper.Map(inDto.Meldung.Data, new KorrekturAdresseType());
        }
    }
}