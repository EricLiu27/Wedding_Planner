#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Forms;

namespace Wedding_Planner.Models;

public class UserWeddingRSVP
{
    [Key]
    public int UserWeddingRSVPId { get; set; }
    // The IDs linking to the adjoining tables   
    public int UserId { get; set; }
    public int WeddingId { get; set; }
    // Our navigation properties - don't forget the ?    
    public User? Guest { get; set; }
    public Wedding? WeddingGuest { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
