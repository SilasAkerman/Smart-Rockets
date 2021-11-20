using System;
using System.Numerics;
using Raylib_cs;

namespace Smart_Rockets
{
    class Target
    {
        Vector2 pos = new Vector2(Program.WIDTH/2, 75);
        public int size = 15;
        Color color = Color.WHITE;

        public Target()
        {
            Random rand = new Random();
            pos.X = rand.Next(150, Program.WIDTH - 150);
            pos.Y = rand.Next(75, Program.HEIGHT / 3);
        }

        public Target(Vector2 mouse)
        {
            pos = mouse;
        }

        public void Display()
        {
            Raylib.DrawCircleV(pos, size, color);
        }

        public Vector2 getPos()
        {
            return pos;
        }
    }
}
