using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulator
{
    class AntColony : Population
    {

        //Initiliazes the population of ants, setting pheromones to 0
        public AntColony(int populationSize)
        {
            particles = new Particle[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                particles[i] = new Particle("Ant" + i.ToString(), new int[] { 0, 0 }, new int[] { 0, 0 });
            }
        }

        //Updates the population's position based on criteria specific to algorithm
        public override void UpdatePopulationPosition()
        {
            pheromones = UpdatePheromones();

            /* [ ] [ ] [ ]
             * [ ] [A] [ ]
             * [ ] [ ] [ ]
             * */
            var movementProbability = new double[9];

            var x = 0;
            var y = 0;

            var sum = 0.0;

            //calculate sum to use in probability calculation
            foreach (Particle ant in particles)
            {
                x = ant.pos[0];
                y = ant.pos[1];

                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if(WithinBounds(x + i, y + j))
                        {
                            sum = sum + pheromones[x + i, y + j] * (1 / Utils.Distance(ant.pos, new int[] { x + i, y + j }));
                        }
                    }
                }

            }

            Random r = new Random();

            //Calculate probability of each position around the ant and update position
            foreach (Particle ant in particles)
            {
                x = ant.pos[0];
                y = ant.pos[1];
                int index = 0;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (WithinBounds(x + i, y + j))
                        {
                            movementProbability[index] = (pheromones[x + i, y + j] * (1 / Utils.Distance(ant.pos, new int[] { x + i, y + j }))) / sum;
                        }
                        else
                        {
                            movementProbability[index] = 0.0;
                        }
                        index += 1;
                    }
                }


                var randValue = r.NextDouble();
                var cumulative = 0.0;

                /* [0] [1] [2]
                 * [3] [A] [5]
                 * [6] [7] [8]
                 * */
                var selectionIndex = 4; //default to be the centre postion (current ant's position)

                for (int i = 0; i < movementProbability.Length; i++)
                {
                    cumulative += movementProbability[i];
                    if (randValue < cumulative)
                    {
                        selectionIndex = i;
                        break;
                    }
                }

                //Nothing was chosen, pick a random direction
                if(selectionIndex == 4)
                {
                    selectionIndex = r.Next(0, 9);
                }

                //Update the ant's position to the position specified by the selectionIndex
                int k=0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if(k == selectionIndex && WithinBounds(ant.pos[0] + i, ant.pos[1] + j))
                        {
                            ant.pos[0] = ant.pos[0] + i;
                            ant.pos[1] = ant.pos[1] + j;
                        }
                        k += 1;
                    }
                }
            }

        }

        /*
         * Updates the pheromones. Strengthening those that have ants with good heuristics scores.
         * Weakening the ones that don't.
         * */
        private double [,] UpdatePheromones()
        {
            var newPheromones = new double[pheromones.GetLength(0), pheromones.GetLength(1)];
            var decayCoefficient = 0.1;

            for (int i = 0; i < pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < pheromones.GetLength(1); j++)
                {
                    newPheromones[i, j] = (1.0 - decayCoefficient) * pheromones[i, j] + AmountPheromoneDeposit(i,j) ;
                }
            }
            
            return newPheromones;
        }

        /* Calculates the amount of pheromones to deposit for the population.
         * 
         * */
        public double AmountPheromoneDeposit(int x, int y)
        {
            
            foreach (Particle ant in particles)
            {
                if (ant.pos[0] == x && ant.pos[1] == y)
                {
                    return CalculateHeuristic(ant);
                }
            }

            return 0.0;
        }

        /*
         * Calculates the heurstic for determining the score of the given Ant
         * @arg Particle ant The ant to calculate the heuristic
         * @return double The score of the heuristic
         * */
        public double CalculateHeuristic(Particle ant)
        {
            return 1/Utils.Distance(ant.pos, target.pos);
        }

        /*
         * Sets the target of the population
         * */
        public void SetTarget(Particle newTarget)
        {
            target = newTarget;
        }

        //Returns true if the given x and y coordinates are within the worlds limits. False otherwise
        public Boolean WithinBounds(int x, int y)
        {
            return (x < _maxPosition[0] && y < _maxPosition[1]) && (x > _minPosition[0] && y > _minPosition[1]);
        }

        public double maxPheromone()
        {
            double maxValue = 0.0;

            for (int i = 0; i < pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < pheromones.GetLength(1); j++)
                {
                    if (pheromones[i, j] > maxValue)
                    {
                        maxValue = pheromones[i, j];
                    }

                }
            }

            return maxValue;
        }
    }
}
