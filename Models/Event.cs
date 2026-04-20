using System.ComponentModel.DataAnnotations;

namespace amonMVC.Models;

public class amon_db
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [StringLength(150, ErrorMessage = "Maximum 150 characters")]
    [Display(Name = "Event Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The description is required")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "The date is required")]
    [Display(Name = "Date & Time")]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "The location is required")]
    [StringLength(200)]
    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "Image / Poster URL")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "The category is required")]
    [StringLength(80)]
    [Display(Name = "Category")]
    public string Category { get; set; } = "General";

    public bool Active { get; set; } = true;

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}