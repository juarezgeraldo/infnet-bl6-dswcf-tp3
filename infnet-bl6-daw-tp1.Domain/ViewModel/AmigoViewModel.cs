using infnet_bl6_daw_tp1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace infnet_bl6_daw_tp1.Domain.ViewModel;

public class AmigoViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }

    [Display(Name = "Data de nascimento")]
    [DataType(DataType.Date)]
    public DateTime Nascimento { get; set; }

    [Display(Name = "Selecionar amigo")]
    public bool amigoSelecionado { get; set; }

    public AmigoViewModel(Amigo amigo)
    {
        this.Id = amigo.Id;
        this.Nome = amigo.Nome;
        this.Sobrenome = amigo.Sobrenome;
        this.Email = amigo.Email;
        this.Nascimento = amigo.Nascimento;
    }

    [Display(Name = "Nome completo")]
    public string NomeCompleto
    {
        get { return Nome + " " + Sobrenome; }
    }
    [Display(Name = "Próximo aniversário")]
    [DataType(DataType.Date)]
    public DateTime ProximoAniversario
    {
        get { return ProximoAniversarioFuncao(); }
    }
    [Display(Name = "Dias para aniversário")]
    public int DiasFaltantes
    {
        get { return CalculaDiasFaltantesFuncao(); }
    }

    public DateTime ProximoAniversarioFuncao()
    {
        DateTime dataProximoAniversario = new(DateTime.Now.Year, Nascimento.Month, Nascimento.Day, 0, 0, 0);
        if (DateTime.Compare(dataProximoAniversario, DateTime.Today) < 0)
        {
            dataProximoAniversario = dataProximoAniversario.AddYears(1);
        }
        return dataProximoAniversario;
    }
    public int CalculaDiasFaltantesFuncao()
    {
        DateTime dataAtual = new(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        DateTime dataAniversario = ProximoAniversarioFuncao();
        DateTime dataProximoAniversario = dataAniversario;

        if (dataAtual.Month == dataAniversario.Month &&
            dataAtual.Day == dataAniversario.Day)
        {
            return 0;
        }
        int difDatas = (int)dataAtual.Subtract(dataProximoAniversario).TotalDays;
        if (difDatas < 0) { difDatas *= -1; }

        return difDatas;
    }
    public bool NomeCompletoPossui(string nomePesquisa)
    {
        return NomeCompleto.ToLowerInvariant().Contains(nomePesquisa.Trim().ToLowerInvariant());
    }
    public static List<AmigoViewModel> GetAll(List<Amigo> amigos)
    {
        var listTipo = new List<AmigoViewModel>();
        foreach (var item in amigos)
        {
            listTipo.Add(new AmigoViewModel(item));
        }
        return listTipo;
    }

}
