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
    /// <summary>
    /// 程序运行中的全局变量
    /// </summary>
    public class GlobalService
    {
        private static Lazy<GlobalService> _instance = new Lazy<GlobalService>(()=>new GlobalService());

        /// <summary>
        /// 全局服务对象
        /// </summary>
        public static GlobalService Instance => _instance.Value;

        /// <summary>
        /// 事件收集器对象
        /// </summary>
        public EventAggregator EventAggregator { get; private set; } = new EventAggregator();

        /// <summary>
        /// 测试方法集合
        /// </summary>
        public List<Method>? Methods { get; set; }
        
        /// <summary>
        /// 自定义界面集合
        /// </summary>
        public Dictionary<string, NotifyUserControl>? Controls { get; set; }
    }
}
