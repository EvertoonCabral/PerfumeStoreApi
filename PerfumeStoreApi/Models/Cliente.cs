using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace PerfumeStoreApi.Models;

public class Cliente
{

    
    public int Id  { get; set; }
    public string Nome { get; set; }
    public string? Cpf { get; set; }
    public string Telefone { get; set; }
    public bool IsAtivo { get; set; }
    
    
    
    }