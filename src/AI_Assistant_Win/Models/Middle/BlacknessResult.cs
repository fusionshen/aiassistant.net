using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AI_Assistant_Win.Models.Middle
{
    public class BlacknessResult : INotifyPropertyChanged
    {
        private int _id = 0;
        public int Id
        {
            get { return _id; }
            set
            {
                if (!_id.Equals(value))
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _testNo = string.Empty;
        public string TestNo
        {
            get { return _testNo; }
            set
            {
                if (string.IsNullOrEmpty(_testNo) || !_testNo.Equals(value))
                {
                    _testNo = value;
                    OnPropertyChanged(nameof(TestNo));
                }
            }
        }

        private string _coilNumber = string.Empty;
        public string CoilNumber
        {
            get { return _coilNumber; }
            set
            {
                if (!_coilNumber.Equals(value))
                {
                    _coilNumber = value;
                    OnPropertyChanged(nameof(CoilNumber));
                }
            }
        }

        private string _size = string.Empty;
        public string Size
        {
            get { return _size; }
            set
            {
                if (!_size.Equals(value))
                {
                    _size = value;
                    OnPropertyChanged(nameof(Size));
                }
            }
        }
        private string _originImagePath = string.Empty;
        public string OriginImagePath
        {
            get { return _originImagePath; }
            set
            {
                if (!_originImagePath.Equals(value))
                {
                    _originImagePath = value;
                    OnPropertyChanged(nameof(OriginImagePath));
                }
            }
        }

        private string _renderImagePath = string.Empty;
        public string RenderImagePath
        {
            get { return _renderImagePath; }
            set
            {
                if (!_renderImagePath.Equals(value))
                {
                    _renderImagePath = value;
                    OnPropertyChanged(nameof(RenderImagePath));
                }
            }
        }

        private string _workGroup = string.Empty;
        public string WorkGroup
        {
            get { return _workGroup; }
            set
            {
                if (!_workGroup.Equals(value))
                {
                    _workGroup = value;
                    OnPropertyChanged(nameof(WorkGroup));
                }
            }
        }

        private string _analyst = string.Empty;
        public string Analyst
        {
            get { return _analyst; }
            set
            {
                if (!_analyst.Equals(value))
                {
                    _analyst = value;
                    OnPropertyChanged(nameof(Analyst));
                }
            }
        }

        private List<Blackness> _items = [];

        public List<Blackness> Items
        {
            get { return _items; }
            set
            {
                if (!_items.SequenceEqual(value))
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public bool IsUploaded { get; set; } = false;
        /// <summary>
        /// 第几次试验
        /// </summary>
        public int? Nth { get; set; } = 1;

        private CalculateScale calculateScale = new();

        /// <summary>
        /// 当时比例尺和当前比例尺要分清楚，这里我偏向作为当时比例尺使用
        /// 每一次变化都要传递给前端，不然在上一条下一条会出问题
        /// </summary>
        public CalculateScale CalculateScale
        {
            get { return calculateScale; }
            set
            {
                calculateScale = value;
                OnPropertyChanged(nameof(CalculateScale));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var param = (BlacknessResult)obj;
            var result = Id.Equals(param.Id) &&
                !string.IsNullOrEmpty(WorkGroup) && WorkGroup.Equals(param.WorkGroup) &&
                !string.IsNullOrEmpty(OriginImagePath) && OriginImagePath.Equals(param.OriginImagePath) &&
                !string.IsNullOrEmpty(RenderImagePath) && RenderImagePath.Equals(param.RenderImagePath) &&
                !string.IsNullOrEmpty(TestNo) && TestNo.Equals(param.TestNo) &&
                !string.IsNullOrEmpty(CoilNumber) && CoilNumber.Equals(param.CoilNumber) &&
                !string.IsNullOrEmpty(Analyst) && Analyst.Equals(param.Analyst) &&
                !string.IsNullOrEmpty(Size) && Size.Equals(param.Size) &&
                Items.SequenceEqual(param.Items);
            return result;
        }

        public override int GetHashCode()
        {
            return (Id * 397) ^ (Items != null ? Items.GetHashCode() : 0); // 根据你的字段定义哈希码计算逻辑
        }
    }
}
