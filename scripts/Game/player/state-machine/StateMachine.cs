namespace Game.Player.StateMachine;

using Godot;

public partial class StateMachine : LimboHsm
{
    private LimboState GroundState { get; set; }
    private LimboState JumpState { get; set; }
    private LimboState FallState { get; set; }
    private LimboState DashState { get; set; }
    private LimboState StunState { get; set; }

    public override void _Ready()
    {
        GroundState = GetNode<LimboState>("Ground");
        JumpState = GetNode<LimboState>("Jump");
        FallState = GetNode<LimboState>("Fall");
        DashState = GetNode<LimboState>("Dash");
        StunState = GetNode<LimboState>("Stun");

        InitializeHsm();
    }

    private void InitializeHsm()
    {
        // Add transitions
        AddTransition(GroundState, JumpState, "jump_pressed");
        AddTransition(GroundState, FallState, "fell_off_ledge");
        AddTransition(GroundState, DashState, "dash_pressed");

        AddTransition(JumpState, FallState, "apex_reached");
        AddTransition(JumpState, DashState, "dash_pressed");
        AddTransition(JumpState, JumpState, "air_jump_used");

        AddTransition(FallState, GroundState, "landed");
        AddTransition(FallState, JumpState, "air_jump_used");
        AddTransition(FallState, DashState, "dash_pressed");

        AddTransition(DashState, GroundState, "dash_ended_ground");
        AddTransition(DashState, FallState, "dash_ended_air");

        AddTransition(ANYSTATE, StunState, "took_damage");

        AddTransition(StunState, GroundState, "recovered_on_ground");
        AddTransition(StunState, FallState, "recovered_in_air");

        InitialState = GroundState;
        Initialize(this);
        SetActive(true);
    }
}
