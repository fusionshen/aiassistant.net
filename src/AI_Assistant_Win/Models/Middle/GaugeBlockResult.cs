using System.ComponentModel;

namespace AI_Assistant_Win.Models.Middle
{
    public class GaugeBlockResult : INotifyPropertyChanged
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

        private string _postion = string.Empty;
        public string Position
        {
            get { return _postion; }
            set
            {
                if (!_postion.Equals(value))
                {
                    _postion = value;
                    OnPropertyChanged(nameof(Position));
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

        private GaugeBlock _item;
        public GaugeBlock Item
        {
            get { return _item; }
            set
            {
                if (_item == null || !_item.Equals(value))
                {
                    _item = value;
                    OnPropertyChanged(nameof(Item));
                }
            }
        }

        public bool IsUploaded { get; set; } = false;

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
            var param = (GaugeBlockResult)obj;
            var result = Id.Equals(param.Id) &&
                !string.IsNullOrEmpty(WorkGroup) && WorkGroup.Equals(param.WorkGroup) &&
                !string.IsNullOrEmpty(OriginImagePath) && OriginImagePath.Equals(param.OriginImagePath) &&
                !string.IsNullOrEmpty(RenderImagePath) && RenderImagePath.Equals(param.RenderImagePath) &&
                !string.IsNullOrEmpty(TestNo) && TestNo.Equals(param.TestNo) &&
                !string.IsNullOrEmpty(CoilNumber) && CoilNumber.Equals(param.CoilNumber) &&
                !string.IsNullOrEmpty(Analyst) && Analyst.Equals(param.Analyst) &&
                !string.IsNullOrEmpty(Position) && Position.Equals(param.Position) &&
                Item != null && Item.Equals(param.Item);
            return result;
        }

        public override int GetHashCode()
        {
            return (Id * 397) ^ (Item != null ? Item.GetHashCode() : 0); // 根据你的字段定义哈希码计算逻辑
        }
    }
}
