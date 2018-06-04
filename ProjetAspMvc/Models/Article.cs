using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetAspMvc.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumArticle { get; set; } 
        [Required]
        public string Designation { get; set; }
        [Required]
        public double PrixU { get; set; }
        [Required]
        public double stock { get; set; }
        [Required]
        public string photo { get; set; }


        public Nullable<int> RefCat { get; set; }
        public virtual Categorie categorie { get; set; }
    }

}