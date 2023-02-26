// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class PathGrid
    {
        public const int CellSize = 16;

        private readonly GridCell[,] grid;

        private readonly Rectangle sourceRect = new Rectangle(0, 0, 1, 1);
        private readonly Texture2D cellDebugTexture;

        // Size of the grid, in cells
        private readonly int width;
        private readonly int height;

        public PathGrid(GraphicsDevice graphicsDevice, int mapWidth, int mapHeight)
        {
            width = mapWidth / CellSize;
            height = mapHeight / CellSize;

            grid = new GridCell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    grid[x, y] = new GridCell();
                }
            }

            cellDebugTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            cellDebugTexture.SetData(new[] { Color.White });
        }


        public void AddItem(Rectangle bounds)
        {
            // TODO - need to differentiate between fixed items (buildings) and mobile items (units)
            foreach (var cell in GetCells(bounds))
            {
                cell.NumOccupants += 1;
            }
        }


        public void RemoveItem(Rectangle bounds)
        {
            // TODO - need to differentiate between fixed items (buildings) and mobile items (units)
            foreach (var cell in GetCells(bounds))
            {
                cell.NumOccupants -= 1;
            }
        }


        public Stack<Vector2> FindPath(Point start, Point finish)
        {
            // TODO - HACK - build a real path! For now, just pick two random waypoints
            var waypoints = new Stack<Vector2>();
            waypoints.Push(new Vector2(finish.X, finish.Y));

            var chaos = new Random(start.GetHashCode());
            var num = chaos.Next(1, 3);

            while (waypoints.Count < num)
            {
                var x = chaos.Next(1, width - 2);
                var y = chaos.Next(1, height - 2);

                if (grid[x, y].NumOccupants == 0)
                {
                    waypoints.Push(new Vector2(x * CellSize, y * CellSize));
                }
            }

            return waypoints;
        }


        public void Update(GameTime gameTime)
        {
            // Nothing to do here...yet
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid[x, y].NumOccupants > 0)
                    {
                        DrawCell(spriteBatch, x, y);
                    }
                }
            }
        }


        private void DrawCell(SpriteBatch spriteBatch, int x, int y)
        {
            var destRect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);

            // TODO - vary the color based on the number of items in the grid cell
            spriteBatch.Draw(cellDebugTexture, destRect, sourceRect, Color.Aqua);
        }


        private IEnumerable<GridCell> GetCells(Rectangle mapRect)
        {
            int x1 = Math.Clamp(mapRect.Left / CellSize, 0, width - 1);
            int x2 = Math.Clamp(mapRect.Right / CellSize, 0, width - 1);
            int y1 = Math.Clamp(mapRect.Top / CellSize, 0, height - 1);
            int y2 = Math.Clamp(mapRect.Bottom / CellSize, 0, height - 1);

            for (var x = x1; x <= x2; x++)
            {
                for (var y = y1; y <= y2; y++)
                {
                    yield return grid[x, y];
                }
            }
        }


        private class GridCell
        {
            public int NumOccupants { get; set; }
        }
    }
}
