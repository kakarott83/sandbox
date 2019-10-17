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

    public class SchufaNamensAenderung : AbstractSchufaMeldungBo
    {
        public SchufaNamensAenderung(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao) 
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaNamensAenderung)
        {
        }
        
        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.neumeldungNachname = Mapper.Map(inDto.Meldung.Data, new NeumeldungNachnameType());
        }
    }
}
