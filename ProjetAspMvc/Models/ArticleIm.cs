using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAspMvc.Models
{
    public class ArticleIm
    {
       
        public int NumArticle { get; set; }
        public string Designation { get; set; }
        public double PrixU { get; set; }
        public double stock { get; set; }
        public string photo { get; set; }
        public Nullable<int> RefCat { get; set; }
        public virtual Categorie categorie { get; set; }
        public HttpPostedFileBase imageFile { get; set; }
    }
}