namespace Scripts.Player.StateMachine;

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
        AddTransition(groundHsm, airHsm, groundHsm.TO_AIR);
        AddTransition(groundHsm, dash, groundHsm.DASH);

        AddTransition(airHsm, groundHsm, airHsm.TO_GROUND);
        AddTransition(airHsm, dash, airHsm.DASH);

        AddTransition(dash, groundHsm, dash.TO_GROUND);
        AddTransition(dash, airHsm, dash.TO_AIR);

        InitialState = groundHsm;
        Initialize(this);
        SetActive(true);
    }
}
