//Program Name:     PowerBallWin.
//
//Description:      
//  This program is designed to simulate the lottery, specifically the powerball                
//  game. You draw five white balls and one red ball, and if all the numbers               
//  on each ball match the winning balls you win.               
//                  
//                  
//
//Date Written:     20201020.
//
//Programmer:       Bradley Hughes
//
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PowerBallWinFormsV7
{
    public partial class PowerballForm : Form
    {    
        //69 white balls, 26 red balls, 5 white balls drawn, 1 red ball drawn.
        //We use const because we never want to use "magic numbers."
        private const int whiteBallHopperCount = 69;

        private const int redBallHopperCount = 26;

        private const int numOfWhiteBallsToBeDrawn = 5;

        private int totalNumberOfDraws = 0;

        bool isTrue = true;


        //Create random number generators references for white balls and red balls.
        private Random whiteBallPicker = null;
     
        private Random redBallPicker = null;

        //Counter and const for counting "dots" displayed
        //in the "Working" animation on the form.
        private int workingDotCount = 0;
        private const int workingDotMax = 65;

        //Delay method millisecond delay constant.
        private int delayConstTimeMilliseconds = 500;

        //Animation timer interval constant.
        private int animationConstIntervalMilliseconds = 250;

        //Create timer object to use to fire an event
        //we can use to display animations on the form
        //while we randomly draw Powerball picks.
        Timer animationTimer = new Timer();

        //>>>>>>You will need to declare varibles you need here.<<<<<<

        //Empty constructor for PowerballForm.
        public PowerballForm()
        {
            InitializeComponent();

            //Set the title caption on the form.
            this.Text = "PowerBall Simulator - Written by Bradley Hughes";
        }

        //This event fires as the form first loads into memory.
        private void PowerballForm_Load(object sender, EventArgs e)
        {
            //Make sure the Reset button is disabled on startup.
            resetButton.Enabled = false;

            //Set the focus to the Simulate Game button.
            simulateGameButton.Focus();
        }

        //This event fires when the user clicks the button to simulate the game.
        private void simulateGameButton_Click(object sender, EventArgs e)
        {
            //Create new random number generator to pick white balls.
            whiteBallPicker = new Random();

            //Run a delay of 1/2 second to ensure that the white ball
            //random number generator and the red ball random number
            //generator are created with different seed values.  Since
            //the constructor for Random uses a date/time stamp as the
            //seed value, it is possible for two random generators created
            //one after the other to have the same seed and therefore
            //generate the same random number sequence.  The delay
            //keeps that from happening.
            Delay(delayConstTimeMilliseconds);

            //Create new random number generator to pick red balls.
            redBallPicker = new Random();

            //Disable the Simulate Game button.
            simulateGameButton.Enabled = false;

            //Disable the Reset button.
            resetButton.Enabled = false;

            //Disable the Display Odds button.
            displayOddsButton.Enabled = false;

            //Set the cursor on the form to the spinning circle thing.
            Cursor = Cursors.WaitCursor;

            //Add the event handler for our animation timer.
            animationTimer.Tick += new EventHandler(AnimateTheForm);

            //Set the animation timer to the specified interval constant.
            animationTimer.Interval = animationConstIntervalMilliseconds;

            //Initially display the "Working" animation on the form.
            workingDotCount = 0;
            workingMsgLabel.Text = "Working.";
            workingMsgLabel.Visible = true;

            //>>>>>>You will need to declare any local varibles you need here<<<<<<
            //>>>>>>as well as the coding to pick the "Winners".             <<<<<<
            //First we create the code block to get the winning white numbers
            int[] redBallPickWinners = new int[1]; //redBallWinners holds the 1 winning red ball.
            int[] whiteBallPickWinners = new int[5]; //whiteBallWinners holds the 5 winning white balls.

            int[] whiteBallArray = new int[whiteBallHopperCount];
            for (int i = 0; i < whiteBallHopperCount; i++) //populate the values of the array
            {
                whiteBallArray[i] = i + 1;
            }
            int whiteHighIndex = whiteBallHopperCount; //highIndex is the 69 white balls
            int[] whiteBallWinners = new int[5]; //whiteBallWinners holds the 5 winning white balls.
            for (int i = 0; i < whiteBallWinners.Length; i++) //loop through the length of the winners 5 times.
            {
                int whitePickIndex = whiteBallPicker.Next(0, whiteHighIndex); //Find a random number between 0 and 69
                whiteBallWinners[i] = whiteBallArray[whitePickIndex]; //Each iteration will set the winner index to the ball picked here.
                for (int j = whitePickIndex; j < whiteBallArray.Length - 1; j++) //move balls down into index picked
                {
                    whiteBallArray[j] = whiteBallArray[j + 1];
                }

                whiteBallArray[whiteBallArray.Length - 1] = whiteBallWinners[i];//set last ball into highest index
                whiteHighIndex--; //move highest index down to eliminate balls picked from possible random pick


            }
            Array.Sort(whiteBallWinners);
            //Refresh our "winner" labels so they will show the
            //Now send each of the whiteBallWinners to their correct label.
            whiteWinnerLabel01.Text = whiteBallWinners[0].ToString();
            whiteWinnerLabel02.Text = whiteBallWinners[1].ToString();
            whiteWinnerLabel03.Text = whiteBallWinners[2].ToString();
            whiteWinnerLabel04.Text = whiteBallWinners[3].ToString();
            whiteWinnerLabel05.Text = whiteBallWinners[4].ToString();

            whiteWinnerLabel01.Refresh();
            whiteWinnerLabel02.Refresh();
            whiteWinnerLabel03.Refresh();
            whiteWinnerLabel04.Refresh();
            whiteWinnerLabel05.Refresh();
          

            //Now do the same thing for the red ball winner.

            int[] redBallArray = new int[redBallHopperCount];
                for (int i = 0; i < redBallHopperCount; i++) //populate the values of the array
                {
                    redBallArray[i] = i + 1;
                }
            int redHighIndex = redBallHopperCount; //highIndex is the 26 red balls

            int[] redBallWinners = new int[1]; //redBallWinners holds the 1 winning red ball.

            int redBallIndex = redBallPicker.Next(0,redHighIndex); //Find a random number between 1 and 26

            redBallWinners[0] = redBallArray[redBallIndex]; //Assign the red ball to the array.

            redWinnerLabel.Text = redBallWinners[0].ToString();  //Write the winner to the label
                                                                 //winning numbers we just picked.

            redWinnerLabel.Refresh();




            //Start the animation timer:
            animationTimer.Start();

            //Driving loop to keep drawing until we match all 5 winning white balls and
            //the 1 winning red ball - at $2 a draw, get your wallet out!!
            while (true)
            {
                //Call the DoEvents method once each loop to allow the form
                //to process all queued events in the event queue (needed
                //for our "animation" of the draw count and "Working" animation.
                Application.DoEvents();
                totalNumberOfDraws++;
                //>>>>>>You will need to declare any local varibles you need here<<<<<<

                //>>>>>>as well as the coding to pick the "Our Picks"<<<<<<
                while (redBallWinners[0] != redBallPickWinners[0])
                {
                    Application.DoEvents();
                    totalNumberOfDraws++;
                    int[] redBallPickArray = new int[redBallHopperCount];
                    for (int i = 0; i < redBallHopperCount; i++) //populate the values of the array
                    {
                        redBallPickArray[i] = i + 1;
                    }
                    int redPickHighIndex = redBallHopperCount; //highIndex is the 26 red balls
                    int redBallPickHighIndex = redBallPicker.Next(0, redPickHighIndex); //Find a random number between 1 and 26

                    redBallPickWinners[0] = redBallPickArray[redBallPickHighIndex]; //Assign the red ball to the array.

                }



                //Our Picks
                while (isTrue)
                {
                    Application.DoEvents();
                    totalNumberOfDraws++;
                    int whiteBallHopperCount = 69;
                    int[] whiteBallPickArray = new int[whiteBallHopperCount];
                    for (int i = 0; i < whiteBallHopperCount; i++) //populate the values of the array
                    {
                        whiteBallPickArray[i] = i + 1;
                    }
                    int whitePickHighIndex = whiteBallHopperCount; //highIndex is the 69 white balls

                    for (int i = 0; i < whiteBallPickWinners.Length; i++) //loop through the length of the winners 5 times.
                    {
                        int whitePickIndex = whiteBallPicker.Next(0, whitePickHighIndex); //Find a random number between 0 and 69
                        whiteBallPickWinners[i] = whiteBallPickArray[whitePickIndex]; //Each iteration will set the winner index to the ball picked here.
                        for (int j = whitePickIndex; j < whiteBallPickArray.Length - 1; j++) //move balls down into index picked
                        {
                            Application.DoEvents();
                            totalNumberOfDraws++;
                            whiteBallPickArray[j] = whiteBallPickArray[j + 1];
                        }

                        whiteBallPickArray[whiteBallPickArray.Length - 1] = whiteBallPickWinners[i];//set last ball into highest index

                        whitePickHighIndex--; //move highest index down to eliminate balls picked from possible random pick

                    }
                    Array.Sort(whiteBallWinners);
                    Array.Sort(whiteBallPickWinners);
                    if (whiteBallWinners.SequenceEqual(whiteBallPickWinners))
                    {
                        
                        isTrue = false;  
                    }
                    else
                    {                     
                        continue;                   
                    }                  
                }
                isTrue = true;
                break;
            }

            //Now send each of the whiteBallWinners to their correct label.
            whitePickLabel01.Text = whiteBallPickWinners[0].ToString();
            whitePickLabel02.Text = whiteBallPickWinners[1].ToString();
            whitePickLabel03.Text = whiteBallPickWinners[2].ToString();
            whitePickLabel04.Text = whiteBallPickWinners[3].ToString();
            whitePickLabel05.Text = whiteBallPickWinners[4].ToString();
            redPickLabel.Text = redBallPickWinners[0].ToString();  //Write the winner to the label

            //refreshing out pick labels 

            whitePickLabel01.Refresh();
            whitePickLabel02.Refresh();
            whitePickLabel03.Refresh();
            whitePickLabel04.Refresh();
            whitePickLabel05.Refresh();
            redPickLabel.Refresh();
            //Stop the animation timer since we don't want any more
            //animations once we have drawn the picks that match the
            //winning numbers.
            animationTimer.Stop();

            //Hide the "Working" animation.
            workingMsgLabel.Visible = false;

            //Enable the Reset button.
            resetButton.Enabled = true;

            //Enable the Reset button.
            displayOddsButton.Enabled = true;

            //Set the focus to the Reset button.
            resetButton.Focus();

            //Set the form cursor back to the arrow.
            Cursor = Cursors.Default;
           
        }

        //Event handler that is fired by our animationTimer each time the Interval elapses.
        private void AnimateTheForm(Object myObject, EventArgs myEventArgs)
        {
            //Display the current draw count on the windows form:
            drawCountLabel.Text = totalNumberOfDraws.ToString("n0");

            //Refresh the draw count label so it will display.
            drawCountLabel.Refresh();

            //Increment the "dot" count for our "Working" animation.
            workingDotCount++;

            //If the display of "dots" gets near the right side of the form,
            //clear it and start again right after the "Working" text.
            if (workingDotCount > 65)
            {
                workingDotCount = 0;
                workingMsgLabel.Text = "Working.";
            }
            else
            {
                //Otherwise, add one more "dot" to the "Working" display.
                workingMsgLabel.Text = workingMsgLabel.Text + ".";
            }

            //Refresh the "Working" label.
            workingMsgLabel.Refresh();
        }

        //A simple delay method that lets us delay the program
        //by the specified milliseconds.  We use this method
        //to cause a delay between creating the two random number
        //generator objects used to draw the white balls and the
        //red balls so that we can ensure that the generators are
        //created with different seed values.
        private void Delay(int delayInMilliseconds)
        {
            int i = 0;

            //Create a timer object to use for the delay.
            System.Timers.Timer delayTimer = new System.Timers.Timer();

            //Set the firing interval to the incoming value.
            delayTimer.Interval = delayInMilliseconds;

            //Tell the timer to run only once.
            delayTimer.AutoReset = false;

            //After the timer has run for the specified interval,
            //tell it what event handler to run.  In this case,
            //we set it to an anounymous event handler that simply
            //sets i to 1.
            delayTimer.Elapsed += (s, args) => i = 1;

            //Start the timer.
            delayTimer.Start();

            //Give the Delay method something to do while
            //the timer is running.  Once .Elapsed fires,
            //it will be set to 1 and this loop will end and
            //thus the entire method will end.
            while (i == 0)
            {
            }
        }

        //Event handler that calculates and displays a message box with the odds
        //for winning the PowerBall game.
        private void displayOddsButton_Click(object sender, EventArgs e)
        {
            int workWhiteBalls = whiteBallHopperCount;
            
            int workRedBalls = redBallHopperCount;

            int workWhiteBallPicks = numOfWhiteBallsToBeDrawn;

            double probabilityOfWinning = 1;

            int oddsOfWinning = 0;

            string s = "The odds of winning the PowerBall jackpot are calculated as follows:\n\n";

            /*  5 out of 69 probability for the first white ball.
                4 out of 68 probability for the second white ball.
                3 out of 67 probability for the third white ball.
                2 out of 66 probability for the fourth white ball.
                1 out of 65 probability for the fifth white ball.

                We multiply each of these probabilities together since
                this is an "and" condition - 5 out of 69 AND 4 out of 68, etc.
            */
            for (int i = workWhiteBallPicks; i >= 1; i--)
            {
                probabilityOfWinning = probabilityOfWinning * ((double)i / (workWhiteBalls - (5 - i)));

                s = s + i + "/" + (workWhiteBalls - (5 - i)) + " * ";
            }

            //Red ball only has one draw, so it is straight probability but we multiply
            //it times the probability calculated above since, again, it is an "and" condition.
            probabilityOfWinning = probabilityOfWinning * ((double)1 / workRedBalls);

            s = s + 1 + "/" + workRedBalls + " = " + probabilityOfWinning.ToString("n20") + "!\n\n";

            //To calculate the odds of winning, take the probability and divide it into 1.
            oddsOfWinning = (int)(1 / probabilityOfWinning);

            s = s + "Or, expressed in odds format, we have 1 chance in every " + oddsOfWinning.ToString("n0") + " draws of picking the winning numbers!!!";

            MessageBox.Show(s,"Odds of Winning",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        //Reset button click event handler.
        private void resetButton_Click(object sender, EventArgs e)
        {
            //Enable the Simulate Game button.
            simulateGameButton.Enabled = true;

            //Set the focus to the Simulate Game button.
            simulateGameButton.Focus();

            //Loop through the winforms controls collection to clear each of the
            //12 labels that display the powerball picks. We could reference
            //each of the labels by name, but this seemed simpler.
            foreach (var ctrl in Controls)
            {
                if (ctrl.GetType() == typeof(GroupBox))
                {
                    GroupBox gbx = ctrl as GroupBox;

                    foreach (var gbxCtrl in gbx.Controls)
                    {
                        Label lbl = (Label)gbxCtrl;

                        if (lbl.BackColor == Color.Yellow || lbl.BackColor == Color.Red)
                        {
                            lbl.Text = "";
                        }        
                    }                   
                }      
            }

            //Clear the draw count label.
            drawCountLabel.Text = "";
            totalNumberOfDraws = 0;
            //Disable the Reset button before we leave.
            resetButton.Enabled = false;
        }
    }
}
