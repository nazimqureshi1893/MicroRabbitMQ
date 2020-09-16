using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Collections.Generic;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventsTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
         {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventsTypes = new List<Type>();

         }
        public Task SendCommond<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);


            }
        }
      
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlertype = typeof(TH);
            if (!_eventsTypes.Contains(typeof(T)))
            {
                _eventsTypes.Add(typeof(T));
            }
            if (!_handlers.ContainsKey(eventName))
            {

                _handlers.Add(eventName,new List<Type>());
            }
            if (_handlers[eventName].Any(s => s.GetType() == handlertype))
            {
                throw new  ArgumentException($"Handle Type {handlertype.Name} already is register for'{eventName}'");
            }
            _handlers[eventName].Add(handlertype);
            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where  T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true,
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Cosumer_Recived;
            channel.BasicConsume(eventName, true, consumer);


        }

        private async Task Cosumer_Recived(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {

                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }

        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using(var scop= _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scop.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventsTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[]
                            { @event});
                    }
                }

            }
        }
    }
}
