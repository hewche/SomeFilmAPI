using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SomeFilmAPI.Models.DB;

public partial class SomeFilmContext : DbContext
{
    public SomeFilmContext()
    {
    }

    public SomeFilmContext(DbContextOptions<SomeFilmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Award> Awards { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Movieparticipation> Movieparticipations { get; set; }

    public virtual DbSet<Movierating> Movieratings { get; set; }

    public virtual DbSet<Movietype> Movietypes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Ratingmpaa> Ratingmpaas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5050;Database=SomeFilm;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Award>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("award_pkey");

            entity.ToTable("award");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genre_pkey");

            entity.ToTable("genre");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movie_pkey");

            entity.ToTable("movie");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.DateMovie).HasColumnName("date_movie");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MovieType).HasColumnName("movie_type");
            entity.Property(e => e.Mpaa).HasColumnName("mpaa");
            entity.Property(e => e.Poster)
                .HasMaxLength(200)
                .HasColumnName("poster");
            entity.Property(e => e.Slogan)
                .HasMaxLength(200)
                .HasColumnName("slogan");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");

            entity.HasOne(d => d.Country).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movie_country_id_fkey");

            entity.HasOne(d => d.MovieTypeNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.MovieType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movie_movie_type_fkey");

            entity.HasOne(d => d.MpaaNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Mpaa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movie_mpaa_fkey");

            entity.HasMany(d => d.Awards).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "Movieaward",
                    r => r.HasOne<Award>().WithMany()
                        .HasForeignKey("AwardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("movieaward_award_id_fkey"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("movieaward_movie_id_fkey"),
                    j =>
                    {
                        j.HasKey("MovieId", "AwardId").HasName("movieaward_pkey");
                        j.ToTable("movieaward");
                        j.IndexerProperty<int>("MovieId").HasColumnName("movie_id");
                        j.IndexerProperty<int>("AwardId").HasColumnName("award_id");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "Moviegenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("moviegenres_genre_id_fkey"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("moviegenres_movie_id_fkey"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId").HasName("moviegenres_pkey");
                        j.ToTable("moviegenres");
                        j.IndexerProperty<int>("MovieId").HasColumnName("movie_id");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");
                    });
        });

        modelBuilder.Entity<Movieparticipation>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.MovieId, e.ProfessionId }).HasName("movieparticipation_pkey");

            entity.ToTable("movieparticipation");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
            entity.Property(e => e.CharacterName)
                .HasMaxLength(200)
                .HasColumnName("character_name");

            entity.HasOne(d => d.Movie).WithMany(p => p.Movieparticipations)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movieparticipation_movie_id_fkey");

            entity.HasOne(d => d.Person).WithMany(p => p.Movieparticipations)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movieparticipation_person_id_fkey");

            entity.HasOne(d => d.Profession).WithMany(p => p.Movieparticipations)
                .HasForeignKey(d => d.ProfessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movieparticipation_profession_id_fkey");
        });

        modelBuilder.Entity<Movierating>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.RatingId }).HasName("movierating_pkey");

            entity.ToTable("movierating");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.Rating)
                .HasPrecision(3, 1)
                .HasColumnName("rating");

            entity.HasOne(d => d.Movie).WithMany(p => p.Movieratings)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movierating_movie_id_fkey");

            entity.HasOne(d => d.RatingNavigation).WithMany(p => p.Movieratings)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movierating_rating_id_fkey");
        });

        modelBuilder.Entity<Movietype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movietype_pkey");

            entity.ToTable("movietype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.ToTable("person");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasMaxLength(200)
                .HasColumnName("photo");

            entity.HasOne(d => d.Country).WithMany(p => p.People)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("person_country_id_fkey");

            entity.HasMany(d => d.Professions).WithMany(p => p.People)
                .UsingEntity<Dictionary<string, object>>(
                    "Personprofession",
                    r => r.HasOne<Profession>().WithMany()
                        .HasForeignKey("ProfessionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personprofessions_profession_id_fkey"),
                    l => l.HasOne<Person>().WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personprofessions_person_id_fkey"),
                    j =>
                    {
                        j.HasKey("PersonId", "ProfessionId").HasName("personprofessions_pkey");
                        j.ToTable("personprofessions");
                        j.IndexerProperty<int>("PersonId").HasColumnName("person_id");
                        j.IndexerProperty<int>("ProfessionId").HasColumnName("profession_id");
                    });
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profession_pkey");

            entity.ToTable("profession");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Ratingmpaa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ratingmpaa_pkey");

            entity.ToTable("ratingmpaa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
