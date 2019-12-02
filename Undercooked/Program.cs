using System;
using NDesk.Options;

namespace Undercooked
{
    /// <summary>
    /// Note: One of the dependencies: NDesk, is no longer maintained. I don't foresee any problems using it.
    /// </summary>
    class Program
    {
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

            var p = new OptionSet() {
                { "h|?|help", "Display help", v => help = v != null },
                { "restock", "Restock the ingredients with quantity below input quantity", v => action = "restock" },
                { "dispose", "Dispose the ingredients with expiration date earlier than current date", v => action = "dispose" },
                { "prepare", "Prepare the dishes specified by dish-name for the specified the quantity", v => action = "prepare" },
                { "total-expense", "Get the total expense for the whole term", v => action = "total_expense" },
                { "total-revenue", "Get the total revenue for the whole term", v => action = "total-revenue" },
                { "profit", "Get the total profit (expense - revenue) for the whole term", v => action = "profit" },
                { "largest-expense", "Get the top n largest expense for the whole term", v => action = "largest-expense" },
                { "largest-revenue", "Get the top n largest revenue for the whole term", v => action = "largest-revenue" },
                { "reserve", "Make a reservation with the given party size, date, time, email, and phone number. Prints out reservation id", v => action = "reserve" },
                { "receipt", "Record the receipt made with reservation id, dish name, quantity", v => action = "receipt" },
                { "dish-name=", "Specify the dish name", v => dish_name = v},
                { "quantity=", "Specify the quantity. Default is 1", v => quantity = int.Parse(v) },
                { "top=", "Specify the top n. Default is 1", v => top = int.Parse(v) },
                { "party-size=", "Specify the party size of the reservation. Default is 2", v => party_size = int.Parse(v) },
                { "date=", "Specify the date for the reservation. Default is today", v => date = DateTime.Parse(v).ToString("d") },
                { "time=", "Specify the time for the reservation in numbers 0 - 23. Default is 18", v => time = int.Parse(v) },
                { "email=", "Specify the email for the reservation", v => email = v },
                { "phone-number=", "Specify the phone number for the reservation", v => phone = v },
                { "reservation-id=", "Specify the reservation id for the receipt", v => res_id = int.Parse(v) },
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
                        Restock(quantity);
                        break;

                    case "dispose":
                        Dispose();
                        break;

                    case "prepare":
                        Prepare(dish_name, quantity);
                        break;

                    case "total-expense":
                        Total_Expense();
                        break;

                    case "total-revenue":
                        Total_Revenue();
                        break;

                    case "profit":
                        Profit();
                        break;

                    case "largest-expense":
                        Largest_Expense(top);
                        break;

                    case "largest-revenue":
                        Largest_Revenue(top);
                        break;

                    case "reserve":
                        Reserve(party_size, date, time, email, phone);
                        break;

                    case "receipt":
                        Receipt(res_id, dish_name, quantity);
                        break;

                    default:
                        Console.WriteLine("Please enter a valid action. Type Undercooked -h for the list of actions.");
                        break;
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private static void DatabaseConnection()
        {
            // https://www.npgsql.org/doc/index.html

            var connString = "Host=localhost;Username=EECS341;Password=123456;Database=localhost";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            Console.WriteLine("hello");
            /*
            // Insert some data
            await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
            {
                cmd.Parameters.AddWithValue("p", "Hello world");
                await cmd.ExecuteNonQueryAsync();
            }
            */

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT * FROM Inventory", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    Console.WriteLine(reader.GetString(0));
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

        }

        private static void Prepare(string dish_name, int quantity)
        {

        }

        private static void Total_Expense()
        {

        }

        private static void Total_Revenue()
        {

        }

        private static void Profit()
        {

        }

        private static void Largest_Expense(int top)
        {

        }

        private static void Largest_Revenue(int top)
        {

        }

        private static void Reserve(int party_size, string date, int time, string email, string phone)
        {

        }

        private static void Receipt(int res_id, string dish_name, int quantity)
        {

        }
    }
}
