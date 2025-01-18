using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;

/// <summary>
/// Bu bizim ana program dosyamız, uygulamamızın başlangıç noktası burası
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// MVC kullanacağımız için önce onu ekleyelim
builder.Services.AddControllersWithViews();

// SQLite veritabanımızı bağlayalım
// appsettings.json'daki ConnectionStrings'den alıyor bağlantıyı
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Eğer geliştirme ortamında değilsek (yani canlıda) hata sayfası gösterelim
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// CSS ve JavaScript dosyalarımız için bu lazım
app.UseStaticFiles();

// URL yönlendirmesi için bunu ekleyelim
app.UseRouting();

// Şimdilik kullanmıyoruz ama ilerde lazım olursa diye yetkilendirmeyi ekledim
app.UseAuthorization();

// Ana sayfamız Home/Index olsun, oradan başlasın uygulama
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();