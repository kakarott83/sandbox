using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using System;
using System.Linq;

namespace Cic.OpenLease.Service
{
    public class PlausibilityCheck
    {
        /// <summary>
        /// Validates availability of Kasko-Ensurance for new/vorführ/fuhrpark
        /// either kasko is checked of fremdversicherung-field is filled
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="fremdkasko"></param>
        /// <returns></returns>
        public static ValidationResultDto checkValidKaskoPlausibility(long sysangebot, String fremdkasko)
        {
            ValidationResultDto rval = new ValidationResultDto();
            rval.validationId = ValidationStatus.KASKONEEDED;
            rval.valid = true;
            rval.hasMessage = true;
            rval.Message = "Für die gewählte Fahrzeugart ist der Abschluss einer Kaskoversicherung verpflichtend. Wählen Sie die Kasko aus oder tragen Sie eine Fremdversicherung ein.";

            using (DdOlExtended Context = new DdOlExtended())
            {
                long isnoservice = Context.ExecuteStoreQuery<long>("select count(*) isnoservice from prproduct, angebot, vart where vart.sysvart = prproduct.sysvart and angebot.sysprproduct=prproduct.sysprproduct and angebot.sysid="+ sysangebot+" and vart.bezeichnung in ('KREDIT','LEASING')", null).FirstOrDefault();
                if (isnoservice > 0)//just for leasing and kredit products, not for service products
                {
                    long obarttyp = Context.ExecuteStoreQuery<long>("select obart.typ from obart,angob,angkalk where angkalk.sysob=angob.sysob and obart.sysobart=angob.sysobart and angkalk.sysangebot=" + sysangebot, null).FirstOrDefault();
                    if (obarttyp == 0 || obarttyp == 2)//needs kasko
                    {
                        long vkflag = Context.ExecuteStoreQuery<long>("select vkflag from angkalkfs, angkalk where syskalk=sysangkalkfs and sysangebot=" + sysangebot, null).FirstOrDefault();
                        if (vkflag == 0)//no aida-kasko
                        {
                            String fremdversicherung = Context.ExecuteStoreQuery<String>("select FREMDVERSICHERUNG from angvs,vstyp where vstyp.sysvstyp=angvs.sysvstyp and CODEMETHOD='KASKO' and sysangebot=" + sysangebot, null).FirstOrDefault();
                            if (fremdversicherung == null || fremdversicherung.Trim().Length == 0)
                            {
                                if (fremdkasko != null && fremdkasko.Trim().Length > 0)
                                    rval.valid=true;
                                else
                                    rval.valid = false;
                            }
                        }
                    }
                }
            }
            return rval;
        }
    }
}