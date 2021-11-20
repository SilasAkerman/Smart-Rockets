using System;
using System.Numerics;
using Raylib_cs;

namespace Smart_Rockets
{
    class Obstacle
    {
        public Vector2 Pos { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        Color color = Color.WHITE;

        public Obstacle()
        {

        }

        public void Display()
        {
            Raylib.DrawRectangle((int)Pos.X - Width/2, (int)Pos.Y - Height/2, Width, Height, color);
        }

        public bool CheckCollision(Vector2 point)
        {
            Rectangle rect = new Rectangle(Pos.X - Width / 2, Pos.Y - Height / 2, Width, Height);
            return Raylib.CheckCollisionPointRec(point, rect);
        }
    }
}
