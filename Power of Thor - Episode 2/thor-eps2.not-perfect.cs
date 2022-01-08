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
        int thorX = int.Parse(inputs[0]);
        int thorY = int.Parse(inputs[1]);
        
        string direction1 = "", direction2 = "";
        int toPosThorX = 0, toPosThorY = 0;
        int nearestEnemyX = 0, nearestEnemyY = 0;
        double distance, distanceMin = 9999, distanceMax = 0, distanceDelta = 0;
        

        // game loop
        while (true)
        {
            toPosThorX = 0;
            toPosThorY = 0;
            distanceMax = 0;
            distanceMin = 99999999;
            distanceDelta = 0;

            inputs = Console.ReadLine().Split(' ');
            int H = int.Parse(inputs[0]); // the remaining number of hammer strikes.
            int N = int.Parse(inputs[1]); // the number of giants which are still present on the map.

            int[,] enemyXY = new int [N,2];

            for (int i = 0; i < N; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int X = int.Parse(inputs[0]);
                int Y = int.Parse(inputs[1]);
                Console.Error.WriteLine($"{X}:{Y}");
                enemyXY[i, 0] = X;
                enemyXY[i, 1] = Y;
            }

            if (N > 0) 
            {
                for (int i = 0; i < N; ++i)
                {
                    toPosThorX += enemyXY[i,0]; 
                    toPosThorY += enemyXY[i,1];
                    distance = Math.Sqrt(((thorX - enemyXY[i,0]) * (thorX - enemyXY[i,0])) + ((thorY - enemyXY[i,1]) *(thorY - enemyXY[i,1])));
                    distanceMin = distanceMin > distance ? distance : distanceMin;
                    distanceMax = distanceMax < distance ? distance : distanceMax;
                }
                toPosThorX /= N;
                toPosThorY /= N;
                distanceDelta = distanceMax - distanceMin;

                double xyDelta = (Math.Sqrt(distanceDelta * distanceDelta) / 2);
                // a2+a2=c2
                // 2*a²=c²
                // a² = c²/2
                // a = wrz(c²/2)
                

                Console.Error.WriteLine($"disMin {distanceMin}; \ndisMax {distanceMax}; \ndisDelta {distanceDelta}\nxy {xyDelta}");

            }
            else
            {
                toPosThorX += enemyXY[0,0]; 
                toPosThorY += enemyXY[0,1];
            }


            Console.Error.WriteLine($"thor Pos: {thorX}:{thorY}\nenem Pos: {toPosThorX}:{toPosThorY}");

            
            if (distanceMin <= 2)
                Console.WriteLine("STRIKE");
            else if (thorX == toPosThorX && thorY == toPosThorY)
                if (distanceMax >= 4)
                {
                    Console.Error.WriteLine($"where am i {thorY}");
                    if (thorY >= 16)
                    {
                        Console.WriteLine($"NW");

                        --thorX;
                        --thorY;
                    }
                    else
                    {
                        Console.WriteLine($"SE");
                        ++thorX;
                        ++thorY;
                    }
                }
                else
                    Console.WriteLine("WAIT");
            else
            { 
                //if (distanceMax >= 5)
                {

                    direction1 = thorY < toPosThorY ? "S" : thorY > toPosThorY ? "N" : "";
                    thorY = thorY < toPosThorY ? ++thorY : thorY > toPosThorY ? --thorY : thorY;

                    direction2 = thorX < toPosThorX ? "E" : thorX > toPosThorX ? "W" : "";
                    thorX = thorX < toPosThorX ? ++thorX : thorX > toPosThorX ? --thorX : thorX;

                    Console.WriteLine($"{direction1}{direction2}");
                }
            }



            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


            // The movement or action to be carried out: WAIT STRIKE N NE E SE S SW W or N
            // Console.WriteLine("WAIT");
        }
    }
}