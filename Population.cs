using System;
using System.Collections.Generic;


namespace Smart_Rockets
{
    class Population
    {
        List<Rocket> rockets = new List<Rocket>();
        List<Rocket> matingPool = new List<Rocket>();

        const int popsize = 1000;

        Random random = new Random();

        public Population()
        {
            for (int i = 0; i < popsize; i++)
            {
                rockets.Add(new Rocket());
            }
        }

        public void Update()
        {
            foreach (Rocket rocket in rockets)
            {
                rocket.Update();
            }
        }

        public void Display()
        {
            foreach (Rocket rocket in rockets)
            {
                rocket.Display();

            }
        }

        public void Evaluate()
        {
            double maxFit = 0;
            foreach (Rocket rocket in rockets)
            {
                rocket.CalcFitness();
                if (rocket.fitness > maxFit) maxFit = rocket.fitness;
            }

            Console.WriteLine(maxFit);

            //Normalize
            foreach (Rocket rocket in rockets)
            {
                rocket.fitness /= maxFit;
            }

            matingPool.Clear();
            foreach (Rocket rocket in rockets)
            {
                int n = (int)(rocket.fitness * 100); // The best rocket will appear 100 times in the matingpool
                for (int i = 0; i < n; i++)
                {
                    matingPool.Add(rocket);
                }
            }
        }

        public void Selection()
        {
            List<Rocket> newRockets = new List<Rocket>();
            for (int i = 0; i < rockets.Count; i++)
            {
                DNA parentA = matingPool[random.Next(0, matingPool.Count - 1)].dna;
                DNA parentB = matingPool[random.Next(0, matingPool.Count - 1)].dna;
                DNA child = parentA.Crossover(parentB);
                child.Mutation();
                newRockets.Add(new Rocket(child));
            }
            rockets = newRockets;
        }

        public void CheckAndResolveRocketCollision(List<Obstacle> obstacles)
        {
            foreach (Rocket rocket in rockets)
            {
                foreach (Obstacle obstacle in obstacles) rocket.CheckAndResolveCollision(obstacle);
            }
        }

        public Rocket GetBestFromPrevious()
        {
            Rocket best = rockets[0];
            foreach (Rocket rocket in rockets)
            {
                if (rocket.fitness > best.fitness) 
                {
                    best = rocket;
                }
            }
            return best;
        }
    }
}
