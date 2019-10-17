using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Generic Assembler that helps transferring attributes by implementing IDtoMapper
    /// </summary>
    /// <typeparam name="T">Type of the Dto</typeparam>
    /// <typeparam name="D">Type of the Domain-Object</typeparam>
    [System.CLSCompliant(true)]
    public class GenericAssembler<T, D>
    {
        #region Private variables
        private IDtoMapper<T, D> _mapper;
        #endregion

        #region Constructors
        public GenericAssembler(IDtoMapper<T,D> m)
        {
            _mapper = m;
        }
        #endregion

        #region Methods

        public T ConvertToDto(D domain)
        {
            Type t = typeof(T);
            T rval = (T)System.Activator.CreateInstance(t);
            _mapper.mapToDto(rval, domain);
            return rval;
        }

        public List<T> ConvertToDto(List<D> domainList)
        {
            List<T> rval = new List<T>();
            foreach(D d in domainList)
            {
                rval.Add(ConvertToDto(d));
            }
            
            return rval;
        }

        public D ConvertFromDto(T dto)
        {
            Type t = typeof(D);
            D rval = (D)System.Activator.CreateInstance(t);
            _mapper.mapFromDto(dto, rval);
            return rval;
        }

        public List<D> ConvertFromDto(List<T> dtoList)
        {
            List<D> rval = new List<D>();
            foreach (T t in dtoList)
            {
                rval.Add(ConvertFromDto(t));
            }

            return rval;
        }
        #endregion
    }
}