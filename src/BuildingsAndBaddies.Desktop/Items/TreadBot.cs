// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class TreadBot : AbstractMovableItem
    {
        public TreadBot(Texture2D texture, int x, int y)
            : base(texture, x, y, 28, 31, 5, 50)
        {
        }
    }
}
