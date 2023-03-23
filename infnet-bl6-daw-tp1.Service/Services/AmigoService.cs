using infnet_bl6_daw_tp1.Domain.Entities;
using infnet_bl6_daw_tp1.Domain.Interfaces;
using infnet_bl6_daw_tp1.Domain.ViewModel;
using Microsoft.Data.SqlClient;
using System.Transactions;

namespace infnet_bl6_daw_tp1.Service.Services
{
    public class AmigoService : IAmigoService
    {
        private readonly string StringConexao = "Server=tcp:dbserveramigos.database.windows.net,1433;Initial Catalog=DBAmigos;Persist Security Info=False;User ID=juarez;Password=123senha@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
/*        public readonly infnet_bl6_daw_tp1DbContext _dbContext;

        public AmigoService(infnet_bl6_daw_tp1DbContext dbContext)
        {
            _dbContext = dbContext;
        }
*/
        public List<AmigoViewModel> GetAll()
        {
            var amigos = new List<AmigoViewModel>();

            using (var connection = new SqlConnection(StringConexao))
            {
                var procedure = "BuscarTodosAmigos";
                var sqlComando = new SqlCommand(procedure, connection);

                sqlComando.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    using (var leitura = sqlComando.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (leitura.Read())
                        {
                            var amigo = new Amigo
                            {
                                Id = (int)leitura["Id"],
                                Nome = leitura["Nome"].ToString(),
                                Sobrenome = leitura["Sobrenome"].ToString(),
                                Email = leitura["Email"].ToString(),
                                Nascimento = (DateTime)leitura["Nascimento"]
                            };
                            amigos.Add(new AmigoViewModel(amigo));
                        }
                    }
                }
                finally { connection.Close(); }
            }

            return amigos;
        }
        Amigo IAmigoService.Add(Amigo amigo)
        {
            using (var connection = new SqlConnection(StringConexao))
            {
                var procedure = "InserirAmigo";
                var sqlComando = new SqlCommand(procedure, connection);
                sqlComando.CommandType = System.Data.CommandType.StoredProcedure;

                sqlComando.Parameters.AddWithValue("@Nome", amigo.Nome);
                sqlComando.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                sqlComando.Parameters.AddWithValue("@Email", amigo.Email);
                sqlComando.Parameters.AddWithValue("@Nascimento", amigo.Nascimento);

                try
                {
                    connection.Open();
                    sqlComando.ExecuteNonQuery();
                }
                finally { connection.Close(); }

            };
            return amigo;
        }

        Amigo IAmigoService.Save(Amigo amigo)
        {
            using (var connection = new SqlConnection(StringConexao))
            {
                var procedure = "AlterarAmigo";
                var sqlComando = new SqlCommand(procedure, connection);
                sqlComando.CommandType = System.Data.CommandType.StoredProcedure;

                sqlComando.Parameters.AddWithValue("@Id", amigo.Id);
                sqlComando.Parameters.AddWithValue("@Nome", amigo.Nome);
                sqlComando.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                sqlComando.Parameters.AddWithValue("@Email", amigo.Email);
                sqlComando.Parameters.AddWithValue("@Nascimento", amigo.Nascimento);

                try
                {
                    connection.Open();
                    sqlComando.ExecuteNonQuery();
                }
                finally { connection.Close(); }

            };
            return amigo;

        }
        Amigo IAmigoService.Remove(Amigo amigo)
        {
            using (var connection = new SqlConnection(StringConexao))
            {
                var procedure = "ExcluirAmigo";
                var sqlComando = new SqlCommand(procedure, connection);
                sqlComando.CommandType = System.Data.CommandType.StoredProcedure;

                sqlComando.Parameters.AddWithValue("@Id", amigo.Id);

                try
                {
                    connection.Open();
                    sqlComando.ExecuteNonQuery();
                }
                finally { connection.Close(); }

            };
            return null;
        }
    }
}
