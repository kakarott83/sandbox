using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.Resources;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
    /// <summary>
    /// Aggregates Zusatzinformations from Provision data
    /// </summary>
    public class KalkulationServicesBo
    {
        /// <summary>
        /// Aggregates Zusatzinformations from Provision data
        /// </summary>
        /// <param name="kalkulation"></param>
        /// <returns></returns>
        public List<Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzinformationDto> aggregateZusatzinformation(Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto kalkulation)
        {
            IProvisionDao pDao = PrismaDaoFactory.getInstance().getProvisionDao();
            
            List<CIC.Database.PRISMA.EF6.Model.PRPROVTYPE> ptypes = pDao.getProvisionTypes();
            bool isDiffLeasing = PrismaDaoFactory.getInstance().getPrismaDao().isDiffLeasing(kalkulation.angAntKalkDto.sysprproduct);

            Dictionary<long, String> pcodeDictionary = new Dictionary<long, string>();
            foreach (CIC.Database.PRISMA.EF6.Model.PRPROVTYPE pt in ptypes)
                pcodeDictionary[pt.SYSPRPROVTYPE] = pt.CODE.ToUpper();


            List<ZusatzinformationDto> zusatzinfos = new List<ZusatzinformationDto>();
            ZusatzinformationDto zi = new ZusatzinformationDto();
            zi.type = ZusatzinformationType.DEFAULT;
            zusatzinfos.Add(zi);
            assignCodes(zi, kalkulation.angAntProvDto, pcodeDictionary);
            zi = new ZusatzinformationDto();
            zi.type = ZusatzinformationType.RAPMAX;
            zusatzinfos.Add(zi);
            assignCodes(zi, kalkulation.angAntProvDtoRapMax, pcodeDictionary);
            zi = new ZusatzinformationDto();
            zi.type = ZusatzinformationType.RAPMIN;
            zusatzinfos.Add(zi);
            assignCodes(zi, kalkulation.angAntProvDtoRapMin, pcodeDictionary);
            // Subventionsgeber (Händler bei Differenzleasing)  --------------------------------------------------
            if (isDiffLeasing)
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    foreach (var angAntSubv in kalkulation.angAntSubvDto)
                    {
                        if (angAntSubv.syssubvg > 0)
                        {
                            angAntSubv.subvGBezeichnung = ctx.ExecuteStoreQuery<String>("select name from person where sysperson=" + angAntSubv.syssubvg, null).FirstOrDefault();
                        }
                    }
                }
            }
            return zusatzinfos;


        }

        /// <summary>
        /// Groups different Provision type codes to one class
        /// </summary>
        /// <param name="zi"></param>
        /// <param name="provisions"></param>
        /// <param name="pcodeDictionary"></param>
        private void assignCodes(ZusatzinformationDto zi, List<Cic.OpenOne.Common.DTO.AngAntProvDto> provisions, Dictionary<long, String> pcodeDictionary)
        {
            if (provisions == null) return;

            IRounding round = RoundingFactory.createRounding();
            
            foreach (Cic.OpenOne.Common.DTO.AngAntProvDto aap in provisions)
            {


                if (pcodeDictionary[aap.sysprprovtype].Contains(PrProvTypeCode.P_Code.ToString().ToUpper()))
                {
                    zi.provisionGrund += aap.provision;
                    zi.provisionTotal += aap.provision;
                }
                if (pcodeDictionary[aap.sysprprovtype].Equals(PrProvTypeCode.P_Code_Neu.ToString().ToUpper()))
                {
                    zi.provisionNeugeld += aap.provision;
                }
                else if (pcodeDictionary[aap.sysprprovtype].Equals(PrProvTypeCode.P_Code_Abl.ToString().ToUpper()))
                {
                    zi.provisionAbloesen += aap.provision;
                }
                else if (pcodeDictionary[aap.sysprprovtype].Equals(PrProvTypeCode.V_Code.ToString().ToUpper()))
                {
                    zi.provisionRsv += aap.provision;
                    zi.provisionTotal += aap.provision; //HR 29.6.2011: Vermutlich soll  hier das Total hochgerechnet werden zuvor wurde provisionRsv doppelt addiert
                }
                else if (pcodeDictionary[aap.sysprprovtype].Equals(PrProvTypeCode.S_Code.ToString().ToUpper()))
                {
                    zi.provisionZusatz += aap.provision;
                    zi.provisionTotal += aap.provision; //HR 29.6.2011: Vermutlich soll  hier das Total hochgerechnet werden zuvor wurde provisionRsv doppelt addiert
                }
            }
            zi.provisionTotal = round.RoundCHF(zi.provisionTotal);
            zi.provisionZusatz = round.RoundCHF(zi.provisionZusatz);
            zi.provisionRsv = round.RoundCHF(zi.provisionRsv);
            zi.provisionGrund = round.RoundCHF(zi.provisionGrund);

        }

           
    }
}