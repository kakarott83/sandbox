using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    interface IRISKEWBS1WSDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string CallSSS1Test();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inputData"></param>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        long CallSSS1CalculateWB(long sysAuskunft, string inputData, long sysWFUser);
    }
}
