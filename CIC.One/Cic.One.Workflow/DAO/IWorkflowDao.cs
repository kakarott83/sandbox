using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Workflow.DAO
{
    public interface IWorkflowDao
    {
        /// <summary>
        /// returns a einrichtungs content string for the wfv syscode
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        String getWfvEinrichtung(String syscode);

        /// <summary>
        /// returns the syscodes befehlszeile
        /// </summary>
        /// <param name="syscode"></param>
        /// <returns></returns>
        String getBefehlszeile(String syscode);

        /// <summary>
        /// creates a eaihot for olstart from browser
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="code"></param>
        /// <returns>guid for link</returns>
        String execOlStart(String area, long sysid, long syswfuser, String code);
    }
}
