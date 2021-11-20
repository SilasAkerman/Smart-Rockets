using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Smart_Rockets
{
    class Program
    {
        public const int WIDTH = 1200;
        public const int HEIGHT = 1000;

        public const int LIFESPAN = 600;

        public static int Count { get; set; } = 0;

        static Population population;
        public static Target target;

        static List<Obstacle> obstacles = new List<Obstacle>();

        static bool viewBestPrevious = false;
        static Rocket bestPrevious;

        static void Main(string[] args)
        {
            Raylib.InitWindow(WIDTH, HEIGHT, "Smart Rockets");
            Raylib.SetTargetFPS(0);

            Init();

            while (!Raylib.WindowShouldClose())
            {
                Display();
                Update();
            }

            Raylib.CloseWindow();
        }

        static void Init()
        {
            population = new Population();
            target = new Target();

            obstacles.Add(new Obstacle
            {
                Pos = new Vector2(900, 700),
                Width = 700,
                Height = 10
            });
            obstacles.Add(new Obstacle
            {
                Pos = new Vector2(300, 900),
                Width = 10,
                Height = 900
            });
            obstacles.Add(new Obstacle
            {
                Pos = new Vector2(600, 400),
                Width = 500,
                Height = 10
            });
        }

        static void Display()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            if (viewBestPrevious) bestPrevious.Display();
            else population.Display();
            if (bestPrevious is not null) bestPrevious.Display(true);
            target.Display();

            foreach (Obstacle obstacle in obstacles) obstacle.Display();

            Raylib.DrawFPS(50, 50);
            Raylib.EndDrawing();
        }

        static void Update()
        {
            population.Update();
            population.CheckAndResolveRocketCollision(obstacles);

            if (!(bestPrevious is null))
            {
                bestPrevious.Update();
                foreach (Obstacle obstacle in obstacles) bestPrevious.CheckAndResolveCollision(obstacle);
            }

            Count++;
            if (Count == LIFESPAN)
            {
                population.Evaluate();

                bestPrevious = population.GetBestFromPrevious();
                bestPrevious.Reset();

                population.Selection();
                Count = 0;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) target = new Target();
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON)) target = new Target(Raylib.GetMousePosition());
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) Init();
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_B) && bestPrevious is not null) viewBestPrevious = !viewBestPrevious;
        }

        public static Vector2 RotateVector2(Vector2 orig, double angle)
        {
            Vector2 rotated = new Vector2(orig.X, orig.Y);
            rotated.X = (float)((Math.Cos(angle) * orig.X) - (Math.Sin(angle) * orig.Y));
            rotated.Y = (float)((Math.Sin(angle) * orig.X) + (Math.Cos(angle) * orig.Y));

            return rotated;
        }

        public static double GetAngleBetweenVector2AndX(Vector2 a)
        {
            return Math.Atan2(a.Y, a.X);
        }

        public static Vector2 GetRandomVector2()
        {
            Random rand = new Random();
            Vector2 vect = Vector2.UnitX;
            vect = RotateVector2(vect, rand.NextDouble() * Math.PI * 2);
            return vect;
        }

        public static Vector2 GetTargetPos()
        {
            return target.getPos();
        }
    }
}
