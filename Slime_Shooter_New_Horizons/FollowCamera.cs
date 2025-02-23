using Microsoft.Xna.Framework;

namespace Slime_Shooter_New_Horizons;

public class FollowCamera
{
    public Vector2 position;
    
    public FollowCamera(Vector2 position)
    {
        this.position = position;
    }

    public void FollowTarget(Rectangle target, Vector2 screenSize)
    {
        position = new Vector2
        (
            -target.X + (screenSize.X / 2 - target.Width / 2), 
            -target.Y + (screenSize.Y / 2 - target.Height / 2)
        );
    }
}