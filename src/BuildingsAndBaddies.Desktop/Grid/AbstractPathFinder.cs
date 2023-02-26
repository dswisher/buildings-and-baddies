// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BuildingsAndBaddies.Desktop.Grid
{
    public abstract class AbstractPathFinder : IPathFinder
    {
        private readonly int[] deltas = new[] { -1, 0, 1 };

        protected AbstractPathFinder(GridCell[,] grid, int cellSize)
        {
            Grid = grid;
            CellSize = cellSize;

            Width = grid.GetUpperBound(0);
            Height = grid.GetUpperBound(1);

            Iteration = 1;
        }


        public int Iteration { get; private set; }
        protected int CellSize { get; }
        protected int Width { get; }
        protected int Height { get; }

        private GridCell[,] Grid { get; }


        public Stack<Vector2> FindPath(int startX, int startY, int finishX, int finishY)
        {
            // To avoid having to recreate a grid or reset it, we use an "iteration" variable. If a grid
            // cell's iteration value matches this iteration value, then we know we have visited that
            // cell during this pathfinding attempt.
            Iteration += 1;

            // Do the algorithm-specific work
            return DoFindPath(startX, startY, finishX, finishY);
        }


        protected abstract Stack<Vector2> DoFindPath(int startX, int startY, int finishX, int finishY);


        protected GridCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return null;
            }

            return Grid[x, y];
        }


        protected IEnumerable<GridCell> GetNeighbors(GridCell cell, bool allowDiagonals = false)
        {
            foreach (var dx in deltas)
            {
                foreach (var dy in deltas)
                {
                    // Skip over current cell
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    // Only process diagonals if requested
                    if (!allowDiagonals && dx != 0 && dy != 0)
                    {
                        continue;
                    }

                    // Find the cell
                    var x = cell.X + dx;
                    var y = cell.Y + dy;
                    var candidate = GetCell(x, y);

                    // Return it if it actually exists
                    if (candidate != null)
                    {
                        yield return candidate;
                    }
                }
            }
        }
    }
}
