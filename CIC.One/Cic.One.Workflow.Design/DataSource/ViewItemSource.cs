using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.Web.BO;
using Cic.One.DTO;

namespace Cic.One.Workflow.Design.DataSource
{
    /// <summary>
    /// Class for accessing database-based items for the WF4-Designer during design-time
    /// Uses the app.config db-config when started from WorkflowEditor,
    /// Uses XY-config TODO, how the xxx is vs determining the used project config file??? db-config when started from visual studio.
    /// </summary>
    public class ViewItemSource
    {
        public IEnumerable<string> VorgangItems() 
        {
            IWorkflowBo bo = BOFactoryFactory.getInstance().getWorkflowBo();
            List<WfvEntry> entries = bo.getVlmConfig("OL");
            List<String> ids = entries.Select(a => a.syscode).Distinct().OrderBy(a => a).ToList();
            return ids;// return new string[] { "Test 1", "Test 2" };
        }
        public List<String> PrintItems(String docarea)
        {
            IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
            List<PrintDocumentDto> docs = bo.getDocumentList(docarea);
            List<String> ids = docs.Select(a => a.code).Distinct().OrderBy(a => a).ToList();
            return ids;
        }
        public IEnumerable<string> PrintAreaItems()
        {
            IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
            List<String> docs = bo.getDocumentAreas();
            docs.Insert(0, "");
            return docs;
        }
        
    }
}
