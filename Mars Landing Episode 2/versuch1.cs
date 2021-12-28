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
            Console.Error.WriteLine($"Height={landingPointY};X1={landingPointX1};X2={landingPointX2} and done = {done}");
        }

        int rotateX = 0;
        int speed = 0;
        double kurve = 0;
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

            if (Xmars < landingPointX1)
                kurve = 1200 * Math.Abs(Math.Pow( landingPointMiddle - Xmars, 0.1));
            if (Xmars > landingPointX2)
                kurve = 1200 * Math.Abs(Math.Pow( Xmars - landingPointMiddle, 0.1));
            
            Console.Error.WriteLine($"Y sollte {kurve}\nY ist   {Ymars}\nDifferenz = {Ymars - kurve}");

            if (Xmars > landingPointX1 && Xmars < landingPointX2)
            {
                Console.Error.WriteLine($"Landing Zone");
                if (hSpeed > 5)
                {
                    rotateX += 2;
                    if (rotateX >= 30)
                        rotateX = 30;
                }
                else if (hSpeed < -5)
                {
                    rotateX -= 2;
                    if (rotateX <= -30)
                        rotateX = -30;
                } else if (hSpeed > 2)
                {
                    rotateX += 1;
                    if (rotateX >= 10)
                        rotateX = 10;
                }
                else if (hSpeed < -2)
                {
                    rotateX -= 1;
                    if (rotateX <= -10)
                        rotateX = -10;
                }
                else if (hSpeed == 0)
                    rotateX = 0;


                if (vSpeed < -15)
                    speed++;
                else if (vSpeed > 15)
                    speed--;

            }
            else
            {
                Console.Error.WriteLine($"Outside");
                if (Xmars < landingPointX1)
                        rotateX -= 5;
                if (landingPointX2 < Xmars)
                    rotateX += 5;

                if (kurve < Ymars)
                {
                    Console.Error.WriteLine($"above Line");
                    speed = 2;
                    
                }
                else if (kurve > Ymars)
                {
                    Console.Error.WriteLine($"below Line");
                    speed = 4;
                    if (rotateX < -30)
                        rotateX = -30;
                    if (rotateX > 30)
                        rotateX = 30;
                }

                if ((Xmars < landingPointX1 && rotateX <= -15)) 
                    rotate = -15;
                if (landingPointX2 < Xmars && rotateX >= 15 )
                    rotate = 15; 



                
                if (hSpeed < -50)
                    rotateX = -22;
                if (hSpeed > 50)
                    rotateX = 22;
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