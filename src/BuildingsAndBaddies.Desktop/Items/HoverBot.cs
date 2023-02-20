using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class HoverBot : AbstractMovableItem
    {
        public HoverBot(Texture2D texture, int x, int y)
            : base(texture, x, y, 32, 30, 3, 250)
        {
        }
    }
}
