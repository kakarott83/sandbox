using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Cic.One.Workflow.Activities
{
    public class GetInput : CodeActivity
    {
        OutArgument<string> data;
        public OutArgument<string> Data
        {
            get { return data; }
            set { data = value; }
        }

        protected override void Execute(CodeActivityContext context)
        {
            string input = Console.ReadLine();
            context.SetValue(data, input);
        }
    }
}
