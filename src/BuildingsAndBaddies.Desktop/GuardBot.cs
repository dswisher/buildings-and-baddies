using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class GuardBot : AbstractCreature
    {
        public GuardBot(Texture2D texture, int x, int y, SoundEffect impactSound)
            : base(texture, x, y, 28, 31, 3, impactSound)
        {
        }
    }
}
