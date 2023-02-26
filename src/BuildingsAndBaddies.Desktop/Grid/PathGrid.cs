// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Grid
{
    public class PathGrid
    {
        public const int CellSize = 16;

        private readonly GridCell[,] grid;

        private readonly Rectangle sourceRect = new Rectangle(0, 0, 1, 1);
        private readonly Texture2D cellDebugTexture;
        private readonly IPathFinder pathFinder;

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
                    grid[x, y] = new GridCell(x, y);
                }
            }

            // TODO - load path finder from settings?
            // pathFinder = new RandomPathFinder(grid, CellSize);
            pathFinder = new BreadthFirstPathFinder(grid, CellSize);

            cellDebugTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            cellDebugTexture.SetData(new[] { Color.White });
        }


        public void AddItem(Rectangle bounds, bool isFixed)
        {
            foreach (var cell in GetCells(bounds))
            {
                if (isFixed)
                {
                    cell.NumBuildings += 1;
                }
                else
                {
                    cell.NumBots += 1;
                }
            }
        }


        public void RemoveItem(Rectangle bounds, bool isFixed)
        {
            foreach (var cell in GetCells(bounds))
            {
                if (isFixed)
                {
                    cell.NumBuildings -= 1;
                }
                else
                {
                    cell.NumBots -= 1;
                }
            }
        }


        public Stack<Vector2> FindPath(int startX, int startY, int finishX, int finishY)
        {
            return pathFinder.FindPath(startX / CellSize, startY / CellSize, finishX / CellSize, finishY / CellSize);
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
                    DrawCell(spriteBatch, grid[x, y]);
                }
            }
        }


        private void DrawCell(SpriteBatch spriteBatch, GridCell cell)
        {
            if (cell.NumBuildings == 0 && cell.NumBots == 0 && cell.Iteration < pathFinder.Iteration)
            {
                return;
            }

            // TODO - vary the color based on the number of items in the grid cell
            Color color;
            if (cell.NumBuildings == 0 && cell.NumBots == 0)
            {
                if (cell.Iteration == pathFinder.Iteration)
                {
                    // No occupants, but visited
                    color = Color.Gainsboro;
                }
                else
                {
                    // No occupants, not visited - do not draw
                    return;
                }
            }
            else
            {
                if (cell.Iteration == pathFinder.Iteration)
                {
                    // Occupants, visited
                    color = Color.Aqua;
                }
                else if (cell.NumBuildings > 0)
                {
                    // Buildings, not visited
                    color = Color.Beige;
                }
                else
                {
                    // Bots, not visited
                    color = Color.LavenderBlush;
                }
            }

            var destRect = new Rectangle(cell.X * CellSize, cell.Y * CellSize, CellSize, CellSize);

            spriteBatch.Draw(cellDebugTexture, destRect, sourceRect, color);
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
    }
}
