using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    using AutoMapper;

    using Common.DTO.Auskunft;

    using DAO.Auskunft;

    using DTO.Auskunft.Schufa;

    using SchufaSiml2AuskunfteiWorkflow;

    using SF;

    public class SchufaMeldungVertragsdaten : AbstractSchufaMeldungBo
    {
        public SchufaMeldungVertragsdaten(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao)
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaMeldungVertragsdaten)
        {
        }

        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.meldungVertragsdaten = Mapper.Map(inDto.Meldung.Data, new MeldungVertragsdatenType());
        }
    }
}
