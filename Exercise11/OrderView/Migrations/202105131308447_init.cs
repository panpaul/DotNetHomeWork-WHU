namespace OrderView
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Product_ProductName = c.String(maxLength: 2147483647),
                        Product_Price = c.Double(nullable: false),
                        ProductName = c.String(maxLength: 2147483647),
                        Price = c.Double(nullable: false),
                        Amount = c.Int(nullable: false),
                        Order_OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .Index(t => t.Order_OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        Customer = c.String(maxLength: 2147483647),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "Order_OrderID", "dbo.Orders");
            DropIndex("dbo.OrderDetails", new[] { "Order_OrderID" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
