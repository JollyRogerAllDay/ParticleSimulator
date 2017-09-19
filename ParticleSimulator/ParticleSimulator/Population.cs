using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulator
{
    class Population
    {
        public Particle[] particles { get; set; }
        private int popSize { get; set; }
        public int[] _minPosition;
        public int[] _maxPosition;
        public Particle target;
        public double[,] pheromones;

        public Population ()
        {
            //Do nothing
        }

        public Population(int populationSize)
        {
            particles = new Particle[populationSize];

            for(int i=0; i < populationSize; i++)
            {
                particles[i] = new Particle("Particle" + i.ToString(), new int[] {0,0}, new int[] {0,0});
            }
        }

        public Population(Particle[] particleCol)
        {
            particles = particleCol;
            popSize = particles.Length;
        }

        //Randomly initialize particles in population
        public void RandomInit(int[] minPosition, int[] maxPosition)
        {
            Random rand = new Random();
            int randPos;
            _minPosition = minPosition;
            _maxPosition = maxPosition;


            foreach(Particle particle in particles)
            {
                for(int i=0; i < particle.pos.Length; i++)
                {
                    randPos = rand.Next(minPosition[i], maxPosition[i]);
                    particle.pos[i] = randPos;
                }
            }

            InitializePheromones();

            target = new Particle("target", new int[] { rand.Next(maxPosition[0]), rand.Next(maxPosition[1]) }, new int[] { 0, 0 });
        }

        //Randomly update the particles position
        public void RandomizePosition()
        {
            Random rand = new Random();
            int randPos;
            int[] minPosition ={ 0, 0 };
            int[] maxPosition = { 600,420};

            foreach (Particle particle in particles)
            {
                for (int i = 0; i < particle.pos.Length; i++)
                {
                    randPos = rand.Next(minPosition[i], maxPosition[i]);
                    particle.pos[i] = randPos;
                }

            }
        }

        //Initializes the pheromones to 0.0
        public void InitializePheromones()
        {
            pheromones = new double[_maxPosition[0], _maxPosition[1]];

            for (int i = 0; i < _maxPosition[0]; i++)
            {
                for (int j = 0; j < _maxPosition[1]; j++)
                {
                    foreach (Particle ant in particles)
                    {
                        if (ant.pos[0] == i && ant.pos[1] == j)
                        {
                            pheromones[i, j] = 1.0;
                        }
                        else
                        {
                            pheromones[i, j] = 0.0;
                        }
                    }
                }
            }
        }

        public virtual void UpdatePopulationPosition() { }
        public virtual double MaxPheromone() { return 0.0; }

        public Particle[] GetParticles()
        {
            return particles;
        }

        public void SetTarget(Particle newTarget)
        {
            target = newTarget;
        }

        public void SetTargetPosition(int x, int y)
        {
            target.pos[0] = x;
            target.pos[1] = y;
        }
    }
}
