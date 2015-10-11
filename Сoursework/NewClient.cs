using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseDAL;
using System.Text.RegularExpressions;

namespace Сoursework
{
    public partial class NewClient : Form
    {
        public NewClient()
        {
            InitializeComponent();
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //if (IsValidEmail(textBox3.Text))
                if (IsValidEmail(textBox3.Text))
            {
                button1.Visible = true;
            }
            else
            {
                button1.Visible = false;
            }
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                valid = check.IsMatch(email);
            }
            return valid;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((NewGood)this.Owner).NewClient = null;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((NewGood)this.Owner).NewClient = new Client();
            ((NewGood)this.Owner).NewClient.FirstName = maskedTextBox1.Text;
            ((NewGood)this.Owner).NewClient.LastName = maskedTextBox2.Text;
            ((NewGood)this.Owner).NewClient.Email = textBox3.Text;
            ((NewGood)this.Owner).NewClient.ClientID = -1;
            ((NewGood)this.Owner).ComboBox1.Visible = false;
            ((NewGood)this.Owner).Button1.Visible = false;
            ((NewGood)this.Owner).Label8.Visible = true;
            ((NewGood)this.Owner).Label8.Text = string.Format("{0} {1}", maskedTextBox1.Text, maskedTextBox2.Text);
            ((NewGood)this.Owner).TextBox1.Visible = true;
            ((NewGood)this.Owner).DateTimePicker1.Visible = true;
            ((NewGood)this.Owner).DateTimePicker2.Visible = true;
            ((NewGood)this.Owner).NumericUpDown1.Visible = true;
            ((NewGood)this.Owner).NumericUpDown2.Visible = true;
            this.Close();
        }

    }
}
