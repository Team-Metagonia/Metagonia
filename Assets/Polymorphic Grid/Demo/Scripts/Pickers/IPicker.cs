namespace TheoryTeam.PolymorphicGrid.Demo
{
    public interface IPicker<T>
    {
        /// <summary>
        /// Update target graphic using value.
        /// </summary>
        /// <param name="value"></param>
        void Pick(T value);
    }
}
