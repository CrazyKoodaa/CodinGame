using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

/**
 * Save humans, destroy zombies!
 **/
public class humans
{
    public int ID;
    public Vector2 currentPosition;
    public Vector2 moveToPistion;


}

public class zombies : humans 
{
    public Vector2 gotoPosition;
    public static void selectZombies()
    {

    }
}

class Player
{

    static void Main(string[] args)
    {
        string[] inputs;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]); // position for hunter
            int y = int.Parse(inputs[1]); // position for hunter

            humans hunter = new humans(){ ID = 0, currentPosition = new Vector2(x, y) };
            
            int humanCount = int.Parse(Console.ReadLine());
            
            humans[] victim = new humans[humanCount];
            
            for (int i = 0; i < humanCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                // humans[i,0] = int.Parse(inputs[0]);
                // humans[i,1] = int.Parse(inputs[1]);
                // humans[i,2] = int.Parse(inputs[2]);
                victim[i] = new humans() { ID = Convert.ToInt32(inputs[0]), currentPosition=new Vector2(float.Parse(inputs[1]), float.Parse(inputs[2])) };
            }
            int zombieCount = int.Parse(Console.ReadLine());
            zombies[] target = new zombies[zombieCount];



            for (int i = 0; i < zombieCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                // int zombieId = int.Parse(inputs[0]);
                // int zombieX = int.Parse(inputs[1]);
                // int zombieY = int.Parse(inputs[2]);
                // int zombieXNext = int.Parse(inputs[3]);
                // int zombieYNext = int.Parse(inputs[4]);
                target[i] = new zombies() { ID = Convert.ToInt32(inputs[0]), currentPosition=new Vector2(float.Parse(inputs[1]), float.Parse(inputs[2])),  gotoPosition = new Vector2(float.Parse(inputs[3]), float.Parse(inputs[4]))};
            }

            Console.Error.WriteLine("------ Hunter -------");
            Console.Error.Write("ID   = " + hunter.ID);
            Console.Error.WriteLine(": Posi = " + hunter.currentPosition);

            foreach (var vic in victim)
            {
                Console.Error.WriteLine("------ Victims -------");
                Console.Error.Write("ID   = " + vic.ID);
                Console.Error.WriteLine(": Posi = " + vic.currentPosition);
            }

            foreach (var zomb in target)
            {
                Console.Error.WriteLine("------ Zombies -------");
                Console.Error.Write("ID   = " + zomb.ID);
                Console.Error.Write(": Posi = " + zomb.currentPosition);
                Console.Error.WriteLine("; goto = " + zomb.gotoPosition);
            }


            
            float minDisti = 9999999999.99f, humMinDistance = 9999999.99f;
            Vector2 targetPosition;

    
            
            foreach (var hum in victim)
            {
                minDisti = 99999999.99f;
                foreach (var zomb in target)
                {
                    float distance = Vector2.Distance(hum.currentPosition, zomb.currentPosition);
                    if (distance < minDisti)
                    {
                        minDisti = distance;
                        targetPosition = zomb.gotoPosition;
                    }
                }

                if (minDisti < humMinDistance)
                {
                    humMinDistance = minDisti;
                    hunter.moveToPistion = targetPosition;
                }   

            
            }
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(hunter.moveToPistion.X + " " + hunter.moveToPistion.Y); // Your destination coordinates

        }
    }
}