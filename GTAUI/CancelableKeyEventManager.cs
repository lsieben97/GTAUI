using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    internal class CancelableKeyEventManager
    {
        private List<EventHandler<KeyEventArgs>> subscribersList = new List<EventHandler<KeyEventArgs>>();

        internal void AddSubscriber(EventHandler<KeyEventArgs> subscriber)
        {
            if (subscribersList.Contains(subscriber)) return;
            subscribersList.Add(subscriber);
        }

        internal void RemoveSubscriber(EventHandler<KeyEventArgs> subscriber)
        {
            if (!subscribersList.Contains(subscriber)) return;
            subscribersList.Remove(subscriber);
        }

        /// <summary>
        /// Fire the event and return <c>true</c> if one of the subscribers canceled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cancelArgs"></param>
        /// <returns></returns>
        internal bool FireEvent(object sender, KeyEventArgs cancelArgs)
        {
            foreach (EventHandler<KeyEventArgs> sub in subscribersList)
            {
                sub(sender, cancelArgs);

                // Stop the Execution after a subscriber cancels the event
                if (cancelArgs.Handled)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
