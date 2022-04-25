using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

class Player
{
    public const string Version = "2.3.2";

    public const int roundInGame = 0;

    public const int TYPE_MONSTER = 0;
    public const int TYPE_MY_HERO = 1;
    public const int TYPE_OP_HERO = 2;

    public const int VERSION_0 = 0;
    public const int VERSION_1 = 1;
    public const int VERSION_2 = 2;

    public const int TRIANGLE_0 = 0;
    public const int TRIANGLE_1 = 1;

    public const int RADIUS_FOG = 2200;
    public const int RADIUS_DEFENSE = 4000;
    public const int RADIUS_WIND = 1200;

    public const int AREA_OFFENSE_TARGET = 500;

    public static Vector2 BASE_LEFT = new Vector2(0, 0);
    public static Vector2 BASE_RIGHT = new Vector2(17630, 9000);

    public static Vector2 PointHomeBase = new Vector2(0, 0);

    public static char enemyIs = ' ';

    public static Random rand = new Random();

    // public static float distanceFromEnemyToEnemyBase;


    //public const int ENEMY_BASE_X = 17630;
    //public const int ENEMY_BASE_Y = 9000;

    //public static Point PointEnemyBase = new Point(ENEMY_BASE_X, ENEMY_BASE_Y);

    public static Vector2 PointForOffense; // = myLibrary.calculateCorrectPositions(PointHomeBase, 11630, 5000);
    public static Vector2[] PointRadiusDefense; // = new Point[] { myLibrary.calculateCorrectPositions(PointHomeBase, 3500, 1000), myLibrary.calculateCorrectPositions(PointHomeBase, 2000, 4000) };

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
        public float DistanceFromEnemyToEnemyBase;
        public float DistanceSpiderToLeftBase;
        public float DistanceSpiderToRightBase;




