using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Class to create BodyRecord 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DEBodyRecord<T> where T : new()
    {
        /// <summary>
        /// DEBodyRecord Konstruktor
        /// </summary>
        public DEBodyRecord(){}
        /// <summary>
        /// Creates new DecisionEngine BodyRecord and sets its vField to inDto
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public T Create(object inputDto)
        {
            Type inputtype = typeof(T);

            T rval = (T)Activator.CreateInstance(inputtype);
            MethodInfo mi = inputtype.GetMethod("set_vSpecified");
            if(mi!=null)
            {
                mi.Invoke(rval,new object[]{inputDto!=null});
            }

            if (inputDto != null)
            {
                mi = inputtype.GetMethod("set_v");
                if (mi != null)
                {
                    Type vType = inputtype.GetProperty("v").PropertyType;
                    // if vType is Enum, parse inputDto to Enum
                    if (vType.IsEnum)
                    {
                        if (vType.GetEnumNames().Contains(inputDto as string))
                        {
                            mi.Invoke(rval, new[] { Enum.Parse(vType, inputDto as string) });
                        }
                    }
                    else
                    {
                        mi.Invoke(rval, new[] { inputDto });
                    }
                }
                
            }
            return rval;
        }
        /// <summary>
        /// Gets vField of a DecisionEngine BodyRecord
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public object GetVParameter(T response)
        {
            Type responsetype = typeof(T);
            if (response != null)
            {
                MethodInfo mi = responsetype.GetMethod("get_v");
                return mi.Invoke(response, null);
            }
            else
            {
                return null;
            }
        }        
    }
}
