using Mapster;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<UpdateUserDto, User>
                .NewConfig()
                .IgnoreNullValues(true);
        }
    }
}
