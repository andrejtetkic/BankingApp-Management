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
    public partial class NewBranch : Form
    {
        IBank proxy;
        List<Bank> banks;
        public NewBranch(IBank proxy)
        {
            InitializeComponent();
            this.proxy = proxy;

            banks = proxy.GetAllBanks();

            bank_combo.Items.Clear();
            
            foreach (Bank bank in banks)
            {
                bank_combo.Items.Add(bank.Name);
            }

            bank_combo.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            error_label.Visible = true;
            if(name_tb.Text == "" || adress_tb.Text == "" || bank_combo.Text == "")
            {
                error_label.Text = "All Fields must be Filled";
            }
            
            Branch branch = new Branch();
            branch.Name = name_tb.Text;
            branch.Address = adress_tb.Text;
            branch.BankID = banks[bank_combo.SelectedIndex].ID;

            proxy.CreateBranch(branch);

            this.Close();
        }
    }
}
