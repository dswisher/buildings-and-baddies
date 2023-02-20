using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class GuardBot : AbstractCreature
    {
        public GuardBot(Texture2D texture, int x, int y)
            : base(texture, x, y, 28, 31, 3, 150)
        {
        }
    }
}
