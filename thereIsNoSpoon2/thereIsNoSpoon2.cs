using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * The machines are gaining ground. Time to show them what we're really made of...
 **/
class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis

        Console.Error.WriteLine($"width: {width}; height: {height}");
        int[,] field = new int[width, height];
        string[] result = new string[width * height];

        for (int y = 0; y < height; y++)
        {
            string line = Console.ReadLine(); // width characters, each either a number or a '.'
            Console.Error.WriteLine($"this is the {y} line {line}");
            for (int x = 0; x < width; x++)
            {
                if (line[x] == '.')
                    field[x,y] = 0;
                else
                    field[x,y] = line[x] - 48;
                Console.Error.Write($"{field[x,y]} ");
            }
            Console.Error.WriteLine();
        }

        int countingGrid = 0;
        bool foundx = false;
        bool foundy = false;
        int connections = 0;
       

        // outer loop are for rows, so it's y 
        for (int y = 0; y < height; y++)
        {
            result[countingGrid] = "";
            // inner loop are for colums, so they are x
            for (int x = 0; x < width; x++)
            {
                result[countingGrid] += $"{x} {y} ";
                Console.Error.WriteLine($"i am know in {x} {y}");
                
                if (field[x,y] > 0)
                {
                    if (x < width)
                    {
                        foundx = false;
                        for (int next = x + 1; next < width; next++)
                        {
                            if (field[next, y] > 0 && !foundx)
                            {
                                result[countingGrid] += $"{next} {y} ";
                                Console.Error.WriteLine($"found a 0 in {next} {y}");
                                conenctions = Math.Abs(field[next, y] - field [x,y]);
                                foundx = true;
                            }

                        }
                    }
                    if (!foundx)
                    {
                        // result[countingGrid] += "-1 -1 ";
                        Console.Error.WriteLine($"Found no 0 in x");

                    }


                    foundy = false;
                    if (y < height - 1)
                    {

                        for (int next = y + 1; next < height; next++)
                        {
                            if (field[x,next] > 0 && !foundy)
                            {
                                result[countingGrid] += $"{x} {next}";
                                Console.Error.WriteLine($"found a 0 in {x} {next}");
                                foundy = true;
                            }
                        }
                    }
                    if (!foundy)
                    {
                        //result[countingGrid] += "-1 -1";
                        Console.Error.WriteLine($"Found no 0 in y");
                    }
                }
                Console.Error.WriteLine($"This is what I have found: {result[countingGrid]}");
                countingGrid++;

            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");


        // Two coordinates and one integer: a node, one of its neighbors, the number of links connecting them.
        Console.WriteLine("0 0 2 0 1");
    }
}