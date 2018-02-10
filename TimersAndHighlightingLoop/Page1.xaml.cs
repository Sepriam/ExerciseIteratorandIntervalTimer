using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace TimersAndHighlightingLoop
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        //Initialise the dicionaries - public static
        public static Dictionary<int, string> dictionary1 =
            new Dictionary<int, string>();
        public static Dictionary<int, string> dictionary2 =
            new Dictionary<int, string>();
        public static Dictionary<int, string> dictionary3 =
            new Dictionary<int, string>();

        //Declare minimum and maximum number of indexs in each dictionary
        public static int Min = 1;
        public static int Max = 30;

        //create an int to declare number of exercises collected from each list
        public static int NumberOfExercisesSelected = 8;

        //can change this to be user defined later on
        //used to indicate number of iterations through the routine
        static int NumOfSets = 3;

        //change this in the main method ( set x exercises selected)
        //this is used to determine number of exercises in total 
        //which is further used to calculate the amount of times the timer should repeat
        static int NumOfRepeats;

        //created an int to cycle through the indexs of items
        static int cycleInt = 0;

        //Creating the timer
        //these create timers of intervals 1000ms (1 second)
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timer2 = new System.Timers.Timer();

        //the below variables are used as amount of seconds to count down from 
        //these can be pre-defined beforehand via user input
        //would be better to change current program to use these variables to reset number rather than hardcoding it inside the method
        static int i = 3;
        static int restCounter = 10;

        //Creating variable that will count down time and will be reset by the above variables
        static int firstTimerCountdown = 0;
        static int secondTimerCountdown = 0;

        //Creating a variable to count the number of times repeated
        static int repeatCounter = 0;

        public Page1()
        {
            InitializeComponent();
            createDictionary();
            
        }
        

        private void Start_button_click(object sender, RoutedEventArgs e)
        {
            this.ListView1.Items.Clear();

            //assign the random numbers create from number generator to an int array
            int[] RandomNumberArray = randomNumberGen();

            /*
             * Creates list to store the value assigned to each key (number)
             * does this by passing random int array to respective dictionary select method
             * dictionary select method returns list of strings for this values
             * */
            List<string> valueList = returnValuesFromDictionary1(RandomNumberArray);
            List<string> valueList2 = returnValuesFromDictionary2(RandomNumberArray);
            List<string> valueList3 = returnValuesFromDictionary3(RandomNumberArray);

            for (int i = 0; i < RandomNumberArray.Count(); i++)
            {

                this.ListView1.Items.Add(new MyItem
                {
                    Beginner = valueList[i],

                    Intermediate = valueList2[i],

                    Advanced = valueList3[i]

                });
            }

            //the below can be done on start button
            //used to calculate how many ties the timer should be repeated
            NumOfRepeats = NumOfSets * NumberOfExercisesSelected;

            

            //on start set the timers to their first state (i.e whatever number they start countdown from)
            firstTimerCountdown = i;

            //the code below calls the timer method(s)
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer2.Elapsed += new ElapsedEventHandler(timer_Elapsed2);
            timer.Interval = 1000;
            timer2.Interval = 1000;

            //start the first timer here --- this can be started upon pressing the start button
            timer.Start();

            

        }

        private void Stop_button_click(object sender, RoutedEventArgs e)
        {

        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {

        }




        /*
         *This is the first timer method
         *It is used to display text on the console
         *inside the WPF program this should be used to change the text displayed inside each of the text boxes
         *This method also calls starts and calls the second (break) timer's method
         */
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (repeatCounter == 0)
            {
                this.Dispatcher.Invoke(() =>
                {
                    ListViewItem Row = this.ListView1.ItemContainerGenerator.ContainerFromIndex(cycleInt) as ListViewItem;
                    Row.Background = Brushes.Red;
                });
            }

            //decrement timer countdown on every iteration
            firstTimerCountdown--;

            //set the label's content to the timer value
            this.Dispatcher.Invoke(() =>
            {
                Label1.Content = "WORK :" + firstTimerCountdown.ToString();
                Label2.Content = "WORK :" + firstTimerCountdown.ToString();
            });
            


            //determine if the first counter is at 0
            if (firstTimerCountdown == 0)
            {
                //if condition is true, clear the console, display the new time
                this.Dispatcher.Invoke(() =>
                {
                    Label1.Content = "WORK :" + firstTimerCountdown.ToString();
                    Label2.Content = "WORK :" + firstTimerCountdown.ToString();
                });
                this.Dispatcher.Invoke(() =>
                {
                    ListViewItem Row = this.ListView1.ItemContainerGenerator.ContainerFromIndex(cycleInt) as ListViewItem;
                    Row.Background = Brushes.White;
                });
                
                //increase cycleInt by +1
                cycleInt++;

                //Stop the first timer
                timer.Stop();

                //reset the first timer to it's start state
                firstTimerCountdown = i;

                //reset the second timer to its original state
                secondTimerCountdown = restCounter;

                //start the second timer                
                timer2.Start();

                //increment the number of times the first timer has been repeated by 1
                repeatCounter++;

                //check if the number of times the first timer has been repeated is equal to the total number it should finish at
                if (repeatCounter == NumOfRepeats)
                {

                    //stop the first timer
                    timer.Close();

                    //dispose of the timer as there is not further need of it
                    timer.Dispose();

                    timer2.Stop();
                    timer2.Dispose();

                    //if the program reaches here, the console will be cleared and display the finished message

                    this.Dispatcher.Invoke(() =>
                    {
                        Label1.Content = "STOP";
                        Label2.Content = "STOP";
                    });


            }
                else
                {

                }
            }

            //call to do garbage collector to clean up the timers
            GC.Collect();
        }



        /*
         * this is the second timer's method
         * it's used as a break timer currently
         * This method will eventually be used to unhighlight the current exercises and change the text inside the textboxes to REST
             */
        private  void timer_Elapsed2(object sender, ElapsedEventArgs e)
        {

            //decrement the break timer everytime this method is called
            secondTimerCountdown--;

            //clear the console and display the amount of rest remaining
            this.Dispatcher.Invoke(() =>
            {
                Label1.Content = "REST :" + secondTimerCountdown.ToString();
                Label2.Content = "REST :" + secondTimerCountdown.ToString();
            });

            //check if the timer is at 0 --has the rest finished?--
            if (secondTimerCountdown == 0)
            {
                //stop the second timer
                timer2.Stop();

                //reset the break timer countdown variable
                secondTimerCountdown = restCounter;

                if ((repeatCounter + 1) % NumberOfExercisesSelected == 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        cycleInt = 0;
                        ListViewItem Row = this.ListView1.ItemContainerGenerator.ContainerFromIndex(cycleInt) as ListViewItem;
                        Row.Background = Brushes.Red;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ListViewItem Row = this.ListView1.ItemContainerGenerator.ContainerFromIndex(cycleInt) as ListViewItem;
                        Row.Background = Brushes.Red;
                    });
                }

                //restart the first timer
                try
                {
                    
                    timer.Start();
                }
                catch (ObjectDisposedException)
                {

                }
            }

            //uneccessary call to the garbage collector
            GC.Collect();
        }


        /*
    * This method is used to initialise the entries into the dictionary
    * adds the entries one by one
    * can change what value is assigned to each key in the method below
    * (BASICALLY WHAT EXERCISES ARE IN EACH LIST)
    * */
        public static void createDictionary()
        {
            dictionary1.Add(1, "Pike hold");
            dictionary1.Add(2, "Pike Push-ups");
            dictionary1.Add(3, "Elevated Pike Hold");
            dictionary1.Add(4, "Straight Arm Plank Hold");
            dictionary1.Add(5, "Australian Pull-ups");
            dictionary1.Add(6, "Jump Negative Pull-ups");
            dictionary1.Add(7, "Dead -> Active Hang");
            dictionary1.Add(8, "Australian Chin-ups");
            dictionary1.Add(9, "Bar Curl");
            dictionary1.Add(10, "Jump Negative Chin-Ups");
            dictionary1.Add(11, "Skull Crushers");
            dictionary1.Add(12, "Bench Dips");
            dictionary1.Add(13, "Jump Negative Dips");
            dictionary1.Add(14, "Wall / Incline Push-ups");
            dictionary1.Add(15, "RESERVED CHEST / TRI");
            dictionary1.Add(16, "RESERVED CHEST / TRI");
            dictionary1.Add(17, "Plank Hold");
            dictionary1.Add(18, "Knee Raises");
            dictionary1.Add(19, "Tuck L-sit");
            dictionary1.Add(20, "Star Crunch");
            dictionary1.Add(21, "Bodyweight Squat");
            dictionary1.Add(22, "Alternating lunges");
            dictionary1.Add(23, "Step-ups");
            dictionary1.Add(24, "Hip Raise");
            dictionary1.Add(25, "Burpees");
            dictionary1.Add(26, "Jump Rope");
            dictionary1.Add(27, "Medicine Ball throw (wall)");
            dictionary1.Add(28, "Mountain Climbers");
            dictionary1.Add(29, "In and Outs");
            dictionary1.Add(30, "High Knees");


            dictionary2.Add(1, "Pike Push-ups");
            dictionary2.Add(2, "Elevated Pike Push-ups");
            dictionary2.Add(3, "Handstand Hold");
            dictionary2.Add(4, "Shoulder Taps");
            dictionary2.Add(5, "Single-Arm Australian Pull-ups");
            dictionary2.Add(6, "Pull-ups");
            dictionary2.Add(7, "Dead -> Active Hang");
            dictionary2.Add(8, "Single Arm Australian Chin-Ups");
            dictionary2.Add(9, "Ring Curls");
            dictionary2.Add(10, "Chin-ups");
            dictionary2.Add(11, "Skull Crushers");
            dictionary2.Add(12, "Straight Bar dips");
            dictionary2.Add(13, "Regular Dips");
            dictionary2.Add(14, "Push-ups");
            dictionary2.Add(15, "RESERVED CHEST / TRI");
            dictionary2.Add(16, "RESERVED CHEST / TRI");
            dictionary2.Add(17, "Plank Knees to Elbow");
            dictionary2.Add(18, "Leg Raises");
            dictionary2.Add(19, "L-Sit");
            dictionary2.Add(20, "Chair Crunch");
            dictionary2.Add(21, "Assisted Pistol Squat");
            dictionary2.Add(22, "Switching (jump) Lunges");
            dictionary2.Add(23, "Box Jumps");
            dictionary2.Add(24, "Single Leg Hip Raise");
            dictionary2.Add(25, "Jump Burpees");
            dictionary2.Add(26, "Jump Rope");
            dictionary2.Add(27, "Sledgehammer Slams");
            dictionary2.Add(28, "Mountain Climbers");
            dictionary2.Add(29, "In and Outs");
            dictionary2.Add(30, "High Knees");


            dictionary3.Add(1, "Elevated Pike Push-ups");
            dictionary3.Add(2, "Handstand Push-ups");
            dictionary3.Add(3, "Handstand Shoulder Taps");
            dictionary3.Add(4, "Plank around the world");
            dictionary3.Add(5, "Pull-ups");
            dictionary3.Add(6, "Muscle ups");
            dictionary3.Add(7, "Dead -> Active Hang");
            dictionary3.Add(8, "1 arm assisted Pull ups");
            dictionary3.Add(9, "Elevated Ring / Bar Curls");
            dictionary3.Add(10, "1 arm asssisted Chin Ups");
            dictionary3.Add(11, "Skull Crushers");
            dictionary3.Add(12, "Assisted Single Arm Straight Bar Dip");
            dictionary3.Add(13, "Explosive dips");
            dictionary3.Add(14, "Explosive Push-ups");
            dictionary3.Add(15, "RESERVED CHEST / TRI");
            dictionary3.Add(16, "RESERVED CHEST / TRI");
            dictionary3.Add(17, "Single leg / arm hold");
            dictionary3.Add(18, "Front Lever Raise");
            dictionary3.Add(19, "Front Lever Drops");
            dictionary3.Add(20, "V-sits");
            dictionary3.Add(21, "Alternating Pistol Squats");
            dictionary3.Add(22, "Crab Walks");
            dictionary3.Add(23, "Burpee -> Box Jump");
            dictionary3.Add(24, "Elevated Single Leg Raise");
            dictionary3.Add(25, "Single Leg Burpees");
            dictionary3.Add(26, "Jump Rope");
            dictionary3.Add(27, "Slam Ball");
            dictionary3.Add(28, "Wall Mountain Climbers");
            dictionary3.Add(29, "In and Outs");
            dictionary3.Add(30, "High Knees");
        }


        /*
       * Takes a random int array
       * creates a new list of strings
       * Grabs the value associated with each key from int array
       * returns the collective values in a list of strings
       * 
       * --NOTE : Can probably narrow this down to 1 method by passing the dictionary in parameters--
       * */
        public static List<string> returnValuesFromDictionary1(int[] _keyArray)
        {
            var retList = new List<string>();

            foreach (int a in _keyArray)
            {
                if (dictionary1.ContainsKey(a))
                {
                    string Value = dictionary1[a];
                    retList.Add(Value);
                }
            }

            return retList;
        }

        public static List<string> returnValuesFromDictionary2(int[] _keyArray)
        {
            var retList = new List<string>();

            foreach (int a in _keyArray)
            {
                if (dictionary1.ContainsKey(a))
                {
                    string Value = dictionary2[a];
                    retList.Add(Value);
                }
            }

            return retList;
        }

        public static List<string> returnValuesFromDictionary3(int[] _keyArray)
        {
            var retList = new List<string>();

            foreach (int a in _keyArray)
            {
                if (dictionary1.ContainsKey(a))
                {
                    string Value = dictionary3[a];
                    retList.Add(Value);
                }
            }

            return retList;
        }


        /*changed the following:
         * - Will check if current number = last number and re-roll if true
         * - Will not check current number if first iteration through loop -- Wont crash due to OutOfBoundaries Exception
         * */
        public static int[] randomNumberGen()
        {

            // this declares an integer array
            // and initializes all of them to their default value
            // which is zero
            int[] test2 = new int[NumberOfExercisesSelected];

            //creates new random
            Random randNum = new Random();
            for (int i = 0; i < test2.Length; i++)
            {
                //selects number between min and max -- See Top of Page
                int NumberCheck = randNum.Next(Min, Max);

                //check to see if a previous iteration through loop has been made
                if (i >= 1)
                {
                    //if the current number = last number, keep re-selecting until this is not true
                    while (test2[i - 1] == NumberCheck)
                    {
                        NumberCheck = randNum.Next(Min, Max);
                    }
                    //add the new number (key) to list
                    test2[i] = NumberCheck;
                }
                else
                {
                    //if here, this is the first time loop has been iterated through
                    test2[i] = NumberCheck;
                }



            }

            //return array of random ints
            return test2;
        }

    }

    public class MyItem
    {
        public string Beginner { get; set; }

        public string Intermediate { get; set; }

        public string Advanced { get; set; }

    }
}
