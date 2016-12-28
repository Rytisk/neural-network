using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_NeuralNetwork_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializer init = new Initializer();

            init.StartTraining();

            Console.Write("First input: ");
            bool f = Convert.ToBoolean(Console.ReadLine());
            Console.Write("Second input: ");
            bool s = Convert.ToBoolean(Console.ReadLine());

            init.Test(f, s);

            Console.WriteLine("Press any key to exit...");
            Console.Read();
            Console.Read();
        }
    }
}
