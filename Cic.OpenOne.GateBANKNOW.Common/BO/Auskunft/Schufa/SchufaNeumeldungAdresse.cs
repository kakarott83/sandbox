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

    public class SchufaNeumeldungAdresse : AbstractSchufaMeldungBo
    {
        public SchufaNeumeldungAdresse(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao) 
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaNeumeldungAdresse)
        {
        }
        
        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.neumeldungAdresse = Mapper.Map(inDto.Meldung.Data, new NeumeldungAdresseType());
        }
    }
}
