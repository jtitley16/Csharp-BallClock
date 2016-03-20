using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ball_Clock
{
    public class Ball_Clock
    {
        //Ball queue must be at least 27 balls minimum, 11 + 11 + 4 + 1
        //Changing the value of n here will allow you to test various clock sizes from this location
        public static int n = getNumberOfBalls();
        public static int[] startState = new int[n];
        public static Queue nQ = new Queue(n);
        public static Stack ballStackMinutes = new Stack(5);
        public static Stack ballStackFiveMinutes = new Stack(12);
        public static Stack ballStackHour = new Stack(12);
        
        //Sets up a new clock, establishes the queue from 1-n and creates a start state copy for comparisons
        public void initialzeBallClock()
        {
            //Fill the queue from 1 to n in ascending order
            qFill();
            //saves the start state in an array
            nQ.CopyTo(startState, 0);
        }
        public static int getNumberOfBalls()
        {
            int n;
            Console.WriteLine("How many balls would you like to use in your clock, please enter a value between 27 and 127?");
            n = Convert.ToInt32(Console.ReadLine());
            while (!(n >= 27 && n <= 127))
            {
                Console.WriteLine("Please use a value between 1 and 127");
                n = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("{0}", n);
            return n;
        }
        //initialize the queue with balls in order 1 to n
        public static void qFill()
        {
            for (int i = 0; i < n; i++)
            {
                nQ.Enqueue(i + 1);
            }
        }
        //Retrieves a primer ball to start the clock
        public int getNewBall()
        {
            int ballValue = (int)nQ.Dequeue();
            //Console.WriteLine("LRU 1 Ball is: {0}", ballValue);
            return ballValue;
        }
        //Dumps a tray of balls and returns them to the queue in reverse order
        public void dumpTray(Stack S, int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                //pops the next ball off the tray and places it into the back of the queue
                nQ.Enqueue(S.Pop());
                /*
                int temp = (int)S.Pop();
                Console.WriteLine("add back to Q1: {0}", temp);
                nQ.Enqueue(temp);
                */
            }
        }
        //This Function will fill each tray and then dump, saving the last ball dumped
        //that ball will be placed into the first position of the new tray
        public int clockcycle(int counter)
        {
            int minuteBall;
            //720 minutes in one 12 hour period
            for (int i = 0; i < 720; i++)
            {
                //Console.WriteLine("minute# {0}", i);
                //Get new ball, before we move through the nested if block
                minuteBall = getNewBall();
                //Console.WriteLine("ball# {0}", minuteBall);
                //if the minute tray has an opening we will deposit the ball here
                if (ballStackMinutes.Count < 4)
                {
                    //Console.WriteLine("1min {0}", ballStackMinutes.Count);
                    ballStackMinutes.Push(minuteBall);
                }
                //If the minute tray is full, 1st we push the new ball to the 5 minute tray, and then we dump the minute tray to the queue
                //to trigger this path the minute tray must be full, and the 5 min tray must have an opening
                //once we drop the ball in a slot then we dump the minunte tray
                else if (ballStackMinutes.Count == 4 && ballStackFiveMinutes.Count < 11)
                {
                    //Console.WriteLine("5min {0}", ballStackFiveMinutes.Count);
                    ballStackFiveMinutes.Push(minuteBall);
                    dumpTray(ballStackMinutes, 4);
                }
                //If the minute tray and 5 minute tray are full we deposit here if and only if there is room
                //1st push the ball to the hour tray
                //2nd dump the minute tray
                //3rd dump the 5 minute tray
                //remember order matters                 
                else if (ballStackMinutes.Count == 4 && ballStackFiveMinutes.Count == 11 && ballStackHour.Count < 11)
                {
                    //Console.WriteLine("1hour {0}", ballStackHour.Count);
                    ballStackHour.Push(minuteBall);
                    dumpTray(ballStackMinutes, 4);
                    dumpTray(ballStackFiveMinutes, 11);
                }
                //with all 3 trays full we arrive here
                //since the new ball will go to the back of the queue from here we can dump all 3 trays in order from:
                //1st minute
                //2nd 5 minute
                //3rd hour
                //then add the new ball to the queue
                //increment the counter
                else
                {
                    dumpTray(ballStackMinutes, 4);
                    dumpTray(ballStackFiveMinutes, 11);
                    dumpTray(ballStackHour, 11);
                    nQ.Enqueue(minuteBall);
                    counter = counter + 1;
                }
            }
            return counter;
        }
        //passes the queue into an array to check for sort ascending order, which will indicate teh machine as reached the initial state.
        public bool qCheck()
        {
            int i;
            int j = 0;
            int[] arrayCompare = new int[n];
            nQ.CopyTo(arrayCompare, 0);
            for (i = 0; i < n; i++)
            {
                if (arrayCompare[i] == startState[i])
                    j++;
                //Return false to end the do while loop in main
                if (j == (n))
                    return false;
            }
            //return true to continue teh do while loop in main
            return true;
        }
        //Displays the number of cycles required to reset the system to it's initial conditions
        public void dispalyAnswer(int iterations)
        {
            Console.WriteLine("The number of iterations to reset a system of {0} balls was: {1}", n, iterations);
        }
    }
}
