using ApuDoingStuff.Properties;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ApuDoingStuff.Database.Models
{
    public partial class BotdbContext : DbContext
    {
        public BotdbContext()
        {
        }

        public BotdbContext(DbContextOptions<BotdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<Dicegamedb> Dicegamedbs { get; set; }
        public virtual DbSet<Suggestion> Suggestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(Resources.UseMySql);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(entity =>
            {
                entity.ToTable("channels");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Channel1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Channel");
            });

            modelBuilder.Entity<Dicegamedb>(entity =>
            {
                entity.ToTable("dicegamedb");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmoteNr)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Locker).HasMaxLength(50);

                entity.Property(e => e.PingMe).HasColumnType("bit(1)");

                entity.Property(e => e.Points).HasDefaultValueSql("'0'");

                entity.Property(e => e.Rank)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'-'");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.ToTable("suggestions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Suggestion1)
                    .HasMaxLength(50)
                    .HasColumnName("Suggestion");

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
