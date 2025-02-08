using StudyNotes.Common;
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

namespace StudyNotes.CustomControl
{
    /// <summary>
    /// PathPreviewControl.xaml 的交互逻辑
    /// </summary>
    public partial class PathPreviewControl : NotifyUserControl
    {
        private Geometry _pathData;
        public Geometry PathData
        {
            get => _pathData;
            set => SetAndNotify(ref _pathData, value);
        }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetAndNotify(ref _text, value);
        }
        private bool _isOK;
        public bool IsOK
        {
            get => _isOK;
            set => SetAndNotify(ref _isOK, value);
        }

        public PathPreviewControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // 尝试解析Path数据
                PathData = Geometry.Parse(Text);
                IsOK = true;
            }
            catch
            {
                IsOK = false;
                // 解析失败时不更新Path
            }
        }
    }
}
