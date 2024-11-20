using Prova.Models;
using Prova.ViewModels;

namespace Prova.AutoMappers.Placemark
{
    public class PlacemarkMap : AutoMapper.Profile
    {
        public PlacemarkMap()
        {
            CreateMap<PlacemarkModel, PlacemarkViewModel>();
        }
    }
}
