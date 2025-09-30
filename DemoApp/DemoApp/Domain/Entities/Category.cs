namespace DemoApp.Domain.Entities
{
    /// <summary>
    /// Financial transaction category.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsIncome { get; set; }
    }
}
