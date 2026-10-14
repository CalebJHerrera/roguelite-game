using Godot;

namespace RogueliteGame.scripts.game.player.state_machine;

public partial class DashState : LimboState
{
    private MovementController _moveController;
    private float _dashTimer;
    private float _direction;

    public override void _Enter()
    {
        _moveController = Blackboard.GetVar("MovementController").As<MovementController>();
        _dashTimer = _moveController.DashDuration;
        
        var xDir = Input.GetAxis("MoveLeft", "MoveRight");
        _direction = xDir != 0 ? xDir : _moveController.GetDirection();

        _moveController.Dash(_direction);
    }

    public override void _PhysicsProcess(double delta)
    {
        var isOnFloor = _moveController.Actor.IsOnFloor();
        
        _moveController.Move();
        _dashTimer -= (float)delta;

        if (_dashTimer <= 0) { return; }

        Dispatch(isOnFloor ? "dash_ended_ground" : "dash_ended_air");
    }
}
