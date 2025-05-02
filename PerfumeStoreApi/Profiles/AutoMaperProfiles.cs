using AutoMapper;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Profiles;

public class AutoMaperProfiles : Profile
{
        public AutoMaperProfiles()
        {
                      
                // Cliente -> ClienteDTO
                CreateMap<Cliente, ClienteDto>();
                // Cliente -> ClienteDetalhesDTO
                CreateMap<Cliente, ClienteDetalhesDto>();
                // Venda -> VendaResumoDTO
                CreateMap<Venda, VendaResumoDto>();
                // ClienteCreateUpdateDTO -> Cliente
                CreateMap<ClienteCreateUpdateDto, Cliente>();  
        }
}