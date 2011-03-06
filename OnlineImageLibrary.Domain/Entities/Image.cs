using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web.Mvc;

namespace OnlineImageLibrary.Domain.Entities
{
    [Table(Name="Images")]
    public class Image
    {
        [HiddenInput(DisplayValue=false)]
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert, UpdateCheck=UpdateCheck.Always)]
        public int ImageID { get; set; }

        [Column]
        public int ChapterID { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Column]
        public string Filename { get; set; }

        [Column]
        public string Title { get; set; }

        [Column]
        public string Artist { get; set; }

        [Column]
        public string Date { get; set; }

        [Column]
        public string Medium { get; set; }

        [Column]
        public string Size { get; set; }

        [Column]
        public string Credits { get; set; }

        [Column]
        public string Description { get; set; }

        [Column]
        public string Theme { get; set; }

        [Column]
        public string Period { get; set; }

        [Column]
        public string Origin { get; set; }

        [Column]
        public string Collection { get; set; }

        [Column]
        public bool IsNew { get; set; }

        [Column]
        public string Figure { get; set; }

        [Column]
        public string Keywords { get; set; }

    }
}
