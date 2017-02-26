using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Library.Importer.Services
{
    public interface IEntityApiService
    {
        event EventHandler EntityCreated;

        void Send(object obj);
        void BeginSend(object obj);
    }

    class EntityApiService
    {
        public event EventHandler EntityCreated;

        public virtual void BeginSend(object obj)
        {
            Task.Factory.StartNew(() => Send(obj));
        }

        public virtual void Send(object obj)
        { 
        }

        protected virtual void FireEntityCreatedEvent()
        {
            EntityCreated.RaiseEvent(this, null);
        }
    }
}
