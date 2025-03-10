﻿using StudyNotes.Common;
using StudyNotes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudyNotes.View
{
    /// <summary>
    /// HelperWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HelperWindow : NotifyWindow
    {
        private static readonly Lazy<HelperWindow> _instance = new Lazy<HelperWindow>(() => new HelperWindow());

        public static HelperWindow Instance { get; set; } = _instance.Value;

        private HelperWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private NotifyUserControl _currentControl;
        public NotifyUserControl CurrentControl
        {
            get => _currentControl;
            set => SetAndNotify(ref _currentControl, value);
        }

        public void Show(string controlName)
        {
            GlobalService.Instance.Controls.TryGetValue(controlName, out NotifyUserControl control);
            CurrentControl = control;
            Show();
        }
    }
}
