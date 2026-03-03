using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Movietype
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
