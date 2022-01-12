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
    public struct giantData
    {
        public int number;
        public double minDistance;
        public double maxDistance;
        public double deltaDistance;
        public int[] middlePosition;
        public int[,] positions;
        public int[] nearest;
    };

    public struct thorData
    {
        public int[] position;
        public int strikesAvailable;
        public int[] goTo;
        public string[] direction;
        public int giantsInHammerDistance;
        public int giantsVShammer;
        public bool fleeing;
        public int bounced; // 0 = YAxis normal, 1 = bounced top, 2 = bounced buttom
                            // 3 = XAxis normal, 4 = bounced left, 5 = bounced right

    };

    public struct mapData
    {
        public int borderX;
        public int borderY;
    }

    static void getThorInitInformation(ref thorData lThor)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        lThor.position = new int[2];
        lThor.position[0] = int.Parse(inputs[0]);
        lThor.position[1] = int.Parse(inputs[1]);
        Console.Error.WriteLine($"Get Thor Information: {lThor.position[0]}:{lThor.position[1]}");
    }

    static void getThorMission(ref thorData lThor, ref giantData lGiant)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        // thorMission = new int[2];
        lThor.strikesAvailable = int.Parse(inputs[0]); // the remaining number of hammer strikes.
        lGiant.number = int.Parse(inputs[1]); // the number of giants which are still present on the map.
        Console.Error.WriteLine($"Strikes: {lThor.strikesAvailable}; giants: {lGiant.number}");
    } 

    static void getGiantPosition(ref giantData lGiant)
    {
        lGiant.positions = new int [lGiant.number, 2];

        for (int i = 0; i < lGiant.number; i++)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            // Console.Error.WriteLine($"Giant {i} -> {X}:{Y}");
            lGiant.positions[i, 0] = X;
            lGiant.positions[i, 1] = Y;
        }
    }

    static void sortGiantPosition(ref thorData lThor, ref giantData lGiant)
    {
        /*
            Start to sort Giants to distance, Bubble Sort :)
        */

        for (int k = 0; k < lGiant.number - 1; ++k)
        {
            for (int i = 0; i < lGiant.number - 1; ++i)
            {
                   
                    double giantDistance = Math.Sqrt(
                        ((lThor.position[0] - lGiant.positions[i,0]) * (lThor.position[0] - lGiant.positions[i,0]))
                        +
                        ((lThor.position[1] - lGiant.positions[i,1]) * (lThor.position[1] - lGiant.positions[i,1]))
                    );

                    double giantDistance2 = Math.Sqrt(
                        ((lThor.position[0] - lGiant.positions[i + 1, 0]) * (lThor.position[0] - lGiant.positions[i + 1, 0]))
                        +
                        ((lThor.position[1] - lGiant.positions[i + 1, 1]) * (lThor.position[1] - lGiant.positions[i + 1, 1]))
                    );
                    if (giantDistance > giantDistance2)
                    {
                        int tempX = lGiant.positions[i, 0];
                        int tempY = lGiant.positions[i, 1];

                        lGiant.positions[i, 0] = lGiant.positions[i + 1, 0];
                        lGiant.positions[i, 1] = lGiant.positions[i + 1, 1];

                        lGiant.positions[i + 1, 0] = tempX;
                        lGiant.positions[i + 1, 1] = tempY;
                    }

            }
        }
        Console.Error.WriteLine($"ThorPos: {lThor.position[0]}:{lThor.position[1]}");
        for (int i = 0; i < lGiant.number; i++)
        {

            Console.Error.WriteLine($"Giant {i} -> {lGiant.positions[i, 0]}:{lGiant.positions[i, 1]}");

        }
    }

    static void mathGiantPosition(ref thorData lThor, ref giantData lGiant)
    {
        lGiant.middlePosition = new int[] {0, 0};
        lGiant.minDistance = 999999999.99;
        lGiant.maxDistance = 0.0;
        lGiant.nearest = new int[] {0, 0};

        for (int i = 0; i < lGiant.number; ++i)
        {
            lGiant.middlePosition[0] += lGiant.positions[i, 0];
            lGiant.middlePosition[1] += lGiant.positions[i, 1];

            double giantDistance = Math.Sqrt(
                ((lThor.position[0] - lGiant.positions[i, 0]) * (lThor.position[0] - lGiant.positions[i, 0]))
                +
                ((lThor.position[1] - lGiant.positions[i, 1]) * (lThor.position[1] - lGiant.positions[i, 1]))
            );

            if (lGiant.minDistance > giantDistance)
            {
                lGiant.minDistance = giantDistance;
                Console.Error.WriteLine($"min: {lGiant.minDistance} - {giantDistance}");

                lGiant.nearest[0] = lGiant.positions[i, 0];
                lGiant.nearest[1] = lGiant.positions[i, 1];

                // Console.Error.Write($"S: {lGiant.positions[i,0]}:{lGiant.positions[i,1]} -> ");
                // Console.Error.WriteLine($"T: {lGiant.nearest[0]}:{lGiant.nearest[1]}");

            }
            
        }
        lGiant.middlePosition[0] /= lGiant.number;
        lGiant.middlePosition[1] /= lGiant.number;
        Console.Error.WriteLine($"MiddleGiant: {lGiant.middlePosition[0]}:{lGiant.middlePosition[1]}");
    }

    static void getCheckRadius(ref thorData lThor, ref giantData lGiant)
    {
        int subPos = 6, addPos = 6;
        lThor.giantsInHammerDistance = 0;

        int[,] XY = new int[2,2];
        
        XY[0,0] = lThor.position[0] - subPos;
        XY[0,1] = lThor.position[1] - subPos;
        XY[1,0] = lThor.position[0] + addPos;
        XY[1,1] = lThor.position[1] + addPos;

        for (int i = 0; i < lGiant.number; ++i)
        {
            if (lGiant.positions[i,0] > XY[0,0] && lGiant.positions[i,0] < XY[1,0]
             && lGiant.positions[i,1] > XY[0,1] && lGiant.positions[i,1] < XY[1,1])
             lThor.giantsInHammerDistance++;
        }   

        if (lGiant.minDistance < 3)
            lThor.fleeing = true;
    }

    static void moveThor(ref thorData lThor, ref giantData lGiant)
    {
        // 0 = YAxis normal, 1 = bounced top, 2 = bounced buttom
        // 3 = bounced left, 5 = bounced right
        switch(lThor.bounced)
        {
            case 0:

                Console.Error.WriteLine($"checking NS");
                // Console.Error.Write($"Copying middle 2 goTo {lGiant.middlePosition[0]}:{lGiant.middlePosition[1]}");
                // Console.Error.WriteLine($"->{lThor.goTo[0]}:{lThor.goTo[1]}");
                // Console.Error.WriteLine($"thorPos: {lThor.position[0]}:{lThor.position[1]}");
                lThor.direction[0] = lThor.position[1] < lThor.goTo[1] ? "S"                 : lThor.position[1] > lThor.goTo[1] ? "N"                  : "";
                lThor.position[1]  = lThor.position[1] < lThor.goTo[1] ? ++lThor.position[1] : lThor.position[1] > lThor.goTo[1] ? --lThor.position[1]  : lThor.position[1];
                
                Console.Error.WriteLine($"checking EW");
                // Console.Error.Write($"Copying middle 2 goTo {lGiant.middlePosition[0]}:{lGiant.middlePosition[1]}");
                // Console.Error.WriteLine($"->{lThor.goTo[0]}:{lThor.goTo[1]}");
                // Console.Error.WriteLine($"thorPos: {lThor.position[0]}:{lThor.position[1]}");
                lThor.direction[1] = lThor.position[0] < lThor.goTo[0] ? "E"                 : lThor.position[0] > lThor.goTo[0] ? "W"                  : "";
                lThor.position[0]  = lThor.position[0] < lThor.goTo[0] ? ++lThor.position[0] : lThor.position[0] > lThor.goTo[0] ? --lThor.position[0]  : lThor.position[0];
                Console.Error.WriteLine($"thorPos: {lThor.position[0]}:{lThor.position[1]}");
                break;

            case 1:
               break;

            case 2:
                break;
            
            case 3:
                break;
            case 4:
                break;
        }
        

        Console.Error.WriteLine($"goTo: ->{lThor.direction[0]}:{lThor.direction[1]}<-");
        if (lThor.direction[0] != "" || lThor.direction[1] != "")
            Console.WriteLine($"{lThor.direction[0]}{lThor.direction[1]}");
        else
            Console.WriteLine("WAIT");


    }

    static void Main(string[] args)
    {
        thorData thor = new thorData();
        giantData giant = new giantData();
        mapData map = new mapData();

        map.borderX = 40;
        map.borderY = 18;
        thor.fleeing = false;

        getThorInitInformation(ref thor);
        thor.goTo = new int[2];
        thor.direction = new string[2];


        while (true)
        {
            getThorMission(ref thor, ref giant);
            getGiantPosition(ref giant);
            sortGiantPosition(ref thor, ref giant);

            thor.giantsVShammer = (giant.number / thor.strikesAvailable) == 0 ? 1 : giant.number / thor.strikesAvailable;
            Console.Error.WriteLine($"gVSh: {thor.giantsVShammer} - {thor.giantsInHammerDistance}");

            mathGiantPosition(ref thor, ref giant);
            getCheckRadius(ref thor, ref giant);

            if (thor.giantsInHammerDistance >= thor.giantsVShammer)
            {
                Console.Error.WriteLine($"Giants in Hammer {thor.giantsInHammerDistance} is >= giantsVShammer {thor.giantsVShammer}");
                Console.WriteLine("STRIKE");
                thor.giantsInHammerDistance = 0;
                thor.giantsVShammer = 0;
                thor.bounced = 0;
            }
            else if (thor.fleeing == false)
            {
                Console.Error.WriteLine($"Not fleeing! Going to position");
                thor.goTo[0] = giant.middlePosition[0];
                thor.goTo[1] = giant.middlePosition[1];
                moveThor(ref thor, ref giant);

            }
            else if (thor.fleeing == true)
            {

                Console.Error.WriteLine($"Fleeing!!!");
                
                if (giant.nearest[0] < thor.position[0]) // flee to the E (toGo )
                {
                    Console.Error.WriteLine($"flee to E");
                    thor.goTo[0] = 40;
                }
                if (thor.position[0] <= giant.nearest[0]) // flee to the W
                {
                    Console.Error.WriteLine($"flee to W");
                    thor.goTo[0] = 0;
                }
                
                if (giant.nearest[1] < thor.position[1]) // flee to the S
                {
                    Console.Error.WriteLine($"flee to S");
                    thor.goTo[1] = 18;
                }
                if (thor.position[1] <= giant.nearest[1]) // flee to the N
                {
                    Console.Error.WriteLine($"flee to N");
                    thor.goTo[1] = 0;
                }

                

                // 0 = YAxis normal, 1 = bounced top, 2 = bounced buttom
                // 3 = bounced left, 5 = bounced right
                
              
                moveThor(ref thor, ref giant);
                
            }
            else
            {
                Console.Error.WriteLine("Waiting");
                Console.WriteLine("WAIT");

            }

        }

    }
}