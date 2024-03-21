using System;
using System.Collections.Generic;

public class Item
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Code { get; set; } // Unique identifier for each item

    public Item(string name, double price, string code)
    {
        Name = name;
        Price = price;
        Code = code;
    }
}

public class Inventory
{
    private Dictionary<string, Item> items = new Dictionary<string, Item>();

    public void AddItem(string code, Item item)
    {
        items.Add(code, item);
    }

    public Item GetItem(string code)
    {
        if (items.ContainsKey(code))
        {
            return items[code];
        }
        else
        {
            return null;
        }
    }

    public Dictionary<string, Item> GetAllItems()
    {
        return items;
    }
}

public class Bank
{
    public double Balance { get; private set; }

    public Bank(double balance)
    {
        Balance = balance;
    }

    public void AddMoney(double amount)
    {
        Balance += amount;
    }

    public bool DeductMoney(double amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class VendingMachine
{
    private Inventory inventory;
    private Bank bank;

    public VendingMachine(Inventory inventory, Bank bank)
    {
        this.inventory = inventory;
        this.bank = bank;
    }

    public void DisplayInventory()
    {
        Console.WriteLine("Items available:");
        foreach (var item in inventory.GetAllItems())
        {
            Console.WriteLine($"{item.Key}: {item.Value.Name} - ${item.Value.Price}");
        }
    }

    public void DisplayBalance()
    {
        Console.WriteLine($"Your balance: ${bank.Balance}");
    }

    public bool BuyItem(string code, User user)
    {
        Item item = inventory.GetItem(code);
        if (item != null)
        {
            if (bank.DeductMoney(item.Price))
            {
                Console.WriteLine($"You bought: {item.Name}");
                user.AddToInventory(item);
                return true;
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid item code!");
            return false;
        }
    }
}

public class User
{
    private VendingMachine vendingMachine;
    private List<Item> purchasedItems;

    public User(VendingMachine vendingMachine)
    {
        this.vendingMachine = vendingMachine;
        purchasedItems = new List<Item>();
    }

    public void DisplayMenu()
    {
        Console.WriteLine("\n1. Vending Machine");
        Console.WriteLine("2. The Bank");
        Console.WriteLine("3. Your Inventory");
        Console.WriteLine("4. Exit");
    }

    public void Start()
    {
        while (true)
        {
            DisplayMenu();
            Console.Write("Enter your choice: ");
            int choice;
            bool isValidChoice = int.TryParse(Console.ReadLine(), out choice);
            if (!isValidChoice)
            {
                Console.WriteLine("Invalid Input");
                continue;
            }
            switch (choice)
            {
                case 1:
                    vendingMachine.DisplayInventory();
                    Console.Write("Enter the code of the item you want to buy: ");
                    string codeToBuy = Console.ReadLine();
                    if (codeToBuy.ToLower() == "back") break;
                    vendingMachine.BuyItem(codeToBuy, this);
                    break;
                case 2:
                    vendingMachine.DisplayBalance();
                    break;
                case 3:
                    DisplayInventory();
                    break;
                case 4:
                    Console.WriteLine("Thank you for using the vending machine. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void AddToInventory(Item item)
    {
        purchasedItems.Add(item);
    }

    public void DisplayInventory()
    {
        Console.WriteLine("\nYour Inventory");
        if (purchasedItems.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
        }
        else
        {
            foreach (var item in purchasedItems)
            {
                Console.WriteLine($"{item.Name} - ${item.Price}");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Inventory inventory = new Inventory();
        inventory.AddItem("1", new Item("Cola", 15, "A1"));
        inventory.AddItem("2", new Item("Fanta", 15, "A2"));
        inventory.AddItem("3", new Item("Chips", 10, "A3"));
        inventory.AddItem("4", new Item("Snicker", 20, "A4"));
        inventory.AddItem("5", new Item("Bounty", 20, "A5"));

        Bank bank = new Bank(200.00);

        VendingMachine vendingMachine = new VendingMachine(inventory, bank);
        User user = new User(vendingMachine);

        user.Start();
    }
}
