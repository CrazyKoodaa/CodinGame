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
class Player
{
    public const int TYPE_MONSTER = 0;
    public const int TYPE_MY_HERO = 1;
    public const int TYPE_OP_HERO = 2;




    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int baseX = int.Parse(inputs[0]); // The corner of the map representing your base
        int baseY = int.Parse(inputs[1]);

        Map.HomeBase = new Vector2(baseX, baseY);
        Map.EnemyBase = Map.setEnemyBase(Map.HomeBase);
        int heroesPerPlayer = int.Parse(Console.ReadLine()); // Always 3

        
        var targetOffense = new List<Entity>();

        Map.setPositions();

        // game loop
        while (true)
        {
            Entity.countingRounds++;
            Console.Error.WriteLine($"--->> ROUND {Entity.countingRounds}");
            #region StartWhile
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

            #endregion

            targetOffense = monsters.Where(x => x.DistanceToEnemyBase < 15000 && x.DistanceToBase > 8000).OrderByDescending(x => x.DistanceToEnemyBase).ToList();

            #region Entities
            for (int i = 0; i < entityCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int id = int.Parse(inputs[0]); // Unique identifier
                int type = int.Parse(inputs[1]); // 0=monster, 1=your hero, 2=opponent hero

                int x = int.Parse(inputs[2]); // Position of this entity
                int y = int.Parse(inputs[3]);
                var posOfEntity = new Vector2(x, y);

                int shieldLife = int.Parse(inputs[4]); // Ignore for this league; Count down until shield spell fades
                int isControlled = int.Parse(inputs[5]); // Ignore for this league; Equals 1 when this entity is under a control spell
                int health = int.Parse(inputs[6]); // Remaining health of this monster
                int vx = int.Parse(inputs[7]); // Trajectory of this monster
                int vy = int.Parse(inputs[8]);
                int nearBase = int.Parse(inputs[9]); // 0=monster with no target yet, 1=monster targeting a base
                int threatFor = int.Parse(inputs[10]); // Given this monster's trajectory, is it a threat to 1=your base, 2=your opponent's base, 0=neither

                var distanceToBase = Vector2.Distance(Map.HomeBase, new Vector2(x,y));
                var distanceToEnemyBase = Vector2.Distance(Map.EnemyBase, new Vector2(x,y));

          
                Entity entity = new Entity(
                    id, type, x, y, shieldLife, isControlled, health, vx, vy, nearBase, threatFor, distanceToBase, distanceToEnemyBase, posOfEntity
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
            #endregion


            

            

            


            for (int i = 0; i < heroesPerPlayer; i++)
            {
                switch (i)
                {
                    case 0: // Defense Player
                        if (myMana > 300)
                            Map.goOffense = true;
                        if (myMana < 70)
                            Map.goOffense = false;

                        if (!Map.goOffense)
                        {
                            Console.Error.WriteLine("Defense");
                            Entity.defense(monsters, myHeroes, oppHeroes, 0);
                        }
                        else
                        {
                            Console.Error.WriteLine("ATTACK!!!!!!!!!!!!!!!!!!!!!!!");

                            Entity.offense(monsters, myHeroes, oppHeroes, 0);
                        }
                        break;
                    case 1: // Defense Player
                        if (Map.goOffense == false)
                            Entity.defense(monsters, myHeroes, oppHeroes, 1);
                        else
                            Entity.defense(monsters, myHeroes, oppHeroes, 2);
                        break;
                    case 2: // Defense Player
                        if (Map.goOffense == false)
                            Entity.defense(monsters, myHeroes, oppHeroes, 0);
                        else
                            Entity.defense(monsters, myHeroes, oppHeroes, 2);
                        break;
                }
            }
            //if (Entity.countingRounds > 10) Entity.countingRounds = 0;
        }
    }
}


internal class Map
{
    public static Vector2 leftBase = new Vector2(0, 0);
    public static Vector2 rightBase = new Vector2(17630, 9000);

    public static Vector2 HomeBase;
    public static Vector2 EnemyBase;

    public static List<Vector2> posDefense = new List<Vector2>();
    public static Vector2 posDefense2;
    public static Vector2 posDefenseMiddle;
    public static Vector2 posMiddleMiddle;
    public static Vector2 posOffenseMiddle;
    public static bool goOffense = false;
    internal static bool shielding;

    internal static void setPosDefenses()
    {
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(7500, 2500)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(4500, 7500)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(4500, 3500)));
    }

    internal static void setPosMiddle()
    {
        posMiddleMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(8500, 4500));
    }

    internal static void setPosOffense()
    {
        posOffenseMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(14000, 6000));
    }

    internal static Vector2 setEnemyBase(Vector2 myHomeBase)
    {
        return myHomeBase.X == 0 ? rightBase : leftBase;
    }

    internal static void setPositions()
    {
        setPosDefenses();
        setPosMiddle();
        setPosOffense();
    }

    internal static bool PointInCircle(Entity hero, Vector2 p, int radius)
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

    
}

