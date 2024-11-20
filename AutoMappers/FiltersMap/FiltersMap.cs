using Prova.DTOs;
using Prova.ViewModels;

namespace Prova.AutoMappers.FiltersMap
{
    public class FiltersMap : AutoMapper.Profile
    {

        public FiltersMap()
        {
            CreateMap<FiltersDto, FiltersViewModel>();
        }
    }
}
