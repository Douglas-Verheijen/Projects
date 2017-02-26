using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Components
{
    public interface IEditComponent
    {
    }

    public abstract class EditComponent<T> : FieldComponent<T>, IEditComponent
    {
    }
}
