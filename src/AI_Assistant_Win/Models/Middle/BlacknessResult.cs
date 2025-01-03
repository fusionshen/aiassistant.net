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
                if (!_testNo.Equals(value))
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
                WorkGroup.Equals(param.WorkGroup) &&
                OriginImagePath.Equals(param.OriginImagePath) &&
                RenderImagePath.Equals(param.RenderImagePath) &&
                TestNo.Equals(param.TestNo) &&
                CoilNumber.Equals(param.CoilNumber) &&
                Analyst.Equals(param.Analyst) &&
                Size.Equals(param.Size) &&
                Items.SequenceEqual(param.Items);
            return result;
        }

        public override int GetHashCode()
        {
            return (Id * 397) ^ (Items != null ? Items.GetHashCode() : 0); // 根据你的字段定义哈希码计算逻辑
        }
    }
}
