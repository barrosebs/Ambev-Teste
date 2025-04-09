using System.Collections.Generic;

namespace Ambev.Application.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
} 