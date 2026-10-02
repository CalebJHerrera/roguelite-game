namespace Game.Player.StateMachine;

using Godot;

public partial class StateMachine : LimboHsm
{
    LimboState groundState;
    LimboState jumpState;
    LimboState fallState;
    LimboState dashState;
    LimboState stunState;

    public override void _Ready()
    {
        groundState = GetNode<LimboState>("Ground");
        jumpState = GetNode<LimboState>("Jump");
        fallState = GetNode<LimboState>("Fall");
        dashState = GetNode<LimboState>("Dash");
        stunState = GetNode<LimboState>("Stun");

        InitializeHsm();
    }

    private void InitializeHsm()
    {
        // Add transitions
        AddTransition(groundState, jumpState, "jump_pressed");
        AddTransition(groundState, fallState, "fell_off_ledge");
        AddTransition(groundState, dashState, "dash_pressed");

        AddTransition(jumpState, fallState, "apex_reached");
        AddTransition(jumpState, dashState, "dash_pressed");

        AddTransition(fallState, groundState, "landed");
        AddTransition(fallState, jumpState, "coyote_failsafe_used");
        AddTransition(fallState, dashState, "dash_pressed");

        AddTransition(dashState, groundState, "dash_ended_ground");
        AddTransition(dashState, fallState, "dash_ended_air");

        AddTransition(ANYSTATE, stunState, "took_damage");

        AddTransition(stunState, groundState, "recovered_on_ground");
        AddTransition(fallState, groundState, "recovered_in_air");

        InitialState = groundState;
        Initialize(this);
        SetActive(true);
    }
}
