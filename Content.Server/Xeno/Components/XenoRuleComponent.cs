namespace Content.Server.Xeno.Components;

[RegisterComponent]
public sealed partial class XenoRuleComponent : Component
{
    /// <summary>
    ///     The minimum needed amount of players.
    ///     I added this stupid restriction because of the game rules for killing sides,
    ///     this is literally the minimum threshold required for correct play,
    ///     otherwise the round will end instantly.
    /// </summary>
    [DataField("minPlayers")]
    public int MinPlayers = 2;

    [DataField("winningXenoEggCount")]
    public int WinningXenoEggCount = 50;

    [DataField("winType")]
    public WinType WinType = WinType.Neutral;

    [DataField("winConditions")]
    public List<WinCondition> WinConditions = new();

    // TODO: use components, don't just cache entity UIDs
    // There have been (and probably still are) bugs where these refer to deleted entities from old rounds.
    public EntityUid? MarineOutpost;
    public EntityUid? XenoPlanet;
}

public enum WinType : byte
{
    /// <summary>
    ///     All xenomorphs destroyed.
    /// </summary>
    MarineMinor,

    /// <summary>
    ///     Nuclear warhead on a world with a grid planet destroyed.
    /// </summary>
    MarineMajor,

    /// <summary>
    ///     Nuclear warhead destroyed in a world with Valkyrie grid.
    /// </summary>
    Neutral,

    /// <summary>
    ///     There are more than 50 xeno eggs.
    /// </summary>
    XenoMinor,

    /// <summary>
    ///     All Marines destroyed.
    /// </summary>
    XenoMajor,
}

public enum WinCondition : byte
{
    NukeExplodedOnXenoPlanet,
    NukeExplodedOnMarineOutpost,
    NukeExplodedOnIncorrectLocation,
    AllXenoDied,
    AllMarineDied,
    EggsWinningCount,
    XenoAbandoned,
}
