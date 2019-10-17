using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IEaihotBo
    {
        /// <summary>
        /// Execute the eaihot for the given id
        /// must be of code CALL_BOS
        /// </summary>
        /// <param name="sysEaiHOT"></param>
        void execEAIHOT(int sysEaiHOT);

        
        /// <summary>
        /// Creates an eaihot for the generic bos call Eventengine feature
        /// </summary>
        /// <param name="code"></param>
        /// <param name="gebiet"></param>
        /// <param name="gebietId"></param>
        /// <param name="syswfuser"></param>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="input3"></param>
        void createEAIBosCall(String code, String gebiet, long gebietId, long syswfuser, String input1, String input2, String input3);
    }
}
