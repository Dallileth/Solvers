using System;
using System.Threading.Tasks;

namespace Dallileth.Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Framework framework = new Framework("Dallileth.Sandbox.Demos");
            await framework.Run();
            Console.Clear();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
