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
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        int landingPointX1 = 0;
        int landingPointX2 = 0;
        int landingPointMiddle = 0;
        int landingPointY = 0;
        int done = 0;

        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
            Console.Error.WriteLine($"landx = {landX}; landY = {landY}");

            if (landY == landingPointY && done == 0)
            {
                landingPointY = landY;
                landingPointX2 = landX;
                done = 1;
            } else if (landY != landingPointY && done == 0)
            {
                landingPointY = landY;
                landingPointX1 = landX;
            }

            landingPointMiddle = (landingPointX1 + landingPointX2) / 2 ;

            Console.Error.WriteLine($"H={landingPointY};X1={landingPointX1};X2={landingPointX2};XM={landingPointMiddle}");
        }

        // safety and physic engine:
        double g = 3.711, maxMthrust = 4;
        int maxHspeed = 20, maxVspeed = 40;

        //different Variables for math stuff
        double mSpeed = 0.00;
        double desiredAngle = ConvertRadiansToDegrees(Math.Acos(g / maxMthrust));

        int mRotate = 0, mThrust = 4;
        int mDistanceToLandingArea = 0;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int xMars = int.Parse(inputs[0]);
            int yMars = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            mSpeed = ConvertRadiansToDegrees(Math.Sqrt(Math.Pow(hSpeed, 2) + Math.Pow(vSpeed, 2)));

            // if not landing area
            if (!((landingPointX1 < xMars) && (xMars < landingPointX2)))
            {
                Console.Error.WriteLine($"not landing Area");
                // Check for direction
                if ((xMars < landingPointX1 && hSpeed < 0) || (landingPointX2 < xMars && hSpeed > 0) || (Math.Abs(hSpeed) < (2 * maxHspeed)))
                {
                    Console.Error.Write($"not over Landing place. \nDesired Angle = {desiredAngle}");
                    //mRotate = Convert.ToInt32(desiredAngle);
                    mRotate = Convert.ToInt32((xMars < landingPointX1) ? -desiredAngle : (landingPointX2 < xMars) ? desiredAngle : 0);
                    Console.Error.WriteLine($"\nmRotate = {mRotate}");
                }
                else if (Math.Abs(hSpeed) > 2 * maxHspeed)
                {
                    mRotate = Convert.ToInt32((xMars < landingPointX1) ? (desiredAngle): (landingPointX2 < xMars) ? (-desiredAngle) : 0);
                    Console.Error.WriteLine($"turn in right direction -> {mRotate}");

                }
            }
            else
            {
                Console.Error.WriteLine($"Landing Area");
                // Landing Area
                if (yMars < landingPointY + 200)
                {
                    Console.Error.WriteLine($"critical Area");
                    mRotate = 0;
                    mThrust = 4;
                }
                else if ((Math.Abs(hSpeed) <= (maxHspeed - 5)) && (Math.Abs(vSpeed) <= (maxVspeed - 5)))
                {
                    Console.Error.WriteLine($"In Save Speed\n{hSpeed} vs {maxHspeed}\n{vSpeed} vs {maxVspeed}");
                    mThrust = 2;
                }
                else
                {
                    mThrust = 4;
                    if (hSpeed != 0)
                    {
                        Console.Error.WriteLine($"correcting Rotation");
                        double sining = hSpeed / mSpeed;
                        double radRotate = Math.Atan(sining);
                        double degRotate = ConvertRadiansToDegrees(radRotate) * 100.0;
                        double mRotateTest = Math.Round(degRotate);
                        Console.Error.Write($"h={hSpeed} m={mSpeed} s={sining} r={radRotate}\nd={degRotate} rot={mRotateTest}");
                        mRotate = Convert.ToInt32(mRotateTest);
                        mRotate = MaxValueAllowed(mRotate, 70, -70);
                        Console.Error.WriteLine($" mRot={mRotate}");
                    }
                    else
                        mRotate = 0;
                }
            }


            // Correct Min and Max Value
            MaxValueAllowed(Convert.ToInt32(mThrust),4,0);
            MaxValueAllowed(mRotate, 90, -90);



            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            //
            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            Console.Error.WriteLine($"Output = {mRotate} {mThrust}");
            Console.WriteLine($"{mRotate} {mThrust}");
        }
    }
    public static double ConvertRadiansToDegrees(double radians)
    {
        double degrees = (180.0 / Math.PI) * radians;
        return (degrees);
    }
    public static int MaxValueAllowed(int value, int Max, int Min)
    {
        if (value > Max) 
        {
            // Console.Error.WriteLine($"Max returned");
            return Max;
        }
        else if (value < Min) 
        {
            // Console.Error.WriteLine($"Min returned");
            return Min;
        }
        // Console.Error.WriteLine($"Returning Value");
        return value;
    }
}