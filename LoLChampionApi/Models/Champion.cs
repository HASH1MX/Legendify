using System.ComponentModel.DataAnnotations;

namespace LoLChampionApi.Models;

public class Champion
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = string.Empty;
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
}
