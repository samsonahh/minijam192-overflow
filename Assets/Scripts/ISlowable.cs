public interface ISlowable
{
    /// <summary>
    /// Sets the player's max speed to a percentage of its normal value for a duration.
    /// </summary>
    /// <param name="maxSpeedPercent">0.0 to 1.0, where 1.0 is 100% (no slow), 0.5 is 50% speed, etc.</param>
    /// <param name="duration">How long the slow lasts (seconds).</param>
    void ApplySlowness(float maxSpeedPercent, float duration);
}