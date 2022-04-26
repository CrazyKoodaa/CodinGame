using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

// Version = "2.4.1"

class Map
{
    public static Vector2 baseLeft = new Vector2(0, 0);
    public static Vector2 baseRight = new Vector2(17630, 9000);

    public static readonly int radiusDefense = 5000;
    public static readonly int radiusDefenseBase = 6000;
    public static readonly int radiusMiddle = 9000;
    public static readonly int radiusOffense = 14500;

    #region Properties
    private static Vector2 HomeBase { get; set; }
    private static Vector2 EnemyBase { get; set; }
    private static Vector2 OffenseAll { get; set; }
    private static Vector2 OffenseLeft { get; set; }
    private static Vector2 OffenseRight { get; set; }
    private static Vector2 MiddleAll { get; set; }
    private static Vector2 MiddleLeft { get; set; }
    private static Vector2 MiddleRight { get; set; }
    private static Vector2 DefenseAll { get; set; }
    private static Vector2 DefenseLeft { get; set; }
    private static Vector2 DefenseRight { get; set; }
    #endregion

    #region BasePosition
    public static void SetHomeBasePosition (Vector2 PointHomeBase) => HomeBase = PointHomeBase;
    public static Vector2 GetHomeBasePosition () => HomeBase;

    public static void SetEnemyBasePosition (Vector2 PointEnemyBase) => EnemyBase = PointEnemyBase;
    public static Vector2 GetEnemyBasePosition () => EnemyBase;
    #endregion

    #region OffensePosition
    public static void SetOffenseAllPosition(Vector2 PointOffenseAll) => OffenseAll = calculateCorrectPositionsFromHome(PointOffenseAll);
    public static Vector2 GetOffenseAllPosition() => OffenseAll;
    public static void SetOffenseRightPosition(Vector2 PointOffenseRight) => OffenseRight = calculateCorrectPositionsFromHome(PointOffenseRight);
    public static Vector2 GetOffenseRightPosition () => OffenseRight;
    public static void SetOffenseLeftPosition(Vector2 PointOffenseLeft) => OffenseLeft = calculateCorrectPositionsFromHome(PointOffenseLeft);
    public static Vector2 GetOffenseLeftPosition () => OffenseLeft;
    #endregion

    #region MiddlePosition
    public static void SetMiddleLeftPosition(Vector2 PointMiddleLeft) => MiddleLeft = calculateCorrectPositionsFromHome(PointMiddleLeft);
    public static Vector2 GetMiddleLeftPosition () => MiddleLeft;
    public static void SetMiddleRightPosition(Vector2 PointMiddleRight) => MiddleRight = calculateCorrectPositionsFromHome(PointMiddleRight);
    public static Vector2 GetMiddleRightPosition () => MiddleRight;
    public static void SetMiddleAllPosition (Vector2 PointMiddleAll) => MiddleAll = calculateCorrectPositionsFromHome(PointMiddleAll);
    public static Vector2 GetMiddleAllPosition () => MiddleAll;
    #endregion

    #region DefensePosition
    public static void SetDefenseLeftPosition(Vector2 PointDefenseLeft) => DefenseLeft = calculateCorrectPositionsFromHome(PointDefenseLeft);
    public static Vector2 GetDefenseLeftPosition () => DefenseLeft;
    public static void SetDefenseRightPosition(Vector2 PointDefenseRight) => DefenseRight = calculateCorrectPositionsFromHome(PointDefenseRight);
    public static Vector2 GetDefenseRightPosition() => DefenseRight;
    public static void SetDefenseAllPosition(Vector2 PointDefenseAll) => DefenseAll = calculateCorrectPositionsFromHome(PointDefenseAll);
    public static Vector2 GetDefenseAllPosition() => DefenseAll;
    #endregion

