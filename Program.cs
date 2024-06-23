using System;
using System.Collections.Generic;

public abstract class Entity
{
    public int Id { get; set; }
}

public class Customer : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Number { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();

    public void Login()
    {
        Console.WriteLine("Customer logged in.");
    }

    public void Register()
    {
        Console.WriteLine("Customer registered.");
    }

    public void BookTable()
    {
        Console.Write("Enter your name: ");
        Name = Console.ReadLine();

        Console.Write("Enter your email: ");
        Email = Console.ReadLine();

        Console.Write("Enter your phone number: ");
        Number = Console.ReadLine();

        Console.WriteLine("Select number of seats:");
        Console.WriteLine("1. 2 seats");
        Console.WriteLine("2. 4 seats");
        Console.WriteLine("3. 6 seats");
        Console.WriteLine("4. Enter manually");
        Console.Write("Enter your choice: ");
        int seats = 0;

        switch (Console.ReadLine())
        {
            case "1":
                seats = 2;
                break;
            case "2":
                seats = 4;
                break;
            case "3":
                seats = 6;
                break;
            case "4":
                Console.Write("Enter number of seats: ");
                seats = int.Parse(Console.ReadLine());
                break;
            default:
                Console.WriteLine("Invalid choice. Defaulting to 2 seats.");
                seats = 2;
                break;
        }

        Table table = new Table { Id = 1, Number = "A1", Seats = seats, Status = "Booked" };
        Reservation reservation = new Reservation { Id = 1, Customer = this, Table = table, Time = DateTime.Now };

        reservation.Display();
    }

    public void PlaceOrder(Menu menu)
    {
        Order order = new Order { Id = Orders.Count + 1, Customer = this, Status = "Pending", Time = DateTime.Now };
        bool ordering = true;

        while (ordering)
        {
            menu.DisplayMenu();
            Console.WriteLine("Enter the numbers of the items to add to your order (e.g., '1,2,3' or '1 2 3' or '1.2.3') or type 'done' to finish:");
            string input = Console.ReadLine();

            if (input.ToLower() == "done")
            {
                ordering = false;
            }
            else
            {
                var separators = new char[] { ',', '.', ' ' };
                var itemNumbers = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var number in itemNumbers)
                {
                    if (int.TryParse(number, out int itemNumber) && itemNumber > 0 && itemNumber <= menu.Items.Count)
                    {
                        order.Items.Add(menu.Items[itemNumber - 1]);
                        Console.WriteLine($"{menu.Items[itemNumber - 1].Name} added to your order.");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid selection: {number}. Please try again.");
                    }
                }
            }
        }

        Orders.Add(order);

        Console.WriteLine("Order Summary:");
        decimal total = 0;
        foreach (var item in order.Items)
        {
            Console.WriteLine($"{item.Name} - ${item.Price}");
            total += item.Price;
        }
        Console.WriteLine($"Total: ${total}");
    }

    public void Pay(Payment payment)
    {
        Console.WriteLine($"Payment of {payment.Amount} made by {Name}.");
    }
    public void ShowOrders()
    {
        foreach (var order in Orders)
        {
            Console.WriteLine($"Order {order.Id} Summary:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Name} - ${item.Price}");
            }
        }
    }
}

public class Table : Entity
{
    public string Number { get; set; }
    public int Seats { get; set; }
    public string Status { get; set; }
}

public class Payment : Entity
{
    public Customer Customer { get; set; }
    public Order Order { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; }
    public string Status { get; set; }

    public void Create()
    {
        Console.WriteLine("Payment created.");
    }

    public void Update()
    {
        Console.WriteLine("Payment updated.");
    }

    public void PrintReceipt()
    {
        Console.WriteLine($"Receipt for {Amount} printed.");
    }
}

public class Menu : Entity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<Menu> Items { get; set; } = new List<Menu>();

    public void Create(Menu item)
    {
        Items.Add(item);
        Console.WriteLine($"Menu item {item.Name} created.");
    }

    public void Update(int id, Menu updatedItem)
    {
        var item = Items.Find(i => i.Id == id);
        if (item != null)
        {
            item.Name = updatedItem.Name;
            item.Price = updatedItem.Price;
            item.Description = updatedItem.Description;
            Console.WriteLine($"Menu item {id} updated.");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }

    public void Delete(int id)
    {
        var item = Items.Find(i => i.Id == id);
        if (item != null)
        {
            Items.Remove(item);
            Console.WriteLine($"Menu item {id} deleted.");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Menu:");
        for (int i = 0; i < Items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Items[i].Name} - ${Items[i].Price}");
        }
    }
}

