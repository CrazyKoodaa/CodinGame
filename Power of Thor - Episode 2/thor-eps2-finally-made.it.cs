using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
 struct sGiant{
        public int iNumber;
        public Vector2 vPosition;
        public float fDistance;
    }

public class Map
{
    internal const int iMaxWidth = 41, iMaxHeight = 19;
    internal string[,] s2dGrid = new string[iMaxWidth, iMaxHeight];

    public void generateMap()
    {
        for (int y = 0; y < iMaxHeight; y++)
            for (int x = 0; x < iMaxWidth; x++)
                s2dGrid[x,y] = ".";
    }

    public void showMap()
    {
        Console.Error.WriteLine($"                          1                   2                   3                  4");
        Console.Error.WriteLine($"      0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9  0");
        for (int y = 0; y < iMaxHeight; y++)
        {
            Console.Error.Write($"{y,2} - ");
            for (int x = 0; x < iMaxWidth; x++)
            {
                Console.Error.Write($"{s2dGrid[x,y],2}");
            }
            Console.Error.Write("\n");
        }
    }

    public void setItemOnMap(string c, int x, int y)
    {
        s2dGrid[x, y] = c;
    }

}

class Person : Map
{
    public Vector2 vOldPosition, vNewPosition;
    public int iStrikesAvailable, iMinEnemyTarget = 0, iEnemyInRange = 0;
    public Vector2 vHammerRangeNegative = new Vector2(-4, -4);
    public Vector2 vHammerRangePositive = new Vector2( 6,  6);

    public string sAction = "WAIT";

    public void hammerRange(ref Map mg)
    {

        Vector2 hammerRangeTopLeft = new Vector2();
        hammerRangeTopLeft = vOldPosition + vHammerRangeNegative;
        Vector2 hammerRangeButtomRight = new Vector2();
        hammerRangeButtomRight = Vector2.Add(vOldPosition, vHammerRangePositive);

        if (hammerRangeTopLeft.Y <= 0)
            hammerRangeTopLeft.Y = 0;
        
        if (hammerRangeTopLeft.X <= 0)
            hammerRangeTopLeft.X = 0;

        if (hammerRangeButtomRight.Y >= iMaxHeight)
            hammerRangeButtomRight.Y = iMaxHeight;

        if (hammerRangeButtomRight.X >= iMaxWidth)
            hammerRangeButtomRight.X = iMaxWidth;
        
        // Console.Error.WriteLine($"{vOldPosition}:{hammerRangeTopLeft}");

        // Console.Error.WriteLine($"{hammerRangeButtomRight}");

        iEnemyInRange = 0;
        for (int y = (int)hammerRangeTopLeft.Y; y < hammerRangeButtomRight.Y; y++) //&& y < iMaxHeight -1 // && y < iMaxHeight && y > 0
        {
            for (int x = (int)hammerRangeTopLeft.X; x < hammerRangeButtomRight.X; x++) // && x < iMaxWidth -1 //&& x < iMaxWidth && x > 0
            {
                if (mg.s2dGrid[x,y] == ".")
                    mg.setItemOnMap("*", x, y);
                if (mg.s2dGrid[x,y] == "g" || mg.s2dGrid[x,y] == "G")
                {
                    mg.setItemOnMap("G", x, y);
                    iEnemyInRange++;
                    // if (iEnemyInRange >= iMinEnemyTarget)
                    // {
                    //     sAction = "STRIKE";
                    // }
                }
            
            }
        }
        Console.Error.WriteLine($"Enemy in Range: {iEnemyInRange}-{iMinEnemyTarget}");
    }


