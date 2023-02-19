using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class TreadBot : AbstractCreature
    {
        public TreadBot(Texture2D texture, int x, int y, SoundEffect impactSound)
            : base(texture, x, y, 28, 31, 5, impactSound)
        {
        }
    }
}
