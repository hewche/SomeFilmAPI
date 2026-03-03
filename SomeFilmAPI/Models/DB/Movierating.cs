using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Movierating
{
    public int MovieId { get; set; }

    public int RatingId { get; set; }

    public decimal? Rating { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Rating RatingNavigation { get; set; } = null!;
}
