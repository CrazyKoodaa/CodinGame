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

        int rotateX = 0;
        int speed = 0;
        double kurve = 0;
        int xDistance = 0;
        rotateX = 0;
        speed = 4;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int Xmars = int.Parse(inputs[0]);
            int Ymars = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            xDistance = Math.Abs(landingPointMiddle - Xmars);
            kurve = 1200 * Math.Pow(xDistance, 0.1);

            
            Console.Error.WriteLine($"Yk = {kurve,5} Ym = {Ymars,5} Differenz = {Ymars - kurve}");

            // Landing Area
            if (Xmars > (landingPointX1) && Xmars < (landingPointX2 + 100))
            {
                Console.Error.WriteLine($"Landing Zone");
                rotateX = Convert.ToInt32(hSpeed * 1.2);

                if (vSpeed < -20)
                {
                    Console.Error.WriteLine($"vSpeed < -20");
                    speed = 4;

                }
                else
                {
                    Console.Error.WriteLine($"vSpeed < -20 else");
                    speed = 2;
                }
            }
            else
            {
                Console.Error.WriteLine($"Outside");
                if (Xmars < landingPointX1)
                {
                    if (kurve < Ymars)
                    {
                        Console.Error.WriteLine($"left above Line");
                        speed = 4;
                        rotateX = -90;
                        if (hSpeed > 50)
                            rotateX = 60;
                        else
                            rotateX = -45;

                    }
                    else
                    {
                        Console.Error.WriteLine($"left below Line");
                        speed = 4;
                        rotateX = -5;
                        if (hSpeed > 40)
                            rotateX = 20;
                        else
                            rotateX = -5;
                    }
                }   
                else if (landingPointX2 < Xmars)
                {
                    if (kurve < Ymars)
                    {
                        Console.Error.WriteLine($"right above Line");
                        speed = 4;
                        if (hSpeed < -50)
                            rotateX = -60;
                        else
                            rotateX = 45;

                    }
                    else
                    {
                        Console.Error.WriteLine($"right below Line");
                        speed = 4;
                        if (hSpeed < -40)
                            rotateX = -20;
                        else
                            rotateX = 5;
                    }

                }      
            }

            if (speed > 4)
                speed = 4;
            if (speed < 0)
                speed = 0;
            
            if (rotateX >= 90)
                rotateX = 90;
            if (rotateX <= -90)
                rotateX = -90;
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            //
            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            Console.Error.WriteLine($"Output = {rotateX} {speed}");
            Console.WriteLine($"{rotateX} {speed}");
        }
    }
}