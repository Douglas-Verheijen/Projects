using Liquid.Library.Importer.Events;
using Liquid.Library.Importer.MovieServiceReference;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liquid.Library.Importer.Services
{
    public interface IEntityBuilderService
    {
        event EventHandler<BuilderEventArgs> EntityBuilt;
        event EventHandler BuildComplete;

        void BeginBuild();
        void Build();
    }

    class EntityBuilderService : IEntityBuilderService
    {
        public event EventHandler<BuilderEventArgs> EntityBuilt;
        public event EventHandler BuildComplete;

        public virtual string[][] Data { get; set; }

        public virtual void BeginBuild()
        {
            Task.Factory.StartNew(() => Build());
        }

        public virtual void Build()
        {
            throw new NotImplementedException();
        }

        public void FireEntityBuiltEvent(BuilderEventArgs args)
        {
            EntityBuilt.RaiseEvent(this, args);
        }

        public void FireBuildCompleteEvent()
        {
            var args = new EventArgs();
            BuildComplete.RaiseEvent(this, args);
        }
    }
}
