using System;

namespace PrisonProblem
{
    class Program
    {
        static void Main()
        {

            #region Declarations

            // Instanciate program to use methods from class
            var p = new Program();

            // Number of prisoners on the problem, and thus, boxes
            int num_prisoners = 100;
            int num_boxes = num_prisoners;

            // Array of prisoners. Each prisoner index means the number of the prisoner, and each index value (bool) means whether the prisoner has found his number inside the box or not
            bool[]prisoners = new bool[num_prisoners];
            //Array of boxes. Each box index means the box number on the problem, every index value, is actually the note value inside the box from the problem
            int[] boxes = new int[num_boxes];

            // To use in generic loops
            int i;

            // == 18446744073709551615 max number if using NASA PC and UINT64
            int tries = 100000;

            // Variables to store metrics values
            int sucesses_random = 0, sucesses_strategy = 0;
            double sucess_rate_random, sucess_rate_strategy;

            #endregion

            // Loops interactively both games and stores records
            for (i = 0; i < tries; i++) 
            {
                p.Reset_Game(prisoners, boxes);
                p.Random_Choices_Game(prisoners, boxes);

                if (p.check_if_survive(prisoners)) sucesses_random++;

                p.Reset_Game(prisoners, boxes);
                p.Strategy_Game(prisoners, boxes);
                if (p.check_if_survive(prisoners)) sucesses_strategy++;

            }

            // Calculate the percentage
            sucess_rate_random = ((double)sucesses_random / tries) * 100.0;
            sucess_rate_strategy = ((double)sucesses_strategy / tries) * 100.0;

            Console.WriteLine();

            Console.WriteLine("The number of iterations run is -> {0}", tries);

            Console.WriteLine("The % of sucesses with random guesses is -> {0}", sucess_rate_random);

            Console.WriteLine("The % of sucesses with strategy guesses is -> {0}", sucess_rate_strategy);

            // Single game below
            /*

            // Populate array of boxes initially from 1 to length, and set all prisoners to false before the testing
            p.Reset_Game(prisoners, boxes);

            p.Strategy_Game(prisoners, boxes);

            // Debug print
            p.printArrayBool(prisoners);

            Console.WriteLine("Num. right guesses -> {0}", p.count_winners(prisoners));

            Console.WriteLine("Survived? -> {0}", p.check_if_survive(prisoners));

            */

        }

        #region Game_Functions

        // Resets the game to start again interactively
        void Reset_Game(bool[] prisoners_array, int[] boxes_array)
        {
            int i;

            // Sets the prisoners to false, and initialize the boxes from 0 to final length
            for (i = 0; i < prisoners_array.Length; i++)
            {
                prisoners_array[i] = false;
                boxes_array[i] = i;
            }

            // Randomize the boxes
            shuffle(boxes_array);

        }

        // Run a single iteration of a game, where all prisoners choose by chance a box. 50% chance each to find their number
        void Random_Choices_Game(bool[] prisoners_array, int[] boxes_array)
        {
            int i, j;

            // Create an array to randomize and represent the choices the current prisoner will make at random
            int[] choices_array = new int[prisoners_array.Length/2];

            // Run a loop on all prisoners, one by one. The prisoners do not interfere with each other
            for (i = 0; i < prisoners_array.Length; i++) 
            {
                // For each iteration, we randomize the choices for each prisoner, to reduce bias
                Initialize_array(choices_array);
                shuffle(choices_array);

                // Prisoner's choices loop, with half boxes max tries
                for (j = 0; j < prisoners_array.Length / 2; j++)
                {
                    // If the box choosen by the current choice contains the prisoner's number, he guessed right, we make the prisoner number true on the base array, and break the loop for the next one
                    if (boxes_array[choices_array[j]] == i)
                    {
                        prisoners_array[i] = true;
                        break;
                    }

                }

            }

        }

        // Run a single iteration of a game, where all prisoners use the recommended strategy. ~~31% chances all of them find their number (== ~~31% chances all boxes closed loops are smaller than the number of guesses per prisoner)
        void Strategy_Game(bool[] prisoners_array, int[] boxes_array)
        {
            // box_pointer is an auxiliary value to hold the value discovered on the current box, which represents the next box to go to
            int i, j, box_pointer;

            // Run a loop on all prisoners, one by one. The prisoners do not interfere with each other
            for (i = 0; i < prisoners_array.Length; i++) 
            {
                // Since box_pointer means which box go next, on the first iteration for every prisoner, we make his first guess be his own number
                box_pointer = i;

                // Prisoner's choices loop, with half boxes max tries
                for (j = 0; j < prisoners_array.Length / 2; j++) 
                {
                    // If the box choosen by the current choice contains the prisoner's number, he guessed right, we make the prisoner number true on the base array, and break the loop for the next one
                    if (boxes_array[box_pointer] == i)
                    {
                        prisoners_array[i] = true;
                        break;
                    }
                    // If the guess falied, the next guess will be the number discovered
                    else 
                    {
                        box_pointer = boxes_array[box_pointer];
                    }

                }

            }

        }

        #endregion

        #region Aux_Functions

        // Initialize an array, from 0 to its length, number by number
        static void Initialize_array(int[] array) 
        {
            int i;

            for(i=0; i<array.Length; i++) 
            {
                array[i] = i;
            }
        }

        // Shuffle an array using the Fisher Yates shuffling method
        void shuffle(int[] array)
        {
            int currentIndex = array.Length, randomIndex, aux;

            // Need to initialize an System class Random object to use the random methods
            var randObj = new Random();

            // Sweep the array from last position to the second
            while (currentIndex != 0)
            {
                // Get a random number inside the array range, and decrease the index while doing it
                randomIndex = (int)Math.Floor(randObj.NextDouble() * currentIndex--);

                // Swap the two indexes values
                aux = array[currentIndex];
                array[currentIndex] = array[randomIndex];
                array[randomIndex] = aux;

            }

        }

        // Aux function to count the number of true instances inside the prisoners array after a game
        int count_winners(bool[] prisoners_array)
        {
            int i, count = 0;

            for (i = 0; i < prisoners_array.Length; i++)
            {
                if (prisoners_array[i] == true) 
                {
                    count++;
                }

            }

            return count;
        }

        // Aux function to check if all prisoners have found the right box and won the game
        bool check_if_survive(bool[] prisoners_array)
        {
            int i;

            for (i = 0; i < prisoners_array.Length; i++)
            {
                if (prisoners_array[i] == false)
                {
                    return false;
                }

            }

            return true;
        }

        // Prints line per line the box array index, and its value
        void printArrayInt(int[] array) 
        {
            int i = 0;

            foreach (int item in array)
            {
                Console.WriteLine("{0} -> {1}",i , item.ToString());
                i++;
            }
            Console.WriteLine();
        }

        // Prints line per line the prisoner array index, and its value
        void printArrayBool(bool[] array)
        {
            int i = 0;

            foreach (bool item in array)
            {
                Console.WriteLine("{0} -> {1}", i, item.ToString());
                i++;
            }
            Console.WriteLine();
        }

        #endregion

    }
}
