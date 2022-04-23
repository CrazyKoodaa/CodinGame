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
        int N = int.Parse(Console.ReadLine()); // Number of elements which make up the association table.
        int Q = int.Parse(Console.ReadLine()); // Number Q of file names to be analyzed.
        string[,] mimetype = new string[N,2];

        Dictionary<string, string> mimetypes = new Dictionary<string, string>();
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            mimetypes.Add(inputs[0].ToLower(), inputs[1]);
        }

        for (int i = 0; i < Q; i++)
        {
            string FNAME = Console.ReadLine(); // One file name per line.
            string [] extension = FNAME.Split('.');
        
            if (extension.Length > 1 && mimetypes.TryGetValue(extension.Last().ToLower(), out string value))
            {
                Console.WriteLine(value);
            }
            else
                Console.WriteLine("UNKNOWN");   
        }
    }
}