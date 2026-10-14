using Godot;

namespace RogueliteGame.scripts.game;

// In case I forget, the player leaf states / AI BT node call the functions inside here. No logic is to be handled in this file, logic is actually in the states/tree nodes.
// The only stuff in here is the modification of the velocities and some other random weird platformer stuff.
[GlobalClass]
public partial class MovementController : Node
{
    [ExportGroup("Actor")]
    [Export] public CharacterBody2D Actor { get; private set; }

    [ExportGroup("Physics")]
    [Export] public int BaseSpeed { get; set; } = 200;
    [Export] public float Acceleration { get; set; } = 35.0f;
    [Export] public float Friction { get; set; } = 25.0f;
    [Export] public float SpeedMultiplier { get; set; } = 1.0f;

    [ExportGroup("Jumps")]
    [Export] public int JumpVelocity { get; set; } = 400;
    [Export] private float CoyoteTime { get; set; } = 0.15f;
    [Export] private float BufferTime { get; set; } = 0.15f;
    [Export] public int MaxJumps { get; set; } = 2;

    [ExportGroup("Dash")]
    [Export] public int DashVelocity { get; set; } = 700;
    [Export] public float DashCooldown { get; set; } = 0.3f;
    [Export] public float DashDuration { get; set; } = 0.15f;
    [Export] public int MaxDashes { get; set; } = 1;

    // Timers & Counters
    private float CoyoteTimer { get; set; }
    private float BufferTimer { get; set; }
    private float DashCooldownTimer { get; set; }
    private int JumpsLeft { get; set; }
    private int DashesLeft { get; set; }

    private int Direction { get; set; } = 1;

    // Random shi
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private float _frictionFactor = 1.0f;

    public void MoveHorizontal(float xDir, double delta)
    {
        var lerpFactor = CalcLerpFactor(Acceleration, delta);
        var targetDirection = Mathf.Lerp(Actor.Velocity.X, xDir * BaseSpeed * SpeedMultiplier, lerpFactor);

        Actor.Velocity = new Vector2(targetDirection, Actor.Velocity.Y);

        if (xDir != 0)
        {
            Direction = Mathf.Sign(xDir); // Potato method
        }
    }

    public void ApplyGravity(double delta)
    {
        var rawAfterGravity = Actor.Velocity.Y + _gravity * (float)delta;
        var afterGravity = Mathf.Min(600f, rawAfterGravity);
        Actor.Velocity = new Vector2(Actor.Velocity.X, afterGravity);
    }

    public void ApplyFriction(double delta, float multiplier = 1.0f)
    {
        // Title misleading, it's just horizontal friction.
        _frictionFactor = Mathf.MoveToward(_frictionFactor, multiplier, (float)delta * 6.0f);
        var lerpFactor = CalcLerpFactor(Friction * _frictionFactor, delta);
        var afterFriction = Mathf.Lerp(Actor.Velocity.X, 0, lerpFactor);

        if (Mathf.Abs(afterFriction) < 1.0f)
        {
            afterFriction = 0.0f; // Lerp is asymptotic, gotta make it actually completely stop this way.
        }

        Actor.Velocity = new Vector2(afterFriction, Actor.Velocity.Y);
    }

    public void Dash(float direction)
    {
        if (!CheckDash()) { return;}
        
        DashCooldownTimer = DashCooldown;
        Actor.Velocity = new Vector2(DashVelocity * direction, 0f);
        DashesLeft--;
    }

    private bool CheckDash()
    {
        return DashCooldownTimer <= 0 && DashesLeft > 0;
    }

    public void QueueJump()
    {
        BufferTimer = BufferTime;
    }

    private bool CheckJump()
    {
        if (JumpsLeft == MaxJumps && CoyoteTimer > 0 && BufferTimer > 0)
        {
            return true;
        }
        else if (JumpsLeft > 0 && BufferTimer > 0)
        {
            return true;
        }
        return false;
    }

    private void ExecuteJump()
    {
        if (!CheckJump()) { return; }
        
        ClearTimers();
        Actor.Velocity = new Vector2(Actor.Velocity.X, -JumpVelocity);
    }

    private void ClearTimers()
    {
        CoyoteTimer = 0f;
        BufferTimer = 0f;
        JumpsLeft--;
    }

    private void UpdateTimers(double delta) // Seems to be a universal update method for timers I guess
    {
        CoyoteTimer -= (float)delta;
        BufferTimer -= (float)delta;

        if (Actor.IsOnFloor())
        {
            CoyoteTimer = CoyoteTime;
            JumpsLeft = MaxJumps;
            DashesLeft = MaxDashes;
        }
        else if (CoyoteTimer <= 0 && JumpsLeft == MaxJumps)
        {
            JumpsLeft = MaxJumps - 1;
        }

        if (DashCooldownTimer > 0)
        {
            DashCooldownTimer -= (float)delta;
        }
    }

    private static float CalcLerpFactor(float var, double delta)
    {
        return 1.0f - Mathf.Exp(-var * (float)delta); // Had a bug with weird friction and acceleration (jittery), so asked gemini, slight idea on how this works.
    }

    public void Move()
    {
        Actor.MoveAndSlide();
    }

    public int GetDirection()
    {
        return Direction; // I guess, man.
    }

    public override void _EnterTree()
    {
        JumpsLeft = MaxJumps;
        DashesLeft = MaxDashes;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateTimers(delta);
        ExecuteJump();
    }
}
