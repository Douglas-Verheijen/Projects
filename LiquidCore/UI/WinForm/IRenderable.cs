using System.Drawing;

namespace Liquid.UI.WinForm
{
    public interface IRenderable
    {
        PointF Position { get; set; }

        void Render(Graphics graphics);
    }
}
