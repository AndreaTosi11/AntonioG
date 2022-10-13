using System.ComponentModel.DataAnnotations;

namespace ProjectWorkDemo.Models
{
    public class GetMezziById
    {
        [Key]
        public int IdMezzo { get; set; }
      
        public string Denominazione { get; set; }
     
        public string TipoAuto { get; set; }
   
        public string Targa { get; set; }

        public DateTime DataImmatricolazione { get; set; }

    }
}
