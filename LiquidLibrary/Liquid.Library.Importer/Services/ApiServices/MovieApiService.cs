using Liquid.Library.Importer.MovieServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Library.Importer.Services
{
    class MovieApiService : EntityApiService
    {
        private readonly MovieApiServiceClient _client;

        public MovieApiService()
        {
            _client = new MovieApiServiceClient();
        }

        public override void Send(object obj)
        {
            var movie = obj as Movie;
            _client.CreateNew(movie);

            FireEntityCreatedEvent();
        }
    }
}
