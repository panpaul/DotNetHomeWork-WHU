using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OrderView
{
    public partial class Form1 : Form
    {
        public readonly OrderService OrderService;
        private List<Order> orderResult;

        public Form1()
        {
            InitializeComponent();
            OrderService = new OrderService();

            OrderService.Find(out orderResult);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] orderFields = {"订单号", "客户名"};
            cmbField.DataSource = orderFields;

            bindingSource1.DataSource = orderResult;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            OrderService.Reset();

            if (tbFilter.Text == "")
            {
                UpdateView(true);
                return;
            }

            var field = (string) cmbField.SelectedItem;
            var customerName = tbFilter.Text;
            if (!uint.TryParse(tbFilter.Text, out var orderId)) orderId = 0;

            switch (field)
            {
                case "订单号":
                    OrderService.Where(o => o.OrderId == orderId).Find(out orderResult);
                    break;
                case "客户名":
                    OrderService.Where(o => o.Customer == customerName).Find(out orderResult);
                    break;
                default:
                    OrderService.Find(out orderResult);
                    break;
            }

            UpdateView(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new Modify(OrderService.GetNextKey());
            addForm.ShowDialog(this);
            addForm.Dispose();

            UpdateView(true);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show(@"没有选中订单", @"错误", MessageBoxButtons.OK);
                return;
            }

            var addForm = new Modify((Order) dataGridView.SelectedRows[0].DataBoundItem);
            addForm.ShowDialog(this);
            addForm.Dispose();

            UpdateView(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 1)
            {
                MessageBox.Show(@"请选中要删除的订单", @"错误", MessageBoxButtons.OK);
                return;
            }

            var item = (Order) dataGridView.SelectedRows[0].DataBoundItem;
            OrderService.Where(o => o.OrderId == item.OrderId).Delete();

            UpdateView(true);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var stream = saveFileDialog.OpenFile();
            OrderService.Export(stream);
            stream.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var stream = openFileDialog.OpenFile();
            OrderService.Import(stream);
            stream.Close();
            UpdateView(true);
        }

        private void UpdateView(bool showAll)
        {
            if (showAll) OrderService.Find(out orderResult);
            bindingSource1.DataSource = orderResult;
            bindingSource1.ResetBindings(false);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, $@"Error happened: {e.Context}", MessageBoxButtons.OK);
            e.ThrowException = false;
        }
    }
}