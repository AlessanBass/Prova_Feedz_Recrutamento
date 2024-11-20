using Prova.DTOs;
using Prova.Enums;
using Prova.InputModel;
using Prova.Models;
using Prova.Repository;
using System.Linq;
using System.Reflection;

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
                if(!CheckFilters(filterRequestInputModel.Cliente, FiltersEnum.Cliente)) {
                    throw new Exception("Invalid value for the client filter!");
                }

                placemarks = placemarks.Where(p => p.Cliente != null &&  
                p.Cliente.Contains(filterRequestInputModel.Cliente, StringComparison.OrdinalIgnoreCase));
            }

            if (filterRequestInputModel.Situacao != null)
            {
                if (!CheckFilters(filterRequestInputModel.Situacao, FiltersEnum.Situacao))
                {
                    throw new Exception("Invalid value for the satisfaction filter!");
                }

                placemarks = placemarks.Where(p => p.Situacao != null &&
                p.Situacao.Contains(filterRequestInputModel.Situacao, StringComparison.OrdinalIgnoreCase));
            }

            if (filterRequestInputModel.Bairro != null)
            {
                if (!CheckFilters(filterRequestInputModel.Bairro, FiltersEnum.Bairro))
                {
                    throw new Exception("Invalid value for the neighborhood filter!");
                }


                placemarks = placemarks.Where(p => p.Bairro != null &&
                p.Bairro.Contains(filterRequestInputModel.Bairro, StringComparison.OrdinalIgnoreCase));
            }

            if (filterRequestInputModel.Referencia != null)
            {
                placemarks = placemarks.Where(p => p.Referencia != null &&
                p.Referencia.Contains(filterRequestInputModel.Referencia, StringComparison.OrdinalIgnoreCase));
            }

            if (filterRequestInputModel.RuaCruzamento != null)
            {
                placemarks = placemarks.Where(p => p.RuaCruzamento != null &&
                p.RuaCruzamento.Contains(filterRequestInputModel.RuaCruzamento, StringComparison.OrdinalIgnoreCase));
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

        private bool CheckFilters(string filter, FiltersEnum typeFilter)
        {
            var filtersDto = GetAvailableFilters();

            if(typeFilter == FiltersEnum.Cliente)
            {
                return filtersDto.Clientes
                    .Where(c => string.Equals(c, filter, StringComparison.OrdinalIgnoreCase))
                    .Any(); 

            }

            if (typeFilter == FiltersEnum.Situacao)
            {
                return filtersDto.Situacoes
                    .Where(c => string.Equals(c, filter, StringComparison.OrdinalIgnoreCase))
                    .Any();
            }

            if (typeFilter == FiltersEnum.Bairro)
            {
                return filtersDto.Bairros
                    .Where(c => string.Equals(c, filter, StringComparison.OrdinalIgnoreCase))
                    .Any();
            }

            return false;
        }
    }
}
