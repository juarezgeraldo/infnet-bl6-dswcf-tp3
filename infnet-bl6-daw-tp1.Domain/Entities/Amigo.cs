using System.ComponentModel.DataAnnotations;

namespace infnet_bl6_daw_tp1.Domain.Entities
{
    public class Amigo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }
    }
}
