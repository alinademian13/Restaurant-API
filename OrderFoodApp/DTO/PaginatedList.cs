﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class PaginatedList<T>
    {
        public const int EntriesPerPage = 5;
        public int NumberOfEntries { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<T> Entries { get; set; }
    }
}
