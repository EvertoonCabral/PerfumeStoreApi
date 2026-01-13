using AutoMapper;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Data.Dtos.Cliente;
using PerfumeStoreApi.Data.Dtos.ItemVenda;
using PerfumeStoreApi.Data.Dtos.Movimentação;
using PerfumeStoreApi.Data.Dtos.Produto;
using PerfumeStoreApi.Data.Dtos.Venda;
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
                CreateMap<MovimentacaoEstoque, MovimentacaoResponse>().ReverseMap();

                CreateMap<Venda, VendaResponse>().ReverseMap();
                CreateMap<Venda, VendaResponseDetail>().ReverseMap();

                
                CreateMap<ItemVenda, ItemVendaResponse>()
                        .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto.Nome))
                        .ForMember(dest => dest.ProdutoMarca, opt => opt.MapFrom(src => src.Produto.Marca));

                CreateMap<Pagamento, PagamentoResponse>();

                CreateMap<CreateVendaRequest, Venda>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.DataVenda, opt => opt.Ignore())
                        .ForMember(dest => dest.Status, opt => opt.Ignore())
                        .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                        .ForMember(dest => dest.Itens, opt => opt.Ignore())
                        .ForMember(dest => dest.Pagamentos, opt => opt.Ignore());

                CreateMap<CreateItemVendaRequest, ItemVenda>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.VendaId, opt => opt.Ignore())
                        .ForMember(dest => dest.Venda, opt => opt.Ignore())
                        .ForMember(dest => dest.Produto, opt => opt.Ignore())
                        .ForMember(dest => dest.PrecoUnitario, opt => opt.Ignore()); 

                CreateMap<CreatePagamentoRequest, Pagamento>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.VendaId, opt => opt.Ignore())
                        .ForMember(dest => dest.Venda, opt => opt.Ignore())
                        .ForMember(dest => dest.DataPagamento, opt => opt.Ignore()) 
                        .ForMember(dest => dest.DataVencimento, opt => opt.Ignore()); 
                
                
        }
}