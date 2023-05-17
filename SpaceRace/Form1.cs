using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Sonia Rusin-Franke, May 17th 2023: Space Race, a recreation of an Atari game where players race rockets to the top of the screen while avoiding asteroids
namespace SpaceRace
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
        }

        string state = "waiting";

        //create players
        Rectangle player1 = new Rectangle(200, 365, 10, 30);
        Rectangle player2 = new Rectangle(400, 365, 10, 30);
        int playerSpeed = 4;

        //set scores
        int p1Score = 0;
        int p2Score = 0;

        //create obstacle lists
        List <Rectangle> leftAsteroidList = new List<Rectangle>();
        List <Rectangle> rightAsteroidList = new List<Rectangle> ();
        int asteroidSpeed = 4;

        bool wDown = false;
        bool sDown = false;
        bool upDown = false;
        bool downDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush greenBrush = new SolidBrush(Color.LawnGreen);

        Random randGen = new Random();

       
        //resets game
        public void InitializeGame()
        {
            p1Score = 0;
            p2Score = 0;

            leftAsteroidList.Clear();
            rightAsteroidList.Clear();

            titleLabel.Text = "";
            subLabel.Text = "";

            p1ScoreLabel.Text = $"{p1Score}";
            p2ScoreLabel.Text = $"{p2Score}";

            player1.X = 200;
            player1.Y = 365;

            player2.X = 400;
            player2.Y = 365;

            state = "playing";
            gameTimer.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.Space:
                    if (state == "waiting" || state == "over")
                    {
                        InitializeGame();
                    }
                    break;
                case Keys.Escape:
                    if (state == "waiting" || state == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
            }
        }


        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move players
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            if (upDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (downDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            //generate asteroids
            if (randGen.Next(1,101) <= 5)
            {
                int y = randGen.Next(0, this.Height - 50);
                Rectangle aster = new Rectangle(600, y, 20, 3);
                rightAsteroidList.Add(aster);
            }
            else if (randGen.Next(1,101) <= 10)
            {
                int y = randGen.Next(0, this.Height - 50);
                Rectangle aster = new Rectangle(-20, y, 20, 3);
                leftAsteroidList.Add(aster);
            }

            //move asteroids
            for (int i = 0; i < rightAsteroidList.Count; i++)
            {
                if (rightAsteroidList[i].X > -20)
                {
                    int x = rightAsteroidList[i].X - asteroidSpeed;
                    rightAsteroidList[i] = new Rectangle(x, rightAsteroidList[i].Y, 20, 3);
                }
                else
                {
                    rightAsteroidList.RemoveAt(i);
                }

                if (player1.IntersectsWith(rightAsteroidList[i]))
                {
                    player1.Y = 365;
                }

                if (player2.IntersectsWith(rightAsteroidList[i]))
                {
                    player2.Y = 365;
                }
            }

            for (int i = 0; i < leftAsteroidList.Count; i++)
            {
                if (leftAsteroidList[i].X < 600)
                {
                    int x = leftAsteroidList[i].X + asteroidSpeed;
                    leftAsteroidList[i] = new Rectangle(x, leftAsteroidList[i].Y, 20, 3);
                }
                else
                {
                    leftAsteroidList.RemoveAt(i);
                }

                if (player1.IntersectsWith(leftAsteroidList[i]))
                {
                    player1.Y = 365;
                }

                if (player2.IntersectsWith(leftAsteroidList[i]))
                {
                    player2.Y = 365;
                }
            }

            if (player1.Y < 0)
            {
                p1Score += 1;
                p1ScoreLabel.Text = $"{p1Score}";
                player1.Y = 365;
            }

            if (player2.Y < 0)
            {
                p2Score += 1;
                p2ScoreLabel.Text = $"{p2Score}";
                player2.Y = 365;
            }

            if (p1Score == 5)
            {
                gameTimer.Stop();

                leftAsteroidList.Clear();
                rightAsteroidList.Clear();

                state = "over";

                titleLabel.Text = "PLAYER 1 WINS";
                subLabel.Text = "Press Space to Restart or Esc to Quit";
            }

            if (p2Score == 5)
            {
                gameTimer.Stop();

                leftAsteroidList.Clear();
                rightAsteroidList.Clear();

                state = "over";

                titleLabel.Text = "PLAYER 2 WINS";
                subLabel.Text = "Press Space to Restart or Esc to Quit";
            }

            Refresh();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (state == "playing")
            {
                e.Graphics.FillRectangle(whiteBrush, player1);
                e.Graphics.FillRectangle(whiteBrush, player2);

            }

            for (int i = 0; i < rightAsteroidList.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, rightAsteroidList[i]);
            }

            for (int i = 0;i < leftAsteroidList.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, leftAsteroidList[i]);
            }
            

        }


    }
}
