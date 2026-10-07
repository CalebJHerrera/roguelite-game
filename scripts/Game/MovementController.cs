namespace Game;

using Godot;

// In case I forget, the player leaf states / AI BT node call the functions inside here. No logic is to be handled in this file, logic is actually in the states/tree nodes.
// The only stuff in here is the modification of the velocities and some other random weird platformer stuff.
[GlobalClass]
public partial class MovementController : Node
{
    [ExportGroup("Actor")]
    [Export] public CharacterBody2D Actor { get; set; }

    [ExportGroup("Physics")]
    [Export] public int BaseSpeed { get; set; } = 200;
    [Export] public int JumpVelocity { get; set; } = 400;
    [Export] public int DashVelocity { get; set; } = 300;
    [Export] public float Acceleration { get; set; } = 35.0f;
    [Export] public float Friction { get; set; } = 25.0f;
    [Export] public float SpeedMultiplier { get; set; } = 1.0f;

    [ExportGroup("Jumps")]
    [Export] private float CoyoteTime { get; set; } = 0.15f;
    [Export] private float BufferTime { get; set; } = 0.15f;
    [Export] public int MaxJumps { get; set; } = 2;

    // Timers & Counters
    private float CoyoteTimer { get; set; } = 0f;
    private float BufferTimer { get; set; } = 0f;
    private int JumpsLeft { get; set; }

    private float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private float FrictionFactor = 1.0f;

    public void MoveHorizontal(float xDir, double delta)
    {
        float lerpFactor = CalcLerpFactor(Acceleration, delta);
        float targetDirection = Mathf.Lerp(Actor.Velocity.X, xDir * BaseSpeed * SpeedMultiplier, lerpFactor);

        Actor.Velocity = new Vector2(targetDirection, Actor.Velocity.Y);
    }

    public void ApplyGravity(double delta)
    {
        float rawAfterGravity = Actor.Velocity.Y + Gravity * (float)delta;
        float afterGravity = Mathf.Min(600f, rawAfterGravity);
        Actor.Velocity = new Vector2(Actor.Velocity.X, afterGravity);
    }

    public void ApplyFriction(double delta, float multiplier = 1.0f)
    {
        // Title mislading, it's just horizontal friction.
        FrictionFactor = Mathf.MoveToward(FrictionFactor, multiplier, (float)delta * 6.0f);
        float lerpFactor = CalcLerpFactor(Friction * FrictionFactor, delta);
        float afterFriction = Mathf.Lerp(Actor.Velocity.X, 0, lerpFactor);

        if (Mathf.Abs(afterFriction) < 1.0f)
        {
            afterFriction = 0.0f; // Lerp is asymptotic, gotta make it actually completely stop this way.
        }

        Actor.Velocity = new Vector2(afterFriction, Actor.Velocity.Y);
    }

    public void Dash()
    {
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
        if (CheckJump())
        {
            ClearTimers();
            Actor.Velocity = new Vector2(Actor.Velocity.X, -JumpVelocity);
        }
    }

    private void ClearTimers()
    {
        CoyoteTimer = 0f;
        BufferTimer = 0f;
        JumpsLeft--;
    }

    private void UpdateTimers(double delta)
    {
        CoyoteTimer -= (float)delta;
        BufferTimer -= (float)delta;

        if (Actor.IsOnFloor())
        {
            CoyoteTimer = CoyoteTime;
            JumpsLeft = MaxJumps;
        }
        else if (CoyoteTimer <= 0 && JumpsLeft == MaxJumps)
        {
            JumpsLeft = MaxJumps - 1;
        }
    }

    private static float CalcLerpFactor(float var, double delta)
    {
        return 1.0f - Mathf.Exp(-var * (float)delta); // Had a bug with weird friction and acceleration (jittery), so asked gemini, no idea how this works.
    }

    public void Move()
    {
        Actor.MoveAndSlide();
    }

    public override void _EnterTree()
    {
        JumpsLeft = MaxJumps;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateTimers(delta);
        ExecuteJump();
    }
}
