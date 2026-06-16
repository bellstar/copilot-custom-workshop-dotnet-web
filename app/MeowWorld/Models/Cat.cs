using System.ComponentModel.DataAnnotations;

namespace MeowWorld.Models;

public class Cat
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    [Range(0, int.MaxValue)]
    public int Age { get; set; }

    [Required]
    [StringLength(50)]
    public required string Breed { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}