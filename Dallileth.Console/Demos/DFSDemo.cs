using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dallileth.DataScience;
using System.IO;
using Dallileth.Sandbox.Models;
using Dallileth.DataScience.Solvers;

namespace Dallileth.Sandbox.Demos
{
    public class DFSDemo : IDemo
    {

        public async Task Run(ScreenRegion region)
        {

            var DFS = Search.DFS(
                  initial_items: new PathSearch[] { new PathSearch { Path = @"c:\searchfolder", PathType = PathType.Directory } },
                  get_neighbors: dir =>
                  {
                      List<PathSearch> search = new List<PathSearch>();
                      foreach(var directory in Directory.GetDirectories(dir.Path, "*.*"))
                      {
                          search.Add(new PathSearch { Path = directory, PathType = PathType.Directory });
                      }
                      foreach(var file in Directory.GetFiles(dir.Path,"*.*"))
                      {
                          search.Add(new PathSearch { Path = file, PathType = PathType.File });
                      }
                      return search.ToArray();
                  },
                  should_yield: search =>
                  {
                      return search.PathType == PathType.File && search.Path.EndsWith(".txt");
                  },
                  max_depth:2);
            if (!DFS.FirstOrDefault().Equals(default(PathSearch)))
            {

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
