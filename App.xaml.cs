using StudyNotes.Common;
using StudyNotes.CustomAttribute;
using StudyNotes.Model;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace StudyNotes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            RegisterControls();
            RegisterTestMethods();
        }

        public void RegisterControls()
        {
            GlobalService.Instance.Controls = new Dictionary<string, NotifyUserControl>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.Namespace == "StudyNotes.CustomControl");
            foreach (Type item in types)
            {
                var obj = (NotifyUserControl)Activator.CreateInstance(item);
                GlobalService.Instance.Controls.Add(item.Name, obj);
            }
        }
        public void RegisterTestMethods()
        {
            GlobalService.Instance.Methods = new List<Method>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.Namespace == "StudyNotes.Functions");
            foreach (Type item in types)
            {
                MethodInfo[] methods = item.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    Attribute attribute = method.GetCustomAttribute(typeof(MethodAttribute), false);
                    if (attribute != null)
                    {
                        Method mt = new Method();
                        //特性传递进来的参数
                        object[] temp = ((MethodAttribute)attribute).Params;

                        //复制参数
                        List<object> obj_list = new List<object>();
                        var method_pm = method.GetParameters();
                        mt.Params = new ObservableCollection<KeyValue>();
                        int i = 0;
                        foreach (var obj_pm in temp)
                        {
                            var pm = method_pm[i].ParameterType;
                            if (typeof(IEnumerable).IsAssignableFrom(pm) && !pm.IsArray && pm.IsGenericType)
                            {
                                // 使用 Activator 动态创建该类型的实例
                                IList list = (IList)Activator.CreateInstance(Type.GetType(pm.FullName));
                                int j = 0;
                                foreach (var _ in temp)
                                {
                                    if (j == i)
                                    {
                                        foreach (var __ in (IEnumerable)_)
                                        {
                                            list.Add(__);
                                        }
                                    }
                                    j += 1;
                                }
                                obj_list.Add(list);
                                mt.Params.Add(new KeyValue(method_pm[i].Name, list, pm.FullName));
                            }
                            else
                            {
                                obj_list.Add(obj_pm);
                                mt.Params.Add(new KeyValue(method_pm[i].Name, obj_pm, pm.FullName));
                            }
                            i += 1;
                        }
                        mt.InstanceType = item.FullName;
                        mt.Name = method.Name;
                        mt.Description = "";
                        mt.ReturnType = method.ReturnParameter.ParameterType.FullName;
                        mt.IsStaticClass = item.IsAbstract && item.IsSealed;
                        GlobalService.Instance.Methods.Add(mt);
                    }
                }
            }
        }
    }

}
