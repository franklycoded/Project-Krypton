namespace KryptonAPI.Data.Models
{
    /// <summary>
    /// This is a temporary measure until EF7 has the Find() method implemented.
    /// Provides an easy way to get the Id of the entity in a generic repository for CRUD operations
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the id of the entity
        /// </summary>
        /// <returns>The id of the entity</returns>
        long Id { get; }
    }
}
