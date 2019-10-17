using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Cic.OpenOne.Service.Contracts;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Service.DTO;
using Cic.OpenOne.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Util.Security;

using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common;
using Cic.OpenOne.Common.Util.SOAP;

namespace Cic.OpenOne.Service
{
    /// <summary>
    /// Sample Service impl.
    /// </summary>
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class SampleService : ISampleService
    {
        /// <summary>
        /// Sample Method 
        /// </summary>
        /// <param name="sampleParameter">Sample parameter</param>
        /// <returns>return value</returns>
        public oSampleMethodDto sampleMethod(iSampleMethodDto sampleParameter)
        {
            //create the returnvalue object
            oSampleMethodDto rval = new oSampleMethodDto();
            CredentialContext cctx = new CredentialContext();

            try
            {

                

                //Validate user credentials - will throw ServiceException if not valid
                MembershipUserValidationInfo mInfo = cctx.validateService();
                //MembershipUserValidation Info may now be used if necessary....

                //call the bo and map the datastructures
                ISampleDao dao = new SampleDao();
                ISampleBo bo = new SampleBo(dao);
                oSampleMethodDto result = new oSampleMethodDto();
                iSampleDto input = null;


                //Automapper-Example:
                Mapper.CreateMap<iSampleMethodDto, iSampleDto>().ForMember(a => a.SampleText, m => m.MapFrom(s => s.inputData));
                Mapper.CreateMap<oSampleDto, oSampleMethodDto>().ForMember(a => a.sampleResult, m => m.MapFrom(s => s.SampleText));
                input = Mapper.Map<iSampleMethodDto, iSampleDto>(sampleParameter);
                //End Automapper

                //Otis-Example
                //Otis.IAssembler<iSampleDto, iSampleMethodDto> asm = Global.OTISCONFIG.GetAssembler<iSampleDto, iSampleMethodDto>();               // retrieve the assembler
                //Otis.IAssembler<oSampleMethodDto, oSampleDto> asm2 = Global.OTISCONFIG.GetAssembler<oSampleMethodDto, oSampleDto>();               // retrieve the assembler
                //input = asm.AssembleFrom(sampleParameter);  // do the transformation
                //End Otis


                //call the bo-object
                oSampleDto output = bo.sampleMethod(input);

                //Automapper, use input object, too, else the message object will vanish!
                rval = Mapper.Map<oSampleDto, oSampleMethodDto>(output, rval);
                //End Automapper


                rval.innerarray = new InnerType[5];
//                rval.innerarray2 = new InnerType[5];
                rval.innerlist = new List<InnerType>();
                for (int i = 0; i < 5; i++)
                {
                    rval.innerarray[i] = new InnerType();
                    rval.innerlist.Add(new InnerType());
                }
                

                //Otis
                //rval = asm2.AssembleFrom(output);  // do the transformation
                //End Otis
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }


        }
    }
}
