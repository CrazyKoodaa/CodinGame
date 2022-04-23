using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        char[,] field = new char[width, height];
        string[] result = new string[width * height];

        for (int y = 0; y < height; y++)
        {
            string line = Console.ReadLine(); // width characters, each either 0 or .
            for (int x = 0; x < width; x++)
            {
                field[x,y] = line[x];
                Console.Error.Write($"{field[x,y]} ");
            }
            Console.Error.WriteLine();
        }


        // fucking so hard with x and y ...
        // x = 0 1 2 3 4
        // y =  0
        //      1
        //      2
        //      3

        int countingGrid = 0;
        bool found = false;

        // outer loop are for rows, so it's y 
        for (int y = 0; y < height; y++)
        {
            result[countingGrid] = "";
            // inner loop are for colums, so they are x
            for (int x = 0; x < width; x++)
            {
                result[countingGrid] += $"{x} {y} ";
                Console.Error.WriteLine($"i am know in {x} {y}");
                if (field[x,y] == '0')
                {
                    if (x < width)
                    {
                        found = false;
                        for (int next = x + 1; next < width; next++)
                        {
                            if (field[next, y] == '0' && !found)
                            {
                                result[countingGrid] += $"{next} {y} ";
                                Console.Error.WriteLine($"found a 0 in {next} {y}");
                                found = true;
                            }

                        }
                    }
                    if (!found)
                    {
                        result[countingGrid] += "-1 -1 ";
                        Console.Error.WriteLine($"Found no 0 in x");

                    }


                    found = false;
                    if (y < height - 1)
                    {
                        for (int next = y + 1; next < height; next++)
                        {
                            if (field[x,next] == '0' && !found)
                            {
                                result[countingGrid] += $"{x} {next}";
                                Console.Error.WriteLine($"found a 0 in {x} {next}");
                                found = true;
                            }
                        }
                    }
                    if (!found)
                    {
                        result[countingGrid] += "-1 -1";
                        Console.Error.WriteLine($"Found no 0 in y");
                    }
                }
                Console.Error.WriteLine($"This is what I have found: {result[countingGrid]}");
                countingGrid++;
            }
        }
        
        for (int i = 0; i < countingGrid; i++)
        {
            if (result[i].Length < 10)
                Console.Error.WriteLine(result[i].Length);
            else 
                Console.WriteLine(result[i]);
        }

        // Console.Error.WriteLine()
        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");


        // Three coordinates: a node, its right neighbor, its bottom neighbor
        //Console.WriteLine("0 0 1 0 0 1");
    }
}