using AutomotrizClient;
using AutomotrizClient.Http;
using Libreria.Datos;
using Libreria.Dominio;
using Newtonsoft.Json;

namespace AutomotrizClient
{
    public partial class Login : Form
    {
        HelperDB oConexion = new HelperDB();
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:5046/login";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var aux  = JsonConvert.DeserializeObject<int>(res);

            int result = oConexion.Login(txtUsuario.Text, txtContrase�a.Text);

            if (result == 1)
            {
                Menu menu = new Menu();
                this.Hide();
                menu.ShowDialog();
            }
            else if (result == 0)
            {
                MessageBox.Show("��Usuario o contrase�a incorrecta!!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}