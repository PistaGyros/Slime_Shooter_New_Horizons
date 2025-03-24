using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class PlortCollector : Building
{
    public Player player;
    public List<int> sellPrices;
    
    public PlortCollector(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier) : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        
    }
    
    

    public void Update(GameTime gameTime)
    {
        if (plortsList != null)
        {
            var outPutCheck = CheckForCollisionsWithPlorts();
            if (outPutCheck.Item1)
            {
                player.coins += sellPrices[outPutCheck.Item2.plortID];
                plortsList.Remove(outPutCheck.Item2);
                Console.WriteLine(player.coins);
            }
        }
    }
    
    public (bool, Plort) CheckForCollisionsWithPlorts()
    {
        bool collision = false;
        Plort collidedPlort = null;
        foreach (var plort in plortsList)
        {
            if(GetCollectorRectangle().Contains(plort.GetCollisionRectangle()))
            {
                collision = true;
                isThrowed = false;
                plort.isThrowed = false;
                collidedPlort = plort;
            }
                
        }
        return (collision, collidedPlort);
    }

    public Rectangle GetCollectorRectangle()
    {
        return new Rectangle((int)(destinationRectangle.X + 5 * scaleMultiplier), 
            (int)(destinationRectangle.Y + 4 * scaleMultiplier), 
            (int)(24 * scaleMultiplier), (int)(15 * scaleMultiplier));
    }
    
}