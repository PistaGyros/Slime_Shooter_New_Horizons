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
        }
        
        List<Vector2> rectVertices = new List<Vector2>
        {
            new Vector2(rect.X, rect.Y),
            new Vector2(rect.Right, rect.Y),
            new Vector2(rect.X, rect.Bottom),
            new Vector2(rect.Right, rect.Bottom),
        };
        foreach (var vertex in rectVertices)
        {
            if (IsPointWithinTriangle(vertex))
                return true;
        }
        
        return intersects;
    }

    public bool CointaintsRectangle(Rectangle rect)
    {
        bool cointaints = false;
        return cointaints;
    }

    public bool IsPointWithinRectangle(Vector2 point, Rectangle rect)
    {
        bool isWithin = false;
        if (point.X >= rect.X && point.X <= rect.X + rect.Width)
            if (point.Y >= rect.Y && point.Y <= rect.Y + rect.Height)
                isWithin = true;
        
        return isWithin;
    }

    public bool IsPointWithinTriangle(Vector2 point)
    {
        // Check if the point is on the same side as the third vertex
        if (SameSide(point, Verticies[2], Verticies[0], Verticies[1]))
            return true;
        else if (SameSide(point, Verticies[0], Verticies[1], Verticies[2]))
            return true;
        else if (SameSide(point, Verticies[1], Verticies[2], Verticies[0]))
            return true;
        else
            return false;
    }
    
    private bool SameSide(Vector2 point1, Vector2 point2, Vector2 a, Vector2 b)
    {
        Vector3 ab = new Vector3(b.X - a.X, b.Y - a.Y, 0);
        Vector3 aPont1 = new Vector3(point1.X - a.X, point1.Y - a.Y, 0);
        Vector3 aPont2 = new Vector3(point2.X - a.X, point2.Y - a.Y, 0);
        Vector3 crossProduct1 = Vector3.Cross(ab, aPont1);
        Vector3 crossProduct2 = Vector3.Cross(ab, aPont2);
        if (Vector3.Dot(crossProduct1, crossProduct2) >= 0)
            return true;
        else
            return false;
    }
}