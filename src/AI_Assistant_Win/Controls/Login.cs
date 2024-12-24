using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models.Response;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class Login : UserControl
    {
        Form form;

        public string Username, Password;

        private readonly ApiBLL apiBLL;
        public Login(Form _form)
        {
            // TODO:国际化
            form = _form;
            InitializeComponent();
            apiBLL = new ApiBLL();
            inputUsername.TextChanged += (s, e) =>
            {
                Username = inputUsername.Text;
            };
            inputPassword.TextChanged += (s, e) =>
            {
                Password = inputPassword.Text;
            };
        }

        public async Task<LoginToken> SignIn()
        {
            return await apiBLL.LoginAsync(Username, Password);
        }
    }
}
