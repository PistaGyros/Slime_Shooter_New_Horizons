using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class DayNightCycle
{
    public double elapsedTime;
    private Texture2D globalLightFakeTexture;
    private Vector2 screenRes;
    
    public DayNight dayOrNight;
    // Alpha value
    private float currentGlobalLightIntensity = 0;
    
    public DayNightCycle(Texture2D globalLightFakeTexture)
    {
        this.globalLightFakeTexture = globalLightFakeTexture;
    }

    public new void Update(Vector2 screenRes, double elapsedTime)
    {
        this.elapsedTime = elapsedTime;
        this.screenRes = screenRes;
        
        DayOrNight();
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        Color nightColor = new Color(Color.MidnightBlue, currentGlobalLightIntensity);
        spriteBatch.Draw(
            globalLightFakeTexture, 
            new Rectangle(0, 0, (int)(screenRes.X * 10), (int)(screenRes.Y * 10)),
            new Rectangle(0, 0, 1, 1), 
            nightColor);
    }

    private void DayOrNight()
    {
        switch (elapsedTime)
        {
            case >= 18000 and <= 72000:
                dayOrNight = DayNight.Day;
                currentGlobalLightIntensity = 0f;       
                break;
            case < 18000 or > 72000:
                dayOrNight = DayNight.Night;
                currentGlobalLightIntensity = 0.5f;
                break;
        }
    }
}