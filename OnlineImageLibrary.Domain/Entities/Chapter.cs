using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace OnlineImageLibrary.Domain.Entities
{
    [Table(Name = "Chapters")]
    public class Chapter
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ChapterID { get; set; }

        [Column]
        public int BookID { get; set; }

        [Column]
        public string Title { get; set; }
    }
}
