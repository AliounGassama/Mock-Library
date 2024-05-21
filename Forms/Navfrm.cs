using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT_3045C.Forms
{
    public partial class Navfrm : Form
    {
        public Navfrm()
        {
            InitializeComponent();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Userfrm frm = new Userfrm();
            frm.ShowDialog();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            Bookfrm frm = new Bookfrm();
            frm.ShowDialog();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Checkoutfrm frm = new Checkoutfrm();
            frm.ShowDialog();
        }
    }
}
