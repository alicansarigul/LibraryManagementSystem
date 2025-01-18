using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    /// <summary>
    /// Bu sınıf bizim veritabanı işlemlerimizi yapacak olan context sınıfımız
    /// Tüm veritabanı tablolarımız ve ilişkileri burada tanımlanıyor
    /// </summary>
    public class LibraryContext : DbContext
    {
        /// <summary>
        /// Context'i oluştururken veritabanı ayarlarını alıyoruz
        /// Program.cs'den geliyor bu ayarlar
        /// </summary>
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Kitaplarımızı tutacağımız tablo
        /// Book modelinden oluşturuluyor
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Öğrencilerimizi tutacağımız tablo
        /// Student modelinden oluşturuluyor
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Ödünç alma kayıtlarını tutacağımız tablo
        /// Hangi öğrenci hangi kitabı ne zaman almış, iade etmiş vs.
        /// </summary>
        public DbSet<BookLoan> BookLoans { get; set; }

        /// <summary>
        /// Tablolar arasındaki ilişkileri burada kuruyoruz
        /// Bir öğrenci birden fazla kitap alabilir
        /// Bir kitap farklı zamanlarda farklı öğrencilere verilebilir
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Önce kitap-ödünç alma ilişkisini kuralım
            // Bir kitap birden fazla kez ödünç alınabilir
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany(b => b.BookLoans)
                .HasForeignKey(bl => bl.BookId);

            // Şimdi de öğrenci-ödünç alma ilişkisini kuralım
            // Bir öğrenci birden fazla kitap alabilir
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Student)
                .WithMany(s => s.BookLoans)
                .HasForeignKey(bl => bl.StudentId);
        }
    }
}