using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace Pong
{
    public partial class Form1 : Form
    {
        Rectangle p1goal = new Rectangle(0, 120, 10, 90);
        Rectangle p2goal = new Rectangle(440, 120, 10, 90);

        Rectangle player1 = new Rectangle(20, 160, 30, 30);
        Rectangle player2 = new Rectangle(300, 160, 30, 30);
        Rectangle ball = new Rectangle(295, 220, 10, 10);

        int player1Score = 0;
        int player2Score = 0;
        int scoreNeededToWin = 10;

        float Xplayer1Speed = 0f;
        float Yplayer1Speed = 0f;

        float Xplayer2Speed = 0f;
        float Yplayer2Speed = 0f;

        float startingSpeed = 1f;
        float playerAcceleration = 1.20f;
        float playerDecceleration = 0.85f;
        float maxPlayerSpeed = 6f;
        int ballXSpeed = 8;
        int ballYSpeed = -8;



        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;

        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);

        SoundPlayer AirHorn = new SoundPlayer(Properties.Resources.Air_Horn);
        SoundPlayer BallBounce = new SoundPlayer(Properties.Resources.Ball_Bounce);
        SoundPlayer YayCheering = new SoundPlayer(Properties.Resources.person_cheering);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(redBrush, player2);
            e.Graphics.FillEllipse(whiteBrush, ball);
            e.Graphics.FillRectangle(blueBrush, p1goal);
            e.Graphics.FillRectangle(redBrush, p2goal);
        }




        //all the code for pressing keys is below
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
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;

                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
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
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;


                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }
        //all the code for pressing keys is above








        //big 'ol tick method
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            int ballXPosition = ball.X;
            int p1XPosition = player1.X;
            int p2XPosition = player2.X;
            int ballYPosition = ball.Y;
            int p1YPosition = player1.Y;
            int p2YPosition = player2.Y;

            //move ball 
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;


            //check if ball hit top or bottom wall and change direction if it does 
            if (ball.Y < 0 || ball.Y > this.Height - ball.Height)
            {
                ballYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed; 
            }




            // player 1 movement:

            doplayerMovement(wDown, sDown, aDown, dDown, ref player1, ref Yplayer1Speed, ref Xplayer1Speed);
            
            //put a limit on the speed of player 1
            limitSpeed(ref Xplayer1Speed, ref Yplayer1Speed);

            player1.X += (int)Xplayer1Speed;
            player1.Y += (int)Yplayer1Speed;






            //player 2 movement:

            doplayerMovement(upArrowDown, downArrowDown, leftArrowDown, rightArrowDown, ref player2, ref Yplayer2Speed, ref Xplayer2Speed);

            //put a limit on the speed of player 2
            limitSpeed(ref Xplayer2Speed, ref Yplayer2Speed);

            player2.X += (int)Xplayer2Speed;
            player2.Y += (int)Yplayer2Speed;




            //Scoring Points
            if (ball.X > 440 || ball.X < 0 || ball.Y > this.Width - ball.Width)
            {
                ballXSpeed *= -1;
            }
            if (p1goal.IntersectsWith(ball))
            {
                scorePoint(1);
            }
            else if (p2goal.IntersectsWith(ball))
            {
                scorePoint(2);
            }


            
            Rectangle p1Top = new Rectangle(p1XPosition, p1YPosition, player1.Width, 8);
            Rectangle p1Bottom = new Rectangle(p1XPosition, p1YPosition + player1.Height, player1.Width, 8);
            Rectangle p1Left = new Rectangle(p1XPosition, p1YPosition, 8, player1.Height);
            Rectangle p1Right = new Rectangle(p1XPosition + player1.Width, p1YPosition, 8, player1.Height);

            Rectangle p2Top = new Rectangle(p2XPosition, p2YPosition, player2.Width, 8);
            Rectangle p2Bottom = new Rectangle(p2XPosition, p2YPosition + player2.Height, player2.Width, 8);
            Rectangle p2Left = new Rectangle(p2XPosition, p2YPosition, 8, player2.Height);
            Rectangle p2Right = new Rectangle(p2XPosition + player2.Width, p2YPosition, 8, player2.Height);            
            
            
            //collision player 1
            if (ball.IntersectsWith(p1Top))
            {
                ball.Y -= 5;
                ballYSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p1Bottom))
            {
                ball.Y += 5;
                ballYSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p1Left))
            {
                ball.X -= 5;
                ballXSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p1Right))
            {
                ball.X += 5;
                ballXSpeed *= -1;
                BallBounce.Play();
            }


            //collision player 2
            if (ball.IntersectsWith(p2Top))
            {
                ball.Y -= 5;
                ballYSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p2Bottom))
            {
                ball.Y += 5;
                ballYSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p2Left))
            {
                ball.X -= 5;
                ballXSpeed *= -1;
                BallBounce.Play();
            }
            else if (ball.IntersectsWith(p2Right))
            {
                ball.X += 5;
                ballXSpeed *= -1;
                BallBounce.Play();
            }
            //All of this repeating code feels terrible... well, it is what it is


            //check score and stop game if either player is at 3
            if (player1Score == scoreNeededToWin)
            {
                doTheWinCode(1);
            }
            else if (player2Score == scoreNeededToWin)
            {
                doTheWinCode(2);
            }

            p1ScoreLabel.Text = player1Score.ToString();
            p2ScoreLabel.Text = player2Score.ToString();

            Refresh();


        }


        //there you have your methods :)
        public void doTheWinCode(int playerVariable)
        {
            gameTimer.Enabled = false;
            winLabel.Visible = true;
            winLabel.Text = $"Player {playerVariable} Wins!!";
            YayCheering.Play();
            Refresh();
            Thread.Sleep(3000);
        }

        public void scorePoint(int playerSide)
        {
            if(playerSide == 1) { player2Score++; }
            else if(playerSide == 2) { player1Score++; }
            ball.X = 220;
            ball.Y = 140;
            ballXSpeed = -ballXSpeed;
            AirHorn.Play();
            Refresh();
            Thread.Sleep(1500);
        }

        public void doplayerMovement(bool upButton, bool downButton, bool leftButton, bool rightButton, ref Rectangle player, ref float YplayerSpeed, ref float XplayerSpeed)
        {
           
            if (upButton == true && player.Y > 0)
            {
                if (YplayerSpeed > -startingSpeed)
                {
                    YplayerSpeed = -startingSpeed;
                }
                YplayerSpeed = YplayerSpeed * playerAcceleration;
            }

            if (downButton == true && player.Y < this.Height - player.Height)
            {
                if (YplayerSpeed < startingSpeed)
                {
                    YplayerSpeed = startingSpeed;
                }
                YplayerSpeed = YplayerSpeed * playerAcceleration;
            }


            if (leftButton == true)
            {
                if (XplayerSpeed > -startingSpeed)
                {
                    XplayerSpeed = -startingSpeed;
                }
                XplayerSpeed = XplayerSpeed * playerAcceleration;
                winLabel.Text = "" + XplayerSpeed;

            }

            if (rightButton == true && player.X < this.Width - player.Width)
            {
                if (XplayerSpeed < startingSpeed)
                {
                    XplayerSpeed = startingSpeed;
                }
                XplayerSpeed = XplayerSpeed * playerAcceleration;
                winLabel.Text = "" + XplayerSpeed;
            }

            if (!leftButton && !rightButton)
            {
                if (XplayerSpeed > 0 || XplayerSpeed < 0)
                {
                    XplayerSpeed *= playerDecceleration;
                }
            }

            if (!upButton && !downButton)
            {
                if (YplayerSpeed > 0 || YplayerSpeed < 0)
                {
                    YplayerSpeed *= playerDecceleration;
                }
            }

        }
        public void limitSpeed(ref float XplayerSpeed, ref float YplayerSpeed)
        {
            if (XplayerSpeed >= maxPlayerSpeed)
            {
                XplayerSpeed = maxPlayerSpeed;
            }
            if (XplayerSpeed <= -maxPlayerSpeed)
            {
                XplayerSpeed = -maxPlayerSpeed;
            }

            if (YplayerSpeed >= maxPlayerSpeed)
            {
                YplayerSpeed = maxPlayerSpeed;
            }
            if (YplayerSpeed <= -maxPlayerSpeed)
            {
                YplayerSpeed = -maxPlayerSpeed;
            }
        }
    }
}