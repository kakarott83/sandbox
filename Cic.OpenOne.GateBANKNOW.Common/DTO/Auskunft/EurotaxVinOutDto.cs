using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxVinRef;


namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
   public class EurotaxVinOutDto
    {
       
    

        public string vinCode {get;set;}

        public VehicleType[] vehicle {get;set;}

        public EquipmentType[] equipment {get;set;}

        public EsacoType[] esaco {get;set;}

        public short statusCode {get;set;}

        public string statusMsg {get;set;}

        public string debugOutput  {get;set;}

        public EquipmentType[] extEquipment  {get;set;}

        public EsacoType[] extEsaco  {get;set;}

        public ValuationResponseType valuationResponse  {get;set;}

        public short detectedMakeId  {get;set;}

        public bool detectedMakeIdFieldSpecified {get;set;}

        public ManufacturerInfoType[] manufOriginData  {get;set;}

        public ManufacturerInfoType[] manufUnknownEquipment  {get;set;}

        public System.DateTime productionDate  {get;set;}

        public bool productionDateFieldSpecified {get;set;}

        public System.DateTime deliveryDate  {get;set;}

        public bool deliveryDateFieldSpecified {get;set;}

        public System.DateTime registrationDate  {get;set;}

        public bool registrationDateFieldSpecified {get;set;}

        public string colourDescription  {get;set;}

        public string colourCode  {get;set;}

        public string upholsteryDescription  {get;set;}

        public string upholsteryCode  {get;set;}

        public string topDescription  {get;set;}

        public string topCode  {get;set;}

        public ISOcountryType deliveryCountry { get; set; }

        public bool deliveryCountryFieldSpecified {get;set;}

        public string vOC  {get;set;}

        
        public string ErrorDescription { get; set; }

       
        public int ErrorCode { get; set; }
        
    }
}
