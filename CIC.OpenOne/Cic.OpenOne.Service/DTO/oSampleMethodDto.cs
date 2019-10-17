using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Otis;
using Cic.OpenOne.Common.DTO;
using System.ServiceModel;
using System.Xml.Serialization;
using Cic.OpenOne.Common.Util.SOAP.Annotations;
using System.Runtime.Serialization;

namespace Cic.OpenOne.Service.DTO
{
    /// <summary>
    /// Output parameter object for sample service
    /// </summary>
    //[XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)] 
    
    public class oSampleMethodDto : oBaseDto
    {
       
        public String sampleResult;
        public DateTime aTimeStamp;
        public long numberlong;
        public long? numberlong2;
        public InnerType inner1;
        //[XmlElementAttribute("ElemNAme")]
        
        [DataMemberAnnotation("This is the account balance")]
        public InnerType[] innerarray;
        public List<InnerType> innerlist;

        //public InnerType? inner2;
        //public InnerType?[] innerarray2;
        //public List<InnerType>? innerlist2;
    }
    /// <summary>
    /// Test Inner Type
    /// </summary>
    public class InnerType
    {
        public InnerType2 innerinner = new InnerType2();
        //public InnerType2? innerinner2 = new InnerType2();
        public String test = "";
        
    }
    /// <summary>
    /// Test Inner Type 2
    /// </summary>
    public class InnerType2
    {
        public String test = "InnerType2";
        
    }
}