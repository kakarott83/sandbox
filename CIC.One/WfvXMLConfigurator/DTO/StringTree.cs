using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfvXmlConfigurator.DTO
{
    public class StringTree
    {
        public string Element { get; set; }
        public IEnumerable<StringTree> Children 
        { 
            get 
            { 
                return children; 
            } 
        }
        private List<StringTree> children = new List<StringTree>();

        public StringTree(string element = "")
        {
            Element = element;
        }

        public static StringTree operator +(StringTree left, IEnumerable<StringTree> arg)
        {
            left.children.AddRange(arg);
            return left;
        }

        public static StringTree operator +(StringTree left, StringTree arg)
        {
            left.children.Add(arg);
            return left;
        }

        public static StringTree operator +(StringTree left, string arg)
        {
            left.children.Add(new StringTree(arg));
            return left;
        }

    }
}
