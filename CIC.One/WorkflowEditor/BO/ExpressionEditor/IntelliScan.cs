using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cic.One.DTO;

namespace Workflows.BO.ExpressionEditor
{
    public class IntelliScan
    {
        public String keyWords { get; set; }
        public TreeNodes _inttelisenseList { get; set; }

        public IntelliScan()
        {
            keyWords = CreateKeywords();
            CreateIntellisenseList();
        }
        private string CreateKeywords()
        {
            StringBuilder words = new StringBuilder();
            var _with1 = words;
            _with1.Append("AddHandler|");
            _with1.Append("AddressOf|");
            _with1.Append("Alias|");
            _with1.Append("And|");
            _with1.Append("AndAlso|");
            _with1.Append("As|");
            _with1.Append("Boolean|");
            _with1.Append("ByRef|");
            _with1.Append("Byte|");
            _with1.Append("ByVal|");
            _with1.Append("Call|");
            _with1.Append("Case|");
            _with1.Append("Catch|");
            _with1.Append("CBool|");
            _with1.Append("CByte|");
            _with1.Append("CChar|");
            _with1.Append("CDate|");
            _with1.Append("CDbl|");
            _with1.Append("CDec|");
            _with1.Append("Char|");
            _with1.Append("CInt|");
            _with1.Append("Class|");
            _with1.Append("CLng|");
            _with1.Append("CObj|");
            _with1.Append("Const|");
            _with1.Append("Continue|");
            _with1.Append("CSByte|");
            _with1.Append("CShort|");
            _with1.Append("CSng|");
            _with1.Append("CStr|");
            _with1.Append("CType|");
            _with1.Append("CUInt|");
            _with1.Append("CULng|");
            _with1.Append("CUShort|");
            _with1.Append("Date|");
            _with1.Append("Decimal|");
            _with1.Append("Declare|");
            _with1.Append("Default|");
            _with1.Append("Delegate|");
            _with1.Append("Dim|");
            _with1.Append("DirectCast|");
            _with1.Append("Do|");
            _with1.Append("Double|");
            _with1.Append("Each|");
            _with1.Append("Else|");
            _with1.Append("ElseIf|");
            _with1.Append("End|");
            _with1.Append("EndIf|");
            _with1.Append("Enum|");
            _with1.Append("Erase|");
            _with1.Append("Error|");
            _with1.Append("Event|");
            _with1.Append("Exit|");
            _with1.Append("False|");
            _with1.Append("Finally|");
            _with1.Append("For|");
            _with1.Append("Friend|");
            _with1.Append("Function|");
            _with1.Append("Get|");
            _with1.Append("GetType|");
            _with1.Append("GetXMLNamespace|");
            _with1.Append("Global|");
            _with1.Append("GoSub|");
            _with1.Append("GoTo|");
            _with1.Append("Handles|");
            _with1.Append("If|");
            _with1.Append("Implements|");
            _with1.Append("Imports|");
            _with1.Append("In|");
            _with1.Append("Inherits|");
            _with1.Append("Integer|");
            _with1.Append("Interface|");
            _with1.Append("Is|");
            _with1.Append("IsNot|");
            _with1.Append("Let|");
            _with1.Append("Lib|");
            _with1.Append("Like|");
            _with1.Append("Long|");
            _with1.Append("Loop|");
            _with1.Append("Me|");
            _with1.Append("Mod|");
            _with1.Append("Module|");
            _with1.Append("MustInherit|");
            _with1.Append("MustOverride|");
            _with1.Append("MyBase|");
            _with1.Append("MyClass|");
            _with1.Append("Namespace|");
            _with1.Append("Narrowing|");
            _with1.Append("New|");
            _with1.Append("Next|");
            _with1.Append("Not|");
            _with1.Append("Nothing|");
            _with1.Append("NotInheritable|");
            _with1.Append("NotOverridable|");
            _with1.Append("Object|");
            _with1.Append("Of|");
            _with1.Append("On|");
            _with1.Append("Operator|");
            _with1.Append("Option|");
            _with1.Append("Optional|");
            _with1.Append("Or|");
            _with1.Append("OrElse|");
            _with1.Append("Out|");
            _with1.Append("Overloads|");
            _with1.Append("Overridable|");
            _with1.Append("Overrides|");
            _with1.Append("ParamArray|");
            _with1.Append("Partial|");
            _with1.Append("Private|");
            _with1.Append("Property|");
            _with1.Append("Protected|");
            _with1.Append("Public|");
            _with1.Append("RaiseEvent|");
            _with1.Append("ReadOnly|");
            _with1.Append("ReDim|");
            _with1.Append("REM|");
            _with1.Append("RemoveHandler|");
            _with1.Append("Resume|");
            _with1.Append("Return|");
            _with1.Append("SByte|");
            _with1.Append("Select|");
            _with1.Append("Set|");
            _with1.Append("Shadows|");
            _with1.Append("Shared|");
            _with1.Append("Short|");
            _with1.Append("Single|");
            _with1.Append("Static|");
            _with1.Append("Step|");
            _with1.Append("Stop|");
            _with1.Append("String|");
            _with1.Append("Structure|");
            _with1.Append("Sub|");
            _with1.Append("SyncLock|");
            _with1.Append("Then|");
            _with1.Append("Throw|");
            _with1.Append("To|");
            _with1.Append("True|");
            _with1.Append("Try|");
            _with1.Append("TryCast|");
            _with1.Append("TypeOf|");
            _with1.Append("UInteger|");
            _with1.Append("ULong|");
            _with1.Append("UShort|");
            _with1.Append("Using|");
            _with1.Append("Variant|");
            _with1.Append("Wend|");
            _with1.Append("While|");
            _with1.Append("Widening|");
            _with1.Append("With|");
            _with1.Append("WithEvents|");
            _with1.Append("WriteOnly|");
            _with1.Append("Xor|");
            _with1.Append("#Const|");
            _with1.Append("#Else|");
            _with1.Append("#ElseIf|");
            _with1.Append("#End|");
            _with1.Append("#If");
            return words.ToString();
        }


        

