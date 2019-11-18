using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaltedHashing
{
    public partial class Home : Form
    {
        private AccountManager _AccountManager;
        public Home()
        {
            InitializeComponent();
            _AccountManager = new AccountManager();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_AccountManager.Login(LoginUserName.Text, LoginPassword.Text))
                MessageBox.Show("Logged in successfully");
            else
                MessageBox.Show("Wrong credentials");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (_AccountManager.Register(RegisterUserName.Text, RegisterPassword.Text))
                MessageBox.Show("User registered successfully");
            else
                MessageBox.Show("Somthing went wrong.. Please try again");
        }
    }
}
