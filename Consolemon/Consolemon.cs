namespace Consolemon;

/// <summary>
/// The Consolemon class tracks all the properties such as species and type for all Consolemon.
/// </summary>
public class Consolemon
{
    /// <summary>
    /// The nickname of the Consolemon. <br/>
    /// At creation, it is the same as the Species. <br/>
    /// When captured, the player is asked if they would like to give the captured Consolemon a nickname. <br/>
    /// If so, the nickname will be changed.
    /// </summary>
    public string Nickname { get; set; }
    
    /// <summary>
    /// The species of Consolemon.
    /// </summary>
    public string ConsolemonSpecies { get; private set; }
    
    /// <summary>
    /// The Type of Consolemon.
    /// </summary>
    public string Type { get; private set; }
    
    /// <summary>
    /// The Encounter Rate of the Consolemon.
    /// </summary>
    public int EncounterRate { get; private set; }
    
    /// <summary>
    /// The Catch Chance of the Consolemon; used to calculate if the player can capture the Consolemon or if it breaks out.
    /// </summary>
    public int CatchChance { get; private set; }

    /// <summary>
    /// The Constructor method of the Consolemon class.
    /// </summary>
    /// <param name="consolemonSpecies">A species of Consolemon.</param>
    /// <param name="type">A type of Consolemon.</param>
    /// <param name="encounterRate">The Encounter Rate of a Consolemon.</param>
    /// <param name="catchChance">The Catch Chance of a Consolemon.</param>
    public Consolemon(string consolemonSpecies, string type, int encounterRate, int catchChance)
    {
        Nickname = consolemonSpecies;
        ConsolemonSpecies = consolemonSpecies;
        Type = type;
        EncounterRate = encounterRate;
        CatchChance = catchChance;
    }
}