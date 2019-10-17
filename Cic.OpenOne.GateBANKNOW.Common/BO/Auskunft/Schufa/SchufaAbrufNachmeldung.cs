using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type;
using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.Schufa
{
    public class SchufaAbrufNachmeldung : AbstractSchufaBo
    {
        public SchufaAbrufNachmeldung(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao) 
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaAbrufNachmeldung)
        {
        }

        protected override SchufaTAktionsHeader GetAktionsHeader(SchufaInDto inDto)
        {
            return inDto.AbrufNachmeldung.Data.AktionsHeader;
        }

        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.abrufNachmeldung = Mapper.Map(inDto.AbrufNachmeldung.Data, new AbrufNachmeldungType());
        }

        protected override void MapToOutput(long sysAuskunft, SchufaOutDto schufaOutDto, ReaktionType reaktion)
        {
            var result = new SchufaAbrufNachmeldungOutDto();

            Mapper.Map(reaktion, result);
            Mapper.Map(reaktion.nachmeldung, result);
            
            schufaOutDto.AbrufNachmeldung = result;
        }
    }
}
