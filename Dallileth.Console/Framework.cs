using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.Sandbox
{
    public class Framework //where T : new()
    {

        ScreenRegion DemoRegion { get; init; } = new ScreenRegion(3, 3, 24, 24);

        List<IDemo> _demos = new List<IDemo>();
        public Framework(string @namespace)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                            .Where(t=> String.Equals(t.Namespace, @namespace, StringComparison.Ordinal)))
            {
                try
                {
                    _demos.Add((IDemo)Activator.CreateInstance(type));
                }
                catch
                {

                }
            }
        }

        public async Task Run()
        {
            IDemo SelectDemo()
            {
                string error;
                while (true)
                {
                    error = "";
                    Console.Clear();
                    for (int i = 0; i < _demos.Count; i++)
                    {
                        Console.WriteLine($"{i}: {_demos[i].GetType().Name}");
                    }
                    Console.WriteLine("q: quit");
                    Console.WriteLine();
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.WriteLine(error);
                        Console.WriteLine();
                    }

                    Console.Write(">");
                    try
                    {
                        var entry = Console.ReadLine();
                        if (entry.StartsWith('q'))
                            return null;
                        else
                            return _demos[int.Parse(entry)];
                    }
                    catch
                    {
                        error = "Invalid selection";
                    }
                }
            }

            while (true)
            {
                var demo = SelectDemo();
                if (demo == null)
                    return;

                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(demo.GetType().Name);

                    await demo.Run(DemoRegion);
                    Console.SetCursorPosition(0,DemoRegion.Y + DemoRegion.H + 1);
                    Console.WriteLine("Completed");
                }
                catch(Exception e)
                {

                    Console.SetCursorPosition(0, DemoRegion.Y + DemoRegion.H + 1);
                    Console.WriteLine(e.Message);
                }
                finally{
                    Console.Write("Press any key to continiue");
                }


            }
        }

    }




}
