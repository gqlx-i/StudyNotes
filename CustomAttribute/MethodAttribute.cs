using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.CustomAttribute
{
    internal class MethodAttribute : Attribute
    {
        public object[] Params { get; set; }
        public MethodAttribute(params object[] objs)
        {
            this.Params = objs;
        }
    }
}
