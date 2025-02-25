using AI_Assistant_Win.Models.Middle;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class CalculatedStep : UserControl
    {
        Form form;

        public CalculatedStep(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        public void SetTracerDetails(ScaleAccuracyTracerHistory tracerHistory, int current)
        {
            stepsCalculate.Current = current;
            labelMPE.Text = $"最大允许误差(MPE, Maximum Permissible Error)是仪器或测量系统在特定条件下允许的最大误差值。编号[{tracerHistory.Scale.Id}]共测量样本数为{tracerHistory.MPEList.Count}，最大误差值为{tracerHistory.Tracer.MPE:F2}mm。";
            labelSame.Text = $"{tracerHistory.Tracer.MeasuredLength}mm量块重复测量{tracerHistory.MethodList.Count}次：{string.Join(",", tracerHistory.MethodList.Select(t => $"{t.CalculatedLength:F2}mm"))}。";
            labelAverage.Text = $"{tracerHistory.Tracer.Average:F2}mm";
            labelStandardDiviation.Text = tracerHistory.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"σ≈{tracerHistory.Tracer.StandardDeviation:F3}mm";
            labelStandardError.Text = tracerHistory.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{tracerHistory.Tracer.StandardError:F3}mm";
            labelUncertainty.Text = tracerHistory.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{tracerHistory.Tracer.Uncertainty:F3}mm";
            labelDistribution.Text = tracerHistory.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"μ±1σ 内: {tracerHistory.Tracer.Pct1Sigma:P2} (理论68.27%)\n" +
                $"μ±2σ 内: {tracerHistory.Tracer.Pct2Sigma:P2} (理论95.45%)\n" +
                $"μ±3σ 内: {tracerHistory.Tracer.Pct3Sigma:P2} (理论99.73%)";
            labelConfidence.Text = tracerHistory.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{tracerHistory.Tracer.DisplayName}";
        }
    }
}
