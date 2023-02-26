// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace BuildingsAndBaddies.Desktop.Grid
{
    public class GridCell
    {
        public GridCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public int NumBuildings { get; set; }
        public int NumBots { get; set; }

        public int Iteration { get; set; }
        public GridCell CameFrom { get; set; }
    }
}
