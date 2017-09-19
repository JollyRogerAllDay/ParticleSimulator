using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ParticleSimulator
{
    class World
    {

        public Algorithm algorithm { get; set; }
        public int[] _minSize;
        public int[] _maxSize;
        public Population pop;
        private Population originalPop;
        private int popSize;
        private int epoch = 0;

        // Creates a World with a population of the given size
        public World (int populationSize)
        {
            popSize = populationSize;
        }

        public World(Population population)
        {
            pop = population;
        }

        public void StartAlgorithm()
        {

        }

        public void NextIteration()
        {
            //pop.RandomizePosition();
            //TODO: Just run ant colony algorithm until I find a better way to encapusulate updating population
            //manange ant activity
            pop.UpdatePopulationPosition();
            //manage pheromone
            //manage daemon action
            //select procedure
            //Computer Solution quality
            epoch++;
        }
        
        public void InitializePopulation(int[] minSize, int[] maxSize)
        {
            _minSize = minSize;
            _maxSize = maxSize;
            //Randomly initialize for now. This can be improved later
            pop.RandomInit(_minSize, _maxSize);
            originalPop = pop;
        }

        public void Draw(Graphics g)
        {
            //Draw population
            Pen mypen = new Pen(Brushes.Black);
            Rectangle particleRect;
            Color colour;

            for (int i = 0; i < pop.pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < pop.pheromones.GetLength(1); j++)
                {
                    //Draw pheromones, the higher the context the darker the colour
                    if (pop.pheromones[i, j] != 0.0)
                    {
                        colour =  (Color.FromArgb((int)Math.Floor(255.0 * Utils.Normalize(pop.pheromones[i, j], 0.0, pop.MaxPheromone())), 100, 255, 100));
                        particleRect = new Rectangle(i, j, 2, 2);
                        mypen = new Pen(new SolidBrush(colour));
                        g.DrawRectangle(mypen, particleRect);
                        g.FillRectangle(Brushes.Green, particleRect);
                    }
                }
            }

            foreach (Particle particle in pop.GetParticles())
            {
                particleRect = new Rectangle(particle.pos[0], particle.pos[1], 5, 5);
                g.DrawRectangle(mypen, particleRect);
                g.FillRectangle(Brushes.Black, particleRect);
            }

            //Draw target
            mypen = new Pen(Brushes.Red);
            Rectangle targetRect = new Rectangle(pop.target.pos[0], pop.target.pos[1], 10, 10);
            g.DrawRectangle(mypen, targetRect);
            g.FillRectangle(Brushes.Red, targetRect);
        }


        //Resets the population back to its original state of intialization
        public void ResetPopulationToOriginalState()
        {
            pop.RandomInit(_minSize, _maxSize);
        }

        public void WriteToConsole()
        {
            Console.WriteLine("Population: size=" + popSize);
            Console.WriteLine("|  Name  |  Position (x,y)  |  Velocity (dx, dy)  |");
            foreach (Particle particle in pop.particles)
            {
                Console.WriteLine(particle._name 
                                    + " (" + particle.pos[0] + "," + particle.pos[1] + ")" 
                                    + " (" + particle.vel[0] + "," + particle.vel[1] + ")");
            }
        }

        public int GetEpoch()
        {
            return epoch;
        }

        public void SetTargetPosition(int x, int y)
        {
            pop.SetTargetPosition(x, y);
        }
    }
}
