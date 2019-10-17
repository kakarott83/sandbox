using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.Utils.Util.Behaviour
{
    public class StringSurrogated : IDataContractSurrogate
    {
        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            if (true) return null;
            Console.WriteLine("GetCustomDataToExport(Member)");
            System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
            if (fieldInfo.IsPublic)
            {
                return "public";
            }
            else
            {
                return "private";
            }
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            if (true) return null;
            Console.WriteLine("GetCustomDataToExport(Member)");
            return "customData";
        }

        public Type GetDataContractType(Type type)
        {
            
            
            if(type.ToString().Contains("ing"))
            {
                Console.WriteLine("GetDataContractType");
            }
            if (type.IsEquivalentTo(Type.GetType("System.String")))//typeof(String).IsAssignableFrom(type))
            {
                return typeof(StringSurrogated);
            }
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            Console.WriteLine("GetDeserializedObject");
            if (obj is String)
            {
                /*Inventory invent = new Inventory();
                invent.pens = ((InventorySurrogated)obj).pens;
                invent.pencils = ((InventorySurrogated)obj).numpencils;
                invent.paper = ((InventorySurrogated)obj).numpaper;*/
                return obj;
            }
            return obj;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            Console.WriteLine("GetObjectToSerialize");
            if (obj is String)
            {
                /*InventorySurrogated isur = new InventorySurrogated();
                isur.numpaper = ((Inventory)obj).paper;
                isur.numpencils = ((Inventory)obj).pencils;
                isur.pens = ((Inventory)obj).pens;
                return isur;*/
            }
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
    }
}
