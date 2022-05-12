using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Messaging
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync<T>(string topic, Action<T> handler) where T : IMessage;
    }
}
