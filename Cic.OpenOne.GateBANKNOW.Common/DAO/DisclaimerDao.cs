using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class DisclaimerDao
    {
        public void createDisclaimer(String area, DisclaimerType dt, long sysid, long syswfuser, string inhalt)
        {
            using (DdOwExtended context = new DdOwExtended())
            {


                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == area
                                 select t).FirstOrDefault();

                long typ = 23;//TODO, other DisclaimerTypes have different types


                WFMMKAT kat = (from k in context.WFMMKAT
                               where k.TYP == typ
                               select k).FirstOrDefault();
                String kurzbezsuffix = "";
                if (dt == DisclaimerType.KUNDENANLAGE)
                    kurzbezsuffix = " Kundenanlage";

                WFMMEMO disclaimerText = new WFMMEMO();
                disclaimerText.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                disclaimerText.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                disclaimerText.CREATEUSER = syswfuser;
                disclaimerText.SYSLEASE = sysid;
                disclaimerText.SYSWFMTABLE = table.SYSWFTABLE;
                disclaimerText.SYSWFMMKAT = kat.SYSWFMMKAT;

                if (dt == DisclaimerType.KUNDENANLAGE)
                {
                    kurzbezsuffix = " Kundenanlage";
                    if ("ANGEBOT".Equals(area))
                    {
                        String infoText = context.ExecuteStoreQuery<String>("select name ||' '||vorname from it,angebot where it.sysit=angebot.sysit and angebot.sysid=" + sysid).FirstOrDefault();
                        if (infoText == null) infoText = "";
                        if (infoText.Length > 40)
                            infoText.Substring(0, 40);
                        disclaimerText.STR01 = infoText;
                    }
                }
                disclaimerText.KURZBESCHREIBUNG = kat.BESCHREIBUNG + kurzbezsuffix;

                context.WFMMEMO.Add(disclaimerText);

                //add to WFMMEMOEXT with whole text
                context.SaveChanges();
                WFMMEMOEXT discExt = new WFMMEMOEXT();
                discExt.SYSWFMMEMO = disclaimerText.SYSWFMMEMO;
                discExt.INHALT = inhalt;
                discExt.RANK = 1;
                context.WFMMEMOEXT.Add(discExt);



                context.SaveChanges();

            }
        }
    }
}
