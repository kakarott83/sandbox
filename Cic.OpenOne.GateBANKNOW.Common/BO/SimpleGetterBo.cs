using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Business object that gets more information from the actual user
    /// </summary>
    public class SimpleGetterBo : AbstractSimpleGetterBo
    {
        /// <summary>
        /// contructs a simpleGetter business object
        /// </summary>
        /// <param name="simpleGetterDao">the data access object use</param>
        public SimpleGetterBo(ISimpleGetterDao simpleGetterDao)
            : base(simpleGetterDao)
        {
        }

        /// <summary>
        /// get profil of the signed in user
        /// </summary>
        /// <returns>ogetProfilDto</returns>
        /// <remarks>informationUeber und mobil sind noch nicht im Konzept enthalten (1.4)
        /// mobil und informationUeber ist im Mapping nicht angegeben</remarks>
        public override ProfilDto getProfil(long sysVpPerole)
        {
            PERSON person;
            PUSER puser;
            ProfilDto ProfilDto;

            person = simpleGetterDao.findPersonBySysperole(sysVpPerole);
            if (person == null)
            {
                throw new Exception("Person not found:" + sysVpPerole);
            }
            puser = simpleGetterDao.getPuser(person);

            ProfilDto = new ProfilDto()
            {
                name = person.NAME,
                vorname = person.VORNAME,
                strasse = person.STRASSE,
                hausnummer = person.HSNR,
                plz = person.PLZ,
                ort = person.ORT,
                telefon = person.TELEFON,
                telefax = person.FAX,
                eMail = person.EMAIL,
                internet = person.URL,
                informationUeber = simpleGetterDao.getinformationUeberBySysPerson(person.SYSPERSON),
                benutzerId = "",
                personenId = person.SYSPERSON.ToString(),
                mobil = person.HANDY,
            };
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new DdOlExtended())
            {
                ProfilDto.extuserid = olCtx.ExecuteStoreQuery<String>("select str03 from peoption where sysid=" + person.SYSPERSON).FirstOrDefault();
                ProfilDto.extdealerid = olCtx.ExecuteStoreQuery<String>("select peoption.str03 from perole vkp, perole,peoption where vkp.sysparent=perole.sysperole and peoption.SYSID=perole.sysperson and vkp.sysperole=" + sysVpPerole).FirstOrDefault();
                ProfilDto.extvscode = olCtx.ExecuteStoreQuery<String>("select vstyp.code from RELX, perole, vstyp where coderelxtype = 'PEROLE2EXT_VSTYP'  and relx.sysparent = perole.sysperole and vstyp.sysvstyp = relx.syschild and perole.sysperole=" + sysVpPerole).FirstOrDefault();
            }

            if (puser != null)
            {
                ProfilDto.benutzerId = puser.EXTERNEID;
            }

            return ProfilDto;
        }

        /// <summary>
        /// get key account manager of the current user
        /// </summary>
        /// <returns>ogetKamDto</returns>
        public override KamDto getKam(long sysPerole)
        {
            PERSON person;
            KamDto KamDto;

            person = simpleGetterDao.findKamPersonBySysperole(sysPerole);
            if (person == null)
            {
                throw new Exception("Person not found:" + sysPerole);
            }
            KamDto = new KamDto()
            {
                name = person.NAME,
                vorname = person.VORNAME,
                telefon = person.TELEFON
            };
            return KamDto;
        }

        /// <summary>
        /// get abwicklungsort of the current user
        /// </summary>
        /// <returns>ogetAbwicklungsortDto</returns>
        public override AbwicklungsortDto getAbwicklungsort(long sysPerole)
        {
            PERSON person;
            AbwicklungsortDto AbwicklungsortDto;

            person = simpleGetterDao.findAbwicklungsortPersonBySysperole(sysPerole);
            if (person == null)
            {
                throw new Exception("Person not found:" + sysPerole);
            }
            AbwicklungsortDto = new AbwicklungsortDto()
            {
                hausnummer = person.HSNR,
                strasse = person.STRASSE,
                plz = person.PLZ,
                ort = person.ORT,
                telefon = person.TELEFON
            };
            return AbwicklungsortDto;
        }

    }
}