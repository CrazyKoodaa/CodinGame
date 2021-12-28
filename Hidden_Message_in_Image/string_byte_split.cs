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
class Solution
{
    static void Main(string[] args)
    {
        string[] inputs;
        string endingBits = "";
        inputs = Console.ReadLine().Split(' ');
        int w = int.Parse(inputs[0]);
        int h = int.Parse(inputs[1]);
        for (int i = 0; i < h; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            for (int j = 0; j < w; j++)
            {
                int pixel = int.Parse(inputs[j]);
                endingBits = endingBits + (pixel % 2);
            }
        }

        int numOfBytes = endingBits.Length / 8;
        byte[] bytes = new byte[numOfBytes];
        for(int i = 0; i < numOfBytes; ++i)
        {
            bytes[i] = Convert.ToByte(endingBits.Substring(8 * i, 8), 2);
            Console.Write(Convert.ToChar(bytes[i]));
        }

        Console.Write("\n");
    }
}