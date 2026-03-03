using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Rating
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Movierating> Movieratings { get; set; } = new List<Movierating>();
}
