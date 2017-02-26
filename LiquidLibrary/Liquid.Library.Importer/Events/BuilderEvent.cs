using Liquid.Library.Importer.MovieServiceReference;
using System;

namespace Liquid.Library.Importer.Events
{
    public class BuilderEventArgs : EventArgs
    {
        public virtual Movie Movie { get; set; }
    }
}
