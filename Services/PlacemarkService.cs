using Prova.DTOs;
using Prova.Enums;
using Prova.InputModel;
using Prova.Models;
using Prova.Repositories;
using SharpKml.Dom;
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

        public byte[] GenerateKml(IEnumerable<object> placemarks)
        {
            // Início do KML
            var kmlContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            kmlContent += "<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n";
            kmlContent += "<Document>\n";

            // Nome do arquivo
            kmlContent += "<name>PLANILHA PARA IMPORTAR GOOGLE MAPS DIRECIONADORES.xlsx</name>\n";

            foreach (var placemark in placemarks)
            {
                kmlContent += "<Placemark>\n";

                // Usando reflexão para acessar as propriedades do placemark
                var nameProperty = placemark.GetType().GetProperty("Name");
                var nameValue = nameProperty != null ? nameProperty.GetValue(placemark)?.ToString() : "Unknown";

                var descriptionProperty = placemark.GetType().GetProperty("Description");
                var descriptionValue = descriptionProperty != null ? descriptionProperty.GetValue(placemark)?.ToString() : "";

                var ruaProperty = placemark.GetType().GetProperty("RuaCruzamento");
                var ruaValue = ruaProperty != null ? ruaProperty.GetValue(placemark)?.ToString() : "";

                var referenciaProperty = placemark.GetType().GetProperty("Referencia");
                var referenciaValue = referenciaProperty != null ? referenciaProperty.GetValue(placemark)?.ToString() : "";

                var situacaoProperty = placemark.GetType().GetProperty("Situacao");
                var situacaoValue = situacaoProperty != null ? situacaoProperty.GetValue(placemark)?.ToString() : "";

                var bairroProperty = placemark.GetType().GetProperty("Bairro");
                var bairroValue = bairroProperty != null ? bairroProperty.GetValue(placemark)?.ToString() : "";

                var clienteProperty = placemark.GetType().GetProperty("Cliente");
                var clienteValue = clienteProperty != null ? clienteProperty.GetValue(placemark)?.ToString() : "";

                var dataProperty = placemark.GetType().GetProperty("Data");
                var dataValue = dataProperty != null ? dataProperty.GetValue(placemark)?.ToString() : "";

                var coordenadasProperty = placemark.GetType().GetProperty("Coordenadas");
                var coordenadasValue = coordenadasProperty != null ? coordenadasProperty.GetValue(placemark)?.ToString() : "";

                var gpxmedialinksProperty = placemark.GetType().GetProperty("GxMediaLinks");
                var gpxmedialinksValue = gpxmedialinksProperty != null ? gpxmedialinksProperty.GetValue(placemark)?.ToString() : "";

                // Incluir as informações do Placemark
                kmlContent += $" <name>{nameValue}</name>\n";
                kmlContent += $" <description><![CDATA[{descriptionValue}]]></description>\n";
                kmlContent += " <styleUrl>#icon-1899-0288D1</styleUrl>\n";

                // Adicionar dados extras no ExtendedData
                kmlContent += " <ExtendedData>\n";
                kmlContent += $"  <Data name=\"RUA/CRUZAMENTO\">\n    <value>{ruaValue}</value>\n</Data>\n";
                kmlContent += $"  <Data name=\"REFERENCIA\">\n    <value>{referenciaValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"BAIRRO\">\n    <value>{bairroValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"SITUAÇÃO\">\n    <value>{situacaoValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"CLIENTE\">\n    <value>{clienteValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"DATA\">\n    <value>{dataValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"COORDENADAS\">\n    <value>{coordenadasValue}</value>\n </Data>\n";
                kmlContent += $"  <Data name=\"gx_media_links\">\n    <value><![CDATA[{gpxmedialinksValue}]]></value>\n </Data>\n";
                kmlContent += " </ExtendedData>\n";

                // Coordenadas (assumindo que o formato é longitude, latitude)
                if (!string.IsNullOrEmpty(coordenadasValue))
                {
                    var coords = coordenadasValue.Split(',');
                    if (coords.Length == 2)
                    {
                        var longitude = coords[0];
                        var latitude = coords[1];
                        kmlContent += "<Point>\n";
                        kmlContent += $"<coordinates>{longitude},{latitude},0</coordinates>\n";
                        kmlContent += "</Point>\n";
                    }
                }

                kmlContent += "</Placemark>\n";
            }

            // Fechar a tag de Document e KML
            kmlContent += "</Document>\n";
            kmlContent += "</kml>\n";

            // Retorna o conteúdo KML como um array de bytes
            return System.Text.Encoding.UTF8.GetBytes(kmlContent);
        }







    }
}
