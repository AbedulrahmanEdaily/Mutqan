using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response
{
    public class PagintedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int page { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
