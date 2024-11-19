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
    }
}
