namespace NServiceBus.Unicast.Tests.Helpers
{
    using System.Collections.Generic;
    using Subscriptions;
    using Subscriptions.MessageDrivenSubscriptions;

    public class FakeSubscriptionStorage : ISubscriptionStorage
    {
        public void Subscribe(Address address, IEnumerable<MessageType> messageTypes)
        {
            foreach (var messageType in messageTypes)
            {
                if (!storage.ContainsKey(messageType))
                    storage[messageType] = new List<Address>();

                if (!storage[messageType].Contains(address))
                    storage[messageType].Add(address);
            }
        }

        public void Unsubscribe(Address address, IEnumerable<MessageType> messageTypes)
        {
            foreach (var messageType in messageTypes)
            {
                if (storage.ContainsKey(messageType))
                    storage[messageType].Remove(address);
            }
        }

        public IEnumerable<Address> GetSubscriberAddressesForMessage(IEnumerable<MessageType> messageTypes)
        {
            var result = new List<Address>();
            foreach (var m in messageTypes)
            {
                if (storage.ContainsKey(m))
                    result.AddRange(storage[m]);
            }

            return result;
        }
        public void FakeSubscribe<T>(Address address)
        {
            ((ISubscriptionStorage)this).Subscribe(address, new[] { new MessageType(typeof(T)) });
        }

        public void Init()
        {
        }

        readonly Dictionary<MessageType, List<Address>> storage = new Dictionary<MessageType, List<Address>>();
    }
}