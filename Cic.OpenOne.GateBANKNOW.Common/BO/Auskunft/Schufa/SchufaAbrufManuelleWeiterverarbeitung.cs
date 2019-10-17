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
    public class SchufaAbrufManuelleWeiterverarbeitung : AbstractSchufaBo
    {
        public SchufaAbrufManuelleWeiterverarbeitung(ISchufaWSDao schufaWsDao, ISchufaDBDao schufaDbDao, IAuskunftDao auskunftDao)
            : base(schufaWsDao, schufaDbDao, auskunftDao, AuskunfttypDao.SchufaAbrufManuelleWeiterverarbeitung)
        {
        }

        protected override SchufaTAktionsHeader GetAktionsHeader(SchufaInDto inDto)
        {
            return inDto.AbrufManuelleWeiterverarbeitung.Data.AktionsHeader;
        }

        protected override void FillRequest(long sysAuskunft, SchufaInDto inDto, AktionType aktion)
        {
            aktion.abrufManuelleWeiterverarbeitung = Mapper.Map(inDto.AbrufManuelleWeiterverarbeitung.Data, new AbrufManuelleWeiterverarbeitungType());
        }

        protected override void MapToOutput(long sysAuskunft, SchufaOutDto schufaOutDto, ReaktionType reaktion)
        {
            var result = new SchufaAnfrageBonitaetsauskunftOutDto();

            Mapper.Map(reaktion, result);
            Mapper.Map(reaktion.bonitaetsauskunft, result);

            schufaOutDto.AnfrageBonitaetsauskunft = result;
        }
    }
}
