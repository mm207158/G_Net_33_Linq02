using LINQ.DataSources;

namespace G_Net_33_Linq02
{
    internal class Program
    {
        static void PrintHeader(int num, string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{"─",-60}");
            Console.WriteLine($"  Exercise {num}: {title}");
            Console.WriteLine($"{"─",-60}");
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            var products = Source.ProductList;
            var customers = Source.CustomerList;

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 1 – Top 3 most expensive products
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(1, "Top 3 most expensive products");

            var top3 = products
                .OrderByDescending(p => p.UnitPrice)
                .Take(3);

            foreach (var p in top3)
                Console.WriteLine($"  {p.ProductName,-40} ${p.UnitPrice,8:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 2 – Page 2 of products (page size = 5)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(2, "Page 2 of products (page size = 5)");

            int pageSize = 5;
            int pageNumber = 2;          // 1-based

            var page2 = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            foreach (var p in page2)
                Console.WriteLine($"  {p.ProductID,3}. {p.ProductName,-40} ${p.UnitPrice,8:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 3 – TakeWhile UnitPrice < $25 (list sorted by price first)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(3, "TakeWhile UnitPrice < $25 (sorted by price)");

            var takeWhileCheap = products
                .OrderBy(p => p.UnitPrice)
                .TakeWhile(p => p.UnitPrice < 25);

            foreach (var p in takeWhileCheap)
                Console.WriteLine($"  {p.ProductName,-40} ${p.UnitPrice,8:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 4 – Are ALL Seafood products in stock?
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(4, "Are ALL Seafood products in stock?");

            bool allSeafoodInStock = products
                .Where(p => p.Category == "Seafood")
                .All(p => p.UnitsInStock > 0);

            Console.WriteLine($"  All Seafood products in stock: {allSeafoodInStock}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 5 – Does the ID list contain 9?
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(5, "Does the ID list contain 9?");

            int[] ids = { 3, 9, 13, 18 };

            bool containsNine = ids.Contains(9);

            Console.WriteLine($"  IDs: [{string.Join(", ", ids)}]");
            Console.WriteLine($"  Contains 9: {containsNine}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 6 – Group products by Category, print count per group
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(6, "Products grouped by Category (with count)");

            var groupedByCategory = products.GroupBy(p => p.Category);

            foreach (var group in groupedByCategory.OrderBy(g => g.Key))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  {group.Key,-20}");
                Console.ResetColor();
                Console.WriteLine($"  Count: {group.Count()}");
            }

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 7 – Group products by Category, project only names per group
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(7, "Product names per Category group");

            var categoryNames = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Names = g.Select(p => p.ProductName).ToList()
                });

            foreach (var group in categoryNames.OrderBy(g => g.Category))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  [{group.Category}]");
                Console.ResetColor();
                foreach (var name in group.Names)
                    Console.WriteLine($"    • {name}");
            }

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 8 – Categories with MORE THAN 3 products
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(8, "Categories with more than 3 products");

            var busyCategories = products
                .GroupBy(p => p.Category)
                .Where(g => g.Count() > 3)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count);

            foreach (var item in busyCategories)
                Console.WriteLine($"  {item.Category,-25} {item.Count} products");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 9 – Query syntax: group customers by Country
            //               → { Country, Count, TotalOrderValue }
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(9, "Customers grouped by Country (query syntax)");

            var customerStats =
                from c in customers
                group c by c.Country into countryGroup
                select new
                {
                    Country = countryGroup.Key,
                    Count = countryGroup.Count(),
                    TotalOrderValue = countryGroup
                                        .SelectMany(c => c.Orders)
                                        .Sum(o => o.Total)
                };

            foreach (var stat in customerStats.OrderByDescending(s => s.TotalOrderValue))
                Console.WriteLine($"  {stat.Country,-15}  Customers: {stat.Count,2}  Total Orders: ${stat.TotalOrderValue,12:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 10 – Total units in stock across all products
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(10, "Total units in stock across all products");

            int totalUnits = products.Sum(p => p.UnitsInStock);

            Console.WriteLine($"  Total units in stock: {totalUnits:N0}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 11 – Cheapest and most expensive product prices
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(11, "Cheapest and most expensive product prices");

            decimal minPrice = products.Min(p => p.UnitPrice);
            decimal maxPrice = products.Max(p => p.UnitPrice);

            var cheapest = products.First(p => p.UnitPrice == minPrice);
            var mostExpensive = products.First(p => p.UnitPrice == maxPrice);

            Console.WriteLine($"  Cheapest   : {cheapest.ProductName,-40} ${minPrice:F2}");
            Console.WriteLine($"  Most Expensive: {mostExpensive.ProductName,-37} ${maxPrice:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 12 – Distinct list of all product categories
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(12, "Distinct product categories");

            var categories = products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c);

            Console.WriteLine("  " + string.Join(", ", categories));

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 13 – Product IDs in setA but NOT in setB  (Except)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(13, "IDs in setA but NOT in setB");

            int[] setA = { 1, 3, 5, 7, 9, 11, 13 };
            int[] setB = { 3, 6, 9, 12, 15, 13 };

            var onlyInA = setA.Except(setB);

            Console.WriteLine($"  setA : [{string.Join(", ", setA)}]");
            Console.WriteLine($"  setB : [{string.Join(", ", setB)}]");
            Console.WriteLine($"  A - B: [{string.Join(", ", onlyInA)}]");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 14 – Countries in list1 but NOT in list2 (case-insensitive)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(14, "Countries in list1 but NOT in list2 (case-insensitive)");

            string[] list1 = { "Germany", "France", "UK", "Spain" };
            string[] list2 = { "france", "SPAIN", "Italy" };

            // Normalise list2 to lowercase for comparison
            var list2Lower = list2.Select(c => c.ToLower()).ToHashSet();

            var onlyInList1 = list1.Where(c => !list2Lower.Contains(c.ToLower()));

            Console.WriteLine($"  list1    : [{string.Join(", ", list1)}]");
            Console.WriteLine($"  list2    : [{string.Join(", ", list2)}]");
            Console.WriteLine($"  Only list1: [{string.Join(", ", onlyInList1)}]");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 15 – Dictionary<int, Product> keyed by ProductID
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(15, "Dictionary<int, Product> keyed by ProductID → retrieve ID=18");

            var productDict = products.ToDictionary(p => p.ProductID);

            var product18 = productDict[18];
            Console.WriteLine($"  Product #18 → {product18.ProductName}  Category: {product18.Category}  Price: ${product18.UnitPrice:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 16 – First product whose price > $50
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(16, "First product with price > $50");

            var firstOver50 = products.First(p => p.UnitPrice > 50);

            Console.WriteLine($"  {firstOver50.ProductName,-40} ${firstOver50.UnitPrice:F2}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 17 – FirstOrDefault with price > $500 (returns null, no throw)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(17, "FirstOrDefault with price > $500 (returns null instead of throwing)");

            var over500 = products.FirstOrDefault(p => p.UnitPrice > 500);

            if (over500 is null)
                Console.WriteLine("  No product found with price > $500  →  result is null (no exception thrown)");
            else
                Console.WriteLine($"  Found: {over500.ProductName}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 18 – Multiplication table row for 7  (Range + Select)
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(18, "Multiplication table row for 7");

            var multiplicationRow7 = Enumerable.Range(1, 10)
                .Select(i => $"7 × {i,2} = {7 * i,3}");

            foreach (var line in multiplicationRow7)
                Console.WriteLine($"  {line}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 19 – Even numbers between 1 and 30
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(19, "Even numbers between 1 and 30");

            var evens = Enumerable.Range(1, 30).Where(n => n % 2 == 0);

            Console.WriteLine("  " + string.Join(", ", evens));

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 20 – Concat first 3 product names + first 3 customer company names
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(20, "Concat first 3 product names + first 3 customer company names");

            var first3Products = products.Take(3).Select(p => p.ProductName);
            var first3Companies = customers.Take(3).Select(c => c.CompanyName);

            var combined = first3Products.Concat(first3Companies);

            int idx = 1;
            foreach (var name in combined)
                Console.WriteLine($"  {idx++,2}. {name}");

            // ─────────────────────────────────────────────────────────────────────────────
            //  Exercise 21 – Zip products with customers by position
            // ─────────────────────────────────────────────────────────────────────────────
            PrintHeader(21, "Zip: ProductName sold to CompanyName (by position)");

            var zipped = products.Zip(customers,
                (p, c) => $"{p.ProductName} sold to {c.CompanyName}");

            foreach (var line in zipped)
                Console.WriteLine($"  {line}");

            Console.WriteLine();
        }
    }
}
