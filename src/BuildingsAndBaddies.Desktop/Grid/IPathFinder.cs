// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BuildingsAndBaddies.Desktop.Grid
{
    public interface IPathFinder
    {
        int Iteration { get; }

        Stack<Vector2> FindPath(int startX, int startY, int finishX, int finishY);
    }
}
