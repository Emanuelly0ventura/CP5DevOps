using System.ComponentModel.DataAnnotations;

namespace DimDim.Web.Models;

public class DimGroup
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do grupo.")]
    [StringLength(80)]
    [Display(Name = "Nome do grupo")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Display(Name = "Criado em")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
