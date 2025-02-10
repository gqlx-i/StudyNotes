using StudyNotes.Common;
using StudyNotes.CustomAttribute;
using StudyNotes.Functions;
using StudyNotes.Interface;
using StudyNotes.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace StudyNotes.ViewModel
{
    /// <summary>
    /// 主界面
    /// </summary>
    public class HomeVM : NotifyBase
    {
        /// <summary>
        /// 测试方法集合
        /// </summary>
        public ObservableCollection<Method> Methods { get; set; } = new ObservableCollection<Method>(GlobalService.Instance.Methods);
        private Method _selectedMethod;
        /// <summary>
        /// 当前选中方法
        /// </summary>
        public Method SelectedMethod
        {
            get => _selectedMethod;
            set => SetAndNotify(ref _selectedMethod, value);
        }

        /// <summary>
        /// 重置按钮命令的绑定
        /// </summary>
        public DelegateCommand ResetCommand { get; set; }
        /// <summary>
        /// 执行按钮命令的绑定
        /// </summary>
        public DelegateCommand ExcuteCommand { get; set; }
        /// <summary>
        /// 取消按钮命令的绑定
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HomeVM()
        {
            _selectedMethod = Methods[0];
            ResetCommand = new DelegateCommand(Reset);
            ExcuteCommand = new DelegateCommand(Excute);
            CancelCommand = new DelegateCommand(Cancel);
        }

        /// <summary>
        /// 重置函数
        /// </summary>
        /// <param name="param"></param>
        public void Reset(object param)
        {
            GlobalService.Instance.CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="param"></param>
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

        /// <summary>
        /// 取消函数
        /// </summary>
        /// <param name="param"></param>
        public void Cancel(object param)
        {
            GlobalService.Instance.CancellationTokenSource.Cancel();
            Reset(null);
        }
    }
}
