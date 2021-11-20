using System;
using System.Collections.Generic;
using System.Numerics;


namespace Smart_Rockets
{
    class DNA
    {
        public Vector2[] genes { get; set; } = new Vector2[Program.LIFESPAN];

        const float VECTOR_SIZE = 0.2f;
        const double mutationRate = 0.002;

        Random random = new Random();

        public DNA()
        {
            for (int i = 0; i < Program.LIFESPAN; i++)
            {
                genes[i] = Program.GetRandomVector2();
                genes[i] *= VECTOR_SIZE;
            }
        }

        public DNA (Vector2[] createdGenes)
        {
            genes = createdGenes;
        }

        public DNA Crossover (DNA other)
        {
            Vector2[] newGenes = new Vector2[Program.LIFESPAN];
            int mid = (int)(random.Next(Program.LIFESPAN)); // Where the DNA starts taking from the other

            for (int i = 0; i < Program.LIFESPAN; i++)
            {
                if (i > mid) newGenes[i] = genes[i];
                else newGenes[i] = other.genes[i];

                //if (random.NextDouble() > 0.5) newGenes[i] = genes[i];
                //else newGenes[i] = other.genes[i];
            }

            return new DNA(newGenes);
        }

        public void Mutation()
        {
            for (int i = 0; i < genes.Length; i++)
            {
                if (random.NextDouble() < mutationRate) genes[i] = Program.GetRandomVector2() * VECTOR_SIZE;
            }
        }
    }
}
