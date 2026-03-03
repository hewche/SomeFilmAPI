using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public string Photo { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Movieparticipation> Movieparticipations { get; set; } = new List<Movieparticipation>();

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
