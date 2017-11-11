namespace GGFanGame.Game
{
    /// <summary>
    /// The states of an object.
    /// </summary>
    internal enum ObjectState
    {
        Idle,
        Walking,
        Jumping,
        Falling,

        Blocking,

        Hurt,
        HurtFalling,
        OnBack,
        StandingUp,
        Dead,

        Attacking,
        JumpAttacking,
        Dashing
    }
}
