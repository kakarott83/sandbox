// OWNER: BK, 27-11-2008
namespace Cic.OpenLease.Model
{
	#region Enums
	// TEST BK 0 BK, Not tested
	[System.CLSCompliant(true)]
	public enum BooleanSelectConstants : int
	{
		CanBeBoth = 0,
		MustBeTrue = 1,
		MustBeFalse = 2,
	}

    // TEST BK 0 BK, Not tested.
    [System.CLSCompliant(true)]
    public enum RNSearchArtConstants : int
    {
        All = 0,
        OutgoingMandantorIncomingCustomer = 1,
        IncomingMandantorOutgoingCustomer = 2,
    }

    // TEST BK 0 BK, Not tested.
    [System.CLSCompliant(true)]
    public enum RNSearchTypeConstants : int
    {
        All = 0,
        WithoutReference = 1,
        OnlyContractReference = 2,
        ContractAndObjectReferences = 3,
    }

    // TEST BK 0 BK, Not tested.
    [System.CLSCompliant(true)]
    public enum RNPaidConstants : int
    {
        All = 0,
        Paid = 1,
        NotPaid = 2,
    }

    // TEST MK 0 MK, Not tested.
    [System.CLSCompliant(true)]
    public enum DeProcessCodes : int
    {
        New = 0,
        ReadyToProcess = 1,
        InProcess = 2,
        Processed = 3,
        Error = 4,
        Delay = 5
    }

    // TEST MK 0 MK, Not tested.
    [System.CLSCompliant(true)]
    public enum DeDefRuls
    {
        NEUKUNDE,
        JUENGER25,
        ANTRAGSCORE,
        LAUFZEITUEBER48,
    }

    // TEST MK 0 MK, Not tested.
    [System.CLSCompliant(true)]
    public enum DeDefCons
    {
        LOHNBESCHEINIGUNG,
        KREMO,
        AUFLAGE,
        DECISION,
    }
	#endregion
}