    public static void startSettingProperties(Vector2 homeBasePosition)
    {
        setCorrectBasePosition(homeBasePosition); // Setting Home and Enemy Base Position

        SetDefenseLeftPosition(setCorrectPositionOnACircle(radiusDefenseBase, -2000)); //radiusDefense = 3000
        SetDefenseRightPosition(setCorrectPositionOnACircle(radiusDefenseBase, -7000)); //radiusDefense = 3000
        SetDefenseAllPosition(setCorrectPositionOnACircle(radiusDefenseBase, -3500));

        Console.Error.WriteLine($"--> Defense Left Positions = {GetDefenseLeftPosition().X} : {GetDefenseLeftPosition().Y}");
        Console.Error.WriteLine($"--> Defense Right Positions = {GetDefenseRightPosition().X} : {GetDefenseRightPosition().Y}");

        SetMiddleLeftPosition(setCorrectPositionOnACircle(radiusMiddle, -2000));
        SetMiddleRightPosition(setCorrectPositionOnACircle(radiusMiddle, -6000));
        SetMiddleAllPosition(setCorrectPositionOnACircle(radiusMiddle, -4500));

        SetOffenseLeftPosition(setCorrectPositionOnACircle(radiusOffense, -7000));
        SetOffenseRightPosition(setCorrectPositionOnACircle(radiusOffense, -11000));
        SetOffenseAllPosition(setCorrectPositionOnACircle(radiusOffense, -5000));

    }
    public static Vector2 setCorrectPositionOnACircle(int radius, float move)
    {
        // myLibrary.RotateByArc(Vector2 Center, Vector2 A, float arc)
        // Vector2 center = GetHomeBasePosition();
        Vector2 center = Map.baseLeft;
        //Console.Error.WriteLine("===============================================");
        Vector2 A = new Vector2(radius, 0);
        //float arc = center.X == 0 ? move : move * -1;
        float arc = move;
        Vector2 rotByArc = myLibrary.RotateByArc(center, A, arc);
        //Console./*Error*/.WriteLine($"rotByArc Pos = {rotByArc.X}:{rotByArc.Y}");

        Vector2 temp = calculateCorrectPositionsFromHome(rotByArc);

        //Console.Error.WriteLine($"Vec2 Return Pos = {temp.X}:{temp.Y}");
        return rotByArc;

    }


    public static void setCorrectBasePosition(Vector2 homeBasePosition)
    {
        SetHomeBasePosition(homeBasePosition);
        SetEnemyBasePosition((homeBasePosition.X == 0) ? baseRight : baseLeft);
    }

    public static Vector2 calculateCorrectPositionsFromHome(Vector2 position)
    {
        var homeX = (int)GetHomeBasePosition().X;
        var homeY = (int)GetHomeBasePosition().Y;
        //Console.Error.WriteLine($"------- >> {homeX}:{homeY} - {position.X}:{position.Y} = {homeX - position.X}:{homeY - position.Y}");
        return new Vector2(Math.Abs(homeX - position.X), Math.Abs(homeY - position.Y));
    }


    /// <summary>
    /// Intersects a line and a circle.
    /// </summary>
    /// <param name="location">the location of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <param name="lineFrom">the starting point of the line</param>
    /// <param name="lineTo">the ending point of the line</param>
    /// <returns>true if the line and circle intersect each other</returns>
    public static bool IntersectLineCircle(Vector2 location, float radius, Vector2 lineFrom, Vector2 lineTo)
    {
        float ab2, acab, h2;

        Vector2 ac = location - lineFrom;
        Vector2 ab = lineTo - lineFrom;
        ab2 = Vector2.Dot(ab, ab);
        acab = Vector2.Dot(ac, ab);

        float t = acab / ab2;

        if (t < 0)
            t = 0;
        else if (t > 1)
            t = 1;

        Vector2 h = ((ab * t) + lineFrom) - location;
        h2 = Vector2.Dot(h, h);

        return (h2 <= (radius * radius));
    }

