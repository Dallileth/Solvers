using Dallileth.DataStrutures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.DataScience.Solvers.GraphSearch
{
    public static class Graphs
    {
        /*
         Todo: Create an async api with:
            int? or TimeSpan? yield_frequency - Task.Yield() to free up processing for other tasks
            CancellationTokenSource cts - Allow execution to be aborted, perhaps returning the best guess
        
            
            AStar largemap - Pathfind on the lower-resolution map (A->B->C->D)
                             Then figure out a path on the higher resolution map (A -> (a>b>c>d>e) -> B)               
         */

        /// <summary>
        /// Finds the shortest path from an initial_node to a node that satisfies the at_destination predicate
        /// </summary>
        /// <param name="initial_nodes">Initial nodes to search</param>
        /// <param name="at_destination">Returns true if we found a solution</param>
        /// <param name="get_actions">Return a list of possible neighbors</param>
        /// <param name="heuristic">Guess how close we are to the destination - e.g. using Manhattan distance</param>
        /// <param name="draw">Draws frontier nodes and visited nodes</param>
        /// <param name="draw_current">Return which node we are actively considering</param>
        /// <returns></returns>
        public static Stack<ACTION> AStar<STATE, ACTION>(
            IEnumerable<STATE> initial_nodes,
            Predicate<STATE> at_destination,
            Func<STATE, IEnumerable<(STATE dest, ACTION act, float cost)>> get_actions,
            Func<STATE, float> heuristic = null,

            Action<(STATE state, ACTION action, float cost)> draw_heatmap = null)
        {
            if (heuristic == null)
                heuristic = (STATE) => 0f;

            var frontier = new PriorityQueue<QueuedState<STATE, ACTION>>();
            var visited = new Dictionary<STATE, (STATE previous_state, ACTION previous_action, float total_cost)>();

            foreach (var initial_node in initial_nodes)
            {
                frontier.Enqueue(new QueuedState<STATE, ACTION>(initial_node, 0));
                visited.Add(initial_node, default);
            }
            STATE current = default;
            while (frontier.Count > 0)
            {
                STATE old_current = current;

                current = frontier.Dequeue().State;
                if (at_destination(current))
                {
                    break;
                }

                var actions = get_actions(current);
                foreach (var action in actions)
                {
                    if (action.cost<=0)
                    {
                        throw new Exception("Costs shouldn't be negative"); //subject to debate!
                    }
                    var new_cost = visited[current].total_cost + action.cost;
                    if (!visited.ContainsKey(action.dest) || new_cost < visited[action.dest].total_cost)
                    {
                        if (visited.ContainsKey(action.dest))
                            visited.Remove(action.dest);


                        var item = (current, action.act, new_cost);
                        visited.Add(action.dest, item);
                        draw_heatmap?.Invoke(item);

                        frontier.Enqueue(new QueuedState<STATE, ACTION>(action.dest, new_cost + heuristic(action.dest)));
                        draw_heatmap?.Invoke(action);
                    }
                }
            }



            Stack<ACTION> q = new Stack<ACTION>();
            //note: we assume that costs can't be negative
            while (visited[current].total_cost > 0)
            {
                var item = visited[current];
                q.Push(item.previous_action);
                current = visited[current].previous_state;
            }
            return q;
        }



        /// <summary>
        /// Depth-first-search algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initial_items"></param>
        /// <param name="get_neighbors"></param>
        /// <param name="should_yield">Return true if T matches something we want</param>
        /// <param name="should_break">Return true if no longer interested in searching</param>
        /// <returns></returns>
        public static IEnumerable<T> DFS<T>(IEnumerable<T> initial_items, Func<T, IEnumerable<T>> get_neighbors, Predicate<T> should_yield = null, Predicate<T> should_break = null)
        {
            Stack<T> stack = new Stack<T>();

            //populate open set
            foreach (var item in initial_items)
            {
                stack.Push(item);
            }

            while (stack.Count >= 1)
            {
                //pop from pop set and explore it
                var current = stack.Pop();

                foreach (var neigh in get_neighbors(current))
                {
                    stack.Push(neigh);
                }

                if ((should_yield?.Invoke(current)) ?? true)
                    yield return current;
                if ((should_break?.Invoke(current)) ?? true)
                    break;
            }
        }

        /// <summary>
        /// Depth-first-search algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initial_items"></param>
        /// <param name="get_neighbors"></param>
        /// <param name="should_yield">Return true if T matches something we want</param>
        /// <param name="should_break">Return true if no longer interested in searching</param>
        /// <returns></returns>
        public static IEnumerable<T> BFS<T>(IEnumerable<T> initial_items, Func<T, IEnumerable<T>> get_neighbors, Predicate<T> should_yield = null, Predicate<T> should_break = null)
        {
            Queue<T> queue = new Queue<T>();

            //populate open set
            foreach (var item in initial_items)
            {
                queue.Enqueue(item);
            }

            while (queue.Count >= 1)
            {
                //pop from pop set and explore it
                var current = queue.Dequeue();

                foreach (var neigh in get_neighbors(current))
                {
                    queue.Enqueue(neigh);
                }

                if ((should_yield?.Invoke(current)) ?? true)
                    yield return current;
                if ((should_break?.Invoke(current)) ?? true)
                    break;
            }
        }
    }

    public struct QueuedState<STATE, ACTION> : IComparable<QueuedState<STATE, ACTION>>
    {

        public STATE State { get; private set; }
        public float Priority { get; private set; }
        internal QueuedState(STATE state, float priority)
        {
            State = state;
            Priority = priority;
        }
        public int CompareTo(QueuedState<STATE, ACTION> other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public int GetHashCode(QueuedState<STATE, ACTION> obj)
        {
            return obj.State.GetHashCode();
        }
    }
}
