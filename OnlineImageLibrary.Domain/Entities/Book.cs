using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace OnlineImageLibrary.Domain.Entities
{
    [Table(Name="Books")]
    public class Book
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int BookID { get; set; }

        [Column]
        public string Title { get; set; }
    }
}
