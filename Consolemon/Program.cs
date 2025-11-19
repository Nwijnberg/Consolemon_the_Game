using System.Text.Json;

namespace Consolemon;

public class Program
{
    private const string CONSOLEMON_STORAGE_FILE = "../../../consolemon-storage.txt";
    private const string CONSOLEMON_LIST_FILE = "../../../consolemon-list.txt";

    private static List<Consolemon> CONSOLEMON_LIST = new();
    private static List<Consolemon> CONSOLEMON_STORAGE = new();
    private static readonly string[] ACTIONS = ["Fight", "Capture", "Consolemon", "Run"];

    public static void Main(string[] args)
    {
        // Configuration
        CONSOLEMON_STORAGE = GetConsolemonStorage();
        CONSOLEMON_LIST = GetConsolemonList();
        bool encounterFinished;

        // Game loop.
        while (true)
        {
            encounterFinished = false;

            // Explore fase
            Console.Clear();
            Console.WriteLine("You are exploring the wilds");
            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("You are exploring the wilds.");
            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("You are exploring the wilds..");
            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("You are exploring the wilds...");
            Thread.Sleep(1000);

            // Pick a random Consolemon to encounter.
            Consolemon encounteredConsolemon = GetEncounteredConsolemon();
            string encounteredConsolemonSprite =
                File.ReadAllText($"../../../Sprites/{encounteredConsolemon.ConsolemonSpecies}.txt");

            // Start encounter.
            while (!encounterFinished)
            {
                Console.Clear();

                // A Consolemon appears!
                Console.WriteLine(
                    $"""

                     You encounter a wild Consolemon!
                     {encounteredConsolemon.Nickname} appears!

                     {encounteredConsolemonSprite}

                     """
                );

                WriteActionBar();

                // Get the action the player chooses to do: Fight, Capture, Show their Consolemon, Run.
                string chosenAction = CheckActionBarInput();

                if (chosenAction == "Fight")
                {
                    // If the player has no consolemon, they cannot fight.
                    if (!CONSOLEMON_STORAGE.Any())
                    {
                        Console.WriteLine("You have no Consolemon to fight with!");
                        Console.WriteLine("Try another option!");
                    }
                    // if the player has a Consolemon, the fight begins.
                    else
                    {
                        Console.WriteLine($"You fight the {encounteredConsolemon.Nickname}!");
                        Console.WriteLine("The 'Fight' scene is not yet implemented!");

                        // After the fight is decided, the encounter ends.
                        encounterFinished = true;
                    }
                }

                if (chosenAction == "Capture")
                {
                    Console.WriteLine($"You try to capture the {encounteredConsolemon.Nickname}!");

                    encounterFinished = CaptureConsolemon(encounteredConsolemon, encounteredConsolemonSprite);
                }

                if (chosenAction == "Consolemon")
                {
                    // If the player has no Consolemon, the player has to choose another action.
                    if (!CONSOLEMON_STORAGE.Any())
                    {
                        Console.WriteLine("You have no Consolemon!");
                        Console.WriteLine("Try another option!");
                    }
                    // If the player has Consolemon, show the full list with information.
                    else
                    {
                        WriteConsolemonStorage(CONSOLEMON_STORAGE);
                    }
                }

                if (chosenAction == "Run")
                {
                    Console.WriteLine($"You run away from the {encounteredConsolemon.Nickname}!");

                    // If the player chooses to run, the encounter ends.
                    encounterFinished = true;
                }

                Thread.Sleep(5000);
            }

            Thread.Sleep(5000);
        }
    }

