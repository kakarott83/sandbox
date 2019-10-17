namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    using OpenOne.Common.DTO;

    public class osetCustomerCheckResult : oBaseDto
    {
        public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W007.executeResponse response { get; set; }
    }
}