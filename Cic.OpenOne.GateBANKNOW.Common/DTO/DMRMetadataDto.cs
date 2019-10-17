namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class DMRMetadataDto
    {
        public string ContractID { get; set; }

        public long? Contact1Person { get; set; }

        public long? Contact1It { get; set; }

        public long? Contact2Person { get; set; }

        public long? Contact2It { get; set; }

        public long? SysID { get; set; }
        
        public long Erfassungsclient { get; set; }

        public long? SysVm { get; set; }

        public DMRMetadataDto Apply(DMRMetadataDto merged)
        {
            if (merged == null)
            {
                return new DMRMetadataDto()
                {
                    Contact1It = Contact1It,
                    ContractID = ContractID,
                    Erfassungsclient = Erfassungsclient,
                    Contact1Person = Contact1Person,
                    Contact2It = Contact2It,
                    Contact2Person = Contact2Person,
                    SysID = SysID,
                    SysVm = SysVm
                };
            }

            return new DMRMetadataDto()
            {
                ContractID = !string.IsNullOrEmpty(ContractID)? ContractID : merged.ContractID,
                Erfassungsclient = Erfassungsclient != 0 ? Erfassungsclient : merged.Erfassungsclient,
                Contact1It = Contact1It ?? merged.Contact1It,
                Contact1Person = Contact1Person ?? merged.Contact1Person,
                Contact2It = Contact2It ?? merged.Contact2It,
                Contact2Person = Contact2Person ?? merged.Contact2Person,
                SysID = SysID ?? merged.SysID,
                SysVm = SysVm ?? merged.SysVm
            };
        }
    }
}