    /// <summary>
    /// This function writes the Action Bar to the Console.
    /// </summary>
    public static void WriteActionBar()
    {
        // The length of the action bar is dynamic, based on the length and number of the actions.
        // There are six characters around each action.
        int availableActionsStringLength = ACTIONS.Length * 6;
        foreach (string action in ACTIONS)
        {
            availableActionsStringLength += action.Length;
        }

        // Write the top border of the action bar.
        for (int i = 0; i < availableActionsStringLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();

        // Write all actions to the action bar.
        foreach (string action in ACTIONS)
        {
            Console.Write($"|  {action}  |");
        }

        Console.WriteLine();

        // Write the bottom border of the action bar.
        for (int i = 0; i < availableActionsStringLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// This function writes the full list of the player's captured Consolemon to the console.
    /// </summary>
    /// <param name="caughtConsolemon">A list of the player's captured Consolemon.</param>
    public static void WriteConsolemonStorage(List<Consolemon> caughtConsolemon)
    {
        string title = "Your Consolemon Storage";

        // The Consolemon Storage has a dynamic size based on the longest Consolemon storage.
        int boxLength = 0;
        int longestConsolemonTotalDescription = 0; // The total description 

        foreach (Consolemon consolemon in caughtConsolemon)
        {
            int consolemonTotalDescription = consolemon.Nickname.Length + consolemon.ConsolemonSpecies.Length +
                                             consolemon.Type.Length;
            if (consolemonTotalDescription > longestConsolemonTotalDescription)
            {
                longestConsolemonTotalDescription = consolemonTotalDescription;
            }
        }

        // Add the There are six characters around each information point (nickname, consolemonspecies, type)
        boxLength += longestConsolemonTotalDescription;
        boxLength += 6 * 3;

        int spaceAroundTitle = (boxLength - title.Length) / 2;

        // --- Write the Title ---

        // Write title border below.
        for (int i = 0; i < boxLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();

        // Space before title.
        for (int i = 0; i < spaceAroundTitle; i++)
        {
            Console.Write(" ");
        }

        // Title.
        Console.Write(title);

        // Space after title.
        for (int i = 0; i < spaceAroundTitle; i++)
        {
            Console.Write(" ");
        }

        Console.WriteLine();

        // Write title border below.
        for (int i = 0; i < boxLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();

        // --- Write all Consolemon in Storage. ---

        // Write consolemon box border above.
        for (int i = 0; i < boxLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();

        // Write consolemon information.
        foreach (Consolemon consolemon in caughtConsolemon)
        {
            Console.WriteLine(
                $"|  {consolemon.Nickname}  ||  {consolemon.ConsolemonSpecies}  ||  {consolemon.Type}  |");
        }

        // Write consolemon box border below.
        for (int i = 0; i < boxLength; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// This function checks the input of the user on the action bar.
    /// </summary>
    /// <returns>The action the user chose from the action bar.</returns>
    public static string CheckActionBarInput()
    {
        // Ask and fetch the input form the console.
        string? input = Console.ReadLine();
        string chosenAction = string.Empty;

        // Get the range in which the use can enter to choose each action, starting at 0 and ending at the last action + 6.
        // 6 being the standard space around each action in the action bar.
        List<int> actionRanges = new List<int>();

        foreach (string action in ACTIONS)
        {
            actionRanges.Add(6 + action.Length);
        }

        if (input != null)
        {
            int inputLocation = input.Length;
            int actionCheckLineIndex = 0;

            // Iterate through all the possible actions until you find which one the player entered.
            for (int actionIndex = 0; actionIndex < ACTIONS.Length; actionIndex++)
            {
                // If the action is found, break the iteration and return the action.
                if (inputLocation >= actionCheckLineIndex &&
                    inputLocation < (actionCheckLineIndex + actionRanges[actionIndex]))
                {
                    chosenAction = ACTIONS[actionIndex];
                    break;
                }

                actionCheckLineIndex += actionRanges[actionIndex];
            }
        }

        // If there was input, put a line between the resulting action output.
        if (!string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine();
        }

        // Return the player's chosen action.
        return chosenAction;
    }

    /// <summary>
    /// This function checks if the user wants to nickname their captured Consolemon.
    /// </summary>
    /// <param name="consolemon">A recently captured Consolemon.</param>
    /// <returns>Returns the recently captured Consolemon</returns>
    public static Consolemon CheckNickname(Consolemon consolemon)
    {
        Console.WriteLine($"Do you want to give your {consolemon.ConsolemonSpecies} a Nickname? Y/N");
        string nicknameAnswer = Console.ReadLine();

        // If the player does not want to nickname their captured Consolemon, return it without Nickname.
        if (string.Equals(nicknameAnswer, "N", StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals(nicknameAnswer, "No", StringComparison.CurrentCultureIgnoreCase))
        {
            // Since the nickname is used to show the consolemon in the Fighting and Storage actions, set its nickname to its species.
            consolemon.Nickname = consolemon.ConsolemonSpecies;
            return consolemon;
        }

        // The player wants to nickname their Consolemon, hooray!
        // Check what the nickname is.
        Console.WriteLine($"Please write the Nickname for your {consolemon.ConsolemonSpecies}:");
        string nickname = Console.ReadLine();

        // The user needs to fill in a nickname or say "No".
        if (string.IsNullOrWhiteSpace(nickname))
        {
            Console.WriteLine("A empty nickname is not valid. Please try again.");
            CheckNickname(consolemon);
            return consolemon;
        }

        // Set the chosen nickname.
        consolemon.Nickname = nickname;

        // Return the captured Consolemon with itsC new nickname.
        return consolemon;
    }

    /// <summary>
    /// This function handles the throwing and capturing with the Consoleball.
    /// </summary>
    /// <param name="encounteredConsolemon">An encountered Consolemon the player is trying to capture.</param>
    /// <param name="encounteredConsolemonSprite">The sprite of an encountered Consolemon the player is trying to capture.</param>
    /// <returns>Returns whether the Consolemon was captured or not.</returns>
    public static bool CaptureConsolemon(Consolemon encounteredConsolemon, string encounteredConsolemonSprite)
    {
        Random captureDecider = new Random();

        // After the first tick, there is a chance the Consolemon will break free.
        // Calculate if the capture was successful by randomly getting a percentage and comparing if it is below (success) or above (failure) the Consolemon's catch chance.
        // Also decide if the Consolemon breaks out on the second or third tick.
        var test = captureDecider.Next(0, 100);
        bool succesfulCapture = test < encounteredConsolemon.CatchChance;
        int breakOutOnTick = captureDecider.Next(1, 2);

        // The capture is in 3 parts.
        for (int timesTicked = 0; timesTicked < 3; timesTicked++)
        {
            Console.Clear();

            if (!succesfulCapture && timesTicked == breakOutOnTick)
            {
                Console.WriteLine(
                    $"""
                     
                                   x       
                             X  ---
                               |-X-|  X
                            x   ---
                            
                     """);

                Thread.Sleep(500);
                Console.Clear();
                
                Console.WriteLine(
                    $"""
                     x                          X                   
                     {encounteredConsolemonSprite}
                     X          x          X
                     Oh no! {encounteredConsolemon.ConsolemonSpecies} broke free!
                     """
                );
                
                Thread.Sleep(500);
                
                return false;
            }

            Console.Clear();
            Console.WriteLine(
                $"""

                            ---
                           |-{timesTicked}-|
                            ---

                 """
            );

            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine(
                $"""

                             ---
                           /-{timesTicked}-/
                           ---

                 """
            );

            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine(
                $"""

                            ---
                           |-{timesTicked}-|
                            ---

                 """
            );

            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine(
                $"""

                           ---
                           \-{timesTicked}-\
                             ---

                 """
            );

            Thread.Sleep(500);
            Console.Clear();

            if ((timesTicked + 1) < 3)
            {
                Console.WriteLine(
                    $"""

                                ---
                               |-{timesTicked + 1}-|
                                ---

                     """
                );
            }
            // If the ball ticked three times, the capture is complete!
            else
            {
                Console.WriteLine(
                    """
                                  *      
                            *  ---
                              |-☆-|  *
                           *   ---

                    """
                );
                Thread.Sleep(500);

                Console.WriteLine($"You captured the {encounteredConsolemon.ConsolemonSpecies}!");

                // Check for a nickname.
                encounteredConsolemon = CheckNickname(encounteredConsolemon);

                // Send the Consolemon to storage and update the local storage.
                CONSOLEMON_STORAGE.Add(encounteredConsolemon);
                UpdateConsolemonStorage();

                Console.WriteLine($"{encounteredConsolemon.Nickname} was sent to the Box!");

                // Now the Consolemon is captured, the encounter ends.
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  This function returns a random Consolemon that was encountered based off an encounter grid.
    /// </summary>
    /// <returns>Returns a random Consolemon that was encountered.</returns>
    public static Consolemon GetEncounteredConsolemon()
    {
        // Pick a random Consolemon to encounter.
        Random consolemonPicker = new Random();
            
        //// Create a grid of all Consolemon the player can encounter.
        //// Each Consolemon will be in the encounter grid as many times as its Encounter Rate (an encounter rate of 3 means 3 instances in the array).
        Consolemon[] consolemonEncounterGrid = new Consolemon[100];
        int consolemonEncounterGridIndex = 0;
            
        //// Add the Consolemon to the encounter grid as many times as its Encounter Rate.
        foreach (Consolemon consolemonInList in CONSOLEMON_LIST)
        {
            for (int i = 0; i < consolemonInList.EncounterRate; i++)
            {
                consolemonEncounterGrid[consolemonEncounterGridIndex] = consolemonInList;
                consolemonEncounterGridIndex++;
            }
        }
            
        //// Pick a random Consolemon out of the consolemon encounter grid.
       return consolemonEncounterGrid[consolemonPicker.Next(0, consolemonEncounterGrid.Length)];
    }

    /// <summary>
    /// This function updates the local Consolemon storage of the player.
    /// This storage is currently an unencrypted text file.
    /// </summary>
    public static void UpdateConsolemonStorage()
    {
        string updatedCaughtConselomonJson = JsonSerializer.Serialize(CONSOLEMON_STORAGE);
        File.WriteAllText(CONSOLEMON_STORAGE_FILE, updatedCaughtConselomonJson);
    }

    /// <summary>
    /// This function fetches the local Consolemon storage of the player.
    /// This storage is currently an unencrypted text file.
    /// </summary>
    /// <returns>Return the local Consolemon storage of the player.</returns>
    public static List<Consolemon> GetConsolemonStorage()
    {
        if (File.Exists(CONSOLEMON_STORAGE_FILE))
        {
            return JsonSerializer.Deserialize<List<Consolemon>>(File.ReadAllText(CONSOLEMON_STORAGE_FILE))!;
        }

        return new();
    }

    /// <summary>
    /// This function updates the list of Consolemon in the game.
    /// This storage is currently an unencrypted text file.
    /// </summary>
    public static void UpdateConsolemonList()
    {
        string updatedCaughtConselomonJson = JsonSerializer.Serialize(CONSOLEMON_LIST);
        File.WriteAllText(CONSOLEMON_LIST_FILE, updatedCaughtConselomonJson);
    }

    /// <summary>
    /// This function fetches the list of Consolemon in the game.
    /// This storage is currently an unencrypted text file.
    /// </summary>
    /// <returns>Return the list of Consolemon in the game.</returns>
    public static List<Consolemon> GetConsolemonList()
    {
        if (File.Exists(CONSOLEMON_LIST_FILE))
        {
            return JsonSerializer.Deserialize<List<Consolemon>>(File.ReadAllText(CONSOLEMON_LIST_FILE))!;
        }

        return new();
    }
}