    public static string DirectionTo(Vector2 vector2)
    {
        if (GetHomeBasePosition().X == 0)
            if (vector2.X < 0 && vector2.Y < 0)
                return "home";
            else if (vector2.X > 0 && vector2.Y > 0)
                return "enemy";

        if (GetHomeBasePosition().X != 0)
            if (vector2.X > 0 && vector2.Y > 0)
                return "home";
            else if (vector2.X < 0 && vector2.Y < 0)
                return "enemy";
        
        return "nothing";
    }
}

class Player
{


    public const int TYPE_MONSTER = 0;
    public const int TYPE_MY_HERO = 1;
    public const int TYPE_OP_HERO = 2;


    public const int TIER_DEFENSE_LEFT = 0;
    public const int TIER_DEFENSE_RIGHT = 1;
    public const int TIER_DEFENSE_ALL = 2;

    public const int TIER_MIDDLE_LEFT = 3;
    public const int TIER_MIDDLE_RIGHT = 4;
    public const int TIER_MIDDLE_ALL = 5;

    public const int TIER_OFFENSE_LEFT = 6;
    public const int TIER_OFFENSE_RIGHT = 7;
    public const int TIER_OFFENSE_ALL = 8;

    public const int RADIUS_FOG = 2200;
    public const int RADIUS_WIND = 1000;

    public const int AREA_OFFENSE_TARGET = 500;


    public static Random rand = new Random();
    
    



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
        public float DistanceFromEntityToBase;
        public float DistanceFromEntityToEnemyBase;
        public Vector2 Direction;
        public string HeadingTo;




