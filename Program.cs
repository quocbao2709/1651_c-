using System;
using System.Collections.Generic;



public class Person
{
    // Encapsulation dữ liệu của đối tượng an toàn khỏi sự truy cập và thay đổi từ bên ngoài
    public int Id { get; protected set; }
    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public string Phone { get; protected set; }
    // protected, cho thay đổi ở trong lớp person hoặc lớp được kế thừa lớp person

    public Person(int id, string name, string email, string phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Tên: {Name}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Số điện thoại: {Phone}");
    }
    // Polymorphism

}
public class Customer : Person
{
    public List<Order> Orders { get; private set; }

    public Customer(int id, string name, string email, string phone)
        : base(id, name, email, phone)
    {
        Orders = new List<Order>();
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
    }

    //public void ShowOrders()
    //{
    //    Console.WriteLine($"Đơn hàng của {Name}:");
    //    foreach (var order in Orders)
    //    {
    //        Console.WriteLine($"- {order}");
    //    }
    //}
    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine("Đơn hàng:");
        foreach (var order in Orders)
        {
            order.DisplayOrder();
        }
    }
}
public class Staff : Person
{
    public string Position { get; private set; }

    public Staff(int id, string name, string email, string phone, string position)
        : base(id, name, email, phone)
    {
        Position = position;
    }
    public void CookOrder(Order order)
    {
        order.UpdateStatus("Cooking");
        Console.WriteLine($"Staff is cooking Order ID: {order.Id}");
    }

    public void ServeOrder(Order order)
    {
        order.UpdateStatus("Served");
        Console.WriteLine($"chef has served Order ID: {order.Id}");
    }
    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Position: {Position}");
    }
}
public class Table
{
    public int Number { get; private set; }
    public int Seats { get; private set; }
    public string Status { get; private set; }

    public Table(int number, int seats, string status)
    {
        Number = number;
        Seats = seats;
        Status = status;
    }

    public void BookTable(Customer customer)
    {
        Status = "Đã đặt";
        Console.WriteLine($"Khách hàng {customer.Name} đã đặt bàn số {Number}.");
    }
}
public class Order
{
    public int Id { get; private set; }
    public List<MenuItem> Items { get; private set; }
    public string Status { get; private set; }

    public Order(int id)
    {
        Id = id;
        Items = new List<MenuItem>();
        Status = "Pending";
    }

    public void AddItem(MenuItem item)
    {
        Items.Add(item);
    }
    public void UpdateStatus(string status)
    {
        Status = status;
    }
    public void DisplayOrder()
    {
        Console.WriteLine($"Order ID: {Id}");
        Console.WriteLine("Items:");
        foreach (var item in Items)
        {
            Console.WriteLine($"- {item.Name}");
        }
        Console.WriteLine($"Status: {Status}");
    }
}
public class Menu
{
    private static Menu _instance;
    public List<MenuItem> Items { get; private set; }

    private Menu()
    {
        Items = new List<MenuItem>();
    }
    // Singleton Pattern
    public static Menu Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Menu();
            }
            return _instance;
        }
    }

    public void AddItem(MenuItemFactory factory)
    {
        Items.Add(factory.CreateMenuItem());
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Thực đơn:");
        for (int i = 0; i < Items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Price} VND");
        }
    }
}

public class MenuItem
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public MenuItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}
public abstract class MenuItemFactory
{
    public abstract MenuItem CreateMenuItem();
}
public class PizzaFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new MenuItem("Pizza", 100000);
    }
}

public class PastaFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new MenuItem("Pasta", 80000);
    }
}

public class SaladFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new MenuItem("Salad", 50000);
    }
}

public class DrinkFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new MenuItem("Nước", 10000);
    }
}



