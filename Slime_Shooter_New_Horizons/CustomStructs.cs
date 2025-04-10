using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public struct Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
{
    public Vector2 v1 = new Vector2(x1, y1);
    public Vector2 v2 = new Vector2(x2, y2);
    public Vector2 v3 = new Vector2(x3, y3);

    public List<Vector2> Verticies
    {
        get
        {
            return new List<Vector2> { v1, v2, v3 };
        }
    }

    public bool IntersectsRectangle(Rectangle rect)
    {
        bool intersects = false;
        foreach (var vertex in Verticies)
        {
            if (IsPointWithinRectangle(vertex, rect))
                return true;
            else if ()

        }
        
        return intersects;
    }

    public bool CointaintsRectangle(Rectangle rect)
    {
        bool cointaints = false;
        return cointaints;
    }

    private bool IsPointWithinRectangle(Vector2 vertex, Rectangle rect)
    {
        bool isWithin = false;
        if (vertex.X >= rect.X && vertex.X <= rect.X + rect.Width)
            if (vertex.Y >= rect.Y && vertex.Y <= rect.Y + rect.Height)
                isWithin = true;
        
        return isWithin;
    }

    private bool RectangleLinesIntersect(List<Vector2> verticies, Rectangle rect)
    {
        bool intersects = false;
        Vector3 line
        
        return intersects;
    }
}