using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// Factory for WorkflowServices
    /// there are two implementations - WF4 and BPE
    /// </summary>
    public class WorkflowServiceFactory
    {
        private static string LOCK = "LOCK";
        private static WorkflowServiceFactory _self;

        /// <summary>
        /// getInstance
        /// </summary>
        /// <returns></returns>
        public static WorkflowServiceFactory getInstance()
        {
            lock (LOCK)
            {
                if (_self == null)
                    _self = new WorkflowServiceFactory();
            }
            return _self;
        }

        /// <summary>
        /// Used for evaluations/CAS
        /// </summary>
        /// <returns></returns>
        public IWorkflowService getWorkflowService()
        {
            return new WorkflowService();
        }

        /// <summary>
        /// Used for evaluations/CAS
        /// </summary>
        /// <returns></returns>
        public IWorkflowService getWorkflowService(WorkflowType type)
        {
            if (type == WorkflowType.BPE)
                return new BPEWorkflowService();
            else
                return getWorkflowService();
        }

    }
}
