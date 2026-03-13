using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int MovieType { get; set; }

    public DateOnly DateMovie { get; set; }

    public string Description { get; set; } = null!;

    public string Slogan { get; set; } = null!;

    public string Poster { get; set; } = null!;

    public int Mpaa { get; set; }

    public virtual Movietype MovieTypeNavigation { get; set; } = null!;

    public virtual ICollection<Movieparticipation> Movieparticipations { get; set; } = new List<Movieparticipation>();

    public virtual ICollection<Movierating> Movieratings { get; set; } = new List<Movierating>();

    public virtual Ratingmpaa MpaaNavigation { get; set; } = null!;

    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
