using System;
using Raylib_cs;
using System.Numerics;

namespace Smart_Rockets
{
    class Rocket
    {
        Vector2 pos = new Vector2();
        Vector2 vel = new Vector2();
        Vector2 acc = new Vector2();

        public DNA dna { get; set; }

        const int width = 35;
        const int height = 8;
        Color color = Color.WHITE;
        const float forceLimit = 4;

        public bool Completed { get { return _completed; } }
        bool _completed = false;

        bool _crashed = false;

        public double fitness { get; set; } = 0;
        int timeToFinish = Program.LIFESPAN;

        public Rocket(DNA child=null)
        {
            pos.X = Program.WIDTH / 2;
            pos.Y = Program.HEIGHT - 20;

            if (!(child is null)) dna = child;
            else dna = new DNA();
        }

        public void ApplyForce(Vector2 force)
        {
            acc += force;
        }

        public void Update()
        {
            double d = Vector2.Distance(pos, Program.GetTargetPos());
            if (d < Program.target.size + 10 && !_completed)
            {
                _completed = true;
                pos = Program.GetTargetPos();
                timeToFinish = Program.Count;
            }

            Edges();

            if (Program.Count < Program.LIFESPAN)
            {
                ApplyForce(dna.genes[Program.Count]);
            }

            if (_completed || _crashed) return;

            vel += acc;
            pos += vel;
            acc *= 0;

            if (vel.Length() > forceLimit) vel = Vector2.Normalize(vel) * forceLimit;
        }

        public void Display(bool highlighted = false)
        {
            Vector2[] boxPoints = new Vector2[4];
            boxPoints[0] = new Vector2(pos.X - width / 2, pos.Y - height / 2) - pos;
            boxPoints[1] = new Vector2(pos.X + width / 2, pos.Y - height / 2) - pos;
            boxPoints[2] = new Vector2(pos.X + width / 2, pos.Y + height / 2) - pos;
            boxPoints[3] = new Vector2(pos.X - width / 2, pos.Y + height / 2) - pos;

            double angle = Program.GetAngleBetweenVector2AndX(vel);

            for (int i = 0; i < boxPoints.Length; i++)
            {
                boxPoints[i] = Program.RotateVector2(boxPoints[i], angle);
                boxPoints[i] += pos;
            }
            
            for (int i = 0; i < boxPoints.Length-1; i++)
            {
                if (!highlighted) Raylib.DrawLineV(boxPoints[i], boxPoints[i + 1], color);
                else Raylib.DrawLineEx(boxPoints[i], boxPoints[i + 1], 2, Color.RED);
            }
            if (!highlighted) Raylib.DrawLineV(boxPoints[boxPoints.Length - 1], boxPoints[0], color);
            else Raylib.DrawLineEx(boxPoints[boxPoints.Length - 1], boxPoints[0], 2, Color.RED);

            //Raylib.DrawLineV(pos, pos + vel * 500, color);

        }

        private void Edges()
        {
            //if (pos.X > Program.WIDTH || pos.X < 0)
            //{
            //    vel.X *= -1;
            //}
            //if (pos.Y > Program.HEIGHT || pos.Y < 0)
            //{
            //    vel.Y *= -1;
            //}

            if (pos.X > Program.WIDTH || pos.X < 0 || pos.Y > Program.HEIGHT || pos.Y < 0)
            {
                _crashed = true;
            }
        }

        public void CalcFitness()
        {
            double dist = Vector2.Distance(pos, Program.GetTargetPos());

            if (dist == 0) fitness = 1;
            else fitness = Math.Pow(1 / dist, 3);

            fitness *= Math.Pow((Program.LIFESPAN - timeToFinish), 2) * 1000 + 1;

            if (_crashed) fitness /= 1000;

        }

        public void CheckAndResolveCollision(Obstacle obstacle)
        {
            if (_crashed) return;
            if (obstacle.CheckCollision(pos))
            {
                _crashed = true;
            }
        }

        public void Reset()
        {
            pos.X = Program.WIDTH / 2;
            pos.Y = Program.HEIGHT - 20;

            vel.X = 0;
            vel.Y = 0;

            acc.X = 0;
            acc.Y = 0;

            _completed = false;
            _crashed = false;
        }
    }
}
