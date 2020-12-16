using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace AppDeiaLanchesWeb
{
    public partial class Pedido
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPedido { get; set; }

        [NotMapped]
        public List<int> Produtos { get; set; }
    }
}