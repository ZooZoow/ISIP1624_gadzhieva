using System;
using System.Collections.Generic;

namespace TextRoguelike
{
    // Базовый класс для всех существ
    public abstract class Creature
    {
        public string Name { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public bool IsAlive => Health > 0;

        protected Creature(string name, int health, int attack, int defense)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public virtual void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }
    }

    // Класс игрока
    public class Player : Creature
    {
        public Weapon EquippedWeapon { get; private set; }
        public Armor EquippedArmor { get; private set; }
        public bool IsFrozen { get; set; }
        private Random random;

        public Player() : base("Игрок", 100, 10, 5)
        {
            EquippedWeapon = new Weapon("Кулаки", 0);
            EquippedArmor = new Armor("Одежда", 0);
            random = new Random();
        }

        public int GetTotalAttack()
        {
            return Attack + EquippedWeapon.AttackBonus;
        }

        public int GetTotalDefense()
        {
            return Defense + EquippedArmor.DefenseBonus;
        }

        public void EquipWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
        }

        public void EquipArmor(Armor armor)
        {
            EquippedArmor = armor;
        }

        public bool TryDodge()
        {
            return random.Next(100) < 40; // 40% шанс уклонения
        }

        public int CalculateBlockDamage(int incomingDamage)
        {
            double blockPercent = random.Next(70, 101) / 100.0; // 70-100% защиты
            return (int)(incomingDamage * (1 - blockPercent));
        }
    }

    // Базовый класс врагов
    public abstract class Enemy : Creature
    {
        protected Random random;

        protected Enemy(string name, int health, int attack, int defense) : base(name, health, attack, defense)
        {
            random = new Random();
        }

        public abstract int CalculateDamage(Player player);
        public abstract void ApplySpecialEffect(Player player);
    }

    // Конкретные типы врагов
    public class Goblin : Enemy
    {
        private double critChance = 0.2; // 20% шанс крита

        public Goblin() : base("Гоблин", 30, 8, 3) { }

        public override int CalculateDamage(Player player)
        {
            bool isCrit = random.NextDouble() < critChance;
            int damage = Attack;

            if (isCrit)
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine("Критический урон!");
            }

            return damage;
        }

        public override void ApplySpecialEffect(Player player)
        {
            // Гоблин не имеет специальных эффектов кроме крита
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 40, 7, 4) { }

        public override int CalculateDamage(Player player)
        {
            // Игнорирует защиту игрока
            return Attack;
        }

        public override void ApplySpecialEffect(Player player)
        {
            // Скелет не имеет дополнительных эффектов
        }
    }

    public class Mage : Enemy
    {
        private double freezeChance = 0.25; // 25% шанс заморозки

        public Mage() : base("Маг", 35, 9, 2) { }

        public override int CalculateDamage(Player player)
        {
            return Attack;
        }

        public override void ApplySpecialEffect(Player player)
        {
            if (random.NextDouble() < freezeChance)
            {
                player.IsFrozen = true;
                Console.WriteLine("Маг заморозил вас! Вы пропустите следующий ход.");
            }
        }
    }

    // Классы боссов
    public class VVG : Goblin
    {
        public VVG() : base()
        {
            Name = "ВВГ (Босс Гоблин)";
            MaxHealth = (int)(MaxHealth * 2.0);
            Health = MaxHealth;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
            // critChance уже увеличен в базовом классе
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base()
        {
            Name = "Ковальский (Босс Скелет)";
            MaxHealth = (int)(MaxHealth * 2.5);
            Health = MaxHealth;
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }

    public class ArchmageCPP : Mage
    {
        public ArchmageCPP() : base()
        {
            Name = "Архимаг C++ (Босс Маг)";
            MaxHealth = (int)(MaxHealth * 1.8);
            Health = MaxHealth;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
            // freezeChance уже увеличен
        }
    }

    public class PestovC : Skeleton
    {
        private double freezeChance = 0.4; // 40% шанс заморозки

        public PestovC() : base()
        {
            Name = "Пестов С-- (Босс Скелет)";
            MaxHealth = (int)(MaxHealth * 1.3);
            Health = MaxHealth;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }

        public override int CalculateDamage(Player player)
        {
            return Attack; // Игнорирует защиту
        }

        public override void ApplySpecialEffect(Player player)
        {
            if (random.NextDouble() < freezeChance)
            {
                player.IsFrozen = true;
                Console.WriteLine("Пестов заморозил вас! Вы пропустите следующий ход.");
            }
        }
    }

    // Классы предметов
    public class Item
    {
        public string Name { get; protected set; }

        public Item(string name)
        {
            Name = name;
        }
    }

    public class Weapon : Item
    {
        public int AttackBonus { get; private set; }

        public Weapon(string name, int attackBonus) : base(name)
        {
            AttackBonus = attackBonus;
        }
    }

    public class Armor : Item
    {
        public int DefenseBonus { get; private set; }

        public Armor(string name, int defenseBonus) : base(name)
        {
            DefenseBonus = defenseBonus;
        }
    }

    public class HealthPotion : Item
    {
        public HealthPotion() : base("Лечебное зелье") { }
    }

    // Основной класс игры
    public class Game
    {
        private Player player;
        private Random random;
        private int turnCount;

        // Списки возможных врагов и предметов
        private List<Func<Enemy>> enemyTypes;
        private List<Func<Enemy>> bossTypes;
        private List<Func<Item>> itemTypes;

        public Game()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;

            InitializeEnemyTypes();
            InitializeItemTypes();
        }

        private void InitializeEnemyTypes()
        {
            enemyTypes = new List<Func<Enemy>>
            {
                () => new Goblin(),
                () => new Skeleton(),
                () => new Mage()
            };

            bossTypes = new List<Func<Enemy>>
            {
                () => new VVG(),
                () => new Kovalsky(),
                () => new ArchmageCPP(),
                () => new PestovC()
            };
        }

        private void InitializeItemTypes()
        {
            itemTypes = new List<Func<Item>>
            {
                () => new HealthPotion(),
                () => new Weapon("Меч", 5),
                () => new Weapon("Топор", 8),
                () => new Weapon("Посох", 3),
                () => new Armor("Кожаная броня", 3),
                () => new Armor("Кольчуга", 6),
                () => new Armor("Латная броня", 10)
            };
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в текстовый рогалик!");
            Console.WriteLine("Каждый ход вы будете встречать либо сундук, либо врага.");
            Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!\n");

            while (player.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\n=== Ход {turnCount} ===");
                Console.WriteLine($"Здоровье: {player.Health}/{player.MaxHealth}");
                Console.WriteLine($"Оружие: {player.EquippedWeapon.Name} (+{player.EquippedWeapon.AttackBonus} атаки)");
                Console.WriteLine($"Броня: {player.EquippedArmor.Name} (+{player.EquippedArmor.DefenseBonus} защиты)");

                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                    continue;
                }

                // 50/50 шанс встретить сундук или врага
                if (random.Next(2) == 0)
                {
                    EncounterChest();
                }
                else
                {
                    // Каждые 10 ходов - босс
                    if (turnCount % 10 == 0)
                    {
                        EncounterBoss();
                    }
                    else
                    {
                        EncounterEnemy();
                    }
                }

                if (player.IsAlive)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("\nИгра окончена! Вы пали в бою.");
            Console.WriteLine($"Вы продержались {turnCount} ходов.");
        }

        private void EncounterEnemy()
        {
            Enemy enemy = enemyTypes[random.Next(enemyTypes.Count)]();
            Console.WriteLine($"\nВы встретили {enemy.Name}!");
            StartCombat(enemy);
        }

        private void EncounterBoss()
        {
            Enemy boss = bossTypes[random.Next(bossTypes.Count)]();
            Console.WriteLine($"\n⚠️  ВНИМАНИЕ! Появился босс - {boss.Name}! ⚠️");
            StartCombat(boss);
        }

        private void EncounterChest()
        {
            Console.WriteLine("\nВы нашли сундук!");
            Item item = itemTypes[random.Next(itemTypes.Count)]();

            if (item is HealthPotion)
            {
                Console.WriteLine("В сундуке лечебное зелье! Вы полностью исцелены.");
                player.Heal(player.MaxHealth);
            }
            else if (item is Weapon newWeapon)
            {
                Console.WriteLine($"В сундуке оружие: {newWeapon.Name} (+{newWeapon.AttackBonus} атаки)");
                Console.WriteLine($"Ваше текущее оружие: {player.EquippedWeapon.Name} (+{player.EquippedWeapon.AttackBonus} атаки)");
                Console.Write("Заменить оружие? (д/н): ");

                if (Console.ReadLine().ToLower() == "д")
                {
                    player.EquipWeapon(newWeapon);
                    Console.WriteLine("Оружие заменено!");
                }
                else
                {
                    Console.WriteLine("Вы оставили оружие в сундуке.");
                }
            }
            else if (item is Armor newArmor)
            {
                Console.WriteLine($"В сундуке броня: {newArmor.Name} (+{newArmor.DefenseBonus} защиты)");
                Console.WriteLine($"Ваша текущая броня: {player.EquippedArmor.Name} (+{player.EquippedArmor.DefenseBonus} защиты)");
                Console.Write("Заменить броню? (д/н): ");

                if (Console.ReadLine().ToLower() == "д")
                {
                    player.EquipArmor(newArmor);
                    Console.WriteLine("Броня заменена!");
                }
                else
                {
                    Console.WriteLine("Вы оставили броню в сундуке.");
                }
            }
        }

        private void StartCombat(Enemy enemy)
        {
            Console.WriteLine($"\n=== БОЙ С {enemy.Name.ToUpper()} ===");
            Console.WriteLine($"Здоровье врага: {enemy.Health}");

            while (player.IsAlive && enemy.IsAlive)
            {
                // Ход игрока
                PlayerTurn(enemy);

                if (!enemy.IsAlive) break;

                // Ход врага
                EnemyTurn(enemy);
            }

            if (!enemy.IsAlive)
            {
                Console.WriteLine($"\nВы победили {enemy.Name}!");
            }
        }

        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ваш ход ---");
            Console.WriteLine("1 - Атаковать");
            Console.WriteLine("2 - Защищаться");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                int damage = player.GetTotalAttack();
                enemy.TakeDamage(damage);
                Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Вы приготовились к защите...");
                // Защита активируется при следующей атаке врага
            }
            else
            {
                Console.WriteLine("Неверный выбор, вы пропускаете ход.");
            }

            Console.WriteLine($"Здоровье врага: {enemy.Health}");
        }

        private void EnemyTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ход врага ---");

            // Проверяем, защищается ли игрок
            bool isDefending = false;
            Console.Write("Защищаться в этом раунде? (д/н): ");
            string defenseChoice = Console.ReadLine();
            isDefending = defenseChoice.ToLower() == "д";

            if (isDefending)
            {
                if (player.TryDodge())
                {
                    Console.WriteLine("Вы успешно уклонились от атаки!");
                    return;
                }
                else
                {
                    Console.WriteLine("Уклонение не удалось! Срабатывает блок.");
                }
            }

            int baseDamage = enemy.CalculateDamage(player);
            int finalDamage = baseDamage;

            if (isDefending)
            {
                finalDamage = player.CalculateBlockDamage(baseDamage);
                Console.WriteLine($"Вы заблокировали {baseDamage - finalDamage} урона!");
            }
            else if (enemy is Skeleton)
            {
                Console.WriteLine("Скелет игнорирует вашу защиту!");
            }
            else
            {
                // Учитываем защиту игрока для обычных атак
                finalDamage = Math.Max(1, baseDamage - player.GetTotalDefense());
            }

            player.TakeDamage(finalDamage);
            Console.WriteLine($"{enemy.Name} наносит вам {finalDamage} урона!");
            Console.WriteLine($"Ваше здоровье: {player.Health}/{player.MaxHealth}");

            // Применяем специальные эффекты врага
            enemy.ApplySpecialEffect(player);
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Game game = new Game();
            game.Start();
        }
    }
}