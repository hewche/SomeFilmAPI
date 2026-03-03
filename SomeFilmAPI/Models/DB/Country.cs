using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
