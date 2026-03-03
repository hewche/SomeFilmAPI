using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Profession
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Movieparticipation> Movieparticipations { get; set; } = new List<Movieparticipation>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
