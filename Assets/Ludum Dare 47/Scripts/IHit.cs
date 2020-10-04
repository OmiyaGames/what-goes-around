namespace LudumDare47
{
    public interface IHit
    {
        /// <summary>
        /// Registers a hit
        /// </summary>
        /// <returns>True if this destroys hit object</returns>
        bool OnHit(int power, bool color);
    }
}
