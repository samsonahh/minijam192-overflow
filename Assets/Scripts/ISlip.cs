public interface ISlip
{
    /// <summary>
    /// Causes the object to slip, e.g., rotate or lose control.
    /// </summary>
    /// <param name="duration">How long the slip effect should last (seconds).</param>
    void Slip(float duration);
}