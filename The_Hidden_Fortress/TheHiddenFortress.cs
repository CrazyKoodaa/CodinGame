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
    static int charToInt(char c)
    {
        if (c >= '0' && c <= '9')
            return c-'0';
        else if (c >= 'a' && c <= 'z')
            return c-'a'+10;
        return c-'A'+36;
    }

    static void Main(string[] args)
    {
        int SIZE = int.Parse(Console.ReadLine());
        string sepGrid;
        int[,] grid = new int[SIZE, SIZE];
        float[,] value = new float[SIZE, SIZE];
        
        float[,] result = new float[SIZE, SIZE];
        float[] rowSum = new float[SIZE];
        float[] colSum = new float[SIZE];
        float allSum = 0f;

        for (int i = 0; i < SIZE; i++)
        {
            sepGrid = Console.ReadLine();
            //Console.Error.WriteLine(sepGrid);
            for (int j = 0; j < SIZE; j++)
            {
                grid[i,j] = charToInt(sepGrid[j]);
                //Console.Error.Write(grid[i,j] + " ");

            }
            //Console.Error.WriteLine();
        }

        // Console.Error.WriteLine($"Sums:");
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                colSum[i] += grid[i,j];
                rowSum[j] += grid[i,j];
            }
            // System.Console.WriteLine($"row = {colSum[i]} ");
            allSum += colSum[i]; 
        }

        allSum *= 2;
        allSum = allSum / (2f * SIZE - 1f);
        //Console.Error.WriteLine($"allSum: {allSum}");
        

        for (int i = 0; i < SIZE; i++)
        {
            // Console.Error.WriteLine($"allSum {allSum} = colSum: {colSum[i]}; rowSum: {rowSum[i]}");
            colSum[i] -= allSum;
            rowSum[i] -= allSum;
            // Console.Error.WriteLine($"-allSum {allSum} = colSum: {colSum[i]}; rowSum: {rowSum[i]}");

            colSum[i] /= (SIZE - 1f);
            rowSum[i] /= (SIZE - 1f);
            // Console.Error.WriteLine($"/SIZE-1 = colSum: {colSum[i]}; rowSum: {rowSum[i]}");
        }


        float max = -999999f;
        float min = 9999999f;

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                // Console.Error.Write("g" + grid[i,j] + " ");
                // Console.Error.Write("v" + value[i,j] + " ");
                // Console.Error.Write("rS" + rowSum[j] + " cS" + colSum[i] + " ");
                value[i,j] = (float)(rowSum[j] + colSum[i]);
                // Console.Error.Write(value[i,j] + " ");
                value[i,j] -= grid[i,j];
                // Console.Error.WriteLine(value[i,j] + " ");
                max = value[i,j] > max ? value[i,j] : max;
                min = value[i,j] < min ? value[i,j] : min;
                
            }
            // Console.Error.WriteLine($"max = {max}");
            // Console.Error.WriteLine($"min = {min}");
            // Console.Error.WriteLine($"grenze = {(min + max)/2 }");

            // Console.Error.WriteLine();
        }

       
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (value[i,j] > (max+min)/2)
                    Console.Write("O");
                else 
                    Console.Write(".");
            }
            Console.WriteLine();
        }

    }


}