using Liquid.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Liquid.UI.WinForm
{
    public interface IWindowsFormRenderService
    {
        bool IsInitialized { get; }
        void Initialize(Form form);
        void Render(IEnumerable<IRenderable> renderables);
        void Render(IRenderable center, IEnumerable<IRenderable> renderables);
    }

    [DefaultImplementation(typeof(IWindowsFormRenderService))]
    class WindowsFormRenderService : IWindowsFormRenderService, IDisposable
    {
        public virtual bool IsInitialized { get; private set; }

        private Form _form;
        private BufferedGraphics _buffer;

        public void Dispose()
        {
            Contract.Requires(_form != null);
            Contract.Requires(_buffer != null);

            _form.Dispose();
            _buffer.Dispose();
        }

        public void Initialize(Form form)
        {
            Contract.Requires(form != null);

            var context = new BufferedGraphicsContext();
            _buffer = context.Allocate(form.CreateGraphics(), form.ClientRectangle);
            _form = form;

            IsInitialized = true;
        }

        public void Render(IEnumerable<IRenderable> renderables)
        {
            Contract.Requires(_form != null);
            Contract.Requires(_buffer != null);

            var context = new BufferedGraphicsContext();
            _buffer = context.Allocate(_form.CreateGraphics(), _form.ClientRectangle);
            _buffer.Graphics.TranslateTransform(_form.ClientRectangle.Width / 2, _form.ClientRectangle.Height / 2);
            _buffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var entity in renderables)
                entity.Render(_buffer.Graphics);

            _buffer.Render();
        }

        public void Render(IRenderable center, IEnumerable<IRenderable> renderables)
        {
            var context = new BufferedGraphicsContext();
            _buffer = context.Allocate(_form.CreateGraphics(), _form.ClientRectangle);
            _buffer.Graphics.TranslateTransform(_form.ClientRectangle.Width / 2 - center.Position.X, _form.ClientRectangle.Height / 2 - center.Position.Y);
            _buffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var entity in renderables)
                entity.Render(_buffer.Graphics);

            center.Render(_buffer.Graphics);

            _buffer.Render();
        }
    }
}