    public void moveTo(Vector2 moveTo, Vector2 giant, Vector2 giant2, Vector2 giant3)
    {
        // implementing 
        Console.Error.WriteLine($"moveto: {moveTo} G1: {giant} G2: {giant2} G3: {giant3}");

        Vector2 vFromThorToGoal = Vector2.Normalize(moveTo - vOldPosition);
        Vector2 vFromThorToGiant1 = Vector2.Normalize(giant - vOldPosition);
        Vector2 vFromThorToGiant2 = Vector2.Normalize(giant2 - vOldPosition);
        Vector2 vFromThorToGiant3 = Vector2.Normalize(giant3 - vOldPosition);

        Single DOT1 = Vector2.Dot(vFromThorToGoal, vFromThorToGiant1);
        Single DOT2 = Vector2.Dot(vFromThorToGoal, vFromThorToGiant2);
        Single DOT3 = Vector2.Dot(vFromThorToGoal, vFromThorToGiant3);

        Single Distance = Vector2.Distance(vOldPosition, giant);

        // Console.Error.WriteLine($"giant: {giant}, T2G: {vFromThorToGoal}, T2E: {vFromThorToGiant1}, \nDot1 {DOT1}, Dot2 {DOT2}, Distance {Distance}");
        Console.Error.WriteLine($"D0: {vFromThorToGoal}, Dot1 {DOT1}, Dot2 {DOT2}, Dot3 {DOT3}, Distance {Distance}");

        float tempX = (float)vFromThorToGoal.X;
        float tempY = (float)vFromThorToGoal.Y;
        string sX = "";
        string sY = "";



        switch (tempX)
        {
            case float x when tempX < 0:
                if (Distance <= 2.3 && (Double.IsNaN(DOT1) || DOT1 >= 0.9 || (DOT1 < 0 && DOT1 > -0.9) || DOT3 < 0))
                {
                    Console.Error.WriteLine("E1");
                    sX = "E";
                    vOldPosition.X++;
                }
                else
                {
                    Console.Error.WriteLine("W1");
                    sX = "W";
                    vOldPosition.X--;
                }
            break;

            case float x when tempX > 0:
                if (Distance <= 2 && (Double.IsNaN(DOT1) || DOT1 < 0 || DOT2 < 0 ||  DOT3 < 0))
                {
                    Console.Error.WriteLine("E2");
                    sX = "E";
                    vOldPosition.X++;
                } else if (DOT1 >=0.5 && DOT2 >=0.3 && DOT3 >=0.8)
                {
                    Console.Error.WriteLine("E5");
                    sX = "E";
                    vOldPosition.X++;

                } else 
                {
                    Console.Error.WriteLine("Nothing4");
                    sX = "";
                }
            break;
            default:
                if (Distance <= 2 && (DOT1 < 0 || DOT3 < 0))
                {
                    Console.Error.WriteLine("W2");
                    sX = "W";
                    vOldPosition.X--;
                } 
                else if (Distance <= 2 && (Double.IsNaN(DOT1) || DOT1 < 0 || DOT2 < 0 ||  DOT3 < 0))
                {
                    Console.Error.WriteLine("E3");
                    sX = "E";
                    vOldPosition.X++;
                }
                else if (DOT1 >=0.8 && DOT2 >=0.8 && DOT3 >=0.8)
                {
                    Console.Error.WriteLine("E4");
                    sX = "E";
                    vOldPosition.X++;

                } else 
                {
                    Console.Error.WriteLine("Nothing5");
                    sX = "";
                }
            break;
        }

        switch (tempY)
        {
            case float y when tempY < 0:

                Console.Error.WriteLine("N1");
                sY = "N";
                vOldPosition.Y--;
              
            break;

            case float y when tempY > 0:
                if (Distance <= 2.3 && (DOT1 == 0 || DOT1 >= 0.7 || DOT2 >= 0.8 || DOT1 < 0 || DOT2 < 0 ||  DOT3 < 0))
                {
                    if (vOldPosition.Y == 1)
                    {
                        Console.Error.WriteLine("North Abyss 1");
                        sY = "";
                    } else
                    {
                        Console.Error.WriteLine("N2");
                        sY = "N";
                        vOldPosition.Y--;
                    }
                } else if (DOT2 < 0.89)
                {
                    Console.Error.WriteLine("S1");
                    sY = "S";
                    vOldPosition.Y++;
                }
                else
                    sY = "";

            break;
            default:
                if (Distance <= 2 && (Double.IsNaN(DOT1) || DOT1 < 0))
                 {
                    
                    Console.Error.WriteLine("N3");
                    sY = "N";
                    vOldPosition.Y--;
                } else if (DOT1 == 1 && DOT2 == 1 && DOT3 == 1)    
                {
                    
                    Console.Error.WriteLine("N4");
                    sY = "N";
                    vOldPosition.Y--;
                }  else    
                    sY = "";
            break;
        }

        sAction = sY + sX;

        if (sAction == "")
            sAction = "WAIT";

    }
}

class Enemy : Map
{
    public int iGiantsAvailable;

    public sGiant [] information = new sGiant[150];

    public void sort()
    {
        // Console.Error.WriteLine($"Entering Sorting Alg with {iGiantsAvailable} Giants\n");
        for (int i = 0; i < iGiantsAvailable - 1; i++)
        {
            // Console.Error.WriteLine($"------------ Starting to sort {i}. Number");
            for (int x = 0; x < iGiantsAvailable - 1; x++)
            {
                // Console.Error.WriteLine($"Starting to analyze {information[x].fDistance} > {information[x+1].fDistance}");
                if (information[x].fDistance > information[x+1].fDistance)
                {
                    //sort
                    //Console.Error.WriteLine($"++++++++++++++++ Changing Pos {x} with {x+1}");
                    Vector2 tempPos = new Vector2();
                    float tempfDistance = 0.0f;

                    tempfDistance = information[x].fDistance;
                    tempPos = information[x].vPosition;

                    information[x].fDistance = information[x+1].fDistance;
                    information[x].vPosition = information[x+1].vPosition;

                    information[x+1].fDistance = tempfDistance;
                    information[x+1].vPosition = tempPos;
                }
            }
            // Console.Error.WriteLine("\n");
        }
    }

