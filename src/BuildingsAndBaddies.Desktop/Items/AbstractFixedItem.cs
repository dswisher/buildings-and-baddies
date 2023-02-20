using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class AbstractFixedItem : AbstractMapItem
    {
        protected AbstractFixedItem(Texture2D texture, int x, int y, int width, int height, int frames)
            : base(texture, x, y, width, height, frames)
        {
        }
    }
}
