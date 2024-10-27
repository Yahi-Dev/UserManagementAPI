using System;
using System.Collections.Generic;

namespace Layer_Entities.ModelsDB;

public partial class Phone
{
    public int PhoneId { get; set; }

    public string UserId { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string? CityCode { get; set; }

    public string? CountryCode { get; set; }


    //RelationShip between User and Phone.
    //A user can have a lot of Phone, but a phone belongs to a person
    public virtual User User { get; set; } = null!;//Navegation Property
}
