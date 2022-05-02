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
    internal static bool DebugDefense = true;


    public const int TYPE_MONSTER = 0;
    public const int TYPE_MY_HERO = 1;
    public const int TYPE_OP_HERO = 2;

    public const int HERO_0 = 0;
    public const int HERO_1 = 1;
    public const int HERO_2 = 2;
    

    public const int ACTION_DEFENSE_LEFT = 0;
    public const int ACTION_DEFENSE_RIGHT = 1;
    public const int ACTION_DEFENSE_MIDDLE = 2;
    public const int ACTION_DEFENSE_ALL = 3;

    public const int ACTION_OFFENSE_LEFT = 10;
    public const int ACTION_OFFENSE_RIGHT = 11;
    public const int ACTION_OFFENSE_MiDDLE = 12;

    public static bool shield1 = false;
    public static bool shield2 = false;
    internal static bool controllingOppHero = false;
    internal static bool alarmOppNearBasis = false;

    internal static Dictionary<int, int> HealthMaxDevided = new Dictionary<int, int>();
    internal static Dictionary<int, bool> gotControlled = new Dictionary<int, bool>();


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
            Map.countingRounds++;
            
           // Console.Error.WriteLine($"--->> ROUND {Map.countingRounds}");
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

            //targetOffense = monsters.Where(x => x.DistanceToEnemyBase < 15000 && x.DistanceToBase > 8000).OrderByDescending(x => x.DistanceToEnemyBase).ToList();

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
                
                if (!HealthMaxDevided.ContainsKey(id))
                    HealthMaxDevided.Add(id, health / 3);

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
                        var tempPosition = monsters[monsters.Count - 1].Position;
                        var tempTrajDirection = new Vector2(vx,vy);
                        var tempDistance = Vector2.Distance(Map.HomeBase, tempPosition);
                       // Console.Error.WriteLine($" ================>> {entity.Id} -> Thread # {entity.ThreatFor} (1 = me, 2=enemy, 0=nothing)");

                        for (int ji = 0; ji < 20; ji++)
                        {
                            var tempFuturePosition = Vector2.Add(tempPosition, tempTrajDirection);
                            var tempFutureDistance = Vector2.Distance(Map.HomeBase, tempFuturePosition);

                            //Console.Error.WriteLine($" ================>> Vector {ji} -> {tempFuturePosition.X} - {tempFuturePosition.Y} -> {Vector2.Distance(Map.HomeBase, tempFuturePosition)}");
                            if (tempFutureDistance < tempDistance)
                            {
                                if (tempFutureDistance < 6500)
                                {
                                   // Console.Error.WriteLine($" ================>> {entity.Id} was {entity.ThreatFor} is ThreadFor 1 now");
                                    entity.ThreatFor = 1;
                                    break;
                                }
                            }
                            else
                            {
                               // Console.Error.WriteLine($" ================>> FutureDistance getting farer away");
                                break;
                            }

                            tempPosition = tempFuturePosition;
                        }

                       // Console.Error.WriteLine($" ================>> finished with {entity.Id}\n");



                        if (!gotControlled.ContainsKey(entity.Id))
                            gotControlled.Add(entity.Id, false);
                        if (entity.ThreatFor == 1)
                            gotControlled[entity.Id] = false;
                        break;
                    case TYPE_OP_HERO:
                        oppHeroes.Add(entity);
                        break;
                    case TYPE_MY_HERO:
                        entity.mana = myMana;
                        
                        myHeroes.Add(entity);
                        break;
                }
            }

            
            foreach (var hero in myHeroes)
            {
                hero.targetSpidersDefenseAll = monsters.OrderBy(x => x.ThreatFor).ThenBy(x => x.DistanceToBase).ToList();
                hero.targetSpidersDefenseBase = hero.targetSpidersDefenseAll.Where(x => x.NearBase == 1).ToList();
                hero.targetSpidersDefenseBaseWarning = hero.targetSpidersDefenseAll.Where(x => x.ThreatFor == 1).ToList();
                hero.targetSpidersDefenseBaseAlarm = hero.targetSpidersDefenseBase.Where(x => x.DistanceToBase < Entity.RADIUS_ALARM_BASE).ToList();

                if (Map.HomeBase.X == 0)
                {
                    hero.targetSpidersDefenseOne = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 8500 && 
                        myMath.checkAboveLine(Map.HomeBase, Map.seperateLeftThirdOneLineUp, x.Position)).OrderByDescending(x => x.ThreatFor).ToList();

                    hero.targetSpidersDefenseTwo = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 8500 && 
                        !myMath.checkAboveLine(Map.HomeBase, Map.seperateLeftThirdTwoDown, x.Position)).OrderByDescending(x => x.ThreatFor).ToList();

                    hero.targetSpidersDefenseThree = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 8500 && 
                        myMath.checkAboveLine(Map.HomeBase, Map.seperateLeftThirdThreeLineUp, x.Position) &&
                        !myMath.checkAboveLine(Map.HomeBase, Map.seperateLeftThirdThreeLineDown, x.Position)
                        ).OrderByDescending(x => x.ThreatFor).ToList();

                    //  && hero.targetSpidersDefenseBaseWarning.Count > 0
                }
                else
                {
                    
                    hero.targetSpidersDefenseTwo = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 8500 &&
                        !myMath.checkAboveLine(Map.HomeBase, Map.seperateRightThirdTwoUp, x.Position)).ToList();

                    hero.targetSpidersDefenseThree = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 8500 &&
                        myMath.checkAboveLine(Map.HomeBase, Map.seperateRightThirdThreeLineDown, x.Position) &&
                        !myMath.checkAboveLine(Map.HomeBase, Map.seperateRightThirdThreeLineUp, x.Position)).ToList();
                    // || hero.targetSpidersDefenseBaseWarning.Count > 0)
                    
                    hero.targetSpidersDefenseOne = hero.targetSpidersDefenseAll.Where(x => x.DistanceToBase < 85000 &&
                        myMath.checkAboveLine(Map.HomeBase, Map.seperateRightThirdOneLineDown, x.Position)).ToList();

                }

                


                hero.targetSpiderIsInControllRadius = hero.targetSpidersDefenseAll.Where(x => Map.PointInCircle(hero, x, Entity.RADIUS_CONTROL) && x.IsControlled == 0).ToList();
                hero.targetSpiderIsInWindRadius = hero.targetSpidersDefenseAll.Where(x => Map.PointInCircle(hero, x, Entity.RADIUS_WIND) && x.ShieldLife == 0).ToList();

                hero.targetDefenseOppHero = oppHeroes.Where(x => x.DistanceToBase < 6000).ToList();
                hero.targetDefenseOppHeroShielded = hero.targetDefenseOppHero.Where(x => x.EntityIsShielded == true).ToList();
                hero.targetDefenseOppHeroIsNotShielded = hero.targetDefenseOppHero.Where(x => x.EntityIsShielded == false).ToList();

                hero.targetDefenseOppHeroAbove = oppHeroes.Where((x) => myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position)).ToList();
                hero.targetDefenseOppHeroBelow = oppHeroes.Where((x) => !myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position)).ToList();

                hero.targetOffenseOppHero = oppHeroes.Where(x => x.DistanceToEnemyBase < 5000).ToList();
                hero.targetOffenseOppHeroShielded = hero.targetOffenseOppHero.Where(x => x.EntityIsShielded == true).ToList();
                hero.targetOffenseOppHeroIsNotShielded = hero.targetOffenseOppHero.Where(x => x.EntityIsShielded == false).OrderBy(x => x.DistanceToEnemyBase).ToList();

                hero.enoughMana = myMana > 9;
            }
            foreach (var spider in myHeroes[0].targetSpidersDefenseOne)
            {
               // Console.Error.WriteLine($"INFO 001 D1: found {spider.Id} - Thread {spider.ThreatFor}");
            }
            foreach (var spider in myHeroes[0].targetSpidersDefenseTwo)
            {
               // Console.Error.WriteLine($"INFO 001 D2: found {spider.Id} - Thread {spider.ThreatFor}");
            }
            foreach (var spider in myHeroes[0].targetSpidersDefenseThree)
            {
               // Console.Error.WriteLine($"INFO 001 D3: found {spider.Id} - Thread {spider.ThreatFor}");
            }

            #endregion



            //if (myMana > 240)
            //    Map.goOffense = true;
            //if (myMana < 10)
            //    Map.goOffense = false;

            //if (!Map.goOffense)
            //{
            //    //Console.Error.WriteLine("Defense");
            //    Entity.defense(monsters, myHeroes, oppHeroes, 0, myMana);
            //}
            //else
            //{
            //    //Console.Error.WriteLine("ATTACK!!!!!!!!!!!!!!!!!!!!!!!");

            //    Entity.offense(monsters, myHeroes, oppHeroes, 0);
            //}

            //if (Map.goOffense == false && myMana < 100)
            //    Entity.defense(monsters, myHeroes, oppHeroes, 1, myMana);
            //else
            //    Entity.defense(monsters, myHeroes, oppHeroes, 2, myMana);
            //if (Map.goOffense == false && myMana < 100)
            //    Entity.defense(monsters, myHeroes, oppHeroes, 0, myMana);
            //else
            //    Entity.defense(monsters, myHeroes, oppHeroes, 2, myMana);


            for (int i = 0; i < heroesPerPlayer; i++)
            {
                if ((myMana < 300) && Map.goOffense == false )
                {
                    Map.goOffense = false;
                    switch (i)
                    {
                        case 0:
                            myHeroes[0].defense(myHeroes, ACTION_DEFENSE_LEFT, HERO_0);
                            break;
                        case 1: // Defense Player
                            myHeroes[1].defense(myHeroes, ACTION_DEFENSE_RIGHT, HERO_1);
                            break;
                        case 2: // Defense Player
                            myHeroes[2].defense(myHeroes, ACTION_DEFENSE_MIDDLE, HERO_2);
                            break;
                    }
                }
                else
                {
                    Map.goOffense = true;
                    Map.posDefense.Clear();
                    Map.setPosDefensesAllLeft();
                    Map.setPosDefensesAllRight();
                    Map.setPosDefenses2();
                   // Console.Error.WriteLine("---------------------------->> GO OFFENSE");

                    switch (i)
                    {
                        case 0:
                            myHeroes[0].defense(myHeroes, ACTION_DEFENSE_ALL, HERO_0);
                            break;
                        case 1: // Defense Player
                            myHeroes[1].defense(myHeroes, ACTION_DEFENSE_ALL, HERO_1);
                            break;
                        case 2: // OFFENSE Player
                            Entity.offense(monsters, myHeroes, oppHeroes, 0);
                            break;
                    }
                }
            }
            shield1 = false;
            shield2 = false;
            // allreadyGoingToOppHero = false;
            controllingOppHero = false;
            //if (Entity.countingRounds > 10) Entity.countingRounds = 0;
        }
    }
}


