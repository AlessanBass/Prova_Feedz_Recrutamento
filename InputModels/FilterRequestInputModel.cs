using System.ComponentModel.DataAnnotations;

namespace Prova.InputModel
{
    public class FilterRequestInputModel
    {

        public string? Cliente { get; set; }
        public string? Situacao { get; set; }
        public string? Bairro { get; set; }

        [MinLength(3, ErrorMessage = "O campo Referencia deve conter pelo menos 3 caracteres.")]
        public string? Referencia { get; set; }

        [MinLength(3, ErrorMessage = "O campo Referencia deve conter pelo menos 3 caracteres.")]
        public string? RuaCruzamento { get; set; }
    }
}
