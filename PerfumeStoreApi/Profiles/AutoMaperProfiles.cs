using AutoMapper;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Data.Dtos.Cliente;
using PerfumeStoreApi.Data.Dtos.ItemVenda;
using PerfumeStoreApi.Data.Dtos.Movimentação;
using PerfumeStoreApi.Data.Dtos.Produto;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Profiles;

public class AutoMaperProfiles : Profile
{
        public AutoMaperProfiles()
        {
                      
                CreateMap<Cliente, ClienteDto>().ReverseMap();
                CreateMap<Cliente, ClienteCreateUpdateDto>().ReverseMap();
                CreateMap<Cliente, ClienteDetalhesDto>()
                        .ForMember(dest => dest.Vendas, opt => opt.MapFrom(src => src.Vendas))
                        .ReverseMap();
                CreateMap<Venda, VendaResumoDto>().ReverseMap();
                CreateMap<Produto, ProdutoCreateUpdateDto>().ReverseMap();
                CreateMap<Produto, ProdutoDto>().ReverseMap();
                CreateMap<Produto, GetProdutosDto>().ReverseMap();
                CreateMap<Estoque, CreateEstoqueRequest>().ReverseMap();
                CreateMap<Estoque, EstoqueResponse>()
                        .ForMember(dest => dest.TotalItens, opt => opt.MapFrom(src => 
                                src.ItensEstoque != null ? src.ItensEstoque.Sum(ie => ie.Quantidade) : 0))
                        .ForMember(dest => dest.TotalProdutos, opt => opt.MapFrom(src => 
                                src.ItensEstoque != null ? src.ItensEstoque.Count : 0));
                CreateMap<ItemEstoque, ItemEstoqueResponse>().ReverseMap();
        }
}