using System;
using System.Collections.Generic;

namespace SomeFilmAPI.Models.DB;

public partial class Movieparticipation
{
    public int PersonId { get; set; }

    public int MovieId { get; set; }

    public int ProfessionId { get; set; }

    public string? CharacterName { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Profession Profession { get; set; } = null!;
}
