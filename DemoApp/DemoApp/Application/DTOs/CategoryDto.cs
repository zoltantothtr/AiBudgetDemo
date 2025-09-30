namespace DemoApp.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsIncome { get; set; }
    }
}
