 public class EShop
    {

        private Dictionary<Item, int> _inventory = new Dictionary<Item, int>();
        public Customer Customer { get; private set; }

        public EShop()
        {
            Customer = new Customer();
            SeedInventory();
        }

        
        public void Run()
        {
            Console.Title = "E-Shop Simulator";

            ShowWelcome();

            
            Customer.Budget = ReadPositiveDecimal("Enter your budget: $");
            if (!Confirm("Confirm budget? (Y/N): "))
            {
                Console.WriteLine("Goodbye!");
                return;
            }

         
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MAIN MENU ===");
                Console.WriteLine($"Budget:     ${Customer.Budget:0.00}");
                Console.WriteLine($"Cart total: ${Customer.CartTotal():0.00}");
                Console.WriteLine($"After buy:  ${(Customer.Budget - Customer.CartTotal()):0.00}");
                Console.WriteLine("--------------------------");
                Console.WriteLine("1) Browse inventory");
                Console.WriteLine("2) View cart");
                Console.WriteLine("3) Checkout");
                Console.WriteLine("4) Exit");
                Console.WriteLine();
                Console.Write("Choose option (1-4): ");

                string option = Console.ReadLine()?.Trim() ?? "";

                if (option == "1")
                {
                    BrowseInventory();
                }
                else if (option == "2")
                {
                    ViewCart();
                }
                else if (option == "3")
                {
                    Checkout();
                }
                else if (option == "4")
                {
                    Console.WriteLine("Thank you for using the E-Shop. Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid option. Press ENTER to continue.");
                    Console.ReadLine();
                }
            }
        }

        

        private void ShowWelcome()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Jean E-Shop!");
            Console.WriteLine("Author: Jean Padilla");
            Console.WriteLine("Version: 1.0");
            Console.WriteLine("Description: This program simulates an e-shop.");
            Console.WriteLine();
            Console.Write("Press ENTER to continue...");
            Console.ReadLine();
        }

       

        private void BrowseInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== INVENTORY ===");
                Console.WriteLine("#  Name                Price   InStock  InCart");
                Console.WriteLine("----------------------------------------------");

                List<Item> items = _inventory.Keys.ToList();

                for (int i = 0; i < items.Count; i++)
                {
                    Item it = items[i];
                    int stock = GetStock(it);
                    int inCart = Customer.GetQuantityInCart(it);

                    Console.WriteLine(
                        $"{i,2}) {it.Name,-18} ${it.Price,6:0.00} {stock,7} {inCart,7}");
                }

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine($"Budget:     ${Customer.Budget:0.00}");
                Console.WriteLine($"Cart total: ${Customer.CartTotal():0.00}");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("A) Add item to cart");
                Console.WriteLine("R) Remove item from cart");
                Console.WriteLine("X) Back to main menu");
                Console.Write("Choose option (A/R/X): ");

                string op = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (op == "X")
                {
                    return; 
                }
                else if (op == "A")
                {
                    if (items.Count == 0)
                    {
                        Console.WriteLine("There are no items.");
                        Pause();
                        continue;
                    }

                    int index = ReadIntInRange($"Enter item # (0-{items.Count - 1}): ", 0, items.Count - 1);
                    Item selected = items[index];
                    int stock = GetStock(selected);

                    if (stock <= 0)
                    {
                        Console.WriteLine("ERROR: Item is sold out.");
                        Pause();
                        continue;
                    }

                    int qty = ReadIntInRange($"Quantity (1-{stock}): ", 1, stock);

                    if (!Confirm($"Add {qty} x {selected.Name}? (Y/N): "))
                    {
                        Console.WriteLine("Operation canceled.");
                        Pause();
                        continue;
                    }

                   
                    AddToCart(selected, qty);
                    Console.WriteLine("Item added to cart.");
                    Pause();
                }
                else if (op == "R")
                {
                    if (items.Count == 0)
                    {
                        Console.WriteLine("There are no items.");
                        Pause();
                        continue;
                    }

                    int index = ReadIntInRange($"Enter item # (0-{items.Count - 1}): ", 0, items.Count - 1);
                    Item selected = items[index];

                    int inCart = Customer.GetQuantityInCart(selected);
                    if (inCart <= 0)
                    {
                        Console.WriteLine("This item is not in your cart.");
                        Pause();
                        continue;
                    }

                    int qty = ReadIntInRange($"Quantity to remove (1-{inCart}): ", 1, inCart);

                    if (!Confirm($"Remove {qty} x {selected.Name}? (Y/N): "))
                    {
                        Console.WriteLine("Operation canceled.");
                        Pause();
                        continue;
                    }

                    RemoveFromCart(selected, qty);
                    Console.WriteLine("Item(s) removed from cart.");
                    Pause();
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid option.");
                    Pause();
                }
            }
        }

       

        private void ViewCart()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== YOUR CART ===");

                var cartItems = Customer.GetCartItems();

                if (cartItems.Count == 0)
                {
                    Console.WriteLine("Your cart is empty.");
                }
                else
                {
                    Console.WriteLine("#  Name                Price   Qty   Subtotal");
                    Console.WriteLine("---------------------------------------------");
                    for (int i = 0; i < cartItems.Count; i++)
                    {
                        Item item = cartItems[i].Key;
                        int qty = cartItems[i].Value;
                        decimal subtotal = item.Price * qty;
                        Console.WriteLine(
                            $"{i,2}) {item.Name,-18} ${item.Price,6:0.00} {qty,4} ${subtotal,8:0.00}");
                    }
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine($"Total: ${Customer.CartTotal():0.00}");
                }

                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("R) Remove item");
                Console.WriteLine("X) Back to main menu");
                Console.Write("Choose option (R/X): ");
                string op = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (op == "X")
                {
                    return;
                }
                else if (op == "R")
                {
                    if (cartItems.Count == 0)
                    {
                        Console.WriteLine("Cart is empty.");
                        Pause();
                        continue;
                    }

                    int index = ReadIntInRange($"Enter item # (0-{cartItems.Count - 1}): ", 0, cartItems.Count - 1);
                    Item item = cartItems[index].Key;
                    int qtyInCart = cartItems[index].Value;

                    int qty = ReadIntInRange($"Quantity to remove (1-{qtyInCart}): ", 1, qtyInCart);

                    if (!Confirm($"Remove {qty} x {item.Name}? (Y/N): "))
                    {
                        Console.WriteLine("Operation canceled.");
                        Pause();
                        continue;
                    }

                    RemoveFromCart(item, qty);
                    Console.WriteLine("Item(s) removed.");
                    Pause();
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid option.");
                    Pause();
                }
            }
        }

        

        private void Checkout()
        {
            Console.Clear();
            Console.WriteLine("=== CHECKOUT ===");

            decimal total = Customer.CartTotal();
            Console.WriteLine($"Cart total: ${total:0.00}");
            Console.WriteLine($"Your budget: ${Customer.Budget:0.00}");

            if (total <= 0)
            {
                Console.WriteLine("Your cart is empty.");
                Pause();
                return;
            }

            if (total > Customer.Budget)
            {
                Console.WriteLine("ERROR: The total exceeds your budget.");
                Console.WriteLine("Please remove some items from your cart.");
                Pause();
                return;
            }

            if (!Confirm("Do you want to complete the purchase? (Y/N): "))
            {
                Console.WriteLine("Checkout canceled.");
                Pause();
                return;
            }

            
            foreach (var kv in Customer.GetCartItems())
            {
                Item item = kv.Key;
                int qty = kv.Value;
                int totalStock = _inventory.TryGetValue(item, out int stock) ? stock : 0;
                SetStock(item, totalStock - qty);
            }

            Customer.Budget -= total;
            Customer.ClearCart();

            Console.WriteLine($"Your purchase was successful! You paid ${total:0.00}.");
            Console.WriteLine($"Remaining budget: ${Customer.Budget:0.00}");
            Pause();
        }

        

        private int GetStock(Item item)
        {
            int totalStock = _inventory.TryGetValue(item, out int stock) ? stock : 0;
            int inCart = Customer.GetQuantityInCart(item);
            return totalStock - inCart;
        }

        private void SetStock(Item item, int stock)
        {
            _inventory[item] = stock;
        }

        private void AddToCart(Item item, int quantity)
        {
            int stock = GetStock(item);
            if (quantity <= 0 || quantity > stock) 
            return;
            Customer.AddToCart(item, quantity);
        }

        private void RemoveFromCart(Item item, int quantity)
        {
            Customer.RemoveFromCart(item, quantity);
        }

        private decimal ReadPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";

                if (decimal.TryParse(input, out decimal value) && value > 0)
                    return value;

                Console.WriteLine("ERROR: Please enter a valid positive number.");
            }
        }

        private int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"ERROR: Please enter a number between {min} and {max}.");
            }
        }

        private bool Confirm(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (input == "Y" || input == "YES")
                    return true;
                if (input == "N" || input == "NO")
                    return false;

                Console.WriteLine("ERROR: Please enter Y or N.");
            }
        }

        private void Pause()
        {
            Console.WriteLine("Press ENTER to continue.");
            Console.ReadLine();
        }

        

        private void SeedInventory()
        {
            AddItem(new Item("A-Grade", "Premium quality item.", 100.00m), 10);
            AddItem(new Item("B-Grade", "Solid mid-tier choice.", 50.00m), 10);
            AddItem(new Item("C-Grade", "Budget-friendly option.", 20.00m), 20);
            AddItem(new Item("Love", "Sold out but priceless.", 19.95m), 0);
            AddItem(new Item("Hug", "Redeem for a hug.", 1.00m), 100);
            AddItem(new Item("Self-Respect", "Very cheap but very valuable.", 0.01m), 9999);
            AddItem(new Item("Mjolnir", "Only the worthy can lift it.", 9999.99m), 1);
        }

        private void AddItem(Item item, int stock)
        {
            _inventory[item] = stock;
        }
    }

    
    public class Customer
    {
        public decimal Budget { get; set; }

        private Dictionary<Item, int> _cart = new Dictionary<Item, int>();

        public void AddToCart(Item item, int quantity)
        {
            if (_cart.ContainsKey(item))
                _cart[item] += quantity;
            else
                _cart[item] = quantity;
        }

        public void RemoveFromCart(Item item, int quantity)
        {
            if (!_cart.ContainsKey(item)) return;

            _cart[item] -= quantity;
            if (_cart[item] <= 0)
                _cart.Remove(item);
        }

        public decimal CartTotal()
        {
            decimal total = 0m;
            foreach (var kv in _cart)
            {
                total += kv.Key.Price * kv.Value;
            }
            return total;
        }

        public int GetQuantityInCart(Item item)
        {
            return _cart.TryGetValue(item, out int qty) ? qty : 0;
        }

        public List<KeyValuePair<Item, int>> GetCartItems()
        {
            return _cart.ToList();
        }

        public void ClearCart()
        {
            _cart.Clear();
        }
    }

    
    public class Item : IEquatable<Item>
    {
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }

        public Item(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public bool Equals(Item? other)
        {
            if (other is null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => Equals(obj as Item);

        public override int GetHashCode() => Name.ToLower().GetHashCode();

        public override string ToString() => $"{Name} - ${Price:0.00}";
    }
