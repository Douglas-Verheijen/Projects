

using Liquid.Library.Importer.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liquid.Library.Importer.Services
{
    public interface IReadService
    {
        event EventHandler<ReadEventArgs> DataRead;
        event EventHandler ReadComplete;

        void BeginRead();
        void Read();
    }

    class ReadService : IReadService
    {
        public event EventHandler<ReadEventArgs> DataRead;
        public event EventHandler ReadComplete;

        public virtual void BeginRead()
        {
            Task.Factory.StartNew(() => Read());
        }

        public virtual void Read()
        {
            throw new NotImplementedException();
        }

        protected void FireDataReadEvent(ReadEventArgs args)
        {
            DataRead.RaiseEvent(this, args);
        }

        protected void FireReadCompleteEvent()
        {
            var args = new EventArgs();
            ReadComplete.RaiseEvent(this, args);
        }
    }
}
