using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Game
    {
        bool _gameOver = false;
        string _playerName = "Hero";
        int _playerHealth = 120;
        int _playerDamage = 200000;
        int _playerDefense = 10;
        int levelScaleMax = 5;
        Random random;
        //Run the game
        public void Run()
        {
            random = new Random();
            int randomNumber = random.Next(0, 1);

            if (randomNumber == 0)
            {
                //murder player
            }
            else
            {
                //Heal plz player
            }

            Start();

            while (_gameOver == false)
            {
                Update();
            }

            End();

        }
        //This function handles the battles for our ladder. roomNum is used to update the our opponent to be the enemy in the current room. 
        //turnCount is used to keep track of how many turns it took the player to beat the enemy
        bool StartBattle(int roomNum, ref int turnCount)
        {
            //initialize default enemy stats
            int enemyHealth = 0;
            int enemyAttack = 0;
            int enemyDefense = 0;
            string enemyName = "";
            //Changes the enemy's default stats based on our current room number. 
            //This is how we make it seem as if the player is fighting different enemies
            switch (roomNum)
            {
                case 0:
                    {
                        enemyHealth = 100;
                        enemyAttack = 20;
                        enemyDefense = 5;
                        enemyName = "Wizard";
                        break;
                    }
                case 1:
                    {
                        enemyHealth = 80;
                        enemyAttack = 30;
                        enemyDefense = 5;
                        enemyName = "Troll";
                        break;
                    }
                case 2:
                    {

                        enemyHealth = 200;
                        enemyAttack = 40;
                        enemyDefense = 10;
                        enemyName = "Giant";
                        break;
                    }
            }

            //Loops until the player or the enemy is dead
            while (_playerHealth > 0 && enemyHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
                PrintStats(enemyName, enemyHealth, enemyAttack, enemyDefense);

                //Get input from the player
                char input;
                GetInput(out input, "Attack", "Defend");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if (input == '1')
                {
                    BlockAttack(ref enemyHealth, _playerDamage, enemyDefense);
                    Console.Clear();
                    Console.WriteLine("You dealt " + (_playerDamage - enemyDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();

                    //After the player attacks, the enemy takes its turn. Since the player decided not to defend, the block attack function is not called.
                    _playerHealth -= enemyAttack;
                    Console.WriteLine(enemyName + " dealt " + enemyAttack + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else
                {
                    BlockAttack(ref _playerHealth, enemyAttack, _playerDefense);
                    Console.WriteLine(enemyName + " dealt " + (enemyAttack - _playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                    Console.Clear();
                }


            }
            //Return whether or not our player died
            return _playerHealth != 0;

        }
        //Decrements the health of a character. The attack value is subtracted by that character's defense
        void BlockAttack(ref int opponentHealth, int attackVal, int opponentDefense)
        {
            int damage = attackVal - opponentDefense;
            if (damage < 0)
            {
                damage = 0;
            }
            opponentHealth -= damage;
        }
        //Scales up the player's stats based on the amount of turns it took in the last battle


        void UpgradeStats(int turnCount)
        {
            int scale = levelScaleMax - turnCount;
            if (scale <= 0)
            {
                scale = 1;
            }
            _playerHealth += 10 * scale;
            _playerDamage *= scale;
            _playerDefense *= scale;
        }
        //Gets input from the player
        //Out's the char variable given. This variables stores the player's input choice.
        //The parameters option1 and option 2 displays the players current chpices to the screen
        void GetInput(out char input, string option1, string option2, string query)
        {
            Console.WriteLine(query);
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }

        }

        void GetInput(out char input, string option1, string option2)
        {
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }
        }
        //Prints the stats given in the parameter list to the console
        void PrintStats(string name, int health, int damage, int defense)
        {
            Console.WriteLine("\n" + name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Damage: " + damage);
            Console.WriteLine("Defense: " + defense);
        }

        //This is used to progress through our game. A recursive function meant to switch the rooms and start the battles inside them.
        void ClimbLadder(int roomNum)
        {
            //Displays context based on which room the player is in
            switch (roomNum)
            {
                case 0:
                    {
                        Console.WriteLine("A disgraced wizard with aura of dark magic blocks your path");
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("Wizard was defeated but, he casted himself teleport to somewhere for fleeing from you.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("A random troll stands before you");
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Troll was trolled by you");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("A half-naked azetic giant stripper has appeared!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("You sent giant car to space.");
                        _gameOver = true;
                        return;
                    }
            }
            int turnCount = 0;
            //Starts a battle. If the player survived the battle, level them up and then proceed to the next room.
            if (StartBattle(roomNum, ref turnCount))
            {
                UpgradeStats(turnCount);
                ClimbLadder(roomNum + 1);
                Console.Clear();
            }
            _gameOver = true;

        }

        //Displays the character selection menu. 
        void SelectCharacter()
        {
            char input = ' ';
            //Loops until a valid option is choosen
            while (input != '1' && input != '2' && input != '3')
            {
                //Prints options
                Console.WriteLine("Welcome! Please select a character.");
                Console.WriteLine("1.Sir Kibble");
                Console.WriteLine("Sir Kibble is from noble house called Griffin for bodyguard of Royalty. Griffin's founder was legendary knight who");
                Console.WriteLine("protected his kingdom multiple and saved a lot Royalty's life. But, Griffin dislike politic and was exiled by greedy and arrogant Royalty");
                Console.WriteLine("");
                Console.WriteLine("2.Gnojoel");
                Console.WriteLine("Gnojoel is mage who bears blood magic from his family and running away from Templer, knight who kill or safeguard mage for fearing magic corruption. Gnojoel wasn't corrupted by blood magic. Gnojoel is such greedy mage. Not money. Not fame. Not glory. Just knowledge for sasifty his curiousity.");
                Console.WriteLine("3.Joedazz");
                Console.WriteLine("Joedazz's race is elves who were enslaved by human and now lower rank is commoner. Human looked down and treat elves as trash. Joedazz's bloodline was legendary Reavor, a warrior who devor blood and flesh for healing his flesh. Joedazz was declared crimenal by noble. because Joedazz killed noble's son. Joedazz killed noble's son for saving Joedazz's sister  because noble's son tried kill her for his pleasure. Joedazz has no place in this city and have no choice but leave city.");
                Console.WriteLine("");
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                //Sets the players default stats based on which character was picked
                switch (input)
                {
                    case '1':
                        {
                            _playerName = "Sir Kibble";
                            _playerHealth = 120;
                            _playerDefense = 10;
                            _playerDamage = 30;
                            break;
                        }
                    case '2':
                        {
                            _playerName = "Gnojoel";
                            _playerHealth = 70;
                            _playerDefense = 2;
                            _playerDamage = 80;
                            break;
                        }
                    case '3':
                        {
                            _playerName = "Joedazz";
                            _playerHealth = 200;
                            _playerDefense = 5;
                            _playerDamage = 25;
                            break;
                        }
                    //If an invalid input is selected display and input message and input over again.
                    default:
                        {
                            Console.WriteLine("Invalid input. Press any key to continue.");
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                }
                Console.Clear();
            }
            //Prints the stats of the choosen character to the screen before the game begins to give the player visual feedback
            PrintStats(_playerName, _playerHealth, _playerDamage, _playerDefense);
            Console.WriteLine("Press any key to continue.");
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }
        //Performed once when the game begins
        public void Start()
        {
            SelectCharacter();
        }

        //Repeated until the game ends
        public void Update()
        {
            ClimbLadder(0);
        }

        //Performed once when the game ends
        public void End()
        {
            //If the player died print death message
            if (_playerHealth <= 0)
            {
                Console.WriteLine("Failure");
                return;
            }
            //Print game over message
            Console.WriteLine("Congrats");
        }
    }
}
