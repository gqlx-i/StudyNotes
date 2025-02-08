using StudyNotes.Common;
using StudyNotes.CustomAttribute;
using StudyNotes.CustomControl;
using StudyNotes.View;
using StudyNotes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Functions
{
    public class WPFTool
    {
        public static HelperWindow HelperWindow { get; set; }= new HelperWindow();

        /// <summary>
        /// path路径实时绘图
        /// </summary>
        /// <param name="controlName"></param>
        [Method(nameof(PathPreviewControl))]
        public void PathPreview(string controlName)
        {
            HelperWindow.Show(controlName);
        }

        /// <summary>
        /// ROI绘制(在HalconSmartWindow上利用Canvas)
        /// </summary>
        /// <param name="controlName"></param>
        [Method(nameof(ROICanvasControl))]
        public void ROICanvas(string controlName)
        {
            HelperWindow.Show(controlName);
        }
    }
}
