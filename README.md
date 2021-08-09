# Solvers

**AStar**
```
Stack<(Direction action,XY expected_result)> path= Graphs.AStar<XY, Direction>(
      initial_states: new XY[] { new XY(3,3) },      
      at_destination: xy => xy.Equals(destination),
      get_actions: xy => 
      {      
          return new (XY state,Direction dir,float cost)[]
          {
            //replace costs with your cost function (e.g. from a map)
            (xy+Direction.Right,Direction.Right,1),
            (xy+Direction.Left,Direction.Left,1),
            (xy+Direction.Up,Direction.Up,1),
            (xy+Direction.Down,Direction.Down,1)
          }
      },
      heuristic: xy =>
      {
          var dx =Math.Abs(xy.X - flag.XY.Value.X);
          var dy =Math.Abs(xy.Y - flag.XY.Value.Y);
          var traversals = dx*dx + dy*dy; 
          return traversals;
      });
```


**DFS / BFS**
```
//execution is deferred until iterated over. e.g. In FirstOrDefault() or iterated over
var results = Graphs.DFS(
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
```
