// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BuildingsAndBaddies.Desktop.Grid
{
    public class RandomPathFinder : AbstractPathFinder
    {
        public RandomPathFinder(GridCell[,] grid, int cellSize)
            : base(grid, cellSize)
        {
        }


        protected override Stack<Vector2> DoFindPath(int startX, int startY, int finishX, int finishY)
        {
            var waypoints = new Stack<Vector2>();
            waypoints.Push(new Vector2(finishX, finishY));

            var chaos = new Random(HashCode.Combine(startX.GetHashCode(), startY.GetHashCode()));
            var num = chaos.Next(1, 3);

            while (waypoints.Count < num)
            {
                var x = chaos.Next(1, Width - 2);
                var y = chaos.Next(1, Height - 2);

                if (GetCell(x, y).NumBuildings == 0)
                {
                    waypoints.Push(new Vector2(x * CellSize, y * CellSize));
                }
            }

            return waypoints;
        }
    }
}