        private void CreateIntellisenseList()
        {
            Cic.One.Workflow.Activities.AddMessage m = new Cic.One.Workflow.Activities.AddMessage();
            WorkflowContext ctx = new WorkflowContext();
            AccountDto acc = new AccountDto();

            _inttelisenseList = new TreeNodes();
            Assembly wfAsm = System.Reflection.Assembly.GetExecutingAssembly();
            List<Assembly> refAsmList = (from x in wfAsm.GetReferencedAssemblies()
                                         select System.Reflection.Assembly.Load(x)).ToList();

            List<Type> typeList = refAsmList.SelectMany(a => (from x in a.GetTypes()
                                                           where x.IsPublic && x.IsVisible && ((x.Namespace != null)
                                                               //&& (!x.Namespace.Contains("Workflows"))
                                                               )

                                                           select x)).ToList();

            _inttelisenseList.Nodes.Clear();
            foreach (Type childAsm in typeList)
            {

                this.AddNode(_inttelisenseList, childAsm.Namespace);
                this.AddTypeNode(_inttelisenseList, childAsm);
            }
            this.AddNode(_inttelisenseList, "New", false);
            this.AddNode(_inttelisenseList, "Cic.One.DTO.WorkflowContext", false);
            
            this.SortNodes(_inttelisenseList);
        }

