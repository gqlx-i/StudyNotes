using StudyNotes.Common;
using StudyNotes.CustomAttribute;
using StudyNotes.Functions;
using StudyNotes.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace StudyNotes.ViewModel
{
    public class HomeVM : NotifyBase
    {
        public ObservableCollection<Method> Methods { get; set; } = new ObservableCollection<Method>(GlobalService.Instance.Methods);
        private Method _selectedMethod;
        public Method SelectedMethod
        {
            get => _selectedMethod;
            set => SetAndNotify(ref _selectedMethod, value);
        }

        public DelegateCommand ExcuteCommand { get; set; }

        public HomeVM()
        {
            ExcuteCommand = new DelegateCommand(Excute);
        }

        public void Excute(object param)
        {
            try
            {
                object instance;
                Type type = Type.GetType(SelectedMethod.InstanceType);
                if (SelectedMethod.IsStaticClass)
                {
                    instance = null;
                }
                else
                {
                    instance = Activator.CreateInstance(type);
                }
                MethodInfo mt = type.GetMethod(SelectedMethod.Name);

                Type returnType = Type.GetType(SelectedMethod.ReturnType);
                var returnVal = mt.Invoke(instance, SelectedMethod.Params.Select(x=>x.Value).ToArray());
                //var y = Convert.ChangeType(returnVal, returnType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
