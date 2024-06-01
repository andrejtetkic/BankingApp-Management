using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class EditBank : Form
    {
        IBank proxy;
        int bankId;
        Bank bank;
        public EditBank(IBank proxy, int bank_id)
        {
            InitializeComponent();
            this.proxy = proxy;
            this.bankId = bank_id;

            bank = proxy.GetBank(bank_id);

            bank_name.Text = bank.Name;
            city_tb.Text = bank.City;
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void create_btn_Click(object sender, EventArgs e)
        {
            error_label.Visible = true;

            if (bank_name.Text == "" || city_tb.Text == "")
            {
                error_label.Text = "All fields must be Filled";
                return;
            }


            Bank up_bank = new Bank();
            up_bank.Name = bank_name.Text;
            up_bank.City = city_tb.Text;
            up_bank.MainBranch = bank.MainBranch;

            proxy.UpdateBank(bankId, up_bank);

            this.Close();
        }
    }
}
