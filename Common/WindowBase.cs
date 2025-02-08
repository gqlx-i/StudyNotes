using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StudyNotes.Common
{
    public class WindowBase : Window
    {
        public WindowBase()
        {
            this.Closing += WindowBase_Closing;
        }

        private void WindowBase_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }

    public class NotifyWindow : WindowBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void SetAndNotify<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return;
            }
            property = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UserControlBase : UserControl
    {

    }
    public class NotifyUserControl : UserControlBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void SetAndNotify<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return;
            }
            property = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
