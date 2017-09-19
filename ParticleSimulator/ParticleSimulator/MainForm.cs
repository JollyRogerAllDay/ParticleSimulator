using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ParticleSimulator
{
    public partial class MainForm : Form
    {
        private World world;
        private Bitmap displayBmp;
        private System.Timers.Timer timer = new System.Timers.Timer();

        public MainForm()
        {
            InitializeComponent();
            displayBmp = new Bitmap (displayPictureBox.Size.Width, displayPictureBox.Size.Height);
            displayPictureBox.Image = displayBmp;
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            Graphics g;
            g = Graphics.FromImage(displayBmp);

            g.Clear(Color.White);
            g.DrawRectangle(new Pen(Brushes.Black), 0,0,100,100);
            g.Dispose();

            textBox1.Text = "Population Size";
        }

        public void Draw()
        {
            Graphics g = Graphics.FromImage(displayBmp);

            g.Clear(Color.White);
            world.Draw(g);
            displayPictureBox.Image = displayBmp;

            g.Dispose();
        }

        public void DrawTarget(Point coordinates)
        {
            Graphics g = Graphics.FromImage(displayBmp);
            Pen mypen = new Pen(Brushes.Red);
            g.DrawRectangle(mypen, coordinates.X, coordinates.Y, 10, 10);
            displayPictureBox.Image = displayBmp;
        }

        private void IterateSimulation()
        {
            world.NextIteration();
            UpdateDisplayText();
            Draw();
        }

        // Handle the TrackBar.ValueChanged event by calculating a value for
        // TextBox1 based on the TrackBar value.  
        private void TrackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            var value = trackBar1.Value;
            timer.Interval = value;
            Console.WriteLine("Value changed:" + value);
        }

        /*
         * Button Controls
         * */
        private void InitializeButton_Click(object sender, EventArgs e)
        {
            int populationSize = Convert.ToInt32(textBox1.Text);
            string algorithm = comboBox1.SelectedItem.ToString();
            Console.WriteLine("Track Bar Value in Initialize:" + trackBar1.Value);

            displayPictureBox.Enabled = true;
            startButton.Enabled = true;
            restartButton.Enabled = true;

            //Initialize Speed Track Bar
           // trackBar1.ValueChanged +=
             //   new System.EventHandler(TrackBar1_ValueChanged);

            //Initialize Timer
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            timer.Interval = Convert.ToInt32(trackBar1.Value*100);
            timer.Enabled = false;

            //Initialize the worlds parameters
            switch (algorithm)
            {
                case "Ant Colony":
                    world = new World(new AntColony(populationSize));
                    world.algorithm = Algorithm.ANT_COLONY;
                    //world.InitializePopulation(new int[] { 0, 0 }, new int[] { displayPictureBox.Size.Width, displayPictureBox.Size.Height });
                    world.InitializePopulation(new int[] { 0, 0 }, new int[] { 100, 100 });
                    world.WriteToConsole();
                    break;
                case "Particle Swarm":
                default:
                    Console.WriteLine("[ERROR] Unknown algorithm.");
                    break;
            }

            Draw();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var maxIterations = 1000;
            int currentEpoch = world.GetEpoch();
            timer.Start();
            //while (currentEpoch < maxIterations)//OR Optimal solution found
            //world.NextIteration
                //manange ant activity
                //manage pheromone
                //manage daemon action
                //select procedure
                //Computer Solution quality
                //timer.Tick
            //Choose best candidate
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            world.ResetPopulationToOriginalState();
            Draw();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        /*
         * Timer Functionality
         * */
        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            IterateSimulation();
        }

        delegate void UpdateDisplayTextCallback();

        private void UpdateDisplayText()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                UpdateDisplayTextCallback d = new UpdateDisplayTextCallback(UpdateDisplayText);
                try
                {
                    this.Invoke(d);
                }
                catch { };
            }
            else
            {
                label6.Text = world.GetEpoch().ToString();
                labelTargetPosX.Text = world.pop.target.pos[0].ToString();
                labelTargetPosY.Text = world.pop.target.pos[1].ToString();
            }
        }

        /* 
         * Listener for what to do when the picture box is clicked
         * */
        private void displayPictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            world.SetTargetPosition(coordinates.X, coordinates.Y);
            UpdateDisplayText();
            DrawTarget(coordinates);
        }

        public void TrackBar1_ValueChanged()
        {

        }
    }
}
