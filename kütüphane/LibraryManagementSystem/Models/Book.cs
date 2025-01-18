using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Kütüphanedeki kitapları temsil eden model sınıfı
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Constructor: BookLoans koleksiyonunu başlat
        /// </summary>
        public Book()
        {
            BookLoans = new List<BookLoan>();
        }

        /// <summary>
        /// Kitabın benzersiz kimlik numarası
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Kitabın başlığı/adı (zorunlu alan)
        /// </summary>
        [Required(ErrorMessage = "Kitap adı zorunludur")]
        [Display(Name = "Kitap Adı")]
        public string Title { get; set; }

        /// <summary>
        /// Kitabın yazarı (zorunlu alan)
        /// </summary>
        [Required(ErrorMessage = "Yazar adı zorunludur")]
        [Display(Name = "Yazar")]
        public string Author { get; set; }

        /// <summary>
        /// Kitabın ISBN numarası (zorunlu alan)
        /// </summary>
        [Required(ErrorMessage = "ISBN zorunludur")]
        public string ISBN { get; set; }

        /// <summary>
        /// Kitabın yayın yılı
        /// </summary>
        [Display(Name = "Yayın Yılı")]
        public int? PublishYear { get; set; }

        /// <summary>
        /// Kitabın yayınevi
        /// </summary>
        [Display(Name = "Yayınevi")]
        public string Publisher { get; set; }

        /// <summary>
        /// Kitabın mevcut durumu (rafta/ödünç verilmiş)
        /// </summary>
        [Display(Name = "Mevcut mu?")]
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Kitabın ödünç alma geçmişi
        /// </summary>
        public virtual ICollection<BookLoan> BookLoans { get; set; }
    }
}