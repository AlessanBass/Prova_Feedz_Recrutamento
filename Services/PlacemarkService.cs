using Prova.DTOs;
using Prova.InputModel;
using Prova.Models;
using Prova.Repository;

namespace Prova.Services
{
    public class PlacemarkService
    {
        private readonly PlacemarkRepository _placemarkRepository;

        public PlacemarkService(PlacemarkRepository placemarkRepository)
        {
            _placemarkRepository =  placemarkRepository;
        }

        public IEnumerable<PlacemarkModel> FilterPlacemarks(FilterRequestInputModel filterRequestInputModel)
        {
            var placemarks = _placemarkRepository.GetPlacemarks();

            if(filterRequestInputModel.Cliente != null)
            {
                placemarks = placemarks.Where(p => p.Cliente != null &&  
                p.Cliente.Contains(filterRequestInputModel.Cliente, StringComparison.OrdinalIgnoreCase));
            }

            return placemarks;
        }

        public FiltersDto GetAvailableFilters()
        {
            var placemarks = _placemarkRepository.GetPlacemarks();

            return new FiltersDto
            {
                Clientes = placemarks.Select(p => p.Cliente).Distinct().ToList(),
                Situacoes = placemarks.Select(p => p.Situacao).Distinct().ToList(),
                Bairros = placemarks.Select(p => p.Bairro).Distinct().ToList(),
            };
        }
    }
}
