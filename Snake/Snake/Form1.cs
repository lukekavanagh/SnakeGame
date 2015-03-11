using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            // set settings to default
            new Settings();

            // set game speed and start timer
            GameTimer.Interval = 1000 / Settings.Speed;
            GameTimer.Tick += UpdateScreen;
            GameTimer.Start();

            //start new game
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();
            // create new player object
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }
        //Place random food object somewhere in the game scheme
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            //check for game over
            if (Settings.Gameover == true)
            {
                if (Inputs.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Inputs.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Inputs.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Inputs.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Inputs.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            {
                Graphics canvas = e.Graphics;
                if (!Settings.Gameover)
                {
                    // set colour of snake
                    Brush snakeColour;
                    //draw snake
                    for (int i = 0; i < Snake.Count; i++)
                    {
                        //draw head
                        if (i == 0)
                            snakeColour = Brushes.Black;//draw head
                        else
                            snakeColour = Brushes.Green; //rest of body
                        //draw snake
                        canvas.FillEllipse(snakeColour,
                            new Rectangle(Snake[i].X * Settings.Width,
                                Snake[i].Y * Settings.Height,
                                Settings.Width, Settings.Height));

                        //Draw food
                        canvas.FillEllipse(Brushes.Red,
                            new Rectangle(food.X * Settings.Width,
                                food.Y * Settings.Height, Settings.Width, Settings.Height));
                    }
                }
                else
                {
                    string gameOver = "Game Over \nYour final score is: " + Settings.Score + "\nPress enter to try again";
                    lblGameOver.Text = gameOver;
                    lblGameOver.Visible = true;
                }
            }
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count -1; i >= 0; i--)
            {
                //move head
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    //get maximum x and y pos
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Width;

                    //detect collision with game borders
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXpos || Snake[i].Y >= maxYpos)
                    {
                        Die();
                    }


                    //detect collision with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                            Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    // detect collision with food piece
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    //move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;

                }
                                            
        }
            }
        private void Eat()
        {
            //Add Circle to body
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            //update score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }
        private void Die()
        {
            Settings.Gameover = true;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Inputs.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Inputs.ChangeState(e.KeyCode, false);
        }

        }

    }


        
        
    
