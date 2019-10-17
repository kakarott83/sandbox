using Cic.OpenOne.AuskunftManagement.Common.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    public class AuskunftBoFactoryFactory 
    {
        public static Cic.OpenOne.AuskunftManagement.Common.BO.SF.ICommonAuskunftBo getFactory(String auskunftTypBezeichnung)
        {
            Cic.OpenOne.AuskunftManagement.Common.BO.SF.ICommonAuskunftBo rval = null;
            rval = AuskunftBoFactory.getServiceFacade(auskunftTypBezeichnung);
            if (rval == null)
            {
                IAuskunftBoFactory  factory = (IAuskunftBoFactory)getInstance("Cic.OpenOne.AuskunftManagement.Common.BO.AuskunftBoFactory");
                rval = factory.getServiceFacadeObject(auskunftTypBezeichnung);
            }
            return rval;
        }

        public static String GetAssemblyNameContainingType(String typeName)
        {
            foreach (var currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (currentassembly.FullName.ToUpper().StartsWith("CIC"))
                {
                    var t = currentassembly.GetType(typeName, false, true);
                    if (t != null)
                    {
                        return currentassembly.FullName;
                    }
                }
            }

            return "not found";
        }

        public static Object getInstance(String classname)
        {
            var asm = GetAssemblyNameContainingType(classname);
            if ("not found".Equals(asm))
            {
                return null;
            }
            //Type t = Type.GetType(searchfactory+", "+asm);
            //Object test1 = t.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var oh = Activator.CreateInstance(asm, classname);
            return oh.Unwrap();
        }
    }
}
