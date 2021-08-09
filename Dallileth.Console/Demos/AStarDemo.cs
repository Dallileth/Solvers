using Dallileth.DataScience.Solvers.GraphSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.Sandbox.Demos
{
    public class AStarDemo : IDemo
    {
        public async Task Run(ScreenRegion region)
        {
            region.Clear(' ');
            await Task.Delay(1000); 

            ScreenObject agent = new ScreenObject('@', new XY(2, 2), region);
            ScreenObject flag = new ScreenObject('!', new XY(region.W - 2, region.H - 2), region);
            //ScreenObject current = new ScreenObject('X', null,region);
            List<ScreenObject> graph = new List<ScreenObject>();
            var map = new float[region.H][];
            Random r = new Random();
            for(int y=0;y<region.H;y++)
            {
                map[y] = new float[region.W];
                for(int x=0;x<region.W;x++)
                {
                    bool full_cost = 
                        y==agent.XY.Value.Y || x==agent.XY.Value.X ||
                        x==flag.XY.Value.X || y==flag.XY.Value.Y?
                        false:
                        r.NextDouble() <= .25;
                    
                    var cost= 1 + 4 * (float)r.NextDouble();
                    map[y][x] = full_cost?100:1+2*(float)r.NextDouble();
                    region.Write(x, y, full_cost?'#':'.');
                }
            }

            var stack = Graphs.AStar<XY, Direction>(
                new XY[] { agent.XY.Value },
                at_destination: xy => xy.Equals(flag.XY),
                get_actions: xy => GetNeighbors(xy, map,region.W,region.H),
                heuristic: xy =>
                {
                    var dx =Math.Abs(xy.X - flag.XY.Value.X);
                    var dy =Math.Abs(xy.Y - flag.XY.Value.Y);
                    var traversals = dx*dx + dy*dy; 
                    return traversals;
                });
            while(stack.Count>0)
            {
                agent.XY = agent.XY.Value + stack.Pop();
                await Task.Delay((int)(map[agent.XY.Value.Y][agent.XY.Value.X]*100));
            }
        }



        IEnumerable<(XY, Direction, float)> GetNeighbors(XY xy, float[][]costs,int w, int h)
        {
            if (0 < xy.X)
                yield return (xy + new XY(-1, 0), Direction.Left, costs[xy.Y][xy.X-1]);
            if (0 < xy.Y)
                yield return (xy + new XY(0, -1), Direction.Up, costs[xy.Y-1][xy.X ]);
            if (xy.X < w-1)
                yield return (xy + new XY(1, 0), Direction.Right, costs[xy.Y][xy.X + 1]);
            if (xy.Y < h-1)
                yield return (xy + new XY(0, 1), Direction.Down, costs[xy.Y+1][xy.X ]);

        }

    }


}