        private void AddNode(TreeNodes targetNodes, string namePath)
        {

            string[] targetPath = namePath.Split('.');
            bool validPath = false;
            TreeNodes existsNodes = null;
            List<TreeNodes> validNodes = (from x in targetNodes.Nodes
                                          where x.Name.ToLower() == targetPath[0].ToLower()
                                          select x).ToList();

            if (validNodes != null && validNodes.Count > 0)
            {
                existsNodes = validNodes[0];
                validPath = true;
            }
            if (!validPath)
            {
                TreeNodes childNodes = new TreeNodes();

                childNodes.Name = targetPath[0];
                childNodes.AddStrings = targetPath[0];
                childNodes.ItemType = TreeNodes.NodeTypes.Namespace;
                childNodes.Parent = targetNodes;
                childNodes.Description = string.Format("Namespace {0}", targetPath[0]);

                targetNodes.AddNode(childNodes);

                String nextPath = namePath.Substring(targetPath[0].Length, (namePath.Length - targetPath[0].Length));
                if (nextPath.StartsWith("."))
                {
                    nextPath = nextPath.Substring(1, (nextPath.Length - 1));
                }
                if ((nextPath.Trim() != ""))
                {
                    this.AddNode(childNodes, nextPath);
                }
            }
            else
            {
                String nextPath = namePath.Substring(targetPath[0].Length, (namePath.Length - targetPath[0].Length));
                if (nextPath.StartsWith("."))
                {
                    nextPath = nextPath.Substring(1, (nextPath.Length - 1));
                }
                if ((nextPath.Trim() != ""))
                {
                    this.AddNode(existsNodes, nextPath);
                }
            }
        }
        private void AddNode(TreeNodes targetNodes, string namePath, bool isNamespace)
        {
            string[] targetPath = namePath.Split('.');
            bool validPath = false;
            TreeNodes existsNodes = null;
            List<TreeNodes> validNodes = (from x in targetNodes.Nodes
                                          where x.Name.ToLower() == targetPath[0].ToLower()
                                          select x).ToList();
            if (validNodes != null && validNodes.Count > 0)
            {
                existsNodes = validNodes[0];
                validPath = true;
            }
            if (!validPath)
            {
                TreeNodes childNodes = new TreeNodes();

                childNodes.Name = targetPath[0];
                childNodes.AddStrings = targetPath[0];
                childNodes.ItemType = isNamespace ? TreeNodes.NodeTypes.Namespace : TreeNodes.NodeTypes.Primitive;
                childNodes.Parent = targetNodes;
                childNodes.Description = string.Format("Namespace {0}", targetPath[0]);

                targetNodes.AddNode(childNodes);
                if (isNamespace)
                {
                    String nextPath = namePath.Substring(targetPath[0].Length, (namePath.Length - targetPath[0].Length));
                    if (nextPath.StartsWith("."))
                    {
                        nextPath = nextPath.Substring(1, (nextPath.Length - 1));
                    }
                    if ((nextPath.Trim() != ""))
                    {
                        this.AddNode(childNodes, nextPath);
                    }
                }
                else if (isNamespace)
                {
                    String nextPath = namePath.Substring(targetPath[0].Length, (namePath.Length - targetPath[0].Length));
                    if (nextPath.StartsWith("."))
                    {
                        nextPath = nextPath.Substring(1, (nextPath.Length - 1));
                    }
                    if ((nextPath.Trim() != ""))
                    {
                        this.AddNode(existsNodes, nextPath);
                    }
                }
            }
        }
        private void AddTypeNode(TreeNodes targetNodes, Type target)
        {
            if ((target.IsAbstract || !target.IsVisible))
            {
                return;
            }
            String typeNamespace = target.Namespace;
            String typeName = target.Name;
            TreeNodes parentNode = this.SearchNodes(targetNodes, typeNamespace);
            TreeNodes newNodes = new TreeNodes();
            newNodes.Name = typeName;
            newNodes.AddStrings = typeName;
            newNodes.Parent = parentNode;
            newNodes.SystemType = target;
            string nodesName = typeName;
            if (target.IsGenericType)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.Class;

                if (typeName.Contains("`"))
                {
                    nodesName = typeName.Substring(0, typeName.LastIndexOf("`"));
                    newNodes.AddStrings = nodesName;
                }
                System.Text.StringBuilder paramStrings = new System.Text.StringBuilder();
                int count = 0;
                foreach (Type childArg in target.GetGenericArguments())
                {
                    if ((count > 0))
                    {
                        paramStrings.Append(", ");
                    }
                    paramStrings.Append(childArg.Name);
                    count++;
                }
                nodesName += "(" + paramStrings.ToString() + ")";
                newNodes.Name = nodesName;
                newNodes.Description = string.Format("Class {0}", newNodes.AddStrings);

            }
            else if (target.IsClass)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.Class;
                newNodes.Description = string.Format("Class {0}", newNodes.AddStrings);
            }
            else if (target.IsEnum)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.Enum;
                newNodes.Description = string.Format("Enum {0}", newNodes.AddStrings);
            }
            else if (target.IsInterface)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.Interface;
                newNodes.Description = string.Format("Interface {0}", newNodes.AddStrings);
            }
            else if (target.IsPrimitive)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.Primitive;
                newNodes.Description = string.Format("{0}", newNodes.AddStrings);
            }
            else if (target.IsValueType)
            {
                newNodes.ItemType = TreeNodes.NodeTypes.ValueType;
                newNodes.Description = string.Format("{0}", newNodes.AddStrings);
            }
            else
                return;
            if ((parentNode == null))
            {
                targetNodes.AddNode(newNodes);
            }
            else
            {
                parentNode.AddNode(newNodes);
            }

            this.AddMethodNode(newNodes, target);
            this.AddPropertyNode(newNodes, target);
            this.AddFieldNode(newNodes, target);
            this.AddEventNode(newNodes, target);
            this.AddNestedTypeNode(newNodes, target);
        }
        private void AddMethodNode(TreeNodes targetNodes, Type targetType)
        {
            /*   System.Threading.Tasks.Parallel.ForEach(wordTagList, target =>
                  {
                      TextRange range = new TextRange(target.StartPosition, target.EndPosition);
                      range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                  });
            */
            System.Threading.Tasks.Parallel.ForEach(targetType.GetMethods((BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance))),
                target =>
                {
                    TreeNodes newNodes = new TreeNodes();
                    newNodes.Name = target.Name;
                    newNodes.AddStrings = target.Name;
                    newNodes.ItemType = TreeNodes.NodeTypes.Method;
                    newNodes.Parent = targetNodes;
                    newNodes.Description = CreateMethodDescription(target);
                    targetNodes.AddNode(newNodes);
                });
        }
        private void AddPropertyNode(TreeNodes targetNodes, Type targetType)
        {

            System.Threading.Tasks.Parallel.ForEach(targetType.GetProperties((BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance))),
                target =>
                {
                    TreeNodes newNodes = new TreeNodes();
                    newNodes.Name = target.Name;
                    newNodes.AddStrings = target.Name;
                    newNodes.ItemType = TreeNodes.NodeTypes.Property;
                    newNodes.Parent = targetNodes;
                    newNodes.Description = CreatePropertyDescription(target);
                    targetNodes.AddNode(newNodes);
                });
        }

        private void AddFieldNode(TreeNodes targetNodes, Type targetType)
        {

            System.Threading.Tasks.Parallel.ForEach(targetType.GetFields((BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance))),
                target =>
                {
                    TreeNodes newNodes = new TreeNodes();
                    newNodes.Name = target.Name;
                    newNodes.AddStrings = target.Name;
                    newNodes.ItemType = TreeNodes.NodeTypes.Field;
                    newNodes.Parent = targetNodes;
                    newNodes.Description = CreateFieldDescription(target);
                    targetNodes.AddNode(newNodes);
                });
        }

        private void AddEventNode(TreeNodes targetNodes, Type targetType)
        {

            System.Threading.Tasks.Parallel.ForEach(targetType.GetEvents((BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance))),
                target =>
                {
                    TreeNodes newNodes = new TreeNodes();
                    newNodes.Name = target.Name;
                    newNodes.AddStrings = target.Name;
                    newNodes.ItemType = TreeNodes.NodeTypes.Event;
                    newNodes.Parent = targetNodes;
                    newNodes.Description = CreateEventDescription(target);
                    targetNodes.AddNode(newNodes);
                });
        }

        private void AddNestedTypeNode(TreeNodes targetNodes, Type targetType)
        {

            System.Threading.Tasks.Parallel.ForEach(targetType.GetNestedTypes((BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance))),
                target =>
                {
                    TreeNodes newNodes = new TreeNodes();
                    newNodes.Name = target.Name;
                    newNodes.AddStrings = target.Name;
                    newNodes.ItemType = TreeNodes.NodeTypes.Method;
                    newNodes.Parent = targetNodes;
                    targetNodes.AddNode(newNodes);
                });
        }

        private TreeNodes SearchNodes(TreeNodes targetNodes, string namePath)
        {
            string[] targetPath = namePath.Split('.');
            bool validPath = false;
            TreeNodes existsNodes = null;
            List<TreeNodes> validNodes = (from x in targetNodes.Nodes
                                          where x.Name.ToLower() == targetPath[0].ToLower()
                                          select x).ToList();
            if (validNodes != null && validNodes.Count > 0)
            {
                existsNodes = validNodes[0];
                validPath = true;
            }

            if (!validPath)
            {
                return targetNodes;
            }
            String nextPath = namePath.Substring(targetPath[0].Length, (namePath.Length - targetPath[0].Length));
            if (nextPath.StartsWith("."))
            {
                nextPath = nextPath.Substring(1, (nextPath.Length - 1));
            }
            if ((nextPath.Trim() == ""))
            {
                return existsNodes;
            }
            return this.SearchNodes(existsNodes, nextPath);
        }

        private void SortNodes(TreeNodes targetNodes)
        {
            targetNodes.Nodes.Sort(new ComparerName());
            foreach (TreeNodes childNode in targetNodes.Nodes)
            {
                this.SortNodes(childNode);
            }
        }




        private string CreateMethodDescription(MethodInfo target)
        {
            StringBuilder desc = new StringBuilder();
            if (target.IsPublic)
                desc.Append("public ");
            if (target.IsFamily)
                desc.Append("protected ");
            if (target.IsAssembly)
                desc.Append("assembly ");
            if (target.IsPrivate)
                desc.Append("private ");
            if (target.IsAbstract)
                desc.Append("abstract ");
            if (target.IsVirtual && !target.IsFinal)
                desc.Append("virtual ");
            if (target.IsStatic)
                desc.Append("static ");

            //if (((!object.ReferenceEquals(target.ReturnType, typeof(Void))))) {
            //desc.Append("method ");
            /*} else {
                desc.Append("Sub ");
            }*/
            if (target.ReturnType != null)
            {
                desc.Append(target.ReturnType.Name);
                desc.Append(CreateGenericParameter(target.ReturnType));
            }
            else desc.Append("void");

            desc.Append(" ");
            desc.Append(target.Name);
            desc.Append(CreateGenericParameter(target));

            desc.Append("(");
            int paramIndex = 0;
            foreach (ParameterInfo param in target.GetParameters())
            {

                if (paramIndex > 0)
                    desc.Append(", ");
               /* if (param.IsOptional)
                    desc.Append("optional ");
                if (param.IsOut)
                {
                    desc.Append("reference ");
                }
                else
                {
                    desc.Append("value ");
                }*/
                desc.Append(param.ParameterType.Name + " " + param.Name);

                desc.Append(CreateGenericParameter(param.ParameterType));
               // if (param.DefaultValue!=null)
              /*  {
                    if (param.DefaultValue == null)
                    {
                        desc.Append(" = Nothing");
                    }
                    else
                    {
                        desc.Append(" = " + param.DefaultValue.ToString());
                    }
                }*/
                paramIndex += 1;
            }
            desc.Append(") ");
           
            return desc.ToString();
        }

        private string CreatePropertyDescription(PropertyInfo target)
        {
            StringBuilder desc = new StringBuilder();

            if (target.CanRead && target.CanWrite)
            {
            }
            else if (target.CanRead)
            {
                desc.Append("ReadOnly ");
            }
            else
            {
                desc.Append("WriteOnly ");
            }

            desc.Append(target.PropertyType.Name+" " + target.Name);
            desc.Append(CreateGenericParameter(target.PropertyType));

            return desc.ToString();
        }

        private string CreateFieldDescription(FieldInfo target)
        {
            StringBuilder desc = new StringBuilder();
            if (target.IsPublic)
                desc.Append("public ");
            if (target.IsPrivate)
                desc.Append("private ");
            if (target.IsStatic)
                desc.Append("static ");

           
            if (target.FieldType != null)
            {
                desc.Append(target.FieldType.Name+" ");
                desc.Append(CreateGenericParameter(target.FieldType));
            }
            desc.Append(target.Name);
            desc.Append(" ");
            return desc.ToString();
        }

        private string CreateEventDescription(EventInfo target)
        {
            StringBuilder desc = new StringBuilder();

           
            if (target.EventHandlerType != null)
            {
                desc.Append(target.EventHandlerType.Name);
                if (target.EventHandlerType.IsGenericType)
                {
                    desc.Append(CreateGenericParameter(target.EventHandlerType));
                }
            }
            desc.Append(target.Name);
            return desc.ToString();
        }

        private string CreateGenericParameter(MethodInfo target)
        {
            StringBuilder result = new StringBuilder();
            if (target.IsGenericMethod)
            {
                result.Append("< ");
                int genIndex = 0;
                foreach (Type genParam in target.GetGenericArguments())
                {
                    if (genIndex > 0)
                        result.Append(", ");
                    result.Append(genParam.Name);
                    genIndex += 1;
                }
                result.Append(">");
            }
            return result.ToString();
        }
        private string CreateGenericParameter(Type target)
        {
            StringBuilder result = new StringBuilder();
            if (target.IsGenericType)
            {
                result.Append("< ");
                int genIndex = 0;
                foreach (Type genParam in target.GetGenericArguments())
                {

                    if (genIndex > 0)
                        result.Append(", ");
                    result.Append(genParam.Name);
                    genIndex += 1;
                }
                result.Append(">");
            }
            return result.ToString();
        }

      
    }
}
