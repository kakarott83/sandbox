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

    public class SchufaLoeschungTodesfall : AbstractSchufaMeldungBo
    {
        public SchufaLoeschungTodesfall(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao) 
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaLoeschungTodesfall)
        {
        }

        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.loeschungTodesfall = Mapper.Map(inDto.Meldung.Data, new MeldungTodesfallType());
        }
    }
}
