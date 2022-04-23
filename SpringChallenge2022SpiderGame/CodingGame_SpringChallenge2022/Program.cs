using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;


/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    public const int TYPE_MONSTER = 0;
    public const int TYPE_MY_HERO = 1;
    public const int TYPE_OP_HERO = 2;
    
    public const int RADIUS_FOG = 2200;
    public const int RADIUS_DEFENSE = 4000;
    public const int RADIUS_WIND = 1200;

    public const int AREA_OFFENSE_TARGET = 500;
    
    public const int ENEMY_BASE_X = 17630;
    public const int ENEMY_BASE_Y = 9000;

    public static Point PointForOffense = new Point(11630, 5000);
    public static Point PointEnemyBase = new Point(ENEMY_BASE_X, ENEMY_BASE_Y);
    public static Point[] PointRadiusDefense = new Point[] { new Point(3500, 1000), new Point(2000, 4000) };

    public static Point PointHomeBase = new Point(0,0);

    public static Entity offenseTargetFocus = null;



    public class Entity
    {
        public int Id;
        public int Type;
        public int X, Y;
        public int ShieldLife;
        public int IsControlled;
        public int Health;
        public int Vx, Vy;
        public int NearBase;
        public int ThreatFor;
        public int ThreadPoints;
        public float DistanceFromEnemyToBase;
        public float DistanceFromEnemyToBase2;

        public Entity(int id, int type, int x, int y, int shieldLife, int isControlled, int health, int vx, int vy, int nearBase, int threatFor, int threadPoints, float distanceFromEnemyToBase, float distanceFromEnemyToBase2)
        {
            this.Id = id;
            this.Type = type;
            this.X = x;
            this.Y = y;
            this.ShieldLife = shieldLife;
            this.IsControlled = isControlled;
            this.Health = health;
            this.Vx = vx;
            this.Vy = vy;
            this.NearBase = nearBase;
            this.ThreatFor = threatFor;
            this.ThreadPoints = threadPoints;
            this.DistanceFromEnemyToBase = distanceFromEnemyToBase;
            this.DistanceFromEnemyToBase2 = distanceFromEnemyToBase2;
        }
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');

        // base_x,base_y: The corner of the map representing your base
        int baseX = int.Parse(inputs[0]);
        int baseY = int.Parse(inputs[1]);
        PointHomeBase = new Point(baseX, baseY);

        // heroesPerPlayer: Always 3
        int heroesPerPlayer = int.Parse(Console.ReadLine());



        // game loop
        while (true)
        {

            inputs = Console.ReadLine().Split(' ');
            int myHealth = int.Parse(inputs[0]); // Your base health
            int myMana = int.Parse(inputs[1]); // Ignore in the first league; Spend ten mana to cast a spell

            inputs = Console.ReadLine().Split(' ');
            int oppHealth = int.Parse(inputs[0]);
            int oppMana = int.Parse(inputs[1]);

            int entityCount = int.Parse(Console.ReadLine()); // Amount of heros and monsters you can see

            List<Entity> myHeroes = new List<Entity>(entityCount);
            List<Entity> oppHeroes = new List<Entity>(entityCount);
            List<Entity> monsters = new List<Entity>(entityCount);
            List<Entity> targets = new List<Entity>(entityCount);

            for (int i = 0; i < entityCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int id = int.Parse(inputs[0]); // Unique identifier
                int type = int.Parse(inputs[1]); // 0=monster, 1=your hero, 2=opponent hero
                int x = int.Parse(inputs[2]); // Position of this entity
                int y = int.Parse(inputs[3]);
                int shieldLife = int.Parse(inputs[4]); // Ignore for this league; Count down until shield spell fades
                int isControlled = int.Parse(inputs[5]); // Ignore for this league; Equals 1 when this entity is under a control spell
                int health = int.Parse(inputs[6]); // Remaining health of this monster
                int vx = int.Parse(inputs[7]); // Trajectory of this monster
                int vy = int.Parse(inputs[8]);
                int nearBase = int.Parse(inputs[9]); // 0=monster with no target yet, 1=monster targeting a base
                int threatFor = int.Parse(inputs[10]); // Given this monster's trajectory, is it a threat to 1=your base, 2=your opponent's base, 0=neither

                var distanceFromEnemyToBase = Vector2.Distance(new Vector2(baseX, baseY), new Vector2(x, y));
                var distanceFromEnemyToBase2 = Vector2.Distance(new Vector2(17630, 9000), new Vector2(x, y));

                var threadPoints = 0;

                switch (type)
                {
                    case TYPE_MONSTER:
                        if (threatFor == 1)
                            threadPoints += 500;
                        if (distanceFromEnemyToBase < 5000f)
                            threadPoints += 500;
                        break;

                }


                Entity entity = new Entity(
                    id, type, x, y, shieldLife, isControlled, health, vx, vy, nearBase, threatFor, threadPoints, distanceFromEnemyToBase, distanceFromEnemyToBase2
                );

                switch (type)
                {
                    case TYPE_MONSTER:
                        monsters.Add(entity);
                        break;
                    case TYPE_MY_HERO:
                        myHeroes.Add(entity);
                        break;
                    case TYPE_OP_HERO:
                        oppHeroes.Add(entity);
                        break;
                }
            }

            targets = monsters.OrderBy(y => y.DistanceFromEnemyToBase).ToList(); //.ThenByDescending(y => y.DistanceFromEnemyToBase)  .Where(c => c.ThreadPoints >= 0)  .ThenByDescending(x => x.ThreadPoints)

            foreach (var target in targets)
                Console.Error.WriteLine("INFO: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEnemyToBase + " " + target.X + ":" + target.Y);
            //Console.Error.WriteLine(target.ThreadPoints);


            for (int i = 0; i < heroesPerPlayer; i++)
            {
                // Entity target = null;
                // Point offensePoint = new Point(-1, -1);

                switch (i)
                {
                    case 0:
                        //Console.Error.WriteLine($"Hero {i} goes defense");
                        defense(new Point(myHeroes[i].X, myHeroes[i].Y), targets, i, myMana, 0);

                        break;
                    case 1:
                        //Console.Error.WriteLine($"Hero {i} goes defense");
                        defense(new Point(myHeroes[i].X, myHeroes[i].Y), targets, i, myMana, 1);
                        break;

                    case 2:
                        //Console.Error.WriteLine($"Hero {i} goes offense");
                        offense(new Point(myHeroes[i].X, myHeroes[i].Y), monsters, i, myMana, 0);
                        break;

                };

            }
        }
    }

    static void defense(Point myHero, List<Entity> myTarget, int hero, int defenseMana, int version)
    {
        Point trianglePoint1, trianglePoint2, trianglePoint3;
        bool targeting = false;
        Console.Error.Write($"\nDEFENSE: Hero {hero}: Start ");

        switch (version)
        {
            case 0:
                // upper Triangle from Base
                trianglePoint1 = new Point(0, 0);
                trianglePoint2 = new Point(5000, 0);
                trianglePoint3 = new Point(5000, 5000);
                targeting = false;

                if (myTarget.Count > 0)
                {
                    Console.Error.Write($"Version 0 -> Counts: {myTarget.Count}");

                    var targetsInReach0 = myTarget.Where(e => myMath.PointInTriangle(new Point(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).ToList(); //.OrderBy(x => x.DistanceFromEnemyToBase)
                    foreach (var item in targetsInReach0)
                    {

                        var itemsPosition = new Point(item.X, item.Y);

                        Console.Error.Write($"DEFENSE 0: targeting: {item.Id}: {item.X} {item.Y} ");

                        var isItWindy = myMath.PointInCircle(myHero, itemsPosition, RADIUS_WIND);
                        if (isItWindy && defenseMana >= 10)
                        {
                            Console.Error.WriteLine($"-> WIND");
                            myAction.wind();
                            targeting = true;
                            break;
                        }
                        else // if (itemsPosition.X <= 5000 && itemsPosition.Y <= 5000)
                        {
                            Console.Error.WriteLine($"-> ATTACK");
                            myAction.move(itemsPosition);
                            targeting = true;
                            break;
                        }

                    }
                    if (!targeting)
                    {
                        Console.Error.WriteLine($"-> WAIT");
                        myAction.move(PointRadiusDefense[0]);
                    }
                }
                else
                    myAction.move(PointRadiusDefense[0]);
                break;
            case 1:
                //lower Triangle from Base
                trianglePoint1 = new Point(0, 0);
                trianglePoint2 = new Point(0, 5000);
                trianglePoint3 = new Point(5000, 5000);
                targeting = false;

                if (myTarget.Count > 0)
                {
                    Console.Error.Write($"Version 1 -> Counts: {myTarget.Count}");
                    var targetsInReach1 = myTarget.Where(e => myMath.PointInTriangle(new Point(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).ToList(); //OrderBy(x => x.DistanceFromEnemyToBase).
                    foreach (var item in targetsInReach1)
                    {
                        
                        var itemsPosition = new Point(item.X, item.Y);

                        Console.Error.Write($"DEFENSE 1: targeting: {item.Id}: {item.X} {item.Y} ");

                        var isItWindy = myMath.PointInCircle(myHero, itemsPosition, RADIUS_WIND);
                        if (isItWindy && defenseMana >= 10)
                        {
                            Console.Error.WriteLine($"-> WIND");
                            myAction.wind();
                            targeting = true;
                            break;
                        }
                        else // if (itemsPosition.X <= 5000 && itemsPosition.Y <= 5000)
                        {
                            Console.Error.WriteLine($"-> ATTACK");
                            myAction.move(itemsPosition);
                            targeting = true;
                            break;
                        }


                    }
                    if (!targeting)
                    {
                        Console.Error.WriteLine($"-> WAIT");
                        myAction.move(PointRadiusDefense[1]);
                    }
                }
                else
                    myAction.move(PointRadiusDefense[1]);
                break;

        }
        //return null;

    }

    static void offense(Point offenseHeroes, List<Entity> monster, int hero, int offenseMana, int version)
    {

        List<Entity> enemyBaseTarget = monster.Where(x => x.DistanceFromEnemyToBase2 < 9000).Where(z => z.DistanceFromEnemyToBase2 > 6000).OrderBy(y => y.DistanceFromEnemyToBase2).ToList(); //Where(x => x.ThreatFor == 2).
        var positionSpider = new Point();

        foreach (var enemy in enemyBaseTarget)
            Console.Error.WriteLine($"OFFENSE-INFO: List of enemyBaseTarget: {enemy.Id}: {enemy.X} {enemy.Y}");

        Console.Error.Write($"\nOFFENSE: Hero {hero}: Start ");

        if (enemyBaseTarget.Where(x => x.Id == offenseTargetFocus.Id) != null)
        {
            Console.Error.WriteLine($"-> Target is gone ");
            offenseTargetFocus = null;
        }

        if (offenseTargetFocus == null)
        {


            switch (version)
            {
                case 0:
                    if (enemyBaseTarget.Count > 0)
                    {
                        foreach (var item in enemyBaseTarget.Take(1))
                        {
                            positionSpider = new Point(item.X, item.Y);
                            offenseTargetFocus = item;
                            Console.Error.Write($"-> Targeting Spider {offenseTargetFocus.Id} ");
                        }
                    }
                    // else myAction.move(PointForOffense);
                    break;
                case 1:
                    break;
            }

        }


        if (offenseTargetFocus != null && !myMath.PointInCircle(offenseHeroes, positionSpider, RADIUS_WIND) ) //
        {
            Console.Error.Write($"-> Moving to Target because it is not in Circle ");
            myAction.move(positionSpider);
        }
        else if (offenseTargetFocus != null)
        {
            Console.Error.Write($"-> inCircle ");
            var isItWindy = myMath.PointInCircle(offenseHeroes, positionSpider, RADIUS_WIND);

            if (offenseMana < 10)
            {
                Console.Error.WriteLine($"-> no Mana ");
                myAction.move(positionSpider);
            }
            else if (offenseMana > 15 && isItWindy)
            {
                Console.Error.WriteLine($"-> Mana -> Wind");
                myAction.wind();
            }
            else myAction.move(PointForOffense);
        }
        else if (offenseTargetFocus == null) myAction.move(PointForOffense);
    }
}

public static class myMath
{
    public static bool PointInCircle(Point hero, Point p, int radius)
    {
        var dx = Math.Abs(hero.X - p.X);
        var dy = Math.Abs(hero.Y - p.Y);
        var R = radius;

        if (dx + dy <= R) return true;
        if (dx > R) return false;
        if (dy > R) return false;
        if ((Math.Pow(dx, 2) + Math.Pow(dy, 2)) <= Math.Pow(R, 2)) return true;
        else return false;
    }
    public static bool PointInTriangle(Point p, Point p0, Point p1, Point p2)
    {
        var s = (p0.X - p2.X) * (p.Y - p2.Y) - (p0.Y - p2.Y) * (p.X - p2.X);
        var t = (p1.X - p0.X) * (p.Y - p0.Y) - (p1.Y - p0.Y) * (p.X - p0.X);

        if ((s < 0) != (t < 0) && s != 0 && t != 0)
            return false;

        var d = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
        return d == 0 || (d < 0) == (s + t <= 0);
    }
}

public static class myAction
{
    public static void wait()
    {
        Console.WriteLine("WAIT");
    }
    public static void move(Point position)
    {
        Console.WriteLine($"MOVE {position.X} {position.Y}");
    }
    public static void wind()
    {
        Console.WriteLine($"SPELL WIND 17630 9000");
    }
}

