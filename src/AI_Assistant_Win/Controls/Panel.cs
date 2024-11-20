using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class Panel : UserControl
    {
        private readonly Overview form;
        public Panel(Overview _form)
        {
            form = _form;
            InitializeComponent();
            button4.Click += Button_Click;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            form.OpenPage("VirtualPanel");
        }
    }
}