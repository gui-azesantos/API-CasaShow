
using API_Projeto_Casa.Models;

namespace API_Projeto_Casa.Models {
    public class Venda {

        public int Id { get; set; }

        public string Nome { get; set; }

        public int Capacidade { get; set; }

        public System.DateTime Data { get; set; }

        public double Preco { get; set; }

        public Local CasaDeShow { get; set; }

        public string Estilo { get; set; }

        public string Imagem { get; set; }
        public bool Status { get; set; }

        public int Qtd { get; set; }

        public string Usuario { get; set; }

        public double Total{ get; set; }

      
    }
}