using Bioskop.Filter;
using System;

namespace Bioskop.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
