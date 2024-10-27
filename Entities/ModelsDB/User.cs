namespace Layer_Entities.ModelsDB;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string NameUser { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? LastLogin { get; set; }

    //RelationShip between User and Phone. A user can have a lot of Phone. 
    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
