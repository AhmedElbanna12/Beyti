using System.ComponentModel.DataAnnotations;

namespace Beyti.ViewModel.chief
{
    public class RecipeVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string Ingredients { get; set; } = null!;

        [Required]
        public int PreparationTime { get; set; }

        [Required]
        public decimal Price { get; set; }

        // هنا بنستخدم IFormFile للرفع
        public IFormFile? ImageFile { get; set; }

        // يخزن مسار الصورة بعد الرفع
        public string? ImagePath { get; set; }
    }
}
