using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using AutoMapper;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class FileattDao : IFileattDao
    {
        /// <summary>
        /// updates/creates Attachement
        /// </summary>
        /// <param name="fileatt"></param>
        /// <returns></returns>
        public FileattDto createOrUpdateFileatt(FileattDto fileatt)
        {
            FILEATT fileattOutput = null;
            FileattDto fileattDto = null;
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (fileatt.sysFileAtt == 0)
                {
                    fileattOutput = new FILEATT();
                    fileattOutput = Mapper.Map<FileattDto, FILEATT>(fileatt, fileattOutput);
                    fileattOutput.SYSFILEATT = 0;
                    ctxOw.FILEATT.Add(fileattOutput);

                }
                else
                {
                    fileattOutput = (from p in ctxOw.FILEATT
                                     where p.SYSFILEATT == fileatt.sysFileAtt
                                     select p).FirstOrDefault();
                    if (fileattOutput != null)
                    {
                        fileattOutput = Mapper.Map<FileattDto, FILEATT>(fileatt, fileattOutput);
                       
                    }
                    else throw new Exception("Fileatt with id " + fileatt.sysFileAtt + " not found!");
                }

                ctxOw.SaveChanges();

                fileattDto = Mapper.Map<FILEATT, FileattDto>(fileattOutput);
            }
            return getFileatt(fileattDto.sysFileAtt);
        }


        /// <summary>
        /// BestehendesFileatt holen
        /// </summary>
        /// <param name="sysfileatt">Primary Key</param>
        /// <returns>Filleatt Ausgang</returns>
        public FileattDto getFileatt(long sysfileatt)
        {
           FileattDto rval = null;
            using (DdOwExtended owCtx = new DdOwExtended())
            {
               FILEATT fileattOut = (from e in owCtx.FILEATT
                                       where e.SYSFILEATT == sysfileatt
                                       select e).FirstOrDefault();

                if (fileattOut != null)
                {
                    
                    rval = Mapper.Map<FILEATT,FileattDto>(fileattOut);
                 
                }
            }
            return rval;
        }

       

        
    }
}
