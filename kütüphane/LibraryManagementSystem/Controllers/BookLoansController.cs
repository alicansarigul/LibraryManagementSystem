using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Bu controller'da kitap ödünç alma ve iade işlemlerini yapıyoruz
    /// Öğrencilere kitap verme ve geri alma işlemleri burada
    /// </summary>
    public class BookLoansController : Controller
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Veritabanı bağlantımızı constructor'dan alıyoruz
        /// </summary>
        public BookLoansController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Kitap ödünç verme sayfasını gösterir
        /// Hangi öğrenciye kitap vereceğimizi önceden biliyoruz (studentId)
        /// </summary>
        public async Task<IActionResult> LoanBook(int? studentId)
        {
            // Öğrenci ID kontrolü
            if (studentId == null)
            {
                return NotFound();
            }

            // Öğrenciyi bul
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            // Sadece rafta olan (ödünç verilmemiş) kitapları listele
            var availableBooks = await _context.Books
                .Where(b => b.IsAvailable)
                .ToListAsync();

            // View'e gönderilecek verileri hazırla
            ViewData["BookId"] = new SelectList(availableBooks, "Id", "Title");
            ViewData["StudentName"] = student.FullName;
            ViewData["StudentId"] = student.Id;

            return View();
        }

        /// <summary>
        /// Kitap ödünç verme işlemini gerçekleştirir
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook(int studentId, int bookId)
        {
            try
            {
                // Kitap ve öğrenciyi bul
                var book = await _context.Books.FindAsync(bookId);
                var student = await _context.Students.FindAsync(studentId);

                if (book == null || student == null)
                {
                    return NotFound();
                }

                // Yeni ödünç alma kaydı oluştur
                var bookLoan = new BookLoan
                {
                    BookId = bookId,
                    StudentId = studentId,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14) // 2 haftalık ödünç verme süresi
                };

                // Kitabın durumunu güncelle (artık rafta değil)
                book.IsAvailable = false;

                // Veritabanına kaydet
                _context.Add(bookLoan);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"{book.Title} kitabı {student.FullName} öğrencisine verildi.";
                return RedirectToAction("Index", "Students");
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yap ve hata mesajı göster
                System.Diagnostics.Debug.WriteLine($"Kitap ödünç verme hatası: {ex.Message}");
                TempData["Error"] = "Kitap ödünç verme işlemi sırasında bir hata oluştu.";
                return RedirectToAction("Index", "Students");
            }
        }

        /// <summary>
        /// Kitap iade işlemini gerçekleştirir
        /// </summary>
        public async Task<IActionResult> ReturnBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ödünç alma kaydını, kitap ve öğrenci bilgileriyle birlikte bul
            var bookLoan = await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Student)
                .FirstOrDefaultAsync(bl => bl.Id == id);

            if (bookLoan == null)
            {
                return NotFound();
            }

            try
            {
                // İade tarihini şimdi olarak ayarla
                bookLoan.ReturnDate = DateTime.Now;
                
                // Kitabı tekrar rafta olarak işaretle
                bookLoan.Book.IsAvailable = true;

                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();

                TempData["Success"] = $"{bookLoan.Book.Title} kitabı iade edildi.";
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yap ve hata mesajı göster
                System.Diagnostics.Debug.WriteLine($"Kitap iade hatası: {ex.Message}");
                TempData["Error"] = "Kitap iade işlemi sırasında bir hata oluştu.";
            }

            return RedirectToAction("Index", "Students");
        }
    }
}