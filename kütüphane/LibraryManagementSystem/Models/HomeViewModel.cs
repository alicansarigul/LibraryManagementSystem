using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Book> RecentBooks { get; set; }
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
    }
}