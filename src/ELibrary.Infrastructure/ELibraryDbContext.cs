using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.BooksGenresModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.GenreModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Domain.RoleModel;
using ELibrary.src.ELibrary.Domain.RefreshTokenModel;
using ELibrary.src.ELibrary.Domain.UserModel;
using ELibrary.src.ELibrary.Domain.UserReadBookModel;
using ELibrary.src.ELibrary.Domain.UserWriteBookModel;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.src.ELibrary.Infrastructure
{
    public partial class ELibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Book { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<Genre> Genre { get; set; } = null!;
        public virtual DbSet<Comment> Comment { get; set; } = null!;
        public virtual DbSet<Rating> Rating { get; set; } = null!;
        public virtual DbSet<BooksGenres> BooksGenres { get; set; } = null!;
        public virtual DbSet<UserWriteBook> WrittenBook { get; set; } = null!;
        public virtual DbSet<UserReadBook> ReadBooks { get; set; } = null!;
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RefreshToken> Tokens { get; set; }

        public ELibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__book__DAE712E872991A56");

                entity.ToTable("book");

                entity.Property(e => e.Id).HasColumnName("id_book");

                entity.Property(e => e.Annotation)
                    .HasMaxLength(500)
                    .HasColumnName("annotation");

                entity.Property(e => e.Cover)
                    .HasMaxLength(255)
                    .HasColumnName("cover");

                entity.Property(e => e.DownloadUrl)
                    .HasMaxLength(255)
                    .HasColumnName("download_url");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date")
                    .HasColumnName("publication_date");
            });

            modelBuilder.Entity<BooksGenres>(entity =>
            {
                //entity.HasNoKey();

                entity.HasKey(e => new { e.BookId, e.GenreId });

                entity.ToTable("book_has_genre");

                entity.Property(e => e.BookId).HasColumnName("id_book");

                entity.Property(e => e.GenreId).HasColumnName("id_genre");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__book_has___id_bo__47DBAE45");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__book_has___id_ge__46E78A0C");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__comment__7E14AC8535B762C5");

                entity.ToTable("comment");

                entity.Property(e => e.Id).HasColumnName("id_comment");

                entity.Property(e => e.Content)
                    .HasMaxLength(250)
                    .HasColumnName("content");

                entity.Property(e => e.DateWriting)
                    .HasColumnType("datetime")
                    .HasColumnName("date_writing");

                entity.Property(e => e.BookId).HasColumnName("id_book");

                entity.Property(e => e.UserId).HasColumnName("id_user");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__comment__id_book__4F7CD00D");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__comment__id_user__4E88ABD4");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__genre__CB767C69B3F6E281");

                entity.ToTable("genre");

                entity.Property(e => e.Id).HasColumnName("id_genre");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__role__3D48441DB4D8CA56");

                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id_role");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.Token, e.UserId });
                entity.ToTable("refresh_token");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnType("datetime")
                    .HasColumnName("expires_at");

                entity.Property(e => e.UserId).HasColumnName("id_user");

                entity.Property(e => e.IssuedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("issued_at");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__refresh_t__id_us__3D5E1FD2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__user__D2D146374134C731");

                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id_user");

                entity.Property(e => e.AboutUser)
                    .HasMaxLength(500)
                    .HasColumnName("about_user");

                entity.Property(e => e.DateRegistration)
                    .HasColumnType("date")
                    .HasColumnName("date_registration");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.RoleId).HasColumnName("id_role");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.PhotoUser)
                    .HasMaxLength(255)
                    .HasColumnName("photo_user");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__user__id_role__398D8EEE");
            });

            modelBuilder.Entity<UserWriteBook>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.BookId, e.UserId });
                ///entity.HasNoKey();

                entity.ToTable("user_has_book");

                entity.Property(e => e.BookId).HasColumnName("id_book");

                entity.Property(e => e.UserId).HasColumnName("id_user");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_has___id_bo__4316F928");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_has___id_us__4222D4EF");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.UserId });

                //entity.HasNoKey();

                entity.ToTable("user_put_rating");

                entity.Property(e => e.BookId).HasColumnName("id_book");

                entity.Property(e => e.UserId).HasColumnName("id_user");

                entity.Property(e => e.Mark).HasColumnName("rating");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_put___id_bo__4BAC3F29");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_put___ratin__4AB81AF0");
            });

            modelBuilder.Entity<UserReadBook>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.BookId, e.UserId });

                entity.ToTable("user_read_book");

                entity.Property(e => e.BookId).HasColumnName("id_book");

                entity.Property(e => e.UserId).HasColumnName("id_user");

                entity.HasOne(d => d.IdBookNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_read__id_bo__403A8C7D");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__user_read__id_us__3F466844");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
