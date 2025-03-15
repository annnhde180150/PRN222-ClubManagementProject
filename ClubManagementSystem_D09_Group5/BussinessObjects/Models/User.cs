using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
    public string Username { get; set; } = string.Empty!;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [MaxLength(50)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string ConfirmPassword { get; set; } = string.Empty!;

    public byte[]? ProfilePicture { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();

    public virtual ICollection<ClubRequest> ClubRequests { get; set; } = new List<ClubRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();

    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
