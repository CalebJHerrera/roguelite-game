namespace Game.Player.StateMachine;

using Godot;

public partial class DashState : LimboState
{
    private MovementController MC { get; set; }
    private float _dashTimer;
    private float _direction;

    public override void _Enter()
    {
        MC = Blackboard.GetVar("MovementController").As<MovementController>();
        float xDir = Input.GetAxis("MoveLeft", "MoveRight");
        _dashTimer = MC.DashDuration;

        if (xDir != 0)
        {
            _direction = xDir;
        }
        else
        {
            _direction = MC.GetDirection();
        }

        MC.Dash(_direction);
    }

    public override void _PhysicsProcess(double delta)
    {
        _dashTimer -= (float)delta;

        MC.Move();

        if (_dashTimer <= 0)
        {
            if (MC.Actor.IsOnFloor())
            {
                Dispatch("dash_ended_ground");
            }
            else
            {
                Dispatch("dash_ended_air");
            }
        }
    }
}
