using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetAspMvc.Models
{
    public class Commande
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumCmd { get; set; }
        [Required]
          public DateTime DateCmd { get; set; }
        [Required]

        public string Id { get; set; }
        public virtual ApplicationUser client { get; set; }
        [Required]
        public Nullable<int> NumArticle { get; set; }
        public virtual Article article { get; set; }
        [Required]
        public double QteArticle { get; set; }
    }
}