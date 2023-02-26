// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BuildingsAndBaddies.Desktop.Grid
{
    public class BreadthFirstPathFinder : AbstractPathFinder
    {
        public BreadthFirstPathFinder(GridCell[,] grid, int cellSize)
            : base(grid, cellSize)
        {
        }


        protected override Stack<Vector2> DoFindPath(int startX, int startY, int finishX, int finishY)
        {
            // Seed the frontier map with the starting cell.
            var frontier = new Queue<GridCell>();
            var startCell = GetCell(startX, startY);
            var finishCell = GetCell(finishX, finishY);

            startCell.Iteration = Iteration;

            frontier.Enqueue(startCell);

            var found = false;
            while (frontier.Count > 0 && !found)
            {
                var current = frontier.Dequeue();
                foreach (var next in GetNeighbors(current))
                {
                    if (next.Iteration != Iteration)
                    {
                        // Skip blocked cells
                        if (next.NumBuildings > 0)
                        {
                            continue;
                        }

                        next.Iteration = Iteration;
                        next.CameFrom = current;

                        // If we have reached the finish, we are done!
                        if (next == finishCell)
                        {
                            found = true;
                            break;
                        }

                        // Not yet at the end, keep looking
                        frontier.Enqueue(next);
                    }
                }
            }

            // Reconstruct the path, if we found one
            var waypoints = new Stack<Vector2>();

            if (!found)
            {
                return waypoints;
            }

            var cell = finishCell;
            while (cell != startCell)
            {
                waypoints.Push(new Vector2(cell.X * CellSize, cell.Y * CellSize));

                cell = cell.CameFrom;
            }

            return waypoints;
        }
    }
}
