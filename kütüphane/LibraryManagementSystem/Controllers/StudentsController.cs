using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Bu controller'da öğrencilerle ilgili tüm işlemleri yapıyoruz
    /// Öğrenci ekleme, silme, güncelleme ve listeleme işlemleri burada
    /// Ayrıca öğrencilerin ödünç aldığı kitapları da buradan takip ediyoruz
    /// </summary>
    public class StudentsController : Controller
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Veritabanı bağlantımızı constructor'dan alıyoruz
        /// </summary>
        public StudentsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tüm öğrencilerin listelendiği ana sayfa
        /// Öğrencilerin ödünç aldığı kitapları da gösteriyoruz
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Öğrencileri ve ödünç aldıkları kitapları birlikte çekiyoruz
            return View(await _context.Students
                .Include(s => s.BookLoans)
                    .ThenInclude(bl => bl.Book)
                .ToListAsync());
        }

        /// <summary>
        /// Yeni öğrenci ekleme sayfasını gösterir
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni öğrenci ekleme işlemini yapar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentNumber,FirstName,LastName,Class,Phone,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                // Yeni öğrenci için boş bir kitap listesi oluştur
                student.BookLoans = new List<BookLoan>();
                
                // Öğrenciyi veritabanına ekle
                _context.Add(student);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Öğrenci başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        /// <summary>
        /// Öğrenci düzenleme sayfasını gösterir
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        /// <summary>
        /// Öğrenci bilgilerini günceller
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentNumber,FirstName,LastName,Class,Phone,Email")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Öğrenci bilgileri güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(student);
        }

        /// <summary>
        /// Öğrenci silme onay sayfasını gösterir
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Öğrenciyi ve ödünç aldığı kitapları birlikte çek
            var student = await _context.Students
                .Include(s => s.BookLoans)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            // Eğer öğrencinin iade edilmemiş kitabı varsa silmeye izin verme
            if (student.BookLoans != null && student.BookLoans.Any(bl => bl.ReturnDate == null))
            {
                TempData["Error"] = "Öğrencinin iade edilmemiş kitapları var! Önce kitapları iade edin.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        /// <summary>
        /// Öğrenciyi siler
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Öğrenci başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Öğrenci silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Öğrencinin var olup olmadığını kontrol eder
        /// </summary>
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}