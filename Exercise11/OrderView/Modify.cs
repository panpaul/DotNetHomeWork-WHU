using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OrderView
{
    public partial class Modify : Form
    {
        private readonly bool isModify;
        private readonly Order order;

        public Modify()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     For Modify
        /// </summary>
        /// <param name="order"> the order to modify </param>
        public Modify(Order order) : this()
        {
            this.order = order;
            isModify = true;
        }

        /// <summary>
        ///     For Add
        /// </summary>
        /// <param name="id"> new order key </param>
        public Modify(int id) : this()
        {
            order = new Order {OrderID = id, Products = new List<OrderDetails>()};
            isModify = false;
        }

        private void Modify_Load(object sender, EventArgs e)
        {
            lbOrderId.DataBindings.Add("Text", order, "OrderID");
            tbCustomer.DataBindings.Add("Text", order, "Customer");
            productsBinding.DataSource = order.Products;


            if (!isModify)
            {
                Text = @"添加订单";
                lbOrderTime.Text = @"(不可用)";
            }
            else
            {
                Text = @"修改订单";
                lbOrderTime.DataBindings.Add("Text", order, "CreatedAt");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbCustomer.Text == "")
            {
                MessageBox.Show(@"客户名为空", @"错误", MessageBoxButtons.OK);
                return;
            }

            if (order.Products is null || order.Products.Count == 0)
            {
                MessageBox.Show(@"订单列表为空", @"错误", MessageBoxButtons.OK);
                return;
            }

            if (order.Products.Exists(p =>
                    string.IsNullOrEmpty(p.ProductName)
                    || string.IsNullOrWhiteSpace(p.ProductName) // 商品名不空
                    || p.Price == 0 // 价格不为0
                    || p.Amount == 0 // 总量不为0
            ))
            {
                MessageBox.Show(@"存在不合法的商品信息", @"错误", MessageBoxButtons.OK);
                return;
            }

            if (order.Products
                .GroupBy(p => p.ProductName)
                .Count(g => g.Count() > 1) != 0)
            {
                MessageBox.Show(@"存在重复的商品信息", @"错误", MessageBoxButtons.OK);
                return;
            }

            if (!isModify) order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;


            using var db = new OrderContext();
            if (isModify)
                db.Entry(db.Orders.Find(order.OrderID)).CurrentValues.SetValues(order);
            else
                db.Orders.Add(order);


            db.SaveChanges();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void productView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, $@"Error happened: {e.Context}", MessageBoxButtons.OK);
            e.ThrowException = false;
        }
    }
}