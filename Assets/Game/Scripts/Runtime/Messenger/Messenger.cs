using System;
using System.Collections.Generic;


namespace Wokarol.MessageSystem
{
    public class Messenger
    {
        public static Messenger Default { get; } = new Messenger();

        private Dictionary<Type, Delegate> messageMappings = new Dictionary<Type, Delegate>();


        /// <summary>
        /// Adds listener for event T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void AddListener<T>(Action<T> handler)
        {
            var type = typeof(T);
            AddListener(handler, type);
        }

        /// <summary>
        /// Sends message of type T to all listeners
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void SendMessage<T>(T message)
        {
            var type = typeof(T);
            SendMessage(message, type);
        }

        /// <summary>
        /// Removes given handler from event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void RemoveListener<T>(Action<T> handler)
        {
            Type type = typeof(T);
            RemoveListener(handler, type);
        }

        /// <summary>
        /// Removes all listeners from all events for a given target
        /// </summary>
        public void RemoveAllListenersFor(object target)
        {
            // Stores all keys to iterate over (iterating over them directly causes error becase d[k] = x is treated as changing a collection)
            Type[] keys = new Type[messageMappings.Count];
            RemoveAllListenersFor(target, keys);
        }


        private void AddListener<T>(Action<T> handler, Type type)
        {
            // Adds delegate to dictionary if there's no of given type
            if (!messageMappings.ContainsKey(type)) {
                messageMappings.Add(type, null);
            }

            // Add new delegate to old one
            messageMappings[type] = Delegate.Combine(messageMappings[type], handler);
        }

        private void SendMessage<T>(T message, Type type)
        {
            if (!messageMappings.ContainsKey(type)) return;

            // Null checks only if type is class, otherwise it generates 17 B of Garbage for each call
            if (type.IsClass && message == null) throw new ArgumentNullException();

            // Casts and invokes delegate assuming it's an Action<T> (should always be)
            ((Action<T>)messageMappings[type]).Invoke(message);
        }

        private void RemoveListener<T>(Action<T> handler, Type type)
        {
            if (!messageMappings.ContainsKey(type)) return;

            // Removes handler from old delegate
            var newDelegate = Delegate.Remove(messageMappings[type], handler);

            // Removes delegate from dictionary is it was nullified
            if (newDelegate != null) {
                messageMappings[type] = newDelegate;
            } else {
                messageMappings.Remove(type);
            }
        }

        private void RemoveAllListenersFor(object target, Type[] keys)
        {
            messageMappings.Keys.CopyTo(keys, 0);

            // Stores all keys that shoudl be removed after for loop iteration
            List<Type> keysToClear = new List<Type>();

            // Iterates over every type of event
            for (int i = 0; i < keys.Length; i++) {
                Type key = keys[i];
                var invocations = messageMappings[key].GetInvocationList();
                var newDelegate = messageMappings[key];

                // Check if any delegate matches target and if so removes it
                for (int j = 0; j < invocations.Length; j++) {
                    if (invocations[j].Target == target) newDelegate = Delegate.Remove(newDelegate, invocations[j]);
                }

                // Add key to "toClear" list if delegate has no listeners after last operation           
                if (newDelegate != null) {
                    messageMappings[key] = newDelegate;
                } else {
                    keysToClear.Add(key);
                }
            }

            // Removes all now empty keys
            for (int i = 0; i < keysToClear.Count; i++) {
                messageMappings.Remove(keysToClear[i]);
            }
        }
    }
}
