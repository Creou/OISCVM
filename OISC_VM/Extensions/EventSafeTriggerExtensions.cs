using System;
using System.ComponentModel;

namespace OISC_VM.Extensions
{
    /// <summary>
    /// Provides safe trigger extensions for the EventHandler class.
    /// </summary>
    public static class EventSafeTriggerExtensions
    {
        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        public static void SafeTrigger(this EventHandler eventToTrigger, Object sender)
        {
            if (eventToTrigger != null)
            {
                eventToTrigger(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The arguments for the event.</param>
        public static void SafeTrigger(this EventHandler eventToTrigger, Object sender, EventArgs eventArgs)
        {
            if (eventToTrigger != null)
            {
                eventToTrigger(sender, eventArgs);
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <typeparam name="TReturnType">The return type of the event trigger.</typeparam>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The arguments for the event.</param>
        /// <param name="retrieveDataFunction">A function used to retrieve data from the event.</param>
        /// <returns>Returns data retrieved from the event arguments.</returns>
        public static TReturnType SafeTrigger<TReturnType>(this EventHandler eventToTrigger, Object sender,
                                                           EventArgs eventArgs, Func<EventArgs, TReturnType> retrieveDataFunction)
        {
            if (eventToTrigger != null)
            {
                eventToTrigger(sender, eventArgs);
                TReturnType returnData = retrieveDataFunction(eventArgs);
                return returnData;
            }
            else
            {
                return default(TReturnType);
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The arguments for the event.</param>
        public static void SafeTrigger<TEventArgs>(this EventHandler<TEventArgs> eventToTrigger, Object sender,
                                                   TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            if (eventToTrigger != null)
            {
                eventToTrigger(sender, eventArgs);
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        public static void SafeTrigger<TEventArgs>(this EventHandler<TEventArgs> eventToTrigger,
                                                   Object sender) where TEventArgs : EventArgs, new()
        {
            if (eventToTrigger != null)
            {
                eventToTrigger(sender, new TEventArgs());
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <typeparam name="TReturnType">The return type of the event trigger.</typeparam>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="retrieveDataFunction">A function used to retrieve data from the event.</param>
        /// <returns>Returns data retrieved from the event arguments.</returns> 
        public static TReturnType SafeTrigger<TEventArgs, TReturnType>(this EventHandler<TEventArgs> eventToTrigger,
                                                                       Object sender,
                                                                       Func<TEventArgs, TReturnType> retrieveDataFunction)
                                                                       where TEventArgs : EventArgs, new()
        {
            if (eventToTrigger != null)
            {
                TEventArgs eventArgs = new TEventArgs();
                eventToTrigger(sender, eventArgs);
                TReturnType returnData = retrieveDataFunction(eventArgs);
                return returnData;
            }
            else
            {
                return default(TReturnType);
            }
        }

        /// <summary>
        /// Safely triggers an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <typeparam name="TReturnType">The return type of the event trigger.</typeparam>
        /// <param name="eventToTrigger">The event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The arguments for the event.</param>
        /// <param name="retrieveDataFunction">A function used to retrieve data from the event.</param>
        /// <returns>Returns data retrieved from the event arguments.</returns> 
        public static TReturnType SafeTrigger<TEventArgs, TReturnType>(this EventHandler<TEventArgs> eventToTrigger,
                                                                       Object sender, TEventArgs eventArgs,
                                                                       Func<TEventArgs, TReturnType> retrieveDataFunction)
                                                                       where TEventArgs : EventArgs
        {
            if (retrieveDataFunction == null)
            {
                throw new ArgumentNullException("retrieveDataFunction");
            }

            if (eventToTrigger != null)
            {
                eventToTrigger(sender, eventArgs);
                TReturnType returnData = retrieveDataFunction(eventArgs);
                return returnData;
            }
            else
            {
                return default(TReturnType);
            }
        }

        /// <summary>
        /// Safely triggers an property changed event.
        /// </summary>
        /// <param name="propertyChangedEventToTrigger">The property changed event to trigger.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public static void SafeTrigger(this PropertyChangedEventHandler propertyChangedEventToTrigger, Object sender, String propertyName)
        {
            if (propertyChangedEventToTrigger != null)
            {
                propertyChangedEventToTrigger(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}