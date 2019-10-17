using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Config;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Reflection;
using System.Data.Metadata.Edm;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Security;


namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Sample Dao Class
    /// </summary>
    public class SampleDao : ISampleDao
    {
        /// <summary>
        /// Default empty Constructor
        /// </summary>
        public SampleDao()
        {
            
        }

        /// <summary>
        /// sample Method implementation
        /// </summary>
        /// <param name="sampleParameter">sample parameter</param>
        /// <exception cref="System.NotImplementedException" >If not implemented</exception>
        /// <returns>sample return value</returns>
        public oSampleDto sampleMethod(iSampleDto sampleParameter)
        {

            //"metadata=\"res://Cic.OpenLease.Model, Version=1.0.42352.0, Culture=neutral, PublicKeyToken=14bbb979f3d3d80d/DdOl.DdOl.csdl|res://Cic.OpenLease.Model, Version=1.0.42352.0, Culture=neutral, PublicKeyToken=14bbb979f3d3d80d/DdOl.DdOl.ssdl|res://Cic.OpenLease.Model, Version=1.0.42352.0, Culture=neutral, PublicKeyToken=14bbb979f3d3d80d/DdOl.DdOl.msl\";provider=Devart.Data.Oracle;provider connection string=\"Data Source=BMWNETVISION;Persist Security Info=True;User ID=CIC;Password=71F10AAF981E449B;\""
            //"metadata=\"res://Cic.OpenOne.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/Model.DdOl.DdOl.csdl|res://Cic.OpenOne.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/Model.DdOl.DdOl.ssdl|res://Cic.OpenOne.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/Model.DdOl.DdOl.msl\";provider=Devart.Data.Oracle;provider connection string=\"User Id=cic;Password=71F10AAF981E449B;Server=CICDBS19;Direct=True;Sid=bmwnetvision;Port=1521;Persist Security Info=True;\""

            //two methods of createing a EF context object
            //a)
            /*string ConnectionString = Configuration.DeliverOpenLeaseConnectionString();
            MetadataWorkspace workspace = new MetadataWorkspace(new string[] { "res://Cic.OpenOne.Common/Model.DdOl.DdOl.csdl", "res://Cic.OpenOne.Common/Model.DdOl.DdOl.ssdl", "res://Cic.OpenOne.Common/Model.DdOl.DdOl.msl" }, new Assembly[] { Assembly.GetAssembly(this.GetType()) });
            OracleConnection conn = new OracleConnection(ConnectionString);
            EntityConnection et2 = new EntityConnection(workspace, conn);
            System.Data.Objects.ObjectContext ctx = new ObjectContext(et2);*/



            //b)
            /*
            String folderName = "Model.DdOl";
            String fileName = "DdOl";
            String assFullName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            String cstr = ConnectionStringBuilder.DeliverConnectionString(assFullName, folderName, fileName, "Devart.Data.Oracle", Configuration.DeliverOpenLeaseConnectionString());
            System.Data.Objects.ObjectContext ctx = new ObjectContext(cstr);
            */

         
            //simplified method b:
            oSampleDto result = new oSampleDto();
            
            using (DdOlExtended ctx = new DdOlExtended())
            {
                
                
                //testing a plain db query without entity usage
                List<long> ps = ctx.ExecuteStoreQuery<long>("select sysprparam from prparam", null).ToList();
               
                result.SampleText += sampleParameter.SampleText + " " + ps.Count;
            }
            return result;
        
        }

    }
}
