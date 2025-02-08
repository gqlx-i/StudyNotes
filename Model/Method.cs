using StudyNotes.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudyNotes.Model
{
    public class Method : NotifyBase
    {
        private string _name;
        /// <summary>
        /// 方法名
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        private string _description;
        /// <summary>
        /// 方法描述
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetAndNotify(ref _description, value);
        }

        private ObservableCollection<KeyValue> _params;
        /// <summary>
        /// 方法调用参数
        /// </summary>
        public ObservableCollection<KeyValue> Params
        {
            get => _params;
            set => SetAndNotify(ref _params, value);
        }

        private string _returnType;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public string ReturnType
        {
            get => _returnType;
            set => SetAndNotify(ref _returnType, value);
        }

        private bool _isStaticClass;
        /// <summary>
        /// 是否静态类
        /// </summary>
        public bool IsStaticClass
        {
            get => _isStaticClass;
            set => SetAndNotify(ref _isStaticClass, value);
        }

        private string _instanceType;
        /// <summary>
        /// 调用实例类型
        /// </summary>
        public string InstanceType
        {
            get => _instanceType;
            set => SetAndNotify(ref _instanceType, value);
        }
    }

    public class KeyValue : NotifyBase
    {
        public KeyValue(string key, object value, string type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
        private string _key;
        public string Key
        {
            get => _key;
            set => SetAndNotify(ref _key, value);
        }
        private object _value;
        public object Value
        {
            get => _value;
            set => SetAndNotify(ref _value, value);
        }
        private string _type;
        public string Type
        {
            get => _type;
            set => SetAndNotify(ref _type, value);
        }
    }
}
