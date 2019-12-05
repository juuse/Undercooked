using System;
using NDesk.Options;
using Npgsql;

namespace Undercooked
{
    /// <summary>
    /// Note: One of the dependencies: NDesk, is no longer maintained. I don't foresee any problems using it.
    /// </summary>
    class Program
    {
        static NpgsqlConnection conn;

        /// <summary>
        /// Parses the input arguments
        /// </summary>
        /// <param name="args">input arguments</param>
        static void Main(string[] args)
        {
            string action = "nothing";
            bool help = false;
            string dish_name = null;
            int quantity = 1;
            int top = 1;
            int party_size = 2;
            string date = DateTime.Today.ToString("d");
            int time = 18;
            string email = null;
            string phone = null;
            int res_id = -1;
            double n = 5;

            var p = new OptionSet() {
                { "h|?|help", "Display help", v => help = v != null },
                { "restock", "Restock the ingredients with quantity below input quantity", v => action = "restock" },
                { "dispose", "Dispose the ingredients with expiration date earlier than current date", v => action = "dispose" },
                { "prepare", "Prepare the dishes specified by dish-name for the specified the quantity", v => action = "prepare" },
                { "total-expense", "Get the total expense for the whole term", v => action = "total-expense" },
                { "total-revenue", "Get the total revenue for the whole term", v => action = "total-revenue" },
                { "profit", "Get the total profit (expense - revenue) for the whole term", v => action = "profit" },
                { "largest-expense", "Get the top n largest expense for the whole term", v => action = "largest-expense" },
                { "largest-revenue", "Get the top n largest revenue for the whole term", v => action = "largest-revenue" },
                { "reserve", "Make a reservation with the given party size, date, time, email, and phone number. Prints out reservation id", v => action = "reserve" },
                { "receipt", "Record the receipt made with reservation id, dish name, quantity", v => action = "receipt" },
                { "find-n", "Find all the dishes with all ingredients more than n dollars", v => action = "find-n" },
                { "find-n-2", "Find all the dishes with only 1 ingredient more than n dollars", v => action = "find-n" },
                { "dish-name=", "Specify the dish name", v => dish_name = v},
                { "quantity=", "Specify the quantity. Default is 1", v => quantity = int.Parse(v) },
                { "top=", "Specify the top n. Default is 1", v => top = int.Parse(v) },
                { "party-size=", "Specify the party size of the reservation. Default is 2", v => party_size = int.Parse(v) },
                { "date=", "Specify the date for the reservation. Default is today", v => date = DateTime.Parse(v).ToString("d") },
                { "time=", "Specify the time for the reservation in numbers 0 - 23. Default is 18", v => time = int.Parse(v) },
                { "email=", "Specify the email for the reservation", v => email = v },
                { "phone-number=", "Specify the phone number for the reservation", v => phone = v },
                { "reservation-id=", "Specify the reservation id for the receipt", v => res_id = int.Parse(v) },
                { "n=", "Specify the ingredient price to check for. Default is 5", v => n = double.Parse(v) },
            };

            try
            {
                p.Parse(args);

                if (help)
                {
                    ShowHelp(p);
                    return;
                }

                DatabaseConnection();

                switch (action)
                {
                    case "restock":
                        Console.WriteLine("Restocking...");
                        Restock(quantity);
                        Console.WriteLine("Finished Restocking.");
                        break;

                    case "dispose":
                        Console.WriteLine("Disposing...");
                        Dispose();
                        Console.WriteLine("Finished Disposing.");
                        break;

                    case "prepare":
                        Console.WriteLine("Preparing...");
                        Prepare(dish_name, quantity);
                        Console.WriteLine("Finished Preparing.");
                        break;

                    case "total-expense":
                        Console.WriteLine("Calculating Total Expense...");
                        Total_Expense();
                        Console.WriteLine("Finished Calculating Total Expense.");
                        break;

                    case "total-revenue":
                        Console.WriteLine("Calculating Total Revenue...");
                        Total_Revenue();
                        Console.WriteLine("Finished Calculating Total Revenue.");
                        break;

                    case "profit":
                        Console.WriteLine("Calculating Profit...");
                        Profit();
                        Console.WriteLine("Finished Calculating Profit.");
                        break;

                    case "largest-expense":
                        Console.WriteLine("Calculating Largest Expense...");
                        Largest_Expense(top);
                        Console.WriteLine("Finished Calculating Largest Expense.");
                        break;

                    case "largest-revenue":
                        Console.WriteLine("Calculating Largest Revenue...");
                        Largest_Revenue(top);
                        Console.WriteLine("Finished Calculating Largest Revenue.");
                        break;

                    case "reserve":
                        Console.WriteLine("Making A Reservation...");
                        Reserve(party_size, date, time, email, phone);
                        Console.WriteLine("Finished Making A Reservation.");
                        break;

                    case "receipt":
                        Console.WriteLine("Making A Receipt...");
                        Receipt(res_id, dish_name, quantity);
                        Console.WriteLine("Finished Making A Receipt...");
                        break;

                    case "find-n":
                        Console.WriteLine("Finding Dishes With All Ingredients More Than " + n + " Dollars...");
                        Find_n(n);
                        Console.WriteLine("Finished Finding Dishes With All Ingredients More Than " + n + " Dollars.");
                        break;

                    case "find-n-2":
                        Console.WriteLine("Finding Dishes With Only 1 Ingredient More Than " + n + " Dollars...");
                        Find_n_2(n);
                        Console.WriteLine("Finished Finding Dishes With Only 1 Ingredient More Than " + n + " Dollars.");
                        break;

                    default:
                        Console.WriteLine("Please enter a valid action. Type Undercooked -h for the list of actions.");
                        break;
                }

                conn.Close();
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private static void DatabaseConnection()
        {
            var connString = "Host=localhost;Username=postgres;Password=123456;Database=postgres";

            conn = new NpgsqlConnection(connString);
            conn.Open();
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static void Restock(int quantity) 
        {
            
        }

        private static void Dispose()
        {
            var cmd = new NpgsqlCommand("UPDATE Inventory SET quantity = 0 WHERE expiration_date <= current_date", conn);
            cmd.ExecuteNonQuery();
        }

        private static void Prepare(string dish_name, int quantity)
        {

        }

        private static void Total_Expense()
        {
            var cmd = new NpgsqlCommand("SELECT SUM(Cost) FROM Expenses", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0} \n", reader[0]);
        }

        private static void Total_Revenue()
        {
            var cmd = new NpgsqlCommand("SELECT SUM(Amount) FROM Revenue", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0} \n", reader[0]);
        }

        private static void Profit()
        {
            var cmd = new NpgsqlCommand("SELECT (SELECT SUM(Amount) FROM Revenue) - (SELECT SUM(Cost) FROM Expenses)", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0} \n", reader[0]);
        }

        private static void Largest_Expense(int top)
        {
            var cmd = new NpgsqlCommand("SELECT EID, EName, MAX(Cost) FROM Expenses GROUP BY EID, EName LIMIT " + top, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0}\t{1}\t{2} \n", reader[0], reader[1], reader[2]);
        }

        private static void Largest_Revenue(int top)
        {
            var cmd = new NpgsqlCommand("SELECT RID, RName, MAX(amount) FROM Revenue GROUP BY RID, RName LIMIT " + top, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0}\t{1}\t{2} \n", reader[0], reader[1], reader[2]);
        }

        private static void Reserve(int party_size, string date, int time, string email, string phone)
        {

        }

        private static void Receipt(int res_id, string dish_name, int quantity)
        {

        }

        

        private static void Find_n(double n)
        {
            var cmd = new NpgsqlCommand("SELECT DishName FROM Menu as m WHERE NOT EXISTS(SELECT DISTINCT(m2.DishID) FROM Menu as m2, Dishes as d, Ingredients as i WHERE m.DishID= m2.DishID AND m.DishID= d.DishID AND i.IngredientID= d.IngredientID AND i.cost <= " + n  + ")", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0} \n", reader[0]);
        }

        private static void Find_n_2(double n)
        {
            var cmd = new NpgsqlCommand("", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                Console.Write("{0} \n", reader[0]);
        }
    }
}
