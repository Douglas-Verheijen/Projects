using Liquid.Library.Importer.Events;
using Liquid.Library.Importer.MovieServiceReference;
using System;

namespace Liquid.Library.Importer.Services
{
    class MovieBuilderService : EntityBuilderService
    {
        public override void Build()
        {
            foreach (var item in Data)
            {
                var movie = new Movie();
                movie.Name = item[0];
                movie.Format = (MovieFormat)Enum.Parse(typeof(MovieFormat), item[1]);

                var args = new BuilderEventArgs();
                args.Movie = movie;

                FireEntityBuiltEvent(args);
            }

            FireBuildCompleteEvent();
        }
    }
}
