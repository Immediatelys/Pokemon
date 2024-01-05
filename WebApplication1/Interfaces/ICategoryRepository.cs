using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);

        bool CategoryExists(int cateId);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);
        bool Save();
    }
}
