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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Client
{
    public partial class EditBranch : Form
    {
        IBank proxy;
        List<Bank> banks;
        int branchId;
        public EditBranch(IBank proxy, int branch_id)
        {
            InitializeComponent();
            this.proxy = proxy;
            this.branchId = branch_id;
            banks = proxy.GetAllBanks();

            bank_combo.Items.Clear();

            List<int> banks_ = new List<int>();
            foreach (Bank bank in banks)
            {
                bank_combo.Items.Add(bank.Name);
                banks_.Add(bank.ID);
            }

            Branch branch = proxy.GetBranch(branch_id);

            name_tb.Text = branch.Name;
            adress_tb.Text = branch.Address;

            bank_combo.SelectedIndex = banks_.IndexOf(branch.BankID);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            error_label.Visible = true;
            if (name_tb.Text == "" || adress_tb.Text == "" || bank_combo.Text == "")
            {
                error_label.Text = "All Fields must be Filled";
            }

            Branch branch = new Branch();
            branch.Name = name_tb.Text;
            branch.Address = adress_tb.Text;
            branch.BankID = banks[bank_combo.SelectedIndex].ID;

            proxy.UpdateBranch(branchId, branch);

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
