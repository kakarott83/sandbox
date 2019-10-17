// OWNER MK, 04-08-2009
namespace Cic.OpenLease.Service
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

        private const string CnstStrApproved = "Aenehmigt";
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
            switch (angebotState)
            {
                case CnstStrNew:
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.New;
                case CnstStrPrinted:
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Printed;
                case CnstStrHandedIn:
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.HandedIn;
                case CnstStrRejected:
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Rejected;
                case CnstStrExpired:
                    return Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses.Expired;
            }

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
            string StrNew = CnstStrNew.ToUpper();
            string StrPrinted = CnstStrPrinted.ToUpper();
            string StrHandedIn = CnstStrHandedIn.ToUpper();
            string StrRejected = CnstStrRejected.ToUpper();
            string StrExpired = CnstStrExpired.ToUpper();

            string StrApproved = CnstStrApproved.ToUpper();
            string StrBeingChecked = CnstStrBeingChecked.ToUpper();
            string StrApprovedWithRequirement = CnstStrApprovedWithRequirement.ToUpper();
            string StrFallenThrough = CnstStrFallenThrough.ToUpper();
            string StrLedOver = CnstStrLedOver.ToUpper();
            string StrResubmitted = CnstStrResubmitted.ToUpper();

            switch (antragtState.ToUpper())
            {
                case CnstStrNew:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.New;
                case CnstStrBeingChecked:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.BeingChecked;
                case CnstStrRejected:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Rejected;
                case CnstStrApproved:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Approved;
                case CnstStrApprovedWithRequirement:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.ApprovedWithRequirement;
                case CnstStrFallenThrough:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.FallenThrough;
                case CnstStrLedOver:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.LedOver;
                case CnstStrResubmitted:
                    return Cic.OpenLease.Model.DdOl.ANTRAG.AntragStatuses.Resubmitted;
            }

            return null;
        }
        #endregion
    }
}
