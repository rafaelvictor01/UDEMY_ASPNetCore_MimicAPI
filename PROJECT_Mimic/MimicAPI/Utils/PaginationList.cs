using System.Collections.Generic;

namespace MimicAPI.Utilitarios
{
    public class PaginationList<T> : List<T>
    {
        public Paginacao Paginacao { get; set; }
    }
}
