using IT_3045C.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace IT_3045C.Forms
{
    public partial class Userfrm : Form
    {
        IT3045CEntities db = new IT3045CEntities();
        User User = new User();
        int UserId = 0;

        public Userfrm()
        {
            InitializeComponent();
        }

        public void ClearData()
        {
            txtUserFirstName.Text = txtUserLastName.Text = txtUserEmail.Text = string.Empty;
            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            UserId = 0;
        }

        public void SetDataInGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = db.Users.ToList<User>();
        }

        private void Userfrm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'iT3045CDataSet.User' table. You can move, or remove it, as needed.
            this.userTableAdapter.Fill(this.iT3045CDataSet.User);
            ClearData();
            SetDataInGridView();
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserFirstName.Text.Trim() == string.Empty || txtUserLastName.Text.Trim() == string.Empty || txtUserEmail.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please fill out all fields", "Invalid Fields");
            }
            else if (!IsValidEmail(txtUserEmail.Text))
            {
                MessageBox.Show("Invalid email address", "Invalid Fields");
            }
            else
            {
                User.UserFirstName = char.ToUpper(txtUserFirstName.Text[0]) + txtUserFirstName.Text.Substring(1).ToLower();
                User.UserLastName = char.ToUpper(txtUserLastName.Text[0]) + txtUserLastName.Text.Substring(1).ToLower();
                User.UserEmail = txtUserEmail.Text;
                if (UserId > 0)
                {
                    db.Entry(User).State = EntityState.Modified;
                }
                else
                {
                    db.Users.Add(User);
                }
                db.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Record Save Successfully");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record ?", "Delete ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Users.Remove(User);
                db.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Record Deleted Successfully");
            }
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell.RowIndex != -1)
            {
                UserId = Convert.ToInt32(dataGridView.CurrentRow.Cells["userIDDataGridViewTextBoxColumn"].Value);
                User = db.Users.Where(x => x.UserID == UserId).FirstOrDefault();
                txtUserFirstName.Text = User.UserFirstName;
                txtUserLastName.Text = User.UserLastName;
                txtUserEmail.Text = User.UserEmail;
            }
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }
    }
}
