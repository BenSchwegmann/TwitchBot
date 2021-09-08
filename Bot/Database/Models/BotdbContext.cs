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

        public virtual DbSet<Banuserconfig> Banuserconfigs { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<Dicegamedb> Dicegamedbs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(Resources.UseMySql);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banuserconfig>(entity =>
            {
                entity.ToTable("banuserconfig");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelName).HasMaxLength(50);

                entity.Property(e => e.State)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");
            });

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

                entity.Property(e => e.PingMe).HasColumnType("bit(1)");

                entity.Property(e => e.Points).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'0'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
