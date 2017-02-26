using System;
using System.Windows.Forms;

namespace Liquid
{
    public static class EventHandlerExtension
    {
        public static void RaiseEvent(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                if (handler.Target is Control)
                {
                    var target = handler.Target as Control;
                    target.Invoke(handler, new object[] { sender, e });
                }
                else
                    handler(sender, e);
            }
        }

        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, T e)
            where T : EventArgs
        {
            if (handler != null)
            {
                if (handler.Target is Control)
                {
                    var target = handler.Target as Control;
                    target.Invoke(handler, new object[] { sender, e });
                }
                else
                    handler(sender, e);
            }
        }
    }
}
