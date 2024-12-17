using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Infra.Interfaces.Queues
{
    public interface IRabbitMQPublisher<T>
    {
        Task PublishMessageAsync(T message, string queueName);
    }
}