public class Kitchen : Entity
{
    public Order Order { get; set; }
    public string Status { get; set; }
    public string Ingredients { get; set; }

    public void ReceiveOrder(Order order)
    {
        Console.WriteLine($"Order {order.Id} received in kitchen.");
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        Console.WriteLine($"Kitchen status updated to {status}.");
    }

    public void CheckIngredients()
    {
        Console.WriteLine("Ingredients checked.");
    }
}

public class Reservation : Entity
{
    public Customer Customer { get; set; }
    public Table Table { get; set; }
    public DateTime Time { get; set; }

    public void Create()
    {
        Console.WriteLine("Reservation created.");
    }

    public void Cancel()
    {
        Console.WriteLine("Reservation cancelled.");
    }

    public void Update()
    {
        Console.WriteLine("Reservation updated.");
    }

    public void Display()
    {
        Console.WriteLine($"Reservation Details:\nCustomer: {Customer.Name}\nEmail: {Customer.Email}\nPhone: {Customer.Number}\nTable: {Table.Number}\nSeats: {Table.Seats}\nTime: {Time}");
    }
}

public class Order : Entity
{
    public Customer Customer { get; set; }
    public List<Menu> Items { get; set; } = new List<Menu>();
    public string Status { get; set; }
    public DateTime Time { get; set; }

    public void Create()
    {
        Console.WriteLine("Order created.");
    }

    public void Update()
    {
        Console.WriteLine("Order updated.");
    }
    public void ProcessOrder()
    {
        Status = "Processed";
        Console.WriteLine("Order processed and ready to be served.");
    }
}

public class Program
{
    static void Main()
    {
        List<Customer> customers = new List<Customer>();
        Menu menu = new Menu();

        // Creating menu items
        menu.Create(new Menu { Id = 1, Name = "Pizza", Price = 9.99M, Description = "Delicious cheese pizza" });
        menu.Create(new Menu { Id = 2, Name = "Pasta", Price = 12.99M, Description = "Creamy Alfredo pasta" });
        menu.Create(new Menu { Id = 3, Name = "Salad", Price = 7.99M, Description = "Fresh garden salad" });
        menu.Create(new Menu { Id = 4, Name = "Water", Price = 1.99M, Description = "Bottled water" });
        menu.Create(new Menu { Id = 5, Name = "Wine", Price = 19.99M, Description = "Red wine" });

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nRestaurant Management System");
            Console.WriteLine("1. Book Table and Show Reservations");
            Console.WriteLine("2. Place Order");
            Console.WriteLine("3. Show and Process Orders");
            Console.WriteLine("4. Calculate and Print Bill");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Customer customer = new Customer();
                    customer.BookTable();
                    customers.Add(customer);
                    Console.WriteLine("Current Reservations:");
                    foreach (var cust in customers)
                    {
                        cust.BookTable();
                    }
                    break;
                case "2":
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("No reservations found. Please book a table first.");
                    }
                    else
                    {
                        Console.WriteLine("Select customer to place order:");
                        for (int i = 0; i < customers.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {customers[i].Name}");
                        }
                        int customerIndex = int.Parse(Console.ReadLine()) - 1;
                        if (customerIndex >= 0 && customerIndex < customers.Count)
                        {
                            customers[customerIndex].PlaceOrder(menu);
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection.");
                        }
                    }
                    break;
                case "3":
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("No orders found.");
                    }
                    else
                    {
                        foreach (var cust in customers)
                        {
                            cust.ShowOrders();
                        }
                        Console.WriteLine("Process orders for customers? (yes/no)");
                        if (Console.ReadLine().ToLower() == "yes")
                        {
                            foreach (var cust in customers)
                            {
                                foreach (var order in cust.Orders)
                                {
                                    order.ProcessOrder();
                                }
                            }
                        }
                    }
                    break;
                case "4":
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("No orders found.");
                    }
                    else
                    {
                        foreach (var cust in customers)
                        {
                            foreach (var order in cust.Orders)
                            {
                                decimal total = 0;
                                Console.WriteLine($"Bill for {cust.Name}:");
                                foreach (var item in order.Items)
                                {
                                    Console.WriteLine($"{item.Name} - ${item.Price}");
                                    total += item.Price;
                                }
                                Console.WriteLine($"Total: ${total}");
                                Payment payment = new Payment { Id = 1, Customer = cust, Order = order, Amount = total, Method = "Credit Card", Status = "Paid" };
                                cust.Pay(payment);
                                payment.PrintReceipt();
                            }
                        }
                    }
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
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