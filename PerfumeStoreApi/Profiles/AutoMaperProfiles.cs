using AutoMapper;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
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


        }
}