internal class myMath
{
    public static Random rand = new Random(); 
    internal static Vector2 calculateCorrectPositionFrom(Vector2 basePosition, Vector2 vector2)
    {
        if (basePosition.X == 0)
        {
            return vector2;
        }
        else
        {
            return Vector2.Subtract(basePosition, vector2);
        }
    }

    internal static bool checkAboveLine(Vector2 pos1, Vector2 pos2, Vector2 target) //Map.HomeBase, new Vector2(7000, 5500), targetDefense[0].Position);
    {
        return ((pos2.X - pos1.X) * (target.Y - pos1.Y) - (pos2.Y - pos1.Y) * (target.X - pos1.X)) < 0;
    }

    internal static Vector2 calculateAwayfromHome(Vector2 position)
    {
        Vector2 direction = Vector2.Subtract(position, Map.HomeBase);  // Vector2.Normalize(
        Vector2 normalizeDir = Vector2.Normalize(direction);
        Vector2 positionTo = new Vector2(direction.X, direction.Y + normalizeDir.Y * 10);
        Console.Error.WriteLine($"calculateAwayfromHome: {positionTo.X} - {positionTo.Y}");
        return positionTo;
    }
}

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
    public float DistanceToBase;
    public float DistanceToEnemyBase;
    public Vector2 Position;
    public bool EnemyControllsEntity;
    public bool EntityIsShielded;

    public bool DirectionEnemyBase;
    public static bool warningControlled = false;
    public static bool warningShielded = false;
    public static int countingShielded = 0;
    public static int countingRounds = 0;

    public const int RADIUS_WIND = 800;
    public static List<Entity> targetDefense = new List<Entity>();


    public Vector2 Direction;
    public string HeadingTo;
    internal static bool goOffense = false;

    public Entity(int id, int type, int x, int y, int shieldLife, int isControlled, int health, int vx, int vy, int nearBase, int threatFor, float distanceToBase, float distanceToEnemyBase, Vector2 position) // , bool directionEnemyBase) //, bool enemyControllsEntity) //, bool enemyShieldsEntity) //, int threadPoints, float distanceFromEntityToBase, float distanceFromEntityToEnemyBase, Vector2 direction, string headingTo) //, float toLeftBase, float toRightBase)
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
        this.DistanceToEnemyBase = distanceToEnemyBase;
        this.DistanceToBase = distanceToBase;
        this.Position = position;
        this.EnemyControllsEntity = isControlled == 1 ? true : false;
        this.EntityIsShielded = shieldLife > 0 ? true : false;
        if ((Map.HomeBase.X == 0 && Vx > 0 && Vy > 0) || (Map.HomeBase.X != 0 && Vx < 0 && Vy < 0))
            this.DirectionEnemyBase = true;
    }

    internal static void move(Vector2 position)
    {
        Console.WriteLine($"MOVE {position.X} {position.Y}");
    }

    internal static void wind(Vector2 position)
    {
        //Vector2 direction = myMath.calculateAwayfromHome(position);
        Console.WriteLine($"SPELL WIND {Map.EnemyBase.X} {Map.EnemyBase.Y}");
    }

    internal static void shield(Entity entity)
    {
        Console.WriteLine($"SPELL SHIELD {entity.Id}");
        Entity.countingShielded++;
    }

    internal static void control(int id, Vector2 direction)
    {
        Console.WriteLine($"SPELL CONTROL {id} {direction.X} {direction.Y}");
    }

    internal static void defense(List<Entity> monsters, List<Entity> myHeroes, List<Entity> oppHeroes, int Version)
    {
        //bool isInMyArea = false;
        //Entity.enemyShieldsSpider = false;
        int distanceDefense = 11000;
        Vector2 seperateLine = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(10000, 9000));
        var tempDefense = Version;


        List<List<Entity>> targetDefense = new List<List<Entity>>();

        targetDefense.Add(new List<Entity>());
        targetDefense.Add(new List<Entity>());
        targetDefense.Add(monsters.Where(x => x.DistanceToBase < distanceDefense).OrderBy(x => x.DistanceToBase).ToList());


        Console.Error.WriteLine($"INFO: defense count = {targetDefense.Last().Count}");

        if (Version != 2)
        {
            foreach (var target in targetDefense.Last())
            {
                if (myMath.checkAboveLine(Map.HomeBase, seperateLine, target.Position))
                {
                    targetDefense[0].Add(target);
                    Console.Error.WriteLine($"DEFENSE LEFT: {target.Id}; Shield: {target.ShieldLife}");
                }
                else
                {
                    targetDefense[1].Add(target);
                    Console.Error.WriteLine($"DEFENSE RIGHT: {target.Id}; Shield: {target.ShieldLife}");
                }
            }
        }
        else
        {
            Console.Error.WriteLine("---------------------> Version is 2 <--------------------");
            Console.Error.WriteLine($"---------------------> {targetDefense[Version].Count}");
        }

        //if (Map.goOffense == true)
        //{
        //    Console.Error.WriteLine("INFO: Defense all");
        //    Version = 2;

        //}
        if (targetDefense[Version].Count == 0)
        {
            Console.Error.WriteLine($"DEFENSE {Version}: No Targets");
            Entity.move(Map.posDefense[Version]);
        }
        else
        {
            var Alarm = Entity.defenseAlarm(myHeroes, myHeroes[Version], targetDefense[Version][0]);

            foreach (var Hero in myHeroes)
            {
                if (myHeroes[Version].EnemyControllsEntity == true && myHeroes[0].EnemyControllsEntity == false)
                    Entity.warningControlled = true;
            }

            if (Entity.warningControlled && myHeroes[Version].EntityIsShielded == false)
                Entity.shield(myHeroes[Version]);
            else if (Alarm && targetDefense[Version][0].EntityIsShielded == false)
                Entity.wind(Map.EnemyBase);
            else
            {
                Console.Error.WriteLine($"DEFENSE {Version}: Targeting {targetDefense[Version][0].Id}");
                Entity.move(targetDefense[Version][0].Position);
            }
        }
    }

    private static bool defenseAlarm(List<Entity> heroes, Entity hero, Entity targetDefense)
    {
        int distanceAlarm = 3000;

        if (targetDefense.DistanceToBase < distanceAlarm && 
            (
            Map.PointInCircle(heroes[0], targetDefense.Position, RADIUS_WIND) && heroes[0].DistanceToBase < distanceAlarm ||
            Map.PointInCircle(heroes[1], targetDefense.Position, RADIUS_WIND) && heroes[1].DistanceToBase < distanceAlarm ||
            Map.PointInCircle(heroes[2], targetDefense.Position, RADIUS_WIND) && heroes[2].DistanceToBase < distanceAlarm))
        {
            Console.Error.WriteLine("DEFENSE LEFT: It's windy");
            return true;
        }
        else
        {
            Console.Error.WriteLine("DEFENSE LEFT: It's not windy");
            return false;
        }
    }

    internal static void offense(List<Entity> monsters, List<Entity> myHeroes, List<Entity> oppHeroes, int Version)
    {
        int distanceOffense = 7000;
        //Vector2 seperateLine = new Vector2(8000, 7000);
        

        List<List<Entity>> targetOffense = new List<List<Entity>>();

        targetOffense.Add(monsters.Where(x => x.DistanceToEnemyBase < distanceOffense).OrderBy(x => x.DistanceToBase).ToList());
        targetOffense.Add(new List<Entity>());
        targetOffense.Add(new List<Entity>());

        var monstersShielded = monsters.Where(x => x.DirectionEnemyBase == true && x.EntityIsShielded == true).Count();
        var nearestOppHero = oppHeroes.OrderBy(x => x.DistanceToEnemyBase).FirstOrDefault();
        if (!(nearestOppHero != null && Map.PointInCircle(myHeroes[Version], nearestOppHero.Position, RADIUS_WIND)))
            nearestOppHero = null;

        bool commanded = false;

        Console.Error.WriteLine($"INFO: OFFENSE count = {targetOffense[0].Count}");

        if (targetOffense[Version].Count == 0) Entity.move(Map.posOffenseMiddle);
        else if (myHeroes[0].EnemyControllsEntity == true && myHeroes[0].EntityIsShielded == false)
            Entity.shield(myHeroes[0]);
        else
        {
            if (Entity.countingRounds % 12 == 0) Entity.countingShielded = 0;

            if (Entity.countingShielded > 1 && nearestOppHero != null && nearestOppHero.EntityIsShielded == false)
            {
                Console.Error.WriteLine("OFFENSE: There are more than 1 correct");
                Entity.control(nearestOppHero.Id, Map.HomeBase);
                commanded = true;
            }
            else if (Entity.countingShielded > 2)
                Entity.move(myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(14500, 6000)));
            else
            {
                foreach (var spider in targetOffense[Version])
                {
                    if (spider.DirectionEnemyBase == false && spider.EntityIsShielded == false)
                    {
                        Console.Error.WriteLine($"OFFENSE {Version}: Controlling {spider.Id}");
                        Entity.control(spider.Id, Map.EnemyBase);
                        commanded = true;
                        break;
                    }
                    if (spider.EntityIsShielded == false && Map.shielding)
                    {

                        Console.Error.WriteLine($"OFFENSE {Version}: Shielding {spider.Id}");
                        Entity.shield(spider);
                        commanded = true;
                        Map.shielding = false;
                        break;
                    }
                    if (spider.EntityIsShielded == false && !Map.shielding)
                    {
                        Console.Error.WriteLine($"OFFENSE {Version}: Winding {spider.Id}");
                        if (Map.PointInCircle(myHeroes[Version], spider.Position, 800))
                            Entity.wind(Map.EnemyBase);
                        else
                        {
                            nearestOppHero = oppHeroes.OrderBy(x => x.DistanceToEnemyBase).FirstOrDefault();
                            if (nearestOppHero != null)
                                Entity.control(nearestOppHero.Id, Map.HomeBase);
                            else
                                Entity.move(spider.Position);
                        }
                        commanded = true;
                        Map.shielding = true;
                        break;
                    }
                    if (Map.PointInCircle(spider, myHeroes[Version].Position, 200) == true)
                    {
                        Console.Error.WriteLine($"OFFENSE {Version}: Waiting because of {spider.Id}");
                        nearestOppHero = oppHeroes.OrderByDescending(x => x.DistanceToEnemyBase).FirstOrDefault();
                        if (nearestOppHero != null)
                            Entity.control(nearestOppHero.Id, Map.HomeBase);
                        else
                            Entity.move(Map.posOffenseMiddle);
                        commanded = true;
                        break;
                    }

                }
                if (commanded == false)
                {
                    Console.Error.WriteLine($"OFFENSE {Version}: Moving to Offense");
                    nearestOppHero = oppHeroes.OrderByDescending(x => x.DistanceToEnemyBase).FirstOrDefault();
                    if (nearestOppHero != null)
                        Entity.control(nearestOppHero.Id, Map.HomeBase);
                    else
                        Entity.move(Map.posOffenseMiddle);
                }
            }
        }
        
