using IT_3045C.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT_3045C.Forms
{
    public partial class Checkoutfrm : Form
    {
        IT3045CEntities db = new IT3045CEntities();
        User User = new User();
        Book Book = new Book();
        Checkout Checkout = new Checkout();
        int UserId = 0;
        int BookId = 0;
        int CheckoutId = 0;

        public Checkoutfrm()
        {
            InitializeComponent();
        }

        public void ClearData()
        {
            txtUserFirstName.Text = txtUserLastName.Text = txtBookTitle.Text = txtBookAuthor.Text = txtCheckoutID.Text = string.Empty;
            btnReturn.Enabled = false;
            UserId = 0;
            BookId = 0;
            CheckoutId = 0;
        }

        public void SetDataInGridView()
        {
            dataGridViewUser.AutoGenerateColumns = false;
            dataGridViewUser.DataSource = db.Users.ToList<User>();
            dataGridViewBook.AutoGenerateColumns = false;
            dataGridViewBook.DataSource = db.Books.ToList<Book>();
            dataGridViewCheckout.AutoGenerateColumns = false;
            dataGridViewCheckout.DataSource = db.Checkouts.ToList<Checkout>();
        }

        private void Checkout_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'iT3045CDataSet.Checkout' table. You can move, or remove it, as needed.
            this.checkoutTableAdapter.Fill(this.iT3045CDataSet.Checkout);
            // TODO: This line of code loads data into the 'iT3045CDataSet.User' table. You can move, or remove it, as needed.
            this.userTableAdapter.Fill(this.iT3045CDataSet.User);
            // TODO: This line of code loads data into the 'iT3045CDataSet.Book' table. You can move, or remove it, as needed.
            this.bookTableAdapter.Fill(this.iT3045CDataSet.Book);
            ClearData();
            SetDataInGridView();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if(UserId > 0 && BookId > 0)
            {
                Checkout.BookID = BookId;
                Checkout.UserID = UserId;
                Book = db.Books.Where(x => x.BookID == BookId).FirstOrDefault();
                if (Book.BookCount > 0)
                {
                    db.Checkouts.Add(Checkout);
                    Book.BookCount -= 1;
                    db.Entry(Book).State = EntityState.Modified;
                    db.SaveChanges();
                    ClearData();
                    SetDataInGridView();
                    MessageBox.Show("Record Save Successfully");
                }
                else
                {
                    MessageBox.Show("Book not available");
                    ClearData();
                    SetDataInGridView();
                }           
            }
        }

        private void dataGridViewBook_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewBook.CurrentCell.RowIndex != -1)
            {
                BookId = Convert.ToInt32(dataGridViewBook.CurrentRow.Cells["bookIDDataGridViewTextBoxColumn1"].Value);
                Book = db.Books.Where(x => x.BookID == BookId).FirstOrDefault();
                txtBookTitle.Text = Book.BookTitle;
                txtBookAuthor.Text = Book.BookAuthor;
            }
        }

        private void dataGridViewUser_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewUser.CurrentCell.RowIndex != -1)
            {
                UserId = Convert.ToInt32(dataGridViewUser.CurrentRow.Cells["userIDDataGridViewTextBoxColumn1"].Value);
                User = db.Users.Where(x => x.UserID == UserId).FirstOrDefault();
                txtUserFirstName.Text = User.UserFirstName;
                txtUserLastName.Text = User.UserLastName;
            }
        }

        private void dataGridViewCheckout_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewCheckout.CurrentCell.RowIndex != -1)
            {
                CheckoutId = Convert.ToInt32(dataGridViewCheckout.CurrentRow.Cells["checkoutIDDataGridViewTextBoxColumn"].Value);
                Checkout = db.Checkouts.Where(x => x.CheckoutID == CheckoutId).FirstOrDefault();
                txtCheckoutID.Text = Checkout.CheckoutID.ToString();
            }
            btnReturn.Enabled = true;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to return this book ?", "Return ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Book = db.Books.Where(x => x.BookID == Checkout.BookID).FirstOrDefault();
                Book.BookCount += 1;
                db.Entry(Book).State = EntityState.Modified;
                db.Checkouts.Remove(Checkout);
                db.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Book Returned Successfully");
            }
        }
    }
}
