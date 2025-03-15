using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Specifications
{
    public class RealEstateAdSpecParams
    {

        // clean code  (GetProducts)

        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }


        //public string? Sort { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public int ClientId { get; set; }

        public string? Neighborhood { get; set; }

        //private string? search;

        //public string Search
        //{
        //    get { return search; }
        //    set { search = value?.ToLower(); }
        //}

    }
}