//    if (Entity.enemyShieldsSpider)
        //    {
        //        if (targetDefense.Count == 0) Entity.move(Map.posMiddleMiddle);
        //        else
        //        {
        //            Console.Error.WriteLine($"DEFENSE99: Targeting {targetDefense[0].Id}");
        //            var windInRange = Map.PointInCircle(myHeroes[i], targetDefense[0].Position, Entity.RADIUS_WIND);
        //            if (targetDefense[0].DistanceToBase < 3000 && windInRange)
        //                Entity.wind(myHeroes[1].Position); //Entity.wind(targetDefense[0].Position);
        //            else
        //            {
        //                if (myHeroes[i].IsControlled == 1 || (Entity.enemyControllsHero == true && myHeroes[i].ShieldLife <= 0))
        //                {
        //                    Entity.shield(myHeroes[i]);
        //                    Entity.enemyControllsHero = true;
        //                }
        //                else
        //                    Entity.move(targetDefense[0].Position);


        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (myMana > 150)
        //        {
        //            if (myHeroes[i].Position != Map.posOffenseMiddle)
        //                Entity.move(Map.posOffenseMiddle);
        //            else
        //            {
        //                var targetAttack = monsters.Where(x => x.DistanceToEnemyBase <= 6000).ToList();
        //                if (targetAttack.Count > 0)
        //                {
        //                    var whatToDo = myMath.rand.Next(2);
        //                    switch (whatToDo)
        //                    {
        //                        case 0:
        //                            Entity.control(targetAttack[0].Id, Map.EnemyBase);
        //                            break;
        //                        case 1:
        //                            var nearestToBase = oppHeroes.Where(x => Map.PointInCircle(myHeroes[i], x.Position, Entity.RADIUS_WIND) == true).OrderBy(i => i.DistanceToEnemyBase).ToList();
        //                            if (nearestToBase.Count > 0)
        //                                Entity.control(oppHeroes[0].Id, Map.HomeBase);
        //                            else
        //                                Entity.shield(targetAttack[0]);

        //                            break;

        //                    }
        //                    if (whatToDo == 0)
        //                        Entity.control(targetAttack[0].Id, Map.EnemyBase);

        //                }
        //                else
        //                    Entity.move(Map.posOffenseMiddle);
        //            }
        //        }
        //        else
        //        {
        //            Console.Error.WriteLine($"OFFENSE: targeting {targetOffense[0].Id}");
        //            Entity.move(targetOffense[0].Position);
        //        }
        //    }
        //}



    }

    private static void wait()
    {
        Console.WriteLine("WAIT");
    }
}