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
        
        
        double DEFIB_LON, PERSON_LON;
        double DEFIB_LAT, PERSON_LAT;
        double x, y, d, dmin = 900000.00;
        string[] DEFIB_ID = {"0", "0", "0", "0", "0", "0"};

        string LON = Console.ReadLine();
        string LAT = Console.ReadLine();
        PERSON_LON = Convert.ToDouble(LON.Replace(",","."));
        PERSON_LAT = Convert.ToDouble(LAT.Replace(",","."));
        Console.Error.WriteLine("P_LON={0};LAT={1}",LON, LAT);
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string DEFIB = Console.ReadLine();
            Console.Error.WriteLine(DEFIB);
            string[] DEFIB_SUBS = DEFIB.Split(";");
            DEFIB_LON = Convert.ToDouble(DEFIB_SUBS[4].Replace(",", "."));
            DEFIB_LAT = Convert.ToDouble(DEFIB_SUBS[5].Replace(",", "."));
            x = (DEFIB_LON - PERSON_LON) * Math.Cos((DEFIB_LAT + PERSON_LAT)/2);
            y = (DEFIB_LAT - PERSON_LAT);
            d = Math.Sqrt((x*x)+(y*y)) * 6371;
            Console.Error.WriteLine("x={0};y={1};d={2}", x, y, d);
            if (d < dmin)
            {
        
                dmin = d;
                DEFIB_ID = DEFIB_SUBS;
                Console.Error.WriteLine(DEFIB_ID[1]);
            }
        }
        Console.Error.WriteLine(DEFIB_ID[1]);
        // Write an answer using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(DEFIB_ID[1]);
    }
}