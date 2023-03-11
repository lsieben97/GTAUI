using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI
{
    internal class CancelableEventManager
    {
        private List<EventHandler<CancelEventArgs>> subscribersList = new List<EventHandler<CancelEventArgs>>();

        internal void AddSubscriber(EventHandler<CancelEventArgs> subscriber)
        {
            if (subscribersList.Contains(subscriber)) return;
            subscribersList.Add(subscriber);
        }

        internal void RemoveSubscriber(EventHandler<CancelEventArgs> subscriber)
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
        internal bool FireEvent(object sender, CancelEventArgs cancelArgs)
        {
            foreach (EventHandler<CancelEventArgs> sub in subscribersList)
            {
                sub(sender, cancelArgs);

                // Stop the Execution after a subscriber cancels the event
                if (cancelArgs.Cancel)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
