using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Slime_Shooter_New_Horizons;

public struct Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
{
    public Vector2 v1 = new Vector2(x1, y1);
    public Vector2 v2 = new Vector2(x2, y2);
    public Vector2 v3 = new Vector2(x3, y3);
    public List<Vector2> Verticies => [v1, v2, v3];
    public Vector2 ALine => new(v3.X - v2.X, v3.Y - v2.Y);
    public Vector2 BLine => new(v3.X - v1.X, v3.Y - v1.Y);
    public Vector2 CLine => new(v2.X - v1.X, v2.Y - v1.Y);

    public float scaleMultiplier;
    private Texture2D colliderTexture;
    
    private Rectangle aLineRec;
    private Rectangle bLineRec;
    private Rectangle cLineRec;
    private float aLineRotation;
    private float bLineRotation;
    private float cLineRotation;

    public int LineThickness = 1;


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
    

    public void DrawCollider(SpriteBatch spriteBatch, Vector2 offset)
    {
        spriteBatch.Draw(colliderTexture, aLineRec, new Rectangle(0, 0, 3, 3), Color.White, aLineRotation, 
            Vector2.Zero, SpriteEffects.None, 0);
        spriteBatch.Draw(colliderTexture, bLineRec, new Rectangle(0, 0, 3, 3), Color.White, bLineRotation, 
            Vector2.Zero, SpriteEffects.None, 0);
        spriteBatch.Draw(colliderTexture, cLineRec, new Rectangle(0, 0, 3, 3), Color.White, cLineRotation,
            Vector2.Zero, SpriteEffects.None, 0);
    }
    

    public void CreateCollider(GraphicsDevice graphicsDevice, Texture2D colliderTexture)
    {
        this.colliderTexture = colliderTexture;
        Vector2 aLineSize = new Vector2(ALine.Length(), LineThickness);
        aLineRec = new Rectangle((int)v2.X, (int)v2.Y, (int)aLineSize.X, (int)aLineSize.Y);
        Vector2 bLineSize = new Vector2(BLine.Length(), LineThickness);
        bLineRec = new Rectangle((int)v3.X, (int)v3.Y, (int)bLineSize.X, (int)bLineSize.Y);
        Vector2 cLineSize = new Vector2(CLine.Length(), LineThickness);
        cLineRec = new Rectangle((int)v1.X, (int)v1.Y, (int)cLineSize.X, (int)cLineSize.Y);
        aLineRotation = MathF.Acos(Vector2.Dot(ALine, -Vector2.One) / Vector2.DistanceSquared(ALine, -Vector2.One));
        if (float.IsNaN(aLineRotation))
            aLineRotation = 0;
        bLineRotation = MathF.Acos(Vector2.Dot(BLine, -Vector2.One) / Vector2.DistanceSquared(BLine, -Vector2.One));
        if (float.IsNaN(bLineRotation))
            bLineRotation = 0;
        cLineRotation = MathF.Acos(Vector2.Dot(CLine, -Vector2.One) / Vector2.DistanceSquared(CLine, -Vector2.One));
        if (float.IsNaN(cLineRotation))
            cLineRotation = 0;
        Console.WriteLine(aLineRec + " " + bLineRec + " " + cLineRec);
        Console.WriteLine(aLineRotation + " " + bLineRotation + " " + cLineRotation);
    }
}