        public Entity(int id, int type, int x, int y, int shieldLife, int isControlled, int health, int vx, int vy, int nearBase, int threatFor, int threadPoints, float distanceFromEntityToBase, float distanceFromEntityToEnemyBase, Vector2 direction, string headingTo) //, float toLeftBase, float toRightBase)
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
            this.DistanceFromEntityToBase = distanceFromEntityToBase;
            this.DistanceFromEntityToEnemyBase = distanceFromEntityToEnemyBase;
            this.Direction = direction;
            this.HeadingTo = headingTo;
        }
    }

    static void Main(string[] args)
    {

        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        Entity offenseTargetFocus = null;

        // base_x,base_y: The corner of the map representing your base
        int baseX = int.Parse(inputs[0]);
        int baseY = int.Parse(inputs[1]);

        
        Map.startSettingProperties(new Vector2(baseX, baseY));

        // heroesPerPlayer: Always 3
        int heroesPerPlayer = int.Parse(Console.ReadLine());


        // game loop
        while (true)
        {
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
            // List<Entity> targets = new List<Entity>(entityCount);
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

                var distanceEntityToHome = myLibrary.calculateCorrectDistance(Map.GetHomeBasePosition(), new Vector2(x, y));
                float distanceEntityToEnemyBase = myLibrary.calculateCorrectDistance(Map.GetEnemyBasePosition(), new Vector2(x, y));

                //var distanceFromSpiderToLeftBase = myLibrary.calculateCorrectDistance(BASE_LEFT, new Vector2(x, y));
                //var distanceFromSpiderToRightBase = myLibrary.calculateCorrectDistance(BASE_RIGHT, new Vector2(x, y));

                var threadPoints = 0;
                string directionToBase = Map.DirectionTo(new Vector2(vx, vy));


                switch (type)
                {
                    case TYPE_MONSTER:
                        if (directionToBase == "home")
                            threadPoints += 500;
                        if (threatFor == 1)
                            threadPoints += 500;
                        if (distanceEntityToHome < 5000f)
                            threadPoints += 500;
                        break;

                }


                Entity entity = new Entity(
                    id, type, x, y, shieldLife, isControlled, health, vx, vy, nearBase, threatFor, threadPoints, distanceEntityToHome, distanceEntityToEnemyBase, new Vector2(vx, vy), directionToBase // , distanceFromSpiderToLeftBase, distanceFromSpiderToRightBase
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

            Console.Error.WriteLine($"MonsterCount = {monsters.Count}");


            var monsterSortedToHomeBase = myLibrary.SortMonsters(monsters, Map.GetHomeBasePosition());
            var monsterSortedToEnemyBase = myLibrary.SortMonsters(monsters, Map.GetEnemyBasePosition());

            foreach (var target in monsterSortedToHomeBase)
                Console.Error.WriteLine("INFO defense: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEntityToBase + " " + target.X + ":" + target.Y);
            foreach (var target in monsterSortedToEnemyBase)
                Console.Error.WriteLine("INFO offense: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEntityToBase + " " + target.X + ":" + target.Y);



            Console.Error.WriteLine("INFO: Start Action for Heroes!!!");


            for (int i = 0; i < heroesPerPlayer; i++)
            {
                switch (i)
                {
                    case 3:
                    case 0:
                        //Console.Error.WriteLine($"Hero {i} goes defense");
                        if (monsterSortedToHomeBase.Count > 0)
                            defense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, TIER_DEFENSE_LEFT, Map.GetHomeBasePosition());
                        else
                        {
                            Console.Error.WriteLine("INFO DEFENSE: Left Position");
                            myLibrary.move(Map.GetDefenseLeftPosition());
                        }
                        break;
                    case 4:
                    case 1:
                        //Console.Error.WriteLine($"\nHero {i} goes defense");
                        if (monsterSortedToHomeBase.Count > 0)
                            defense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, TIER_DEFENSE_RIGHT, Map.GetHomeBasePosition());
                        else
                        {
                            Console.Error.WriteLine("INFO DEFENSE: Right Position");
                            myLibrary.move(Map.GetDefenseRightPosition());
                        }
                        break;
                    case 5:
                    case 2:
                        //Console.Error.WriteLine($"\nHero {i} goes offense");
                        if ((monsterSortedToHomeBase.Count > 0 && monsterSortedToHomeBase.Count < 5 && monsterSortedToHomeBase[0].Health < 15) || myHealth < 1) //myMana < 30 && 
                            offenseTargetFocus = offense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToHomeBase, i, myMana, TIER_MIDDLE_ALL, oppHeroes, Map.GetHomeBasePosition(), offenseTargetFocus);
                        else if (monsterSortedToEnemyBase.Count > 0 && monsterSortedToEnemyBase[0].Health >= 20)
                        {
                            offenseTargetFocus = offense(new Vector2(myHeroes[i].X, myHeroes[i].Y), monsterSortedToEnemyBase, i, myMana, TIER_OFFENSE_ALL, oppHeroes, Map.GetEnemyBasePosition(), offenseTargetFocus);

                        }
                        else
                            myLibrary.move(Map.GetOffenseAllPosition());
                        break;

                };


            }
            if (offenseTargetFocus != null)
                Console.Error.WriteLine($"\nINFO END: Target for next Round: {offenseTargetFocus.Id}");
            else
                Console.Error.WriteLine($"\nINFO END: no Target for next Round");
        }
    }




    static void defense(Vector2 myHero, List<Player.Entity> myTarget, int hero, int defenseMana, int version, Vector2 homeBase)
    {
        Vector2 trianglePoint1 = new Vector2();
        Vector2 trianglePoint2 = new Vector2();
        Vector2 trianglePoint3 = new Vector2();
        //bool targeting = false;

        Console.Error.Write($"\nDEFENSE: Hero {hero}: Start ");

        switch (version)
        {
            case 0:
                // Left Triangle from Base
                // TIER_DEFENSE_LEFT = 0

                Console.Error.Write($"-> TIER_DEFENSE_LEFT ");
                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(Map.baseLeft, version);
                break;

            case 1:
                // Right Triangle from Base
                // TIER_DEFENSE_RIGHT = 2

                Console.Error.Write($"-> TIER_DEFENSE_RIGHT ");
                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(Map.baseLeft, version);
                break;

            case 2:
                //Defense from Base
                // TIER_DEFENSE_ALL = 3

                Console.Error.Write($"-> TIER_DEFENSE_ALL ");
                (trianglePoint1, trianglePoint2, trianglePoint3) = myLibrary.formTriangle(Map.baseLeft, version);
                break;

        }

        var targetsInReach = myTarget.Where(e => myLibrary.PointInTriangle(new Vector2(e.X, e.Y), trianglePoint1, trianglePoint2, trianglePoint3)).OrderBy(x => x.DistanceFromEntityToBase).ToList(); //.OrderBy(x => x.DistanceFromEnemyToBase)

        if (targetsInReach.Count > 0)
        {
            var itemsPosition = new Vector2(targetsInReach[0].X, targetsInReach[0].Y);

            Console.Error.Write($"-> targeting: {targetsInReach[0].Id}: {targetsInReach[0].X} {targetsInReach[0].Y} ");

            var windInRange = myLibrary.PointInCircle(myHero, itemsPosition, RADIUS_WIND);

            if (windInRange && defenseMana >= 10 && targetsInReach[0].DistanceFromEntityToBase < 3000)
            {
                myLibrary.wind();
                Console.Error.WriteLine($"-> WIND");
            }
            else
            {
                Console.Error.WriteLine($"-> ATTACK");
                myLibrary.move(itemsPosition);
            }

        }
        else
        {
            Console.Error.Write($"-> no Enemy in Triangle\n --> ");
            switch (version)
            {
                case 0:
                    // Left Triangle from Base
                    // TIER_DEFENSE_LEFT = 0
                    Console.Error.Write($"-> going defense left ");
                    myLibrary.move(Map.GetDefenseLeftPosition());

                    break;
                case 1:
                    // Right Triangle from Base
                    // TIER_DEFENSE_RIGHT = 2
                    Console.Error.Write($"-> going defense right ");
                    myLibrary.move(Map.GetDefenseRightPosition());

                    break;

                case 2:
                    //Defense from Base
                    // TIER_DEFENSE_ALL = 3
                    Console.Error.Write($"-> going defense all ");
                    myLibrary.move(Map.GetDefenseAllPosition());
                    break;

            }

        }

    }

    static Entity offense(Vector2 offenseHeroes, List<Player.Entity> monster, int hero, int offenseMana, int defaultPosition, List<Entity> oppHeroes, Vector2 toBase, Entity offenseTargetFocus)
    {

        var positionSpider = new Vector2();
        float nearestOppHeroMin = 9999.0f; // = myLibrary.calculateCorrectDistance(BASE_RIGHT, new Vector2(x, y));
        int targetOppHeroID = 0;
        List<Player.Entity> enemyBaseTarget = new List<Player.Entity>();
        List<Player.Entity> enemyBaseTargetNear = new List<Player.Entity>();
        List<Player.Entity> enemyBaseTargetFar = new List<Player.Entity>();

        Vector2 targetOppHeroPos = new Vector2();
        if (oppHeroes.Count > 0)
        {
            foreach (var oppHero in oppHeroes)
            {
                var nearestOppHeroDistance = myLibrary.calculateCorrectDistance(Map.GetEnemyBasePosition(), new Vector2(oppHero.X, oppHero.Y));
                if (nearestOppHeroDistance < nearestOppHeroMin)
                {
                    nearestOppHeroDistance = nearestOppHeroMin;
                    targetOppHeroID = oppHero.Id;
                    targetOppHeroPos = new Vector2(oppHero.X, oppHero.Y);

                }
                Console.Error.WriteLine($"Targeting OpHero {targetOppHeroID}");
            }
        }
        else
            targetOppHeroPos = Map.GetOffenseAllPosition();


        Console.Error.Write($"\nOFFENSE: Hero {hero}: Start ");
        

        if (offenseTargetFocus != null)
        {
            Console.Error.Write("-> offenseTargetFocus is not null ");
            var tempCheck = monster.Where(x => x.Id == offenseTargetFocus.Id).ToList();

            if (tempCheck.Count == 0)
            {
                Console.Error.WriteLine($"-> Target is gone ");
                offenseTargetFocus = null;
            }
        }
        

        //Console.Error.WriteLine($"�������������� {tempCheck}");

        if (offenseTargetFocus != null)
            Console.Error.WriteLine($"==>>>Targeting == {offenseTargetFocus.Id}");
        else
            Console.Error.WriteLine($"==>>>offenseTargetFocus is null");

        if (offenseTargetFocus == null)
        {
            Console.Error.WriteLine($"INFO OFFENSE: 5 Middle, 8 Offense {defaultPosition}");
            switch (defaultPosition)
            {
                case 5:
                    enemyBaseTarget = monster.Where(x => x.DistanceFromEntityToEnemyBase < Map.radiusOffense).OrderByDescending(y => y.DistanceFromEntityToEnemyBase).ToList(); //Where(x => x.ThreatFor == 2). .Where(z => z.DistanceFromEnemyToEnemyBase > 5000)
                    break;
                case 8:

                    enemyBaseTarget = monster.Where(x => x.DistanceFromEntityToEnemyBase < Map.radiusMiddle).ToList(); //Where(x => x.ThreatFor == 2). .Where(z => z.DistanceFromEnemyToEnemyBase > 5000)
                    enemyBaseTargetNear = monster.Where(x => x.DistanceFromEntityToEnemyBase < Map.radiusDefense).OrderByDescending(y => y.DistanceFromEntityToEnemyBase).ToList(); //Where(x => x.ThreatFor == 2). .Where(z => z.DistanceFromEnemyToEnemyBase > 5000)
                    break;
            }

            if (enemyBaseTarget.Count > 0)
            {
                foreach (var target in enemyBaseTarget)
                    Console.Error.WriteLine("INFO OFFENSE: targets are: " + target.Id + " " + target.Type + " " + target.ThreadPoints + " " + target.DistanceFromEntityToEnemyBase + " " + target.Direction.X + ":" + target.Direction.Y);

                Console.Error.Write("-> getting Target ");

                positionSpider = new Vector2(enemyBaseTarget[0].X, enemyBaseTarget[0].Y); //new Vector2(item.X, item.Y);
                offenseTargetFocus = enemyBaseTarget[0];
                Console.Error.Write($"-> Targeting {offenseTargetFocus.Id} Heading {offenseTargetFocus.HeadingTo}; Mana: {offenseMana} ");






                if (offenseTargetFocus.HeadingTo != "enemy" && offenseMana > 20 && enemyBaseTarget[0].DistanceFromEntityToEnemyBase < 6000)
                {
                    Console.Error.WriteLine($"-> Mana -> Control");
                    myLibrary.control(offenseTargetFocus.Id);

                }
                else if (offenseTargetFocus.HeadingTo == "enemy" && offenseTargetFocus.ShieldLife == 0 && offenseMana > 10 && enemyBaseTarget[0].DistanceFromEntityToEnemyBase < 6000)
                {
                    Console.Error.WriteLine($"-> Mana -> Shield");
                    myLibrary.shield(offenseTargetFocus.Id);
                }
                else if (offenseMana > 200)
                {
                    Console.Error.WriteLine($"-> Mana -> Wind");
                    myLibrary.wind();
                }
                else if (offenseTargetFocus.ShieldLife == 0) // && offenseTargetFocus.HeadingTo != "enemy")
                    myLibrary.move(positionSpider);
                else
                    myLibrary.move(Map.GetOffenseAllPosition());



            }
            else
                switch (defaultPosition)
                {
                    case 5:
                        myLibrary.move(Map.GetMiddleAllPosition());
                        break;
                    case 8:
                        Console.Error.WriteLine("-> heading OffenseAll");
                        myLibrary.move(Map.GetOffenseAllPosition());
                        break;
                }

        }
        else
        {

            
            var tempCheck = monster.Where(x => x.Id == offenseTargetFocus.Id).ToList();

            if (tempCheck == null)
            {
                Console.Error.WriteLine($"-> Target is gone ");
                offenseTargetFocus = null;
                return offenseTargetFocus;
            }

            Console.Error.WriteLine($"==>>Targeting Attack {tempCheck[0].Id}");
            myLibrary.move(new Vector2(tempCheck[0].X, tempCheck[0].Y));
            return tempCheck[0];
        }

        // myLibrary.move(new Vector2(offenseTargetFocus.X, offenseTargetFocus.Y));
        return offenseTargetFocus;
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
            Console.WriteLine($"MOVE {(int)position.X} {(int)position.Y}");
        }
        public static void wind()
        {
            Console.WriteLine($"SPELL WIND {(int)Map.GetEnemyBasePosition().X} {(int)Map.GetEnemyBasePosition().Y}");
        }

        public static void control(int controlID)
        {
            Console.WriteLine($"SPELL CONTROL {controlID} {(int)Map.GetEnemyBasePosition().X}  {(int)Map.GetEnemyBasePosition().Y}");

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

        internal static List<Player.Entity> SortMonsters(List<Player.Entity> monsters, Vector2 direction)
        {

            return direction.X == 0 ? monsters.OrderBy(y => y.DistanceFromEntityToBase).ToList() : monsters.Where(x => x.DistanceFromEntityToEnemyBase < 8000).OrderByDescending(y => y.DistanceFromEntityToEnemyBase).ToList();


            //.ThenByDescending(y => y.DistanceFromEnemyToBase)  .Where(c => c.ThreadPoints >= 0)  .ThenByDescending(x => x.ThreadPoints)
        }



        public static (Vector2 trianglePoint1, Vector2 trianglePoint2, Vector2 trianglePoint3) formTriangle(Vector2 whereToForm, int whichTrianglePart)
        {
        
            if (whichTrianglePart == 0)
                return (
                Map.calculateCorrectPositionsFromHome(whereToForm), 
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle(Map.radiusDefenseBase + 1000, whereToForm.Y)), 
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle(Map.radiusDefenseBase + 1000, -7000)));
            else if (whichTrianglePart == 1)
                return (
                Map.calculateCorrectPositionsFromHome(whereToForm),
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle(Map.radiusDefenseBase + 500, -5000)),
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle((int)whereToForm.X, -18000)));
            else if (whichTrianglePart == 2)
                return (
                Map.calculateCorrectPositionsFromHome(whereToForm),
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle(Map.radiusDefenseBase + 500, whereToForm.Y)),
                Map.calculateCorrectPositionsFromHome(Map.setCorrectPositionOnACircle((int)whereToForm.X, -15000)));

            return (new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        }

        public static Vector2 calculateCorrectPositions(Vector2 whereToForm, int deltaX, int deltaY)
        {
            return new Vector2(Math.Abs((int)whereToForm.X - deltaX), Math.Abs((int)whereToForm.Y - deltaY));
        }


        public static Vector2 RotateByArc(Vector2 Center, Vector2 A, float arc)
        {
            //calculate radius
            float radius = Vector2.Distance(Center, A);

            //calculate angle from arc
            float angle = arc / radius;

            Vector2 B = RotateByRadians(Center, A, angle);

            return B;
        }

        public static Vector2 RotateByRadians(Vector2 Center, Vector2 A, float angle)
        {
            //Move calculation to 0,0
            Vector2 v = Vector2.Subtract(A, Center);

            //rotate x and y
            float x = (float)(v.X * Math.Cos(angle) + v.Y * Math.Sin(angle));
            float y = (float)(v.Y * Math.Cos(angle) - v.X * Math.Sin(angle));

            //move back to center
            Vector2 B = new Vector2(x, y) + Center;

            return B;
        }


        #endregion

    }

