using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int nbFloors = int.Parse(inputs[0]); // number of floors
        int[] iNumberOfFloors = new int [nbFloors];

        int width = int.Parse(inputs[1]) - 1; // width of the area
        Console.Error.WriteLine($"width {width}");
        int nbRounds = int.Parse(inputs[2]); // maximum number of rounds
        int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        int exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        //Console.Error.WriteLine($"xFloor {exitFloor} xPos {exitPos}");
        int nbTotalClones = int.Parse(inputs[5]); // number of generated clones
        int nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
        int nbElevators = int.Parse(inputs[7]); // number of elevators
        Console.Error.WriteLine($"nbElevators = {nbElevators}");

        int waitOnePos = 0;
        if (nbElevators > 0)
        {
            for (int i = 0; i < nbElevators; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
                int elevatorPos = int.Parse(inputs[1]); // position of the elevator on its floor
                //Console.Error.WriteLine($"eFloor {elevatorFloor} ePos {elevatorPos}");
                iNumberOfFloors[elevatorFloor] = elevatorPos; 
                Console.Error.WriteLine($"elevator = {elevatorFloor} {iNumberOfFloors[elevatorFloor]}");
            }
        }
        else
            iNumberOfFloors[0] = exitPos;
        
        iNumberOfFloors[nbElevators] = exitPos;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
            int clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
            string direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT
            if (cloneFloor < 0)
                cloneFloor = 0;

            Console.Error.WriteLine($"cFloor {cloneFloor} cPos {clonePos} dir {direction}");
            Console.Error.WriteLine($"NumberFloors {iNumberOfFloors[cloneFloor]}");
            

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            switch(clonePos)
            {
                case int x when cloneFloor < 0:
                    Console.WriteLine("WAIT");
                    break;
                case int x when clonePos == width - 1
                                || clonePos == 0 
                                || (iNumberOfFloors[cloneFloor] < (clonePos) && direction == "RIGHT")
                                || (iNumberOfFloors[cloneFloor] > (clonePos) && direction == "LEFT"):
                    if (waitOnePos == 1)
                    {
                        Console.WriteLine("BLOCK");
                        waitOnePos = 0;
                    }
                    else
                    {
                        Console.WriteLine("WAIT");
                        waitOnePos = 1;
                    }
                    break;
                default:

                    Console.WriteLine("WAIT"); // action: WAIT or BLOCK
                    break;
            }

        }
    }
}