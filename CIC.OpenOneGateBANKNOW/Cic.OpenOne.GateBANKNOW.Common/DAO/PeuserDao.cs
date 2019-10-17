using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// PE USER DAO
    /// </summary>
    public class PeuserDao : IPeuserDao
    {
        /// <summary>
        /// Auflisten der verfügbaren Bennutzer
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public DropListDto[] listAvailableUser(long sysperole)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                long haendler = PeRoleUtil.FindRootPEROLEByRoleType(context, sysperole, (int)RoleTypeTyp.HAENDLER);
                // HR 22.08.2011: Von NULL auf 0 gesetzt, da long niemals NULL sein kann.
                if (haendler != 0)
                {
                    List<DropListDto> users = new List<DropListDto>();
                    List<PEROLE> rollen = new List<PEROLE>();

                    var query = from baum in context.PEROLE
                                where baum.SYSPARENT == haendler
                                select baum;
                    if (query != null)
                    {
                        foreach (PEROLE rolle in query)
                        {
                            
                            if (rolle.ROLETYPE == null)
                                context.Entry(rolle).Reference(f => f.ROLETYPE).Load();
                            if (rolle.ROLETYPE != null)
                            {
                                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                                if ( (rolle.VALIDFROM < aktuell || rolle.VALIDUNTIL == null) && (rolle.VALIDUNTIL > aktuell || rolle.VALIDUNTIL == null) && rolle.ROLETYPE.TYP == (int)RoleTypeTyp.VERKAEUFER)
                                {
                                    users.Add(new DropListDto
                                    {
                                        sysID = (long)rolle.SYSPERSON,
                                        code = rolle.NAME,
                                        bezeichnung = rolle.NAME
                                    });
                                }
                            }
                        }
                    }
                    return users.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Auflisten der verfügbaren Bennutzer
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DropListDto[] listAvailableUser(long sysperole, PeroleActiveStatus status)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                long haendler = PeRoleUtil.FindRootPEROLEByRoleType(context , sysperole, (int)RoleTypeTyp.HAENDLER);
                // HR 22.08.2011: Von NULL auf 0 gesetzt, da long niemals NULL sein kann.
                bool filter = true;
                if (haendler != 0)
                {
                    List<DropListDto> users = new List<DropListDto>();
                    List<PEROLE> rollen = new List<PEROLE>();

                    var query = from baum in context.PEROLE
                                where baum.SYSPARENT == haendler
                                select baum;
                    if (query != null)
                    {
                        foreach (PEROLE rolle in query)
                        {
                            
                            if (rolle.ROLETYPE == null)
                                context.Entry(rolle).Reference(f => f.ROLETYPE).Load();
                            if (rolle.ROLETYPE != null)
                            {
                                if (status == PeroleActiveStatus.ACTIVE && rolle.INACTIVEFLAG == 1) filter = false;
                                if (status == PeroleActiveStatus.ACTIVE && rolle.INACTIVEFLAG == 0) filter = true;
                                if (status == PeroleActiveStatus.INACTIVE && rolle.INACTIVEFLAG == 1) filter = true;
                                if (status == PeroleActiveStatus.INACTIVE && rolle.INACTIVEFLAG == 0) filter = false;

                                DateTime aktuell = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                                if ((rolle.VALIDFROM < aktuell || rolle.VALIDUNTIL == null) && (rolle.VALIDUNTIL > aktuell || rolle.VALIDUNTIL == null) && rolle.ROLETYPE.TYP == (int)RoleTypeTyp.VERKAEUFER && filter)
                                {
                                   
                                    users.Add(new DropListDto
                                    {
                                        sysID = (long)rolle.SYSPERSON,
                                        code = rolle.NAME,
                                        bezeichnung = rolle.NAME
                                    });
                                }
                            }
                        }
                    }
                    return users.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
