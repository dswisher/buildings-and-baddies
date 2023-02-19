using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class HoverBot : AbstractCreature
    {
        public HoverBot(Texture2D texture, int x, int y, SoundEffect impactSound)
            : base(texture, x, y, 32, 30, 3, impactSound)
        {
        }
    }
}
