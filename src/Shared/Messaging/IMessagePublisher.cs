using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyScada.Shared.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string topic, T message) where T : IMessage;
    }
}