public class Program
    {
        static void Main()
        {
            List<Customer> customers = new List<Customer>();
            List<Table> tables = new List<Table>();
            List<Staff> staffs = new List<Staff>();
            Menu menu = Menu.Instance;


                menu.AddItem(new PizzaFactory());
                menu.AddItem(new PastaFactory());
                menu.AddItem(new SaladFactory());
                menu.AddItem(new DrinkFactory());


            tables.Add(new Table(1, 2, "Trống"));
            tables.Add(new Table(2, 4, "Trống"));
            tables.Add(new Table(3, 6, "Trống"));

            staffs.Add(new Staff(1, "Nguyễn Văn tuan", "abc.com", "0123456789", "Phục vụ"));
            staffs.Add(new Staff(2, "nguyen thi kim", "cba.com", "0987654321", "Đầu bếp"));

            bool exit = false;
            while (!exit)
            {
            Console.WriteLine("\nHệ thống quản lý nhà hàng");
            Console.WriteLine("1. Đặt bàn");
            Console.WriteLine("2. Hiển thị đặt chỗ");
            Console.WriteLine("3. Đặt món");
            Console.WriteLine("4. Chế biến món ăn");
            Console.WriteLine("5. Đem món ra bàn");
            Console.WriteLine("6. Hiển thị các đơn hàng");
            Console.WriteLine("7. Thoát");
            Console.Write("Nhập lựa chọn của bạn: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Nhập tên của bạn: ");
                        string name = Console.ReadLine();
                        Console.Write("Nhập email của bạn: ");
                        string email = Console.ReadLine();
                        Console.Write("Nhập số điện thoại của bạn: ");
                        string phone = Console.ReadLine();

                        Customer customer = new Customer(customers.Count + 1, name, email, phone);
                        customers.Add(customer);

                        Console.WriteLine("Chọn bàn để đặt:");
                        for (int i = 0; i < tables.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. Bàn số {tables[i].Number} - {tables[i].Seats} chỗ - {tables[i].Status}");
                        }
                        Console.Write("Nhập số thứ tự của bàn: ");
                        int tableIndex = int.Parse(Console.ReadLine()) - 1;

                        if (tableIndex >= 0 && tableIndex < tables.Count)
                        {
                            tables[tableIndex].BookTable(customer);
                        }
                        else
                        {
                            Console.WriteLine("Lựa chọn không hợp lệ.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Danh sách đặt chỗ:");
                        foreach (var table in tables)
                        {
                            if (table.Status == "Đã đặt")
                            {
                                Console.WriteLine($"Bàn số {table.Number} - {table.Seats} chỗ - {table.Status}");
                            }
                        }
                        break;

                    case "3":
                        if (customers.Count == 0)
                        {
                            Console.WriteLine("Không có khách hàng nào.");
                            break;
                        }

                        Console.WriteLine("Chọn khách hàng để đặt món:");
                        for (int i = 0; i < customers.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {customers[i].Name}");
                        }
                        Console.Write("Nhập số thứ tự của khách hàng: ");
                        int customerIndex = int.Parse(Console.ReadLine()) - 1;

                        if (customerIndex >= 0 && customerIndex < customers.Count)
                        {
                            Customer selectedCustomer = customers[customerIndex];
                            Order newOrder = new Order(selectedCustomer.Orders.Count + 1);

                            bool ordering = true;
                            while (ordering)
                            {
                                menu.DisplayMenu();
                                Console.Write("Nhập số thứ tự của món để thêm vào đơn hàng (hoặc gõ 'done' để hoàn tất): ");
                                string input = Console.ReadLine();

                                if (input.ToLower() == "done")
                                {
                                    ordering = false;
                                    selectedCustomer.AddOrder(newOrder);
                                }
                                else
                                {
                                    int itemIndex = int.Parse(input) - 1;
                                    if (itemIndex >= 0 && itemIndex < menu.Items.Count)
                                    {
                                        newOrder.AddItem(menu.Items[itemIndex]);
                                        Console.WriteLine($"Đã thêm {menu.Items[itemIndex].Name} vào đơn hàng.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Lựa chọn không hợp lệ.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Lựa chọn không hợp lệ.");
                        }
                        break;

                    case "4":
                        if (customers.Count == 0)
                        {
                            Console.WriteLine("Không có khách hàng nào.");
                            break;  
                        }
                            foreach (var customer1 in customers)
                            {   
                                foreach (var order in customer1.Orders)
                                {

                                    if (order.Status == "Pending")
                                    {
                                        staffs[0].CookOrder(order); // Assuming the first staff cooks the order
                                        order.UpdateStatus("Cooking");
                                    }
                                }
                            }
                            Console.WriteLine("Tất cả các món ăn đã được chế biến.");
                        
                        break;

                    case "5":
                        if (customers.Count == 0)
                        {
                            Console.WriteLine("Không có khách hàng nào.");
                            break;
                        }

                        foreach (var customer1 in customers)
                        {
                            foreach (var order in customer1.Orders)
                            {
                                if (order.Status == "Cooking")
                                {
                                    staffs[0].ServeOrder(order);// Assuming the first staff serves the order
                                    order.UpdateStatus("Served");
                                }
                            }
                        }
                        Console.WriteLine("Tất cả các món ăn đã được phục vụ.");
                        break;

                    case "6":
                        Console.WriteLine("Danh sách đơn hàng:");
                        foreach (var cust in customers)
                        {
                            cust.DisplayInfo();
                        }
                        break;
                    case "7":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }
    }


//1. đặt bàn bao gồm có tên , số, email, số lượng bàn ghế
//2.chọn món trong menu, được chọn nhiều món, có thể gọi thêm nước hoặc rượu. được gọi nhiều lần
//3. bếp nhận món và ra món
//4. khách ra về tính bill ,


//---------------
//1 đặt bàn - show ra những người đã đặt bàn
//2 gọi món ăn trong menu, chọn món các kiểu ....
//3 show ra các món ăn đã được gọi và có tùy chọn chế bến món ăn xong đưa ra cho khách và thoát .
//4 tính bill in ra những gì đã được gọi	