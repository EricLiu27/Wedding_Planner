#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Forms;

namespace Wedding_Planner.Models;
public class Wedding
{
    [Key]
    public int WeddingId { get; set; }
    [Required]
    [Display(Name = "Wedder One:")]
    public string WedderOne { get; set; }
    [Required]
    [Display(Name = "Wedder Two:")]
    public string WedderTwo { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [FutureDate]
    public DateTime Date { get; set; }
    [Required]
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? Planner { get; set; }

    public List<UserWeddingRSVP> UsersGuest { get; set; } = new();
}

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        DateTime Input = (DateTime)value;
        DateTime Now = DateTime.Now;

        if (Input < Now)
        {
            return new ValidationResult("Date must be in the future!");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}