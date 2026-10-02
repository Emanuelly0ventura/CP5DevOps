using System.ComponentModel.DataAnnotations;

namespace DimDim.Web.Models;

public class Expense
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe a descrição da despesa.")]
    [StringLength(120)]
    [Display(Name = "Despesa")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(typeof(decimal), "0.01", "9999999.99", ErrorMessage = "O valor deve ser maior que zero.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Valor (R$)")]
    public decimal Amount { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Data")]
    public DateTime ExpenseDate { get; set; } = DateTime.Today;

    [StringLength(60)]
    [Display(Name = "Categoria")]
    public string? Category { get; set; }

    [Required]
    [Display(Name = "Grupo")]
    public int DimGroupId { get; set; }

    public DimGroup? DimGroup { get; set; }
}
