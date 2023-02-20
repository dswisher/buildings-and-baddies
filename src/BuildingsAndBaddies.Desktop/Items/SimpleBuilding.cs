// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class SimpleBuilding : AbstractFixedItem
    {
        public SimpleBuilding(Texture2D texture, int x, int y)
            : base(texture, x, y, texture.Width, texture.Height, 1)
        {
        }


        public override void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            if (debugMode)
            {
                spriteBatch.DrawRectangle(Bounds, Color.Green);
            }

            // Let the base class handle the rest
            base.Draw(spriteBatch, debugMode);
        }
    }
}
