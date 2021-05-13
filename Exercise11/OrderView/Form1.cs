using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace OrderView
{
    public partial class Form1 : Form
    {
        private readonly OrderContext context = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] orderFields = {"订单号", "客户名"};
            cmbField.DataSource = orderFields;

            context.Orders.Load();
            bindingSource1.DataSource = context.Orders.Local.ToBindingList();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            context.SaveChanges();

            if (tbFilter.Text == "")
            {
                context.Orders.Load();
                bindingSource1.DataSource = context.Orders.Local.ToBindingList();
                return;
            }

            var field = (string) cmbField.SelectedItem;
            var customerName = tbFilter.Text;
            if (!int.TryParse(tbFilter.Text, out var orderId)) orderId = 0;

            ObservableCollection<Order> data;
            switch (field)
            {
                case "订单号":
                    data = new ObservableCollection<Order>(from item in context.Orders
                        where item.OrderID == orderId
                        select item
                    );
                    //context.Orders.Where(o => o.OrderID == orderId).Load();
                    break;
                case "客户名":
                    data = new ObservableCollection<Order>(from item in context.Orders
                        where item.Customer == customerName
                        select item
                    );
                    //context.Orders.Where(o => o.Customer == customerName).Load();
                    break;
                default:
                    return;
            }

            bindingSource1.DataSource = data;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id;
            try
            {
                id = context.Orders.Max(table => table.OrderID) + 1;
            }
            catch (Exception err)
            {
                id = 1;
            }

            var addForm = new Modify(id);
            addForm.ShowDialog(this);
            addForm.Dispose();

            context.Orders.Load();
            bindingSource1.DataSource = context.Orders.Local.ToBindingList();
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

            context.Orders.Load();
            bindingSource1.DataSource = context.Orders.Local.ToBindingList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 1)
            {
                MessageBox.Show(@"请选中要删除的订单", @"错误", MessageBoxButtons.OK);
                return;
            }

            var item = (Order) dataGridView.SelectedRows[0].DataBoundItem;

            var tbr = context.Orders.Find(item.OrderID);
            context.Orders.Remove(tbr);
            context.SaveChanges();

            context.Orders.Load();
            bindingSource1.DataSource = context.Orders.Local.ToBindingList();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var stream = saveFileDialog.OpenFile();

            using var writer = XmlWriter.Create(stream);
            context.Orders.Include("Products").Load();
            var serializer = new DataContractSerializer(context.Orders.GetType());
            serializer.WriteObject(writer, context.Orders);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var stream = openFileDialog.OpenFile();

            var serializer = new DataContractSerializer(typeof(List<Order>));
            var o = (List<Order>) serializer.ReadObject(stream) ?? new List<Order>();

            context.Orders.RemoveRange(context.Orders);
            context.OrderDetails.RemoveRange(context.OrderDetails);
            context.Orders.AddRange(o);
            context.SaveChanges();

            stream.Close();
            context.Orders.Load();
            bindingSource1.DataSource = context.Orders.Local.ToBindingList();
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, $@"Error happened: {e.Context}", MessageBoxButtons.OK);
            e.ThrowException = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            base.OnClosing(e);
            context.Dispose();
        }
    }
}