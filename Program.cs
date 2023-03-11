namespace ChangeCalculator
{

    class Program
    {
        static void Main(string[] args)
        {
            // TestMode();
            UIMode();
        }

        /*
            Det här är huvudvägen att köra programmet för kassor.
        */
        static void UIMode()
        {
            bool run = true;
            while (run)
            {
                try
                {

                    Console.WriteLine("============================");
                    Console.WriteLine(" Hur mycket kostar varorna?");
                    var cost = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine(" Hur mycket har kunden betalat?");
                    var payment = Convert.ToDouble(Console.ReadLine());
                    var res = GetChange(payment, cost);
                    writeDictionary(res);
                    Console.WriteLine("============================");
                    Console.WriteLine("Tryck på q + enter för att lämna");
                    var option = Console.ReadLine();
                    if (option.ToLower() == "q")
                    {
                        run = false;
                    }

                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }
        /*
        Det här testläget har jag använt bara för att snabbt kunna testa applikationen.
        */
        static void TestMode()
        {

            TestGetChange(300, 200); // 1 x 100kr
            TestGetChange(350, 200); // 1 x 100 + 1 x 50kr
            TestGetChange(400, 199.5); // 1 x 200 + 1 x 50 öre
            TestGetChange(1000, 799.25);
            TestGetChange(1000, 99.5);
            TestGetChange(1000, 9.24);
            TestGetChange(1000, 299.75);
            TestGetChange(1000, 700);
            TestGetChange(1000, 799);
            TestGetChange(1000, 11000);
            TestGetChange(100, 200); // invalid state

        }

        static bool TestGetChange(double payment, double cost)
        {
            try
            {
                Console.WriteLine("Testing Payment: " + payment + " Cost: " + cost);
                var result = GetChange(payment, cost);
                // skriver ut resultat 
                writeDictionary(result);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
        Jag tänkte först hur jag personligen hade räknat och delat ut växel fysiskt själv.  
        */
        public static Dictionary<double, double> GetChange(double payment, double cost)
        {

            double[] bills = { 1000, 500, 200, 100, 50, 20, 10, 5, 1, 0.5 };
            double diff = RoundDecimal(payment - cost);
            Dictionary<double, double> result = new Dictionary<double, double>();
            if (diff < 0)
            {
                throw new Exception("Transaktionen gick inte igenom, summan täcker inte avgiften.");
            }

            while (diff >= bills[bills.Length - 1])
            {
                var bill = GetBestFitBill(diff, bills);
                diff = diff - bill;
                if (result.ContainsKey(bill))
                {
                    result[bill]++;
                }
                else
                {
                    result.Add(bill, 1);
                }
            }
            return result;

        }
        public static double GetBestFitBill(double diff, double[] bills)
        {
            foreach (double bill in bills)
            {
                double d = diff - bill;
                if (d >= 0)
                {
                    return bill;
                }
            }
            return 0;
        }

        /*
        När jag körde mina tester fick jag lite problem med decimaltalen så jag skrev den här funktionen istället.
        */
        static double RoundDecimal(double value)
        {
            double deci = value % 1;
            double integer = value - deci;
            if (deci < 0.25)
            {
                return integer + 0;
            }
            if (deci >= 0.25 && deci < 0.75)
            {
                return integer + 0.50;
            }
            return integer + 1;

        }

        /*
        Det blev lite problem när jag skrev ut dictionary objectet i konsolen så jag skrev det här istället.
        */
        static void writeDictionary(Dictionary<double, double> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine(item.Value + " x " + item.Key);
            }
        }
    }
}
