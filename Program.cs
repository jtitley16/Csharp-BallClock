using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ball_Clock
{
    public class Program
    {
        static void Main(string[] args)
        {
            //iter will track the number of cycles the system goes through, we can multiply it by .5 to find the number of days the system took to reset with n balls
            int counter = 0;
            bool check = true;
            //Creates a new clock
            var myClock = new Ball_Clock();
            //Initializes teh clock with local variables
            myClock.initialzeBallClock();
            //The machine starts off in teh start state so a do while will allow us to run once before the 1st comparison is made
            do
            {
                counter = myClock.clockcycle(counter);
                check = myClock.qCheck();
            }while (check);
            myClock.dispalyAnswer(counter);
        }
    }
}