public class        Entity
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

    
    public static readonly int RADIUS_WIND = 1200;
    public static readonly int RADIUS_CONTROL = 2200;

    //public static List<Entity> targetDefense = new List<Entity>();


    public Vector2 Direction;
    public string HeadingTo;
    internal static bool goOffense = false;
    internal int mana;
    internal List<Entity> targetSpidersDefenseAll;
    internal List<Entity> targetSpidersDefenseBase;
    internal List<Entity> targetSpidersDefenseBaseWarning;
    internal List<Entity> targetSpidersDefenseOne;
    internal List<Entity> targetSpidersDefenseTwo;
    internal List<Entity> targetSpidersDefenseThree;
    internal List<Entity> targetSpiderIsInControllRadius;
    internal List<Entity> targetSpiderIsInWindRadius;
    
    internal List<Entity> targetDefenseOppHero;
    internal List<Entity> targetDefenseOppHeroIsNotShielded;
    internal List<Entity> targetDefenseOppHeroShielded;

    internal List<Entity> targetOffenseOppHero;
    internal List<Entity> targetOffenseOppHeroShielded;
    internal List<Entity> targetOffenseOppHeroIsNotShielded;
    internal List<Entity> targetSpidersDefenseBaseAlarm;

    internal static int RADIUS_ALARM_BASE = 5000;
    
    internal bool enoughMana;
    internal List<Entity> targetDefenseOppHeroAbove;
    internal List<Entity> targetDefenseOppHeroBelow;

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
        else
            this.DirectionEnemyBase = false;

    }

    internal static void move(Vector2 position)
    {
        Console.WriteLine($"MOVE {position.X} {position.Y}");
    }

    internal static void wind(Vector2 position)
    {
        //Vector2 direction = myMath.calculateAwayfromHome(position);
        Console.WriteLine($"SPELL WIND {position.X} {position.Y}");
        
    }

    public static void shield(Entity entity)
    {
        Console.WriteLine($"SPELL SHIELD {entity.Id}");
        Map.countingShielded++;
    }

    internal static void control(int id, Vector2 direction)
    {
        Console.WriteLine($"SPELL CONTROL {id} {direction.X} {direction.Y}");
    }
    private static void wait()
    {
        Console.WriteLine("WAIT");
    }

    internal static void offense(List<Entity> monsters, List<Entity> myHeroes, List<Entity> oppHeroes, int Version)
    {
        int distanceOffense = 4500;
        //Vector2 seperateLine = new Vector2(8000, 7000);
        

        List<List<Entity>> targetOffense = new List<List<Entity>>();

        targetOffense.Add(monsters.Where(x => x.DistanceToEnemyBase < distanceOffense).OrderBy(x => x.DistanceToBase).ToList());
        targetOffense.Add(new List<Entity>());
        targetOffense.Add(new List<Entity>());


        Console.Error.WriteLine($" ===============> MANA <========== {myHeroes[2].mana}");
        if (myHeroes[2].mana < 120)
        {
            foreach (var spider in monsters)
            {
                
                if (Map.PointInCircle(myHeroes[2], spider, RADIUS_CONTROL) && spider.DistanceToEnemyBase < 7500 && spider.DistanceToEnemyBase > 6000) 
                {
                    //Entity.move(spider.Position);
                    Entity.control(spider.Id, Map.EnemyBase);
                    return;
                }
            }


            Console.Error.WriteLine($" ===============> MANA <========== No Control");
            

            foreach (var spider in monsters)
            {
                if (oppHeroes.Where(x => x.ShieldLife > 0).Count() > 0 && myHeroes[2].enoughMana)
                {
                    if (!spider.EntityIsShielded && spider.DistanceToEnemyBase < 5000)
                    {
                        Entity.shield(spider);
                        return;
                    }
                }

                Console.Error.WriteLine($" ===============> MANA <========== No Shield");
                if (Map.PointInCircle(myHeroes[2], spider, RADIUS_WIND) && myHeroes[2].enoughMana)
                {
                    Entity.wind(Map.EnemyBase);
                    return;
                }

                Console.Error.WriteLine($" ===============> MANA <========== No wind");
                if (spider.DistanceToEnemyBase < 5000)
                {
                    Entity.move(spider.Position);
                    return;
                }
                Console.Error.WriteLine($" ===============> MANA <========== No move spider");
            }
            var helping = oppHeroes.Where(x => x.DistanceToEnemyBase < 6500).OrderBy(x => x.DistanceToEnemyBase).ToList();
            if (helping.Count > 0)
            {
                Entity.move(helping[0].Position);
                return;
            }
            Console.Error.WriteLine($" ===============> MANA <========== No opp Hero moving");
            Entity.move(Map.posOffenseMiddle);
            return;
        }

        foreach (var spider in targetOffense[0].Where(x => x.ThreatFor == 2))
        {
            if (!spider.EntityIsShielded && spider.Health > 15 && myHeroes[2].mana > 100)
            {
               // Console.Error.WriteLine($"OFFENSE --> SHIELDING {spider.Id}");
                Entity.shield(spider);

                return;
            }
        }

        if (myHeroes[2].targetOffenseOppHeroIsNotShielded.Count > 0 && monsters.Where(x => x.DistanceToEnemyBase < distanceOffense).Count() > 0 && myHeroes[2].enoughMana)
        {
            foreach (var oppHero in myHeroes[2].targetOffenseOppHeroIsNotShielded)
            {

                Entity.control(oppHero.Id, Map.HomeBase);
                return;
            }
            //if (!spider.EntityIsShielded && Map.PointInCircle(myHeroes[2], spider, RADIUS_WIND) && myHeroes[2].mana > 9)
        //    {
        //       // Console.Error.WriteLine($"OFFENSE --> Winding {spider.Id}");
        //        Entity.wind(Map.EnemyBase);

                //        return;
                //    }


        }



        //foreach (var spider in targetOffense[0].Where(x => x.ThreatFor == 2).ToList())
        //{
        //    if (!spider.EntityIsShielded && Map.PointInCircle(myHeroes[2], spider, RADIUS_WIND) && myHeroes[2].mana > 9)
        //    {
        //       // Console.Error.WriteLine($"OFFENSE --> Winding {spider.Id}");
        //        Entity.wind(Map.EnemyBase);

        //        return;
        //    }
        //}

        //if (!myHeroes[2].EntityIsShielded)
        //{
        //    Entity.shield(myHeroes[2]);
        //    return;
        //}







        Entity.move(Map.posOffenseMiddle);
        return;
        //var monstersShielded = monsters.Where(x => x.DirectionEnemyBase == true && x.EntityIsShielded == true).Count();

        //var nearestOppHero = oppHeroes.Where(y => (Map.PointInCircle(myHeroes[0], y, Entity.RADIUS_CONTROL))).OrderBy(x => x.DistanceToEnemyBase).ToList();

        //foreach (var nearestOp in nearestOppHero)
        //{
        //   // Console.Error.WriteLine($"OFFENSE INFO 001: Found {nearestOp.Id}");
        //}

        ////if (nearestOppHero.Count > 0)
        ////{
        ////    nearestOppHero.Clear();
        ////    foreach (var opHero in nearestOppHero)
        ////    {

        ////       // Console.Error.WriteLine($"OFFENSE INFO 002: Checking {opHero.Id} - Hero #{myHeroes[0]}");
        ////        if (Map.PointInCircle(myHeroes[0], opHero, RADIUS_CONTROL))
        ////        {

        ////            nearestOppHero.Add(opHero);
        ////           // Console.Error.WriteLine($"OFFENSE INFO 003: Adding OppHero {opHero.Id}");
        ////            break;
        ////        }
        ////    }
        ////}

        ////foreach (var nearestOp in nearestOppHero)
        ////{
        ////   // Console.Error.WriteLine($"OFFENSE INFO 004: Found {nearestOp.Id}");
        ////}
        ////if (!
        ////    (nearestOppHero != null && Map.PointInCircle(myHeroes[Version], nearestOppHero.Position, RADIUS_WIND)))
        ////    nearestOppHero = null;

        //bool commanded = false;

        //Console.Error.WriteLine($"INFO 005: OFFENSE count = {targetOffense[0].Count}");

        //if (targetOffense[Version].Count == 0)
        //{
        //    //Entity.move(Map.posOffenseMiddle); dfghjk
        //    //if (myHeroes[0].DistanceToEnemyBase < 4000)
        //    //{
        //    //    Map.setPosOffense2();
        //    //}
        //    //else if (myHeroes[0].DistanceToEnemyBase > 7000)
        //    //{
        //    //Console.Error.WriteLine("OFFENSE: Moving back 2 because nothing to do");
        //    //Map.setPosOffense();
        //    //}
        //    if (nearestOppHero.Count > 0 && nearestOppHero[0].DistanceToEnemyBase < 6000)
        //        Entity.move(nearestOppHero[0].Position);
        //    else
        //        Entity.move(Map.posOffenseMiddle);
        //}
        //else if (myHeroes[0].ShieldLife == 0)
        //    Entity.shield(myHeroes[0]);
        //else
        //{
        //    // if (Map.countingRounds % 20 == 0) Map.countingShielded = 0;

        //   // Console.Error.WriteLine($"------------> 006 counting Shielded {Map.countingShielded}");
        //    if (Map.countingShielded > 1 && nearestOppHero.Count > 0)// && nearestOppHero[0].EntityIsShielded == false)
        //    {
        //       // Console.Error.WriteLine("OFFENSE 007: There are more than 1 correct");
        //        Entity.control(nearestOppHero[0].Id, Map.HomeBase);
        //        commanded = true;
        //    }
        //    else if (Map.countingShielded > 2 && myHeroes[0].ShieldLife > 1)
        //        Entity.move(myMath.calculateCorrectPositionFrom(Map.HomeBase, Map.posOffenseMiddle));
        //    else if (myHeroes[0].ShieldLife == 1)
        //        Entity.shield(myHeroes[0]);
        //    else
        //    {
        //        foreach (var spider in targetOffense[Version])
        //        {
        //            if (spider.DirectionEnemyBase == false && spider.EntityIsShielded == false)
        //            {
        //               // Console.Error.WriteLine($"OFFENSE {Version} 008: Controlling {spider.Id}");
        //                Entity.control(spider.Id, Map.EnemyBase);
        //                commanded = true;
        //                break;
        //            }
        //            if (spider.EntityIsShielded == false && Map.shielding)
        //            {

        //               // Console.Error.WriteLine($"OFFENSE {Version} 009: Shielding {spider.Id}");
        //                Entity.shield(spider);
        //                Map.countingShielded++;
        //               // Console.Error.WriteLine($"OFFENSE INFO 010: Shields up {Map.countingShielded}");
        //                commanded = true;
        //                Map.shielding = true; //false
        //                break;
        //            }
        //            //if (spider.EntityIsShielded == false && !Map.shielding)
        //            //{
        //            //    //Console.Error.WriteLine($"OFFENSE {Version}: Winding {spider.Id}");
        //            //    if (Map.PointInCircle(myHeroes[Version], spider, 800))
        //            //        Entity.wind(Map.EnemyBase);
        //            //    else
        //            //    {
        //            //        //nearestOppHero = oppHeroes.OrderBy(x => x.DistanceToEnemyBase).ToList();
        //            //        bool opHeroNearHero = false;
        //            //        foreach (var opHero in nearestOppHero)
        //            //        {
        //            //            if (nearestOppHero.Count > 0 && Map.PointInCircle(myHeroes[0], opHero, 2200))
        //            //            {
        //            //                Entity.control(opHero.Id, Map.HomeBase);
        //            //                opHeroNearHero = true;
        //            //                break;
        //            //            }
        //            //        }
        //            //        if (opHeroNearHero == false)

        //            //            Entity.move(spider.Position);
        //            //    }
        //            //    commanded = true;
        //            //    Map.shielding = true;
        //            //    break;
        //            //}
        //            if (Map.PointInCircle(spider, myHeroes[Version], RADIUS_CONTROL) == true)
        //            {
        //               // Console.Error.WriteLine($"OFFENSE {Version} 011: Waiting because of {spider.Id}");
        //                //nearestOppHero = oppHeroes.OrderByDescending(x => x.DistanceToEnemyBase).FirstOrDefault();
        //                if (nearestOppHero.Count > 0) // && !spider.DirectionEnemyBase)
        //                    Entity.control(nearestOppHero[0].Id, Map.HomeBase);
        //                else
        //                    Entity.move(Map.posOffenseMiddle);
        //                commanded = true;
        //                break;
        //            }

        //        }
        //        if (commanded == false)
        //        {
        //           // Console.Error.WriteLine($"OFFENSE {Version} 012: Moving to Offense????????????");
        //            //nearestOppHero = oppHeroes.OrderByDescending(x => x.DistanceToEnemyBase).FirstOrDefault();
        //            if (nearestOppHero.Count > 0)
        //                Entity.control(nearestOppHero[0].Id, Map.HomeBase);
        //            else
        //            {
        //                //if (myHeroes[0].DistanceToEnemyBase < 4000)
        //                //{
        //                    //Map.setPosOffense();
        //                //}
        //                //else if (myHeroes[0].DistanceToEnemyBase > 7000)
        //                //{
        //                //   // Console.Error.WriteLine("OFFENSE: Moving back because nothing to do");
        //                //    Map.setPosOffense2();
        //                //}

        //                Entity.move(Map.posOffenseMiddle);
        //            }
        //        }
        //    }
        //}
    }

    internal void defense(List<Entity> Heroes, int action_ID, int hero_ID)
    {
        //if (Map.countingRounds >= 90)
        //    Map.setPosDefenseAlarm();

        if (action_ID == 3)
        {
           // Console.Error.WriteLine("------------------->>>> ACTION 3");


            

            if (Map.HomeBase.X == 0)
            {
                Heroes[hero_ID].targetSpidersDefenseOne = Heroes[hero_ID].targetSpidersDefenseAll.Where(x => x.DistanceToBase < 6000).ToList(); // && myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position)

                Heroes[hero_ID].targetSpidersDefenseTwo = Heroes[hero_ID].targetSpidersDefenseAll.Where(x => x.DistanceToBase < 6000).ToList(); // && !myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position)

            } 
            else
            {
                Heroes[hero_ID].targetSpidersDefenseOne = Heroes[hero_ID].targetSpidersDefenseAll.Where(x => x.DistanceToBase < 6000).ToList(); // !myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position))

                Heroes[hero_ID].targetSpidersDefenseTwo = Heroes[hero_ID].targetSpidersDefenseAll.Where(x => x.DistanceToBase < 6000).ToList(); //                    myMath.checkAboveLine(Map.HomeBase, Map.seperateMiddleLine, x.Position)).ToList();

                    
                
            }

        }

        bool warningControlled = Heroes[hero_ID].targetDefenseOppHero.Count > 0 ? true : false;
        
        bool targetDefenseOppHero = Heroes[hero_ID].targetDefenseOppHero.Count > 0 ? true : false;
        bool targetDefenseOppHeroIsNotShielded = Heroes[hero_ID].targetDefenseOppHeroIsNotShielded.Count > 0 ? true : false;
        bool targetDefenseOppHeroShielded = Heroes[hero_ID].targetDefenseOppHeroShielded.Count > 0 ? true : false;
        bool targetDefenseOppHeroAbove = Heroes[hero_ID].targetDefenseOppHeroAbove.Count > 0 ? true : false;
        bool targetDefenseOppHeroBelow = Heroes[hero_ID].targetDefenseOppHeroBelow.Count > 0 ? true : false;


        bool targetOffenseOppHero = Heroes[hero_ID].targetOffenseOppHero.Count > 0 ? true : false;
        bool targetOffenseOppHeroShielded = Heroes[hero_ID].targetDefenseOppHeroShielded.Count > 0 ? true : false;
        bool targetOffenseOppHeroIsNotShielded = Heroes[hero_ID].targetDefenseOppHeroIsNotShielded.Count > 0 ? true : false;

        bool targetSpidersAll = Heroes[hero_ID].targetSpidersDefenseAll.Count > 0 ? true : false;
        bool targetSpidersDefenseBase = Heroes[hero_ID].targetSpidersDefenseBase.Count > 0 ? true : false;
        bool targetSpidersDefenseBaseAlarm = Heroes[hero_ID].targetSpidersDefenseBaseAlarm.Count > 0 ? true : false;
        bool targetSpidersDefenseBaseWarning = Heroes[hero_ID].targetSpidersDefenseBaseWarning.Count > 0 ? true : false;
        
        bool targetSpiderIsInControllRadius = Heroes[hero_ID].targetSpiderIsInControllRadius.Count > 0 ? true : false;
        bool targetSpiderIsInWindRadius = Heroes[hero_ID].targetSpiderIsInWindRadius.Count > 0 ? true : false;

        bool targetSpidersDefenseOne = Heroes[hero_ID].targetSpidersDefenseOne.Count > 0 ? true : false;
        bool targetSpidersDefenseTwo = Heroes[hero_ID].targetSpidersDefenseTwo.Count > 0 ? true : false;
        bool targetSpidersDefenseThree = Heroes[hero_ID].targetSpidersDefenseThree.Count > 0 ? true : false;

        // If Opponent is in my Base Area -> shields up
        //if (warningControlled && !Heroes[hero_ID].EntityIsShielded && Heroes[hero_ID].enoughMana)
        //{
        //    Entity.shield(Heroes[hero_ID]);
        //    if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 001: SHIELDS UP HERO {hero_ID}!!");
        //    return;
        //}



        var tempDistance = 6500;
        if (action_ID == 3) tempDistance = 5000;

        var tempControlSpider = 6500;
        if (action_ID == 3) tempControlSpider = 8000;

        // I've got Spiders in my Base -> wind
        if (targetSpiderIsInWindRadius && Heroes[hero_ID].enoughMana) //targetSpidersDefenseBaseAlarm && 
        {
            foreach (var spider in Heroes[hero_ID].targetSpiderIsInWindRadius)
            {
                if (!spider.EntityIsShielded && (spider.DistanceToBase < tempDistance) && spider.Health > Player.HealthMaxDevided[spider.Id])
                {
                    if ((hero_ID == 0 && targetDefenseOppHeroAbove) || (hero_ID == 1 && targetDefenseOppHeroBelow) || (hero_ID == 2) || targetSpidersDefenseBaseAlarm)
                    {
                        Entity.wind(myMath.multiplyVectorByScalar(spider.Position));
                        if (Player.DebugDefense == true)  Console.Error.WriteLine($"DEFENSE 003: Hero {hero_ID} winds Spider {Heroes[hero_ID].targetSpiderIsInWindRadius[0].Id}");
                        return;
                    }

                }
            }
            if (Player.DebugDefense == true)  Console.Error.WriteLine($"DEFENSE 004: Hero {hero_ID} ->  No Spiders for wind");
        }

        // do something with the enemy in my base
        if (targetSpidersDefenseBaseAlarm && targetDefenseOppHeroIsNotShielded && targetDefenseOppHero && Heroes[2].enoughMana &&
            Map.PointInCircle(Heroes[2], Heroes[hero_ID].targetDefenseOppHero[0], RADIUS_WIND) && hero_ID == 2) //targetSpidersDefenseBaseAlarm && 
        {

            if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 002: Coming for Help");
            Entity.wind(Map.EnemyBase);
            return;
        }
        else if (targetSpidersDefenseBaseAlarm && targetDefenseOppHeroIsNotShielded && targetDefenseOppHero && Heroes[2].enoughMana &&
            !Map.PointInCircle(Heroes[2], Heroes[hero_ID].targetDefenseOppHero[0], RADIUS_CONTROL) && hero_ID == 2)
        {
            if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 002: Running for Help");
            Entity.move(Heroes[hero_ID].targetDefenseOppHero[0].Position);
            return;
        }

        // I've got Spiders in my Base -> control
        
        if (targetSpiderIsInControllRadius && Heroes[hero_ID].enoughMana) //targetSpidersDefenseBaseAlarm && 
        {
            foreach (var spider in Heroes[hero_ID].targetSpiderIsInControllRadius)
            {
                if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 003: Hero {hero_ID} could control Spider {Heroes[hero_ID].targetSpiderIsInControllRadius[0].Id}");
                if (!spider.EntityIsShielded  && spider.Health > Player.HealthMaxDevided[spider.Id] && Player.gotControlled[spider.Id] == false && action_ID == 3 && spider.DistanceToBase > 5500) //spider.DistanceToBase > 5500 && spider.DistanceToBase < tempControlSpider
                {
                    if ((hero_ID == 0 && targetDefenseOppHeroAbove) || (hero_ID == 1 && targetDefenseOppHeroBelow) || (hero_ID == 2) || targetSpidersDefenseBaseAlarm)
                    {
                        Entity.control(spider.Id, myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(14000, 7000)));
                        Player.gotControlled[spider.Id] = true;
                        return;
                    }
                    
                }
            }
            if (Player.DebugDefense == true)  Console.Error.WriteLine($"DEFENSE 004: Hero {hero_ID} -> No Spiders for control");
        }


        // ((spider.DistanceToBase < 6800 && spider.DistanceToBase > 6000)) ||





        if (targetSpidersDefenseBaseAlarm == true && hero_ID == 0)
        {
            Entity.move(Heroes[hero_ID].targetSpidersDefenseBaseAlarm[0].Position);
            return;
        }
        // I've got Spiders in my Area -> move
        switch (hero_ID)
        {
            case 0: //ACTION_DEFENSE_LEFT
                if (targetSpidersDefenseOne)
                {
                    if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 005: Hero: {hero_ID}, Defense One: Spiders found");
                    if (Player.DebugDefense == true)
                        foreach (var spider in Heroes[hero_ID].targetSpidersDefenseOne.OrderBy(x => x.DistanceToBase).ThenByDescending(x => x.ThreatFor == 1))
                        {
                           // Console.Error.WriteLine($"DEFENSE 006: Hero: {hero_ID} -> Targets {spider.Id}");
                        }

                    Entity.move(Heroes[hero_ID].targetSpidersDefenseOne[0].Position);
                    return;
                }
                break;
            case 1: //ACTION_DEFENSE_RIGHTheri
                if (targetSpidersDefenseTwo)
                {
                    if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 007: Hero: {hero_ID}, Defense Two: Spiders found");
                    if (Player.DebugDefense == true)
                        foreach (var spider in Heroes[hero_ID].targetSpidersDefenseTwo.OrderBy(x => x.DistanceToBase).ThenByDescending(x => x.ThreatFor == 1))
                        {
                           // Console.Error.WriteLine($"DEFENSE 008: Hero: {hero_ID} -> Targets {spider.Id}");
                            Entity.move(Heroes[hero_ID].targetSpidersDefenseTwo[0].Position);
                            return;
                        }
                }
                break;
            case 2: //ACTION_DEFENSE_MIDDLE
                //if (Map.countingRounds > 70 && (targetSpidersDefenseBaseAlarm))
                //{
                //    if (Player.DebugDefense == true) 
                //       // Console.Error.WriteLine($"DEFENSE 007: Hero: {hero_ID}, Defense Two: Spiders found");

                //    if (Player.DebugDefense == true)
                //        foreach (var spider in Heroes[hero_ID].targetSpidersDefenseBaseAlarm.OrderByDescending(x => x.ThreatFor == 1))
                //        {
                //           // Console.Error.WriteLine($"DEFENSE 008: Hero: {hero_ID} -> Targets {spider.Id}");
                //        }
                //    Entity.move(Heroes[hero_ID].targetSpidersDefenseBaseAlarm[0].Position);
                //    return;
                //}



                if (targetSpidersDefenseThree)
                {
                    if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 009: Hero: {hero_ID}, Defense Two: Spiders found");
                    if (Player.DebugDefense == true)
                        foreach (var spider in Heroes[hero_ID].targetSpidersDefenseThree.OrderBy(x => x.DistanceToBase).ThenByDescending(x => x.ThreatFor == 1))
                        {
                           // Console.Error.WriteLine($"DEFENSE 010: Hero: {hero_ID} -> Targets {spider.Id}");
                            Entity.move(spider.Position);
                            return;
                        }
                }
                break;
                
        }

        //// if no targets in my base -> move to defaultpos
        //if (!targetSpidersAll)
        //{
        if (Player.DebugDefense == true)// Console.Error.WriteLine($"DEFENSE 002: No Targets, Hero {hero_ID} moving to Pos {action_ID}");
        Entity.move(Map.posDefense[hero_ID]);
        return;
        //}

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
    internal static bool shielding = true;

    public static int countingShielded = 0;
    public static int countingRounds = 0;
    

    public static Vector2 seperateMiddleLine = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(10000, 9000));

    public static Vector2 seperateLeftThirdOneLineUp = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(18000, 7500));

    public static Vector2 seperateLeftThirdThreeLineDown = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(18000, 5000));
    public static Vector2 seperateLeftThirdThreeLineUp = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(5000, 9000));

    public static Vector2 seperateLeftThirdTwoDown = myMath.calculateCorrectPositionFrom(Map.HomeBase, new Vector2(7000, 9000));



    public static Vector2 seperateRightThirdTwoUp = new Vector2(8000, 0);

    public static Vector2 seperateRightThirdThreeLineDown = new Vector2(11000, 0);
    public static Vector2 seperateRightThirdThreeLineUp = new Vector2(0, 4500);

    public static Vector2 seperateRightThirdOneLineDown = new Vector2(0, 3000);


    internal static void setPosDefenses()
    {
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(6500, 800)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(2500, 6500)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(5500, 4500)));
        posDefenseMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(2000, 2000));
    }

    internal static void setPosDefenses2()
    {
        posDefense.Clear();
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(3500, 1500)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(2000, 3000)));
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(5500, 4500)));
        posDefenseMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(2000, 2000));
    }

    internal static void setPosDefenseAlarm()
    {
        posDefense.RemoveAt(2);
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(1500, 1500)));
    }

    internal static void setPosMiddle()
    {
        posMiddleMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(8500, 4500));
    }

    internal static void setPosOffense()
    {
        posOffenseMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(14500, 6500));
    }

    internal static void setPosOffense2()
    {
        posOffenseMiddle = myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(11500, 5000));
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

    internal static bool PointInCircle(Entity hero, Entity target, int radius)
    {
        //Console.Error.WriteLine($"INFO PointInCircle: Distance is {hero.DistanceToBase - target.DistanceToBase}");
        var dx = Math.Abs(hero.X - target.X); // hero has 13000 - 5000
        var dy = Math.Abs(hero.Y - target.Y); // enemy3 is 2811 - 2937
        var R = radius; // is 2200

        //Console.Error.WriteLine($"INFO pointInCircle: dx: {dx}, dy: {dy}, R:{R}");

        // 13000 - 2811 = 10189
        // 5000 - 2937 = 2063

        if (dx + dy <= R) return true; // 10189 + 2063 = 12252
        //Console.Error.WriteLine($"INFO pointInCircle: 2");
        if (dx > R) return false;
        //Console.Error.WriteLine($"INFO pointInCircle: 3");
        if (dy > R) return false;
        //Console.Error.WriteLine($"INFO pointInCircle: 4");
        if ((Math.Pow(dx, 2) + Math.Pow(dy, 2)) <= Math.Pow(R, 2)) return true;

        // 103.815.721 + 4.255.969 = 108.071.690 <> 4.840.000
        else
        {
            //Console.Error.WriteLine($"INFO pointInCircle: 5");
            return false;
        }

    }

    internal static void setPosDefensesAllLeft()
    {
        
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(6000, 2000)));
    }

    internal static void setPosDefensesAllRight()
    {
        posDefense.Add(myMath.calculateCorrectPositionFrom(HomeBase, new Vector2(3000, 5000)));
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
        //Console.Error.WriteLine($"calculateAwayfromHome: {positionTo.X} - {positionTo.Y}");
        return positionTo;
    }

    internal static Vector2 multiplyVectorByScalar(Vector2 Entity)
    {
        Vector2 vector1;

        if (Map.HomeBase.X == 0) 
            vector1 = Entity; //new Vector2(20, 30);
        else
            vector1 = Vector2.Subtract(Entity, Map.HomeBase);

       // Console.Error.WriteLine($"--------->> Vector for Scalar is now {vector1.X} : {vector1.Y}");
        float scalar1 = 10f;
        

        Vector2 vectorResult = new Vector2();

        // Multiply the vector by the scalar.
        // vectorResult is equal to (1500,2250)
        vectorResult = Vector2.Multiply(vector1, scalar1);

        return vectorResult;
    }
}

