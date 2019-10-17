// OWNER BK, 04-08-2009
using System.ComponentModel;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Enums
    public enum SubmitStatus
    {
        [Description("")]
        OK = 0,
        [Description("VVTNEEDED")]
        VVTNEEDED,
        [Description("INVALIDVTNR")]
        INVALIDVTNR,
        [Description("DELIVERYDATENOTINFUTURE")]
        DELIVERYDATENOTINFUTURE,
        [Description("KASKONEEDED")]
        KASKONEEDED,
        [Description("MAXMVZEXCEEDED")]
        MAXMVZEXCEEDED,
        [Description("SERVICEEXTENSIONNOTSUPPORTED")]
        SERVICEEXTENSIONNOTSUPPORTED,
        [Description("IBANVALIDATIONFAILED")]
        IBANVALIDATIONFAILED,
        [Description("MANDATVALIDATIONFAILED")]
        MANDATVALIDATIONFAILED,
        [Description("EXTENSIONONLYENDMONTH")]
        EXTENSIONONLYENDMONTH,
        [Description("SCHUFANEEDED")]
        SCHUFANEEDED,
        [Description("PRODUCTEXPIRED")]
        PRODUCTEXPIRED,
        [Description("NEWSTATENOTPOSSIBLE")]
        NEWSTATENOTPOSSIBLE,
        [Description("SAVENOTPOSSIBLE")]
        SAVENOTPOSSIBLE,
        [Description("OFFERNOTFOUND")]
        OFFERNOTFOUND,
        [Description("INVALIDSTATUS")]
        INVALIDSTATUS,
        [Description("NOPERMISSION")]
        NOPERMISSION,
        [Description("TECHNICALISSUE")]
        TECHNICALISSUE
    }
    public enum ValidationStatus
    {
        [Description("UNSPECIFIED")]
        UNSPECIFIED,
        [Description("KASKONEEDED")]
        KASKONEEDED,
        [Description("DELETEDIBANBIC")]
        DELETEDIBANBIC
    }
    public enum AngebotCancelStatus
    {
        ANGEBOT_CANCELED_OK = 0,
        ANGEBOT_INVALID_STATE = 1,
        ANTRAG_INVALID_STATE = 2,
        ANGEBOT_NOT_FOUND = 3

    }
    #endregion
}
