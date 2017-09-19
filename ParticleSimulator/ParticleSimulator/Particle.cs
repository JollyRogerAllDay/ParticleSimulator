using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulator
{
    class Particle
    {
        public int[] pos;
        public int[] vel;
        public string _name;

        public Particle()
        {
            //Do nothing
        }

        public Particle (string name, int [] position, int[] velocity)
        {
            pos = position;
            vel = velocity;
            _name = name;
        }
    }
}
