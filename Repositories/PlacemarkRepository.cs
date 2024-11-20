using Prova.Models;
using SharpKml.Dom;
using SharpKml.Engine;

namespace Prova.Repositories
{
    public class PlacemarkRepository
    {
        private readonly KmlFile _kmlFile;

        public PlacemarkRepository(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            _kmlFile = KmlFile.Load(stream);
        }

        public IEnumerable<PlacemarkModel> GetPlacemarks()
        {
            var placemarks = new List<PlacemarkModel>();

            // Identifica se o root é um Folder ou Document
            var root = _kmlFile.Root as Container ??
                       (_kmlFile.Root as Kml)?.Feature as Container;

            if (root == null)
                return placemarks;

            // Extrai as Placemarks
            ExtractPlacemarks(root, placemarks);

            return placemarks;
        }

        private void ExtractPlacemarks(Container container, List<PlacemarkModel> placemarks)
        {
            foreach (var feature in container.Features)
            {
                if (feature is Placemark placemark)
                {
                    var extendedData = placemark.ExtendedData?.Data;

                    placemarks.Add(new PlacemarkModel
                    {
                        Name = placemark.Name,
                        Description = placemark.Description?.Text,
                        Cliente = GetValue(extendedData, "CLIENTE"),
                        Situacao = GetValue(extendedData, "SITUAÇÃO"),
                        Bairro = GetValue(extendedData, "BAIRRO"),
                        Referencia = GetValue(extendedData, "REFERENCIA"),
                        RuaCruzamento = GetValue(extendedData, "RUA/CRUZAMENTO"),
                        Data = GetValue(extendedData, "DATA"),
                        Coordenadas = GetValue(extendedData, "COORDENADAS"),
                        GxMediaLinks = GetValue(extendedData, "gx_media_links"),
                    });
                }
                else if (feature is Container childContainer)
                {
                    // Caso tenha um subfolder, processa recursivamente
                    ExtractPlacemarks(childContainer, placemarks);
                }
            }
        }

        private string GetValue(IEnumerable<Data> data, string name)
        {
            return data?.FirstOrDefault(d => d.Name == name)?.Value ?? string.Empty;
        }
    }
}
