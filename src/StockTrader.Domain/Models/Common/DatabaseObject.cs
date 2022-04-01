namespace StockTrader.Domain.Models
{
    /// <summary>
    /// Base interface for all database objects
    /// NOTE: Id can be manipulated to not be auto-generated
    ///       Override the virtual property to manipulate
    /// </summary>
    public abstract class DatabaseObject
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}
