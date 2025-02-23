using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size) :
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier, numFrames, numCollums, size)
    {
        
    }
}