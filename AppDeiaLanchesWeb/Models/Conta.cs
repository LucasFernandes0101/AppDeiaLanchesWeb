using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace AppDeiaLanchesWeb
{
    public partial class Conta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }

        [NotMapped]
        public int IdPedido { get; set; }
    }
}