using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs
{
    public class PageProductsRequestDto
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public string SortBy { get; set; } = "Name";
        public string? SearchBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
