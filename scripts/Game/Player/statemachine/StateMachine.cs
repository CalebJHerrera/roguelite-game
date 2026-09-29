namespace Game.Player.StateMachine;

using Godot;

public partial class Hsm : LimboHsm
{
    private LimboHsm groundHsm;
    private LimboHsm airHsm;

    private LimboState dash;

    public override void _Ready()
    {
        groundHsm = GetNode<LimboHsm>("GroundHSM");
        airHsm = GetNode<LimboHsm>("AirHSM");

        dash = GetNode<LimboState>("Dash");

        InitializeHsm();
    }

    private void InitializeHsm()
    {
        AddTransition(groundHsm, airHsm, "GROUND_TO_AIR");
        AddTransition(groundHsm, dash, "GROUND_TO_DASH");

        AddTransition(airHsm, groundHsm, "AIR_TO_GROUND");
        AddTransition(airHsm, dash, "AIR_TO_DASH");

        AddTransition(dash, groundHsm, "DASH_TO_GROUND");
        AddTransition(dash, airHsm, "DASH_TO_AIR");

        InitialState = groundHsm;
        Initialize(this);
        SetActive(true);
    }
}
