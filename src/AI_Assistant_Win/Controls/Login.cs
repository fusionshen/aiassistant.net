using AI_Assistant_Win.Business;
using AI_Assistant_Win.Utils;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class Login : UserControl
    {
        Form form;

        public string Username, Password;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();
        public Login(Form _form)
        {
            form = _form;
            InitializeComponent();
            inputUsername.TextChanged += (s, e) =>
            {
                Username = inputUsername.Text;
            };
            inputPassword.TextChanged += (s, e) =>
            {
                Password = inputPassword.Text;
            };
        }

        public async Task<bool> SignIn()
        {
            return await apiBLL.LoginAsync(Username, Password);
        }
    }
}
