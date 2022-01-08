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
    };

    public struct mapData
    {
        public int borderX;
        public int borderY;
        public int bounce;
    }

    static void Main(string[] args)
    {
        thorData thor = new thorData();
        giantData giant = new giantData();

        int kanteX = 2, kanteY = 2;

        /*
            Get first input DATA
        */
        thor.position = getThorInitInformation();
        thor.goTo = new int[] {0, 0};
        thor.giantsInHammerDistance = 0;
        thor.direction = new string[] { "", ""};
        
        giant.minDistance = 999999.99;
        giant.maxDistance = 0.0;
        giant.deltaDistance = 0.0;
        giant.middlePosition = new int[] {0, 0};
        int start2Flee = 2;

        // game loop
        while (true)
        {
            /*
                Get second input DATA
            */
            int[] ThorMissionData = getThorMission();
            thor.strikesAvailable = ThorMissionData[0];
            giant.number = ThorMissionData[1];
            //Console.Error.WriteLine($"Number of Giants: {giant.number}");
            //Console.Error.WriteLine($"Number of Strikes: {thor.strikesAvailable}");

            /*
                Get third input DATA
            */
            giant.positions = getGiantPosition(giant.number);

            /*
                Make some calculation.
                giantVShammer = check how many Giants there has to exist in range for an optimum on strikes per giants
            */
            thor.giantsVShammer = (giant.number / thor.strikesAvailable) == 0 ? 1 : giant.number / thor.strikesAvailable;
            Console.Error.WriteLine($"gVSh: {thor.giantsVShammer}");

            /*
                check how many Giants are in Radius
            */
            thor.giantsInHammerDistance = getCheckRadius(thor.position, giant.positions, giant.number);
            Console.Error.WriteLine($"Giants in Distance: {thor.giantsInHammerDistance}");

            /*
                check where the middle XY is, from all Giants
                (trying with finding the middle point of the nearest Giants which I need for strikes per giant)
            */
            double[] GiantMinMaxMidXYPosition = getGiantMinMaxMidXYPosition(giant.number, giant.positions, thor.position);
            // thor.giantsVShammer = (thor.giantsVShammer + 1) < giant.number ? thor.giantsVShammer + 1 : thor.giantsVShammer;

            // double[] GiantMinMaxMidXYPosition = getGiantMinMaxMidXYPositionVShammer(giant.number, giant.positions, thor.position, thor.giantsVShammer);

            /*
                Sort the return of getGiantMin..... to different Variables of the struct
            */
            giant.minDistance = GiantMinMaxMidXYPosition[0];
            giant.maxDistance = GiantMinMaxMidXYPosition[1];
            giant.middlePosition = new int[] { (int)GiantMinMaxMidXYPosition[2], (int)GiantMinMaxMidXYPosition[3] };
            thor.goTo[0] = giant.middlePosition[0];
            thor.goTo[1] = giant.middlePosition[1];
            giant.nearest = new int []{ (int)GiantMinMaxMidXYPosition[4], (int)GiantMinMaxMidXYPosition[5] };
            Console.Error.WriteLine($"Min: {giant.minDistance}; max: {giant.maxDistance}; \nMid: {giant.middlePosition[0]}:{giant.middlePosition[1]}");


            Console.Error.WriteLine($"Fleeing: {start2Flee}");


            /*
                Start moving around and do some action :)
            */

            if (thor.giantsInHammerDistance >= thor.giantsVShammer)
            {
                Console.Error.WriteLine($"STRIKE 1");
                actionThor("strike");
                start2Flee = 0;
            }
            else if (giant.minDistance >= 1)
            {
                if (start2Flee == 1 || giant.minDistance <= 2.61)
                {
                    Console.Error.WriteLine($"I have to flee :(");  
                    start2Flee = 1;
                    Console.Error.WriteLine($"Thor Pos: {thor.position[0]}:{thor.position[1]}");
                    Console.Error.WriteLine($"Giant Near: {giant.nearest[0]}:{giant.nearest[1]}");

                    // Thor.pos = 0
                    // Direction = 1
                    // Check X Abys!

                    switch(thor.position[0])
                    {
                        case 0:
                            thor.direction[1] = "E";
                            ++thor.position[0];
                            kanteX = 1;
                            break;
                        case 40:
                            thor.direction[1] = "W";
                            --thor.position[0];
                            kanteX = 0;
                            break;
                        default:
                            thor.direction[1] = "";
                            break;
                    };

                    // Check X Axis 
                    // Thor.pos = 0
                    // Direction = 1
                    // kanteX = 1 -> Thor was at 0
                    // kanteX = 0 -> Thor was at 40 or default
                     
                    if (kanteX == 1 && thor.position[0] < giant.nearest[0])
                    {
                        thor.direction[1] = "E";
                        --thor.position[0];
                    }
                    else if (thor.position[0] < giant.nearest[0])
                    {
                        thor.direction[1] = "W";
                        ++thor.position[0];
                    }
                    else
                        thor.direction[1] = "";


                    // Thor.pos = 1
                    // Direction = 0
                    // Check Y- Abys!


                    switch(thor.position[1])
                    {
                        case 0:
                            Console.Error.WriteLine("I am at the north abys!");
                            kanteY = 1;
                            break;
                        case 17:
                            Console.Error.WriteLine("I am at the south abys!");
                            kanteY = 0;
                            break;
                        default:
                            Console.Error.WriteLine("No north/south abys!)");
                            thor.direction[0] = "";
                            break;
                    };

                    // Thor.pos = 0
                    // Direction = 0
                    // Check Y Axis!

                    switch(kanteY)
                    {
                        case 2:
                            Console.Error.WriteLine($"kanteYswitch 2?: {kanteY}");
                            if (thor.position[1] > giant.nearest[1])
                            {
                                thor.direction[0] = "S";
                                ++thor.position[1];
                            }
                            else if (thor.position[1] < giant.nearest[1])
                            {
                                thor.direction[0] = "N";
                                --thor.position[1];
                            }
                            else
                                thor.direction[0] = "";
                            break;
                        case 0:
                            Console.Error.WriteLine($"kanteYswitch 0?: {kanteY}");
                            thor.direction[0] = "N";
                            --thor.position[1];
                            break;
                        case 1:
                            Console.Error.WriteLine($"kanteYswitch 1?: {kanteY}");
                            thor.direction[0] = "S";
                            ++thor.position[1];
                            break;
                        default:
                            break;
                    }                    


                    if (thor.direction[0] == "" && thor.direction[1] == "")
                        Console.WriteLine("WAIT");
                    else
                        Console.WriteLine($"{thor.direction[0]}{thor.direction[1]}");
                    

                }
                else if ( (thor.position[0] == thor.goTo[0] && thor.position[1] == thor.goTo[1]) )
                {
                    Console.Error.WriteLine($"I am there. Going to wait");
                    Console.WriteLine("WAIT");
                }
                else
                {
                    Console.Error.WriteLine($"Not in position");
                    if (giant.minDistance >= 1)
                    {
                        Console.Error.WriteLine($"Going to Position");
                        Console.Error.WriteLine($"Thor Pos: {thor.position[0]}:{thor.position[1]}");
                        Console.Error.WriteLine($"Thor goTo: {thor.goTo[0]}:{thor.goTo[1]}");
                        Console.Error.WriteLine($"Giant Near: {giant.nearest[0]}:{giant.nearest[1]}");

                        //N  => direction[0] =  Y Axis
                        // W => direction[1] =  X Axis

                        thor.direction[0] = thor.position[0] < thor.goTo[0] ? "S" : thor.position[0] > thor.goTo[0] ? "N" : "";
                        thor.direction[1] = thor.position[1] < thor.goTo[1] ? "E" : thor.position[1] > thor.goTo[1] ? "W" : "";

                        moveThor($"{thor.direction[0]}{thor.direction[1]}", 0);

                        // thor.position[0] = thor.position[0] < thor.goTo[0] ? ++thor.position[0] : thor.position[0] > thor.goTo[0] ? --thor.position[0] : thor.position[0];
                        
                        // thor.position[1] = thor.position[1] < thor.goTo[1] ? ++thor.position[1] : thor.position[1] > thor.goTo[1] ? --thor.position[1] : thor.position[1];
                        // Console.WriteLine($"{thor.direction[1]}{thor.direction[0]}");
                    }
                    else
                    {   
                        Console.Error.WriteLine($"I have flee arround");
                        start2Flee = 1;
                        Console.Error.WriteLine($"Fleeing: {start2Flee}");
                        // if (giant.maxDistance > 10)
                        // {
                        Console.Error.WriteLine($"Thor Pos: {thor.position[0]}:{thor.position[1]}");
                        Console.Error.WriteLine($"Giant Near: {giant.nearest[0]}:{giant.nearest[1]}");

                        thor.direction[0] = thor.position[0] < giant.nearest[0] ? "W" : thor.position[0] > giant.nearest[0] ? "E" : "";
                        thor.position[0] = thor.position[0] < giant.nearest[0] ? --thor.position[0] : thor.position[0] > giant.nearest[0] ? ++thor.position[0] : thor.position[0];
                        
                        thor.direction[1] = thor.position[1] < giant.nearest[1] ? "N" : thor.position[1] > giant.nearest[1] ? "S" : "";
                        thor.position[1] = thor.position[1] < giant.nearest[1] ? --thor.position[1] : thor.position[1] > giant.nearest[1] ? ++thor.position[1] : thor.position[1];
                    Console.WriteLine($"{thor.direction[1]}{thor.direction[0]}");

                    }


                }

            }
            else
            {
                Console.Error.WriteLine($"STRIKE 2");
                actionThor("strike");
                start2Flee = 0;
            }

        }
    }

    static int[] getThorInitInformation()
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int[] thor = new int[2];
        thor[0] = int.Parse(inputs[0]);
        thor[1] = int.Parse(inputs[1]);
        return thor;
    }

    static int[] getThorMission()
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int[] thorMission = new int[2];
        thorMission[0] = int.Parse(inputs[0]); // the remaining number of hammer strikes.
        thorMission[1] = int.Parse(inputs[1]); // the number of giants which are still present on the map.
        return thorMission;
    } 

    static void actionThor(string action)
    {
        Console.Error.Write($"Thor going to Action: ");
        if (action == "wait")
        {
            Console.Error.WriteLine("WAIT");
            Console.WriteLine("WAIT");
        } else if (action == "strike")
        {
            Console.Error.WriteLine("Striking");
            Console.WriteLine("STRIKE");
        }

    }

    static void moveThor(string direction, int bounce)
    {
        Console.Error.Write($"Going to move Thor: ");
        switch(direction)
        {
            case string XY when (InStr(1, direction, "N") && bounce == 0):
                thor.position[1]--;
                break;
            case string xy when (InStr(1, direction, "S") && bounce == 0):
                thor.position[1]++;
                break;
            case string XY when (InStr(1, direction, "E") && bounce == 0):
                thor.position[0]++;
                break;
            case string xy when (InStr(1, direction, "W") && bounce == 0):
                thor.position[0]--;
                break;
        }
        Console.Error.WriteLine($"{direction}");
        Console.WriteLine($"{direction}");
    
        
    }

    static int[,] getGiantPosition(int giantNumber)
    {
        int[,] giantPosition = new int [giantNumber, 2];

        for (int i = 0; i < giantNumber; i++)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            //Console.Error.WriteLine($"Giant {i} -> {X}:{Y}");
            giantPosition[i, 0] = X;
            giantPosition[i, 1] = Y;
        }
        return giantPosition;
    }

    static double[] getGiantMinMaxMidXYPosition(int giantNumber, int[,] giantPosition, int[] thorPosition)
    {
        double[] giantMiddle = new double[] {0, 0}; //middle postion for thor 
        double[] giantDistances = new double[6]; //min, max, MidX, MidY, NearX, NearY
        double[] nearest = new double[2];

        giantDistances[0] = 99999999.99;
        for (int i = 0; i < giantNumber; ++i)
        {
            giantMiddle[0] += giantPosition[i, 0];
            giantMiddle[1] += giantPosition[i, 1];

            double giantDistance = Math.Sqrt(
                ((thorPosition[0] - giantPosition[i,0]) * (thorPosition[0] - giantPosition[i,0]))
                +
                ((thorPosition[1] - giantPosition[i,1]) * (thorPosition[1] - giantPosition[i,1]))
            );

            if (giantDistance < giantDistances[0])
            {
                giantDistances[0] = giantDistance;
                nearest[0] = giantPosition[i,0];
                nearest[1] = giantPosition[i,1];

            }
            giantDistances[1] = giantDistance > giantDistances[1] ? giantDistance : giantDistances[1];
            //Console.Error.WriteLine($"Dis: {giantDistance}; gDis: {giantDistances[0]}");
        }

        giantMiddle[0] /= giantNumber;
        giantMiddle[1] /= giantNumber;

        giantDistances[2] = giantMiddle[0];
        giantDistances[3] = giantMiddle[1];

        giantDistances[4] = nearest[0];
        giantDistances[5] = nearest[1];
        

        
        // double xyDelta = (Math.Sqrt(distanceDelta * distanceDelta) / 2);
        // a2+a2=c2
        // 2*a²=c²
        // a² = c²/2
        // a = wrz(c²/2)
        

        return giantDistances;
    }

    static double[] getGiantMinMaxMidXYPositionVShammer(int giantNumber, int[,] giantPosition, int[] thorPosition, int giantsToKill)
    {
        double[] giantMiddle = new double[] {0, 0}; //middle postion for thor 
        double[] giantDistances = new double[6]; //min, max, MidX, MidY, NearX, NearY
        double[] nearest = new double[2];


        /*
            Start to sort Giants to distance, Bubble Sort :)
        */

        for (int k = 0; k < giantNumber - 1; ++k)
        {
            for (int i = 0; i < giantNumber - 1; ++i)
            {
                   
                    double giantDistance = Math.Sqrt(
                        ((thorPosition[0] - giantPosition[i,0]) * (thorPosition[0] - giantPosition[i,0]))
                        +
                        ((thorPosition[1] - giantPosition[i,1]) * (thorPosition[1] - giantPosition[i,1]))
                    );

                    double giantDistance2 = Math.Sqrt(
                        ((thorPosition[0] - giantPosition[i + 1, 0]) * (thorPosition[0] - giantPosition[i + 1, 0]))
                        +
                        ((thorPosition[1] - giantPosition[i + 1, 1]) * (thorPosition[1] - giantPosition[i + 1, 1]))
                    );
                    if (giantDistance > giantDistance2)
                    {
                        int tempX = giantPosition[i, 0];
                        int tempY = giantPosition[i, 1];

                        giantPosition[i, 0] = giantPosition[i + 1, 0];
                        giantPosition[i, 1] = giantPosition[i + 1, 1];

                        giantPosition[i + 1, 0] = tempX;
                        giantPosition[i + 1, 1] = tempY;
                    }

            }
        }

        giantDistances[0] = 999999.99;
        Console.Error.WriteLine($"Giant 2 Kill: {giantsToKill}");
        for (int i = 0; i < giantsToKill; ++i)
        {
            giantMiddle[0] += giantPosition[i, 0];
            giantMiddle[1] += giantPosition[i, 1];

            double giantDistance = Math.Sqrt(
                ((thorPosition[0] - giantPosition[i,0]) * (thorPosition[0] - giantPosition[i,0]))
                +
                ((thorPosition[1] - giantPosition[i,1]) * (thorPosition[1] - giantPosition[i,1]))
            );

            if (giantDistance < giantDistances[0])
            {
                giantDistances[0] = giantDistance;
                nearest[0] = giantPosition[i,0];
                nearest[1] = giantPosition[i,1];

            }
            giantDistances[1] = giantDistance > giantDistances[1] ? giantDistance : giantDistances[1];
            Console.Error.WriteLine($"Dis: {giantDistance}; pos:  {giantPosition[i, 0]}:{giantPosition[i, 1]}");
        }

        giantMiddle[0] /= giantsToKill;
        giantMiddle[1] /= giantsToKill;

        giantDistances[2] = giantMiddle[0];
        giantDistances[3] = giantMiddle[1];

        giantDistances[4] = nearest[0];
        giantDistances[5] = nearest[1];
        


        return giantDistances;
    }

    static int getCheckRadius(int[] thorPos, int[,] giantPos, int giantNumber)
    {
        // 10, 10 Position 
        // check 4, 4 to 15, 15 Position
        int subPos = 6, addPos = 6, countedGiants = 0;


        int[] firstXY = new int[2];
        int[] secondXY = new int[2];
        
        firstXY[0] = thorPos[0] - subPos;
        firstXY[1] = thorPos[1] - subPos;
        secondXY[0] = thorPos[0] + addPos;
        secondXY[1] = thorPos[1] + addPos;
        
        for (int i = 0; i < giantNumber; ++i)
        {
            if (giantPos[i, 0] > firstXY[0] && giantPos[i, 0] < secondXY[0] && giantPos[i, 1] > firstXY[1] && giantPos[i, 1] < secondXY[1])
                countedGiants++;
        }   

        return countedGiants;     
    }
}