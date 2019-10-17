// OWNER MK, 04-08-2009
using Cic.OpenOne.Common.Util;
namespace Cic.OpenLease.Model.DdOl
{
    [System.CLSCompliant(true)]
    public static class StateHelper
    {
        #region Constants
        private const string CnstStrNew = "Neu";
        private const string CnstStrPrinted = "Gedruckt";
        private const string CnstStrHandedIn = "Eingereicht";
        private const string CnstStrRejected = "Abgelehnt";
        private const string CnstStrExpired = "Abgelaufen";

        private const string CnstStrApproved = "Genehmigt";
        private const string CnstStrBeingChecked = "In Prüfung";
        private const string CnstStrApprovedWithRequirement = "Genehmigt mit Auflagen";
        private const string CnstStrFallenThrough = "Nicht zu Stande gekommen";
        private const string CnstStrLedOver = "Übergeleitet";
        private const string CnstStrResubmitted = "Erneut eingereicht";
        #endregion

        #region Methods
        // NOTE MK, Q&D
        public static string DeliverAngebotState(Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses angebotState)
        {
            switch (angebotState)
            {
                case Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.New:
                    return CnstStrNew;
                case Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Printed:
                    return CnstStrPrinted;
                case Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.HandedIn:
                    return CnstStrHandedIn;
                case Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Rejected:
                    return CnstStrRejected;
                case Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Expired:
                    return CnstStrExpired;
            }

            return string.Empty;
        }

        // NOTE MK, Q&D
        public static Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses? DeliverAngebotState(string angebotState)
        {           
            string ANGEBOTState;
            string StrNew;
            string StrPrinted;
            string StrHandedIn;
            string StrRejected;
            string StrExpired;

            if (!StringUtil.IsTrimedNullOrEmpty(angebotState))
            {
                // NOTE JJ, Not key sensitive
                ANGEBOTState = angebotState.ToUpper();                
                StrNew = CnstStrNew.ToUpper();
                StrPrinted = CnstStrPrinted.ToUpper();
                StrHandedIn = CnstStrHandedIn.ToUpper();
                StrRejected = CnstStrRejected.ToUpper();
                StrExpired = CnstStrExpired.ToUpper();

                if (ANGEBOTState == StrNew)
                {
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.New;
                }
                else if (ANGEBOTState == StrPrinted)
                {
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Printed;
                }
                else if (ANGEBOTState == StrHandedIn)
                {
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.HandedIn;
                }
                else if (ANGEBOTState == StrRejected)
                {
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Rejected;
                }
                else if (ANGEBOTState == StrExpired)
                {
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Expired;
                }
            }

            // Unknown
            return null;
        }

        // NOTE MK, Q&D
        public static string DeliverAntragState(Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses antragState)
        {
            switch (antragState)
            {
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.New:
                    return CnstStrNew;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.BeingChecked:
                    return CnstStrBeingChecked;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Rejected:
                    return CnstStrRejected;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Approved:
                    return CnstStrApproved;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.ApprovedWithRequirement:
                    return CnstStrApprovedWithRequirement;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.FallenThrough:
                    return CnstStrFallenThrough;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.LedOver:
                    return CnstStrLedOver;
                case Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Resubmitted:
                    return CnstStrResubmitted;
            }

            return string.Empty;
        }

        // NOTE MK, Q&D
        public static Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses? DeliverAntragState(string antragtState)
        {
            string ANTRAGState;
            string StrNew;
            string StrBeingChecked;
            string StrRejected;
            string StrApproved;
            string StrApprovedWithRequirement;
            string StrFallenThrough;
            string StrLedOver;
            string StrResubmitted;

            if (!StringUtil.IsTrimedNullOrEmpty(antragtState))
            {
                // NOTE JJ, Not key sensitive
                ANTRAGState = antragtState.ToUpper();
                StrNew = CnstStrNew.ToUpper();
                StrBeingChecked = CnstStrBeingChecked.ToUpper();
                StrRejected = CnstStrRejected.ToUpper();
                StrApproved = CnstStrApproved.ToUpper();
                StrApprovedWithRequirement = CnstStrApprovedWithRequirement.ToUpper();
                StrFallenThrough = CnstStrFallenThrough.ToUpper();
                StrLedOver = CnstStrLedOver.ToUpper();
                StrResubmitted = CnstStrResubmitted.ToUpper();

                if (ANTRAGState == StrNew)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.New;
                }
                else if (ANTRAGState == StrBeingChecked)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.BeingChecked;
                }
                else if (ANTRAGState == StrRejected)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Rejected;
                }
                else if (ANTRAGState == StrApproved)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Approved;
                }
                else if (ANTRAGState == StrApprovedWithRequirement)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.ApprovedWithRequirement;
                }
                else if (ANTRAGState == StrFallenThrough)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.FallenThrough;
                }
                else if (ANTRAGState == StrLedOver)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.LedOver;
                }
                else if (ANTRAGState == StrResubmitted)
                {
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Resubmitted;
                }
            }

            // Unknown
            return null;
        }
        #endregion
    }
}