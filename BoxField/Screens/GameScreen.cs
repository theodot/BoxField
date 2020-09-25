using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys
        Boolean leftArrowDown, rightArrowDown;

        //used to draw boxes on screen
        SolidBrush boxBrush = new SolidBrush(Color.White);

        //a list to hold a column of boxes        
        List<Box> left = new List<Box>();
        List<Box> right = new List<Box>();
        int leftX = 200;
        int gap = 200;

        Box hero;
        int heroSpeed = 10;
        int heroSize = 30;
        string patternDirection = "right";
        Boolean moveRight = true;
        int patternLength = 10;
        int patternSpeed = 7;

        Random randNum = new Random();

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        public void MakeBox()
        {
            //get colour for box
            int rand = randNum.Next(1, 4);
            Color c = Color.White;

            if (rand == 1)
            {
                c = Color.Red;
            }
            else if (rand == 2)
            {
                c = Color.Yellow;
            }
            else if (rand == 3)
            {
                c = Color.Orange;
            }

            // determine the x value of the left box
            patternLength--;

            if (patternLength == 0)
            {
                moveRight = !moveRight;
                
                patternLength = randNum.Next(3, 9);
                patternSpeed = randNum.Next(2, 25);
            }

            if(moveRight)
            {
                leftX += patternSpeed;
            }
            else
            {
                leftX -= patternSpeed;
            }
            

            //add box
            Box newBox = new Box(leftX, 0, 20, c);
            left.Add(newBox);

            Box newBox2 = new Box(leftX + gap, 0, 20, c);
            right.Add(newBox2);
        }
        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            //set game start values

            MakeBox();

            hero = new Box(this.Width / 2 - heroSize / 2, 370, heroSize);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;           
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //update location of all boxes (drop down screen)
            foreach (Box b in left)
            {
                b.Move(5);
            }

            foreach (Box b in right)
            {
                b.Move(5);
            }

            //remove box if it has gone of screen
            if (left[0].y > 400)
            {
                left.RemoveAt(0);
                right.RemoveAt(0);
            }

            //add new box if it is time
            if (left[left.Count - 1].y > 21)
            {
                MakeBox();
            }

            // move hero
            if (leftArrowDown == true)
            {
                hero.Move(heroSpeed, "left");
            }
            else if (rightArrowDown == true)
            {
                hero.Move(heroSpeed, "right");
            }

            // check for collision
            Rectangle heroRec = new Rectangle(hero.x, hero.y, hero.size, hero.size);

            if (left.Count >= 4)
            {
                // check bottom 4 boxes 
                for (int i = 0; i < 4; i++)
                {
                    Rectangle boxRec = new Rectangle(left[i].x, left[i].y, left[i].size, left[i].size);
                    Rectangle rightBoxRec = new Rectangle(right[i].x, right[i].y, right[i].size, right[i].size);

                    if (boxRec.IntersectsWith(heroRec) || rightBoxRec.IntersectsWith(heroRec))
                    {
                        gameLoop.Enabled = false;
                    }
                }
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach (Box b in left)
            {
                boxBrush.Color = b.color;
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            foreach (Box b in right)
            {
                boxBrush.Color = b.color;
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            //draw hero character
            boxBrush.Color = Color.White;
            e.Graphics.FillRectangle(boxBrush, hero.x, hero.y, hero.size, hero.size);

            e.Graphics.FillRectangle(boxBrush, 0, 400, this.Width, 2);
        }
    }
}
