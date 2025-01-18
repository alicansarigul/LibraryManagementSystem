using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Bu controller'da kitaplarla ilgili tüm işlemleri yapıyoruz
    /// Kitap ekleme, silme, güncelleme, listeleme gibi işlemler burada
    /// </summary>
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Veritabanı bağlantımızı constructor'dan alıyoruz
        /// Program.cs'de tanımladığımız DbContext buraya geliyor
        /// </summary>
        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tüm kitapların listelendiği ana sayfa
        /// Buradan kitapları görüntüleyebiliyoruz
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Tüm kitapları veritabanından çekip view'e gönderiyoruz
            return View(await _context.Books.ToListAsync());
        }

        /// <summary>
        /// Yeni kitap ekleme sayfasını gösterir
        /// GET isteği geldiğinde boş form gösteriyoruz
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni kitap ekleme işlemini yapan method
        /// Form post edildiğinde burası çalışıyor
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ISBN,PublishYear,Publisher,IsAvailable")] Book book)
        {
            // Form verileri geçerliyse kitabı kaydediyoruz
            if (ModelState.IsValid)
            {
                // Yeni kitap için boş bir ödünç alma listesi oluştur
                book.BookLoans = new List<BookLoan>();
                
                // Kitabı veritabanına ekle
                _context.Add(book);
                await _context.SaveChangesAsync();
                
                // Başarılı mesajı göster
                TempData["Success"] = "Kitap başarıyla eklendi.";
                
                // Kitap listesine geri dön
                return RedirectToAction(nameof(Index));
            }
            
            // Form hatalıysa tekrar form sayfasını göster
            return View(book);
        }

        /// <summary>
        /// Kitap düzenleme sayfasını gösterir
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            // Id kontrolü
            if (id == null)
            {
                return NotFound();
            }

            // Kitabı bul
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        /// <summary>
        /// Kitap bilgilerini günceller
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,PublishYear,Publisher,IsAvailable")] Book book)
        {
            // Id kontrolü
            if (id != book.Id)
            {
                return NotFound();
            }

            // Form verileri geçerliyse güncelle
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Kitap başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(book);
        }

        /// <summary>
        /// Kitap silme onay sayfasını gösterir
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        /// <summary>
        /// Kitabı siler
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookLoans)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Kitabın ödünç kayıtları varsa silmeyi engelle
            if (book.BookLoans != null && book.BookLoans.Any())
            {
                TempData["Error"] = "Bu kitap ödünç verilmiş. Önce ödünç kayıtlarını silmeniz gerekiyor.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kitap başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kitabın var olup olmadığını kontrol eder
        /// </summary>
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}