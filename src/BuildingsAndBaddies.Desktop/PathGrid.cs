// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
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
            foreach (var cell in GetCells(bounds))
            {
                cell.NumOccupants += 1;
            }
        }


        public void RemoveItem(Rectangle bounds)
        {
            foreach (var cell in GetCells(bounds))
            {
                cell.NumOccupants -= 1;
            }
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
            int x1 = mapRect.Left / CellSize;
            int x2 = mapRect.Right / CellSize;
            int y1 = mapRect.Top / CellSize;
            int y2 = mapRect.Bottom / CellSize;

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