        public Entity(int id, int type, int x, int y, int shieldLife, int isControlled, int health, int vx, int vy, int nearBase, int threatFor, int threadPoints, float distanceFromEnemyToBase, float distanceFromEnemyToEnemyBase, float toLeftBase, float toRightBase)
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
            this.DistanceFromEnemyToEnemyBase = distanceFromEnemyToEnemyBase;
            this.DistanceSpiderToLeftBase = toLeftBase;
            this.DistanceSpiderToRightBase = toRightBase;
        }
    }

    static void Main(string[] args)
    {
        Console.Error.WriteLine(Version);
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');

        // base_x,base_y: The corner of the map representing your base
        int baseX = int.Parse(inputs[0]);
        int baseY = int.Parse(inputs[1]);
        PointHomeBase = new Vector2(baseX, baseY);

        PointForOffense = myLibrary.calculateCorrectPositions(PointHomeBase, 13000, 6000);
        PointRadiusDefense = new Vector2[] { myLibrary.calculateCorrectPositions(PointHomeBase, 3500, 1000), myLibrary.calculateCorrectPositions(PointHomeBase, 2000, 4000), myLibrary.calculateCorrectPositions(PointHomeBase, 800, 800) };

        // heroesPerPlayer: Always 3
        int heroesPerPlayer = int.Parse(Console.ReadLine());


        // game loop
        while (true)
        {
            #region StartWhile

            roundInGame++;

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
            #endregion


            #region Entities
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

                var distanceFromEnemyToMyBase = myLibrary.calculateCorrectDistance(PointHomeBase, new Vector2(x, y));
                float distanceFromEnemyToEnemyBase = myLibrary.calculateCorrectDistance(myLibrary.PointEnemyBase(), new Vector2(x, y));//(PointHomeBase.X == 0 ? BASE_RIGHT : BASE_LEFT)

                var distanceFromSpiderToLeftBase = myLibrary.calculateCorrectDistance(BASE_LEFT, new Vector2(x, y));
                var distanceFromSpiderToRightBase = myLibrary.calculateCorrectDistance(BASE_RIGHT, new Vector2(x, y));

                var threadPoints = 0;

                switch (type)
                {
                    case TYPE_MONSTER:
                        if (threatFor == 1)
                            threadPoints += 500;
                        if (distanceFromEnemyToMyBase < 5000f)
                            threadPoints += 500;
                        break;

                }


                Entity entity = new Entity(
                    id, type, x, y, shieldLife, isControlled, health, vx, vy, nearBase, threatFor, threadPoints, distanceFromEnemyToMyBase, distanceFromEnemyToEnemyBase, distanceFromSpiderToLeftBase, distanceFromSpiderToRightBase
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

            #endregion entities



            var monsterSortedToHomeBase = myLibrary.SortMonsters(monsters, myLibrary.HomeBaseIs(PointHomeBase));
            var monsterSortedToEnemyBase = myLibrary.SortMonsters(monsters, myLibrary.EnemyBaseIs(PointHomeBase));

            //foreach (var target in monsterSortedToHomeBase)
            //    Console.Error.WriteLine("INFO defense: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEnemyToBase + " " + target.X + ":" + target.Y);
            //foreach (var target in monsterSortedToEnemyBase)
            //    Console.Error.WriteLine("INFO OFFENSE: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEnemyToBase + " " + target.X + ":" + target.Y);

            //Console.Error.WriteLine(target.ThreadPoints);


            for (int i = 0; i < heroesPerPlayer; i++)
            {
                switch (i)
                {
                    case 0:
                        //Console.Error.WriteLine($"Hero {i} goes defense");
                        defense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, VERSION_0, PointHomeBase);

                        break;
                    case 1:
                        //Console.Error.WriteLine($"Hero {i} goes defense");
                        defense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, VERSION_1, PointHomeBase);
                        break;

                    case 2:
                        //Console.Error.WriteLine($"Hero {i} goes offense");
                        if (myMana < 12 && myHealth < 1)
                            defense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, VERSION_2, PointHomeBase);
                        else
                            offense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToEnemyBase, i, myMana, VERSION_0, oppHeroes);
                        break;

                };

            }
        }
    }




    static void defense(Vector2 myHero, List<Player.Entity> myTarget, int hero, int defenseMana, int version, Vector2 homeBase)
    {
        Vector2 trianglePoint1, trianglePoint2, trianglePoint3;
        bool targeting = false;
        Console.Error.Write($"\nDEFENSE: Hero {hero}: Start ");

        switch (version)
        {
            case 0:
                // upper Triangle from Base
                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(homeBase, version);


                targeting = false;

                if (myTarget.Count > 0)
                {
                    Console.Error.Write($"Version 0 -> Counts: {myTarget.Count} ");

                    var targetsInReach0 = myTarget.Where(e => myLibrary.PointInTriangle(new Vector2(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).ToList(); //.OrderBy(x => x.DistanceFromEnemyToBase)
                    foreach (var item in targetsInReach0)
                    {

                        var itemsPosition = new Vector2(item.X, item.Y);

                        Console.Error.Write($"-> targeting: {item.Id}: {item.X} {item.Y} ");

                        var isItWindy = myLibrary.PointInCircle(myHero, itemsPosition, RADIUS_WIND);
                        if (isItWindy && defenseMana >= 10)
                        {



                            if (item.DistanceFromEnemyToBase < 3500)
                            {
                                myLibrary.wind(myLibrary.EnemyBaseIs(PointHomeBase));
                                Console.Error.WriteLine($"-> WIND");
                            }
                            else
                            {
                                Console.Error.WriteLine($"-> NO WIND");
                                myLibrary.move(itemsPosition);
                            }


                            targeting = true;
                            break;
                        }
                        else // if (itemsPosition.X <= 5000 && itemsPosition.Y <= 5000)
                        {
                            Console.Error.WriteLine($"-> ATTACK");
                            myLibrary.move(itemsPosition);
                            targeting = true;
                            break;
                        }

                    }
                    if (!targeting)
                    {
                        Console.Error.WriteLine($"-> WAIT");
                        myLibrary.move(PointRadiusDefense[0]);
                    }
                }
                else
                    myLibrary.move(PointRadiusDefense[0]);
                break;
            case 1:
                //lower Triangle from Base
                //trianglePoint1 = new Point(0, 0);
                //trianglePoint2 = new Point(0, 5000);
                //trianglePoint3 = new Point(5000, 5000);

                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(homeBase, version);
                targeting = false;

                if (myTarget.Count > 0)
                {
                    Console.Error.Write($"Version 1 -> Counts: {myTarget.Count}");
                    var targetsInReach1 = myTarget.Where(e => myLibrary.PointInTriangle(new Vector2(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).ToList(); //OrderBy(x => x.DistanceFromEnemyToBase).
                    foreach (var item in targetsInReach1)
                    {

                        var itemsPosition = new Vector2(item.X, item.Y);

                        Console.Error.Write($"DEFENSE 1: targeting: {item.Id}: {item.X} {item.Y} ");

                        var isItWindy = myLibrary.PointInCircle(myHero, itemsPosition, RADIUS_WIND);
                        if (isItWindy && defenseMana >= 10)
                        {

                            if (item.DistanceFromEnemyToBase < 3000)
                            {
                                myLibrary.wind(myLibrary.EnemyBaseIs(PointHomeBase));
                                Console.Error.WriteLine($"-> WIND");
                            }
                            else
                            {
                                Console.Error.WriteLine($"-> NO WIND");
                                myLibrary.move(itemsPosition);
                            }

                            targeting = true;
                            break;
                        }
                        else // if (itemsPosition.X <= 5000 && itemsPosition.Y <= 5000)
                        {
                            Console.Error.WriteLine($"-> ATTACK");
                            myLibrary.move(itemsPosition);
                            targeting = true;
                            break;
                        }


                    }
                    if (!targeting)
                    {
                        Console.Error.WriteLine($"-> WAIT");
                        myLibrary.move(PointRadiusDefense[1]);
                    }
                }
                else
                    myLibrary.move(PointRadiusDefense[1]);
                break;

            case 2:
                //lower Triangle from Base
                //trianglePoint1 = new Point(0, 0);
                //trianglePoint2 = new Point(0, 5000);
                //trianglePoint3 = new Point(5000, 5000);

                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(homeBase, version);
                targeting = false;

                if (myTarget.Count > 0)
                {
                    Console.Error.Write($"Version 2 -> Counts: {myTarget.Count}");
                    var targetsInReach1 = myTarget.Where(e => myLibrary.PointInTriangle(new Vector2(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).ToList(); //OrderBy(x => x.DistanceFromEnemyToBase).
                    foreach (var item in targetsInReach1)
                    {

                        var itemsPosition = new Vector2(item.X, item.Y);

                        Console.Error.Write($"DEFENSE 2: targeting: {item.Id}: {item.X} {item.Y} ");

                        var isItWindy = myLibrary.PointInCircle(myHero, itemsPosition, RADIUS_WIND);
                        if (isItWindy && defenseMana >= 10)
                        {

                            if (item.DistanceFromEnemyToBase < 3000)
                            {
                                myLibrary.wind(myLibrary.EnemyBaseIs(PointHomeBase));
                                Console.Error.WriteLine($"-> WIND");
                            }
                            else
                            {
                                Console.Error.WriteLine($"-> NO WIND");
                                myLibrary.move(itemsPosition);
                            }

                            targeting = true;
                            break;
                        }
                        else // if (itemsPosition.X <= 5000 && itemsPosition.Y <= 5000)
                        {
                            Console.Error.WriteLine($"-> ATTACK");
                            myLibrary.move(itemsPosition);
                            targeting = true;
                            break;
                        }


                    }
                    if (!targeting)
                    {
                        Console.Error.WriteLine($"-> WAIT");
                        myLibrary.move(PointRadiusDefense[2]);
                    }
                }
                else
                    myLibrary.move(PointRadiusDefense[2]);
                break;

        }
        //return null;

    }

    static void offense(Vector2 offenseHeroes, List<Player.Entity> monster, int hero, int offenseMana, int version, List<Entity> oppHeroes)
    {

        List<Player.Entity> enemyBaseTarget = monster.Where(x => x.DistanceFromEnemyToEnemyBase < 8000).OrderByDescending(y => y.DistanceFromEnemyToEnemyBase).ToList(); //Where(x => x.ThreatFor == 2). .Where(z => z.DistanceFromEnemyToEnemyBase > 5000)
        var positionSpider = new Vector2();
        Vector2 targetOppHeroPos = myLibrary.calculateCorrectPositions(myLibrary.PointEnemyBase(), 5500, 3500);
        float nearestOppHeroMin = 9999.0f; // = myLibrary.calculateCorrectDistance(BASE_RIGHT, new Vector2(x, y));
        int targetOppHeroID = 0;

        foreach (var enemy in enemyBaseTarget)
            Console.Error.WriteLine($"OFFENSE-INFO: List of enemyBaseTarget: {enemy.Id}: {enemy.X} {enemy.Y} - {enemy.Vx} {enemy.Vy}");

        Console.Error.Write($"\nOFFENSE: Hero {hero}: Start ");

        if (enemyBaseTarget.Where(x => x.Id == offenseTargetFocus.Id) != null)
        {
            Console.Error.WriteLine($"-> Target is gone ");
            offenseTargetFocus = null;
        }

        if (offenseTargetFocus == null)
        {
            Console.Error.Write($"-> Counting {enemyBaseTarget.Count} ");
            Console.Error.Write("-> getting Target ");

            if (enemyBaseTarget.Count > 0)
            {
                switch (version)
                {
                    case 0:
                        {
                            foreach (var item in enemyBaseTarget.Take(1))
                            {
                                positionSpider = new Vector2(item.X, item.Y);
                                offenseTargetFocus = item;
                                Console.Error.Write($"-> Targeting Spider {offenseTargetFocus.Id} Controlled {offenseTargetFocus.IsControlled}");
                            }
                            foreach (var oppHero in oppHeroes)
                            {
                                var nearestOppHeroDistance = myLibrary.calculateCorrectDistance(myLibrary.PointEnemyBase(), new Vector2(oppHero.X, oppHero.Y));
                                if (nearestOppHeroDistance < nearestOppHeroMin)
                                {
                                    nearestOppHeroDistance = nearestOppHeroMin;
                                    targetOppHeroID = oppHero.Id;
                                    targetOppHeroPos = new Vector2(oppHero.X, oppHero.Y);

                                }


                            }
                            Console.Error.WriteLine($" ----------------------------> Opponent {targetOppHeroID}");
                        }
                        // else myLibrary.move(PointForOffense);
                        break;
                    case 1:
                        break;
                }


                Console.Error.WriteLine($"-> {offenseTargetFocus.Id} ");

                if (offenseTargetFocus != null && !myLibrary.PointInCircle(offenseHeroes, positionSpider, RADIUS_WIND) && (((PointHomeBase.X == 0 && offenseTargetFocus.Vx < 0 && offenseTargetFocus.Vy < 0) || (PointHomeBase.X != 0 && offenseTargetFocus.Vx > 0 && offenseTargetFocus.Vy > 0)) || offenseMana < 10)) //
                {
                    Console.Error.Write($"-> Moving to Target because it is not in Circle ");
                    myLibrary.move(positionSpider);
                }

                else if (offenseTargetFocus != null)
                {
                    Console.Error.Write($"-> inCircle ");
                    var isItWindy = myLibrary.PointInCircle(offenseHeroes, positionSpider, RADIUS_WIND);
                    Console.Error.WriteLine($"--> Target direction {offenseTargetFocus.Vx}:{offenseTargetFocus.Vy}");


                    if (((PointHomeBase.X == 0 && offenseTargetFocus.Vx < 0 && offenseTargetFocus.Vy < 0) || (PointHomeBase.X != 0 && offenseTargetFocus.Vx > 0 && offenseTargetFocus.Vy > 0) ) && offenseMana < 25)
                    {
                        Console.Error.WriteLine($"-> no Mana ");
                        myLibrary.move(positionSpider);
                    }
                    else 
                    {
                        if ((PointHomeBase.X == 0 && (offenseTargetFocus.Vx < 0 || offenseTargetFocus.Vy < 0)) || (PointHomeBase.X != 0 && (offenseTargetFocus.Vx > 0 || offenseTargetFocus.Vy > 0)))
                        {

                            Console.Error.WriteLine($"-> Mana -> Control");
                            //if (myLibrary.calculateCorrectDistance(offenseHeroes, targetOppHeroPos) < 2000)
                            myLibrary.control(myLibrary.EnemyBaseIs(PointHomeBase), offenseTargetFocus.Id);
                            //else
                            //    myLibrary.move(positionSpider);
                        }
                        else if (offenseTargetFocus.ShieldLife == 0)
                        {
                            Console.Error.WriteLine($"-> Mana -> Shield");
                            //var monsterID = monster.Where(x => x.DistanceFromEnemyToEnemyBase < 7000).Select(x => x.Id).ToList(); //.Where(y => y.ThreatFor == 2)
                            //if ( monsterID.Count > 0)
                            myLibrary.shield(offenseTargetFocus.Id);
                            //else
                            //    myLibrary.move(positionSpider);
                        }
                        else if (offenseMana > 35)
                        {
                            Console.Error.WriteLine($"-> Mana -> Wind");
                            myLibrary.wind(myLibrary.EnemyBaseIs(PointHomeBase));
                        }
                        else
                        {
                            Console.Error.WriteLine("No Spell for now");
                            myLibrary.move(PointForOffense);

                        }


                    }
                }
                else myLibrary.move(targetOppHeroPos);
            }
            else myLibrary.move(targetOppHeroPos);
        }
    }
}
public static class myLibrary
{
    #region commands
    public static void wait()
    {
        Console.WriteLine("WAIT");
    }
    public static void move(Vector2 position)
    {
        Console.WriteLine($"MOVE {position.X} {position.Y}");
    }
    public static void wind(char direction)
    {
        if (direction == 'l') Console.WriteLine($"SPELL WIND 0 0");
        else if(direction == 'r') Console.WriteLine($"SPELL WIND 17630 9000");
    }

    public static void control(char direction, int opHeroNear)
    {

        if (direction == 'l') Console.WriteLine($"SPELL CONTROL {opHeroNear}  0 0");
        else if (direction == 'r') Console.WriteLine($"SPELL CONTROL {opHeroNear} 17630 9000");
    }

    internal static void shield(int spider)
    {
        Console.Error.WriteLine($"\nGoing to shield {spider}");
        Console.WriteLine($"SPELL SHIELD {spider}");
    }

    #endregion

    #region Maths
    public static bool PointInCircle(Vector2 hero, Vector2 p, int radius)
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

    public static bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var s = (p0.X - p2.X) * (p.Y - p2.Y) - (p0.Y - p2.Y) * (p.X - p2.X);
        var t = (p1.X - p0.X) * (p.Y - p0.Y) - (p1.Y - p0.Y) * (p.X - p0.X);

        if ((s < 0) != (t < 0) && s != 0 && t != 0)
            return false;

        var d = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
        return d == 0 || (d < 0) == (s + t <= 0);
    }

    public static float calculateCorrectDistance(Vector2 bases, Vector2 entity)
    {
        return (Math.Abs(Vector2.Distance(bases, entity)));
    }

    internal static List<Player.Entity> SortMonsters(List<Player.Entity> monsters, char towards)
    {
        return towards == 'l' ? monsters.OrderBy(y => y.DistanceSpiderToLeftBase).ToList() : monsters.OrderBy(y =>y.DistanceSpiderToRightBase).ToList();


     //.ThenByDescending(y => y.DistanceFromEnemyToBase)  .Where(c => c.ThreadPoints >= 0)  .ThenByDescending(x => x.ThreadPoints)
    }

    public static Vector2 PointEnemyBase()
    {
        return Player.PointHomeBase.X == 0 ? Player.BASE_RIGHT : Player.BASE_LEFT;
    }

    public static char EnemyBaseIs (Vector2 homebase)
    {
        return Player.PointHomeBase.X == 0 ? 'r' : 'l';
    }

    public static char HomeBaseIs(Vector2 pointHomeBase)
    {
        return Player.PointHomeBase.X == 0 ? 'l' : 'r';
    }

    public static (Vector2 trianglePoint1, Vector2 trianglePoint2, Vector2 trianglePoint3) formTriangle(Vector2 whereToForm, int whichTrianglePart)
    {

        if (whichTrianglePart == 0)
            return (calculateCorrectPositions(whereToForm, 0, 0),
                calculateCorrectPositions(whereToForm, 5000, 0),
                calculateCorrectPositions(whereToForm, 5000, 5000));
         else if (whichTrianglePart == 1)
            return (calculateCorrectPositions(whereToForm, 0, 0),
                calculateCorrectPositions(whereToForm, 0, 5000),
                calculateCorrectPositions(whereToForm, 5000, 5000));
        else if (whichTrianglePart == 2)
            return (calculateCorrectPositions(whereToForm, 0, 0),
                calculateCorrectPositions(whereToForm, 0, 7000),
                calculateCorrectPositions(whereToForm, 7000, 0));
        return (new Vector2(0,0), new Vector2(0, 0), new Vector2(0, 0));
    }

    public static Vector2 calculateCorrectPositions(Vector2 whereToForm, int deltaX, int deltaY)
    {
        return new Vector2 (Math.Abs((int)whereToForm.X - deltaX), Math.Abs((int)whereToForm.Y - deltaY));
    }





    #endregion

}
