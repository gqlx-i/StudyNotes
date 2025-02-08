using StudyNotes.Functions;
using StudyNotes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Common
{
    public class GlobalService
    {
        private static Lazy<GlobalService> _instance = new Lazy<GlobalService>(()=>new GlobalService());
        public static GlobalService Instance => _instance.Value;

        public List<Method> Methods { get; set; }
        public Dictionary<string, NotifyUserControl> Controls { get; set; }
    }
}
