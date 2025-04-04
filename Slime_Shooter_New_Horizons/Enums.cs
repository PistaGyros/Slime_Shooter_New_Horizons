namespace Slime_Shooter_New_Horizons;

public enum PlayerOrientation
{
    Right,
    Up,
    Left,
    Down
}

public enum PlayerStatus
{
    Idle,
    Walking,
    Throwing,
    Vacuuming
}

public enum SlimeOrientation
{
    Left,
    Right,
}

public enum SlimeStatus
{
    Idle,
    BeingVacuumed,
    Jumping
}

public enum PlotMenu
{
    Start,
    Upgrades
}

public enum PlotTypes
{
    Empty,
    Corral,
    Garden,
    Pond,
    Storage
}

public enum CorralUpgrades
{
    None,
    AutoFeeder,
    AutoCollector,
    SunProtection
}

public enum GardenUpgrades
{
    None,
    Sprinkler,
    Fertilizer,
    AutoPicker
}