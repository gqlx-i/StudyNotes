using StudyNotes.Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Model
{
    public class ROIData : NotifyBase
    {
        public ROIData(string name)
        {
            _name = name;
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }
        private double _value;
        public double Value
        {
            get => _value;
            set => SetAndNotify(ref _value, value);
        }

    }
    public class HDrawingObjectData
    {
        public HDrawingObject HDrawingObject { get; set; }
        public ERegionMergeMode Mode { get; set; }
        public HDrawingObjectData(HDrawingObject obj, ERegionMergeMode mode)
        {
            HDrawingObject = obj;
            Mode = mode;
        }
    }
    public enum EShape
    {
        Rectangle1,
        Rectangle2,
        Circle,
        Ellipse,
        Line,
    }
    public enum ERegionMergeMode
    {
        Union,
        Difference,
        Intersection,
        XOR
    }
}
