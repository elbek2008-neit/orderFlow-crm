using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Dtos
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public MetaDataDto Meta { get; set; } = new MetaDataDto();
    }

    public class MetaDataDto
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }
    }
}
