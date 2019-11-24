using System;
using NDesk.Options;

namespace Undercooked
{
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

            var p = new OptionSet() {
               { "restock", "Restock the ingredients", v => action = "restock" },
               { "h|?|help", "Display help", v => help = v != null },
            };

            p.Parse(args);

            if (help)
            {
                ShowHelp(p);
                return;
            }

            switch (action)
            {
                case "restock":
                    Console.WriteLine("Restocking...");
                    break;

                default:
                    Console.WriteLine("Please enter a valid action. Type Undercooked -h for the list of actions.");
                    break;
            }

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