    public void showPosition()
    {
        for (int i = 0; i < iGiantsAvailable; i++)
            {
                Console.Error.WriteLine($"Giant {information[i].iNumber,3}. {information[i].vPosition,15} - Distance: {information[i].fDistance,4:F2}");
            }
    }

    public Vector2 analyse(int minGiantRange)
    {
        //analyse
        Vector2 vGiantsSum = new Vector2();
        Vector2 vGiantsDiv = new Vector2();

        for (int i = 0; i < minGiantRange; i++)
        {
            vGiantsSum.X = vGiantsSum.X + information[i].vPosition.X;
            vGiantsSum.Y = vGiantsSum.Y + information[i].vPosition.Y;
            //Console.Error.WriteLine($"analyse {i}: {vGiantsSum}");
        }

        double tempMin = Math.Ceiling((double)minGiantRange);
        double tempX = Math.Ceiling(vGiantsSum.X / tempMin);
        double tempY = Math.Ceiling(vGiantsSum.Y / tempMin);
        vGiantsDiv.X = (float)tempX;
        vGiantsDiv.Y = (float)tempY;

        //double result = Math.Ceiling(1.02);

        Console.Error.WriteLine($"E-analyse: Thor should go to = {vGiantsDiv}; minGiantRange = {minGiantRange}");

        setItemOnMap("Z", (int)vGiantsDiv.X, (int)vGiantsDiv.Y);
        return vGiantsDiv;
    }
}

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');


        Map MapGrid = new Map();
        Person Thor = new Person();
        Enemy Giants = new Enemy();

        Thor.vOldPosition = new Vector2(float.Parse(inputs[0]), float.Parse(inputs[1]));

        // Console.Error.WriteLine($"Main: Thor pos: {Thor.vOldPosition}");
        // MapGrid.generateMap();
        // MapGrid.setItemOnMap("t", (int)Thor.vOldPosition.X, (int)Thor.vOldPosition.Y);
        // MapGrid.showMap();

        // game loop
        while (true)
        {
            MapGrid.generateMap();
            MapGrid.setItemOnMap("T", (int)Thor.vOldPosition.X, (int)Thor.vOldPosition.Y);
            inputs = Console.ReadLine().Split(' ');
            Thor.iStrikesAvailable = int.Parse(inputs[0]); // the remaining number of hammer strikes.
            Giants.iGiantsAvailable = int.Parse(inputs[1]); // the number of giants which are still present on the map.

            Thor.iMinEnemyTarget = (int)Math.Ceiling((double)Giants.iGiantsAvailable / (double)Thor.iStrikesAvailable);
            if (Thor.iMinEnemyTarget == 0)
                Thor.iMinEnemyTarget = 1;

            // Console.Error.WriteLine($"Main 1: Giants.iGiantsAvailable: {Giants.iGiantsAvailable}");
            // Console.Error.WriteLine($"Main 1: Thor.iStrikesAvailable: { Thor.iStrikesAvailable}");
            // Console.Error.WriteLine($"Main 1: Thor.iMinEnemyTarget: {Thor.iMinEnemyTarget}");

            for (int i = 0; i < Giants.iGiantsAvailable; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int X = int.Parse(inputs[0]);
                int Y = int.Parse(inputs[1]);
                Vector2 position = new Vector2(X, Y);

                Giants.information[i].iNumber = i;
                Giants.information[i].vPosition = position;
                Giants.information[i].fDistance = Vector2.Distance(Thor.vOldPosition, Giants.information[i].vPosition);
                
                MapGrid.setItemOnMap("g", (int)Giants.information[i].vPosition.X, (int)Giants.information[i].vPosition.Y);
            }
            //MapGrid.showMap();

            // Giants.showPosition();
            // Console.Error.WriteLine("-------------");
            Giants.sort();
            // Giants.showPosition();
            
            Thor.hammerRange(ref MapGrid);
            MapGrid.showMap();
            Thor.hammerRange(ref MapGrid);
            //MapGrid.showMap();
            // Console.Error.WriteLine($"Main 1: sAction is {Thor.sAction}");
            if (Thor.iEnemyInRange >= Thor.iMinEnemyTarget)
            {
                Console.WriteLine("STRIKE");
            }
            else
            {
                Vector2 tempAnalyze = new Vector2();
                tempAnalyze = Giants.analyse(Thor.iMinEnemyTarget);
                Thor.setItemOnMap("Z", (int)tempAnalyze.X, (int)tempAnalyze.Y);

                Thor.moveTo(tempAnalyze, Giants.information[0].vPosition, Giants.information[1].vPosition, Giants.information[2].vPosition);
                Console.WriteLine(Thor.sAction);
            }
            

            // The movement or action to be carried out: WAIT STRIKE N NE E SE S SW W or N
            
        }
    }
}