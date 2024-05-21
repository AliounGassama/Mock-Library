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
    public partial class Bookfrm : Form
    {

        IT3045CEntities db = new IT3045CEntities();
        Book Book = new Book();
        int BookId = 0;

        public Bookfrm()
        {
            InitializeComponent();
        }

        public void ClearData()
        {
            txtBookTitle.Text = txtBookAuthor.Text = txtBookCount.Text = string.Empty;
            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            BookId = 0;
        }

        public void SetDataInGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = db.Books.ToList<Book>();
        }

        private void Bookfrm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'iT3045CDataSet.User' table. You can move, or remove it, as needed.
            this.bookTableAdapter.Fill(this.iT3045CDataSet.Book);
            ClearData();
            SetDataInGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtBookTitle.Text.Trim() == string.Empty || txtBookAuthor.Text.Trim() == string.Empty || txtBookCount.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please fill out all fields", "Invalid Fields");
            }
            else if (!int.TryParse(txtBookCount.Text, out int bookCount))
            {
                MessageBox.Show("Must provide a number for 'Number of Books' field", "Invalid Fields");
            }
            else
            {
                Book.BookTitle = txtBookTitle.Text;
                Book.BookAuthor = txtBookAuthor.Text;
                Book.BookCount = bookCount;
                if (BookId > 0)
                {
                    db.Entry(Book).State = EntityState.Modified;
                }
                else
                {
                    db.Books.Add(Book);
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
                db.Books.Remove(Book);
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
                BookId = Convert.ToInt32(dataGridView.CurrentRow.Cells["bookIDDataGridViewTextBoxColumn"].Value);
                Book = db.Books.Where(x => x.BookID == BookId).FirstOrDefault();
                txtBookTitle.Text = Book.BookTitle;
                txtBookAuthor.Text = Book.BookAuthor;
                txtBookCount.Text = Book.BookCount.ToString();
            }
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }
    }
}
