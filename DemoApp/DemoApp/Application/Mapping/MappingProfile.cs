namespace DemoApp.Application.Mapping
{
    using AutoMapper;
    using DemoApp.Application.DTOs;
    using DemoApp.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Category, CategoryDto>();
            this.CreateMap<Transaction, TransactionDto>();
        }
    }
}
