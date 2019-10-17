using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    using AutoMapper;

    using DAO.Auskunft;

    using DTO.Auskunft;
    using DTO.Auskunft.Schufa;

    using SchufaSiml2AuskunfteiWorkflow;
    
    using SF;

    public class SchufaKorrekturVerbraucherdaten : AbstractSchufaMeldungBo
    {
        public SchufaKorrekturVerbraucherdaten(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao) 
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaKorrekturVerbraucherdaten)
        {
        }
        
        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.korrekturVerbraucherdaten = Mapper.Map(inDto.Meldung.Data, new KorrekturVerbraucherType());
        }
    }
}
