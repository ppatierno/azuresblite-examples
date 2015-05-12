using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
using System;
using System.IO;
using System.Text;

namespace IoTVenice
{
    public class Scenarios
    {
        // Azure Service Bus SDK
        //private string connectionString = "Endpoint=sb://iotvenice.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=v2FzDtp+s6eTPlg6LaPq3AXeDFm94fqZ0LiuJRrwQLY=";

        //private string connectionString = "Endpoint=sb://ppatiernoeventhubs.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zyrR1I/Nl6zWpNZJob/BUsIDTXanbyoDfuyaFqlkpgk=";

        public string ConnectionString { get; set; }

        public void Scenario1_EventHubSend(string eventHubEntity)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);

            EventData data = new EventData(Encoding.UTF8.GetBytes("Body"));
            data.Properties["time"] = DateTime.UtcNow;

            client.Send(data);

            client.Close();
            factory.Close();
        }

        public void Scenario2_EventHubSendToPartition(string eventHubEntity, string partitionId)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);

            EventHubSender sender = client.CreatePartitionedSender(partitionId);

            EventData data = new EventData(Encoding.UTF8.GetBytes("Body"));
            data.Properties["time"] = DateTime.UtcNow;

            sender.Send(data);

            sender.Close();
            client.Close();
            factory.Close();
        }

        public void Scenario3_EventHubSendToPublisher(string eventHubEntity, string publisher)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);

            EventHubSender sender = client.CreateSender(publisher);

            EventData data = new EventData(Encoding.UTF8.GetBytes("Body"));
            data.Properties["time"] = DateTime.UtcNow;

            sender.Send(data);

            sender.Close();
            client.Close();
            factory.Close();
        }

        public void Scenario4_EventHubSendToPublisherWithToken(string sbnamespace, string eventHubEntity, string publisher, string sharedAccessKeyName, string sharedAccessKey)
        {
            string token = SharedAccessSignatureTokenProvider.GetPublisherSharedAccessSignature(
                new Uri(sbnamespace),
                eventHubEntity,
                publisher,
                sharedAccessKeyName,
                sharedAccessKey,
                new TimeSpan(0, 20, 0));

            string connectionString = ServiceBusConnectionStringBuilder.CreateUsingSharedAccessSignature(
                new Uri(sbnamespace),
                eventHubEntity,
                publisher,
                token);

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);

            EventHubSender sender = client.CreateSender(publisher);

            EventData data = new EventData(Encoding.UTF8.GetBytes("Body"));
            data.Properties["time"] = DateTime.UtcNow;

            sender.Send(data);

            sender.Close();
            client.Close();
            factory.Close();
        }

        public void Scenario5_EventHubSendToPartitionKey(string eventHubEntity, string partitionKey)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);

            EventData data = new EventData(Encoding.UTF8.GetBytes("Body"));
            data.Properties["time"] = DateTime.UtcNow;
            data.PartitionKey = partitionKey;

            client.Send(data);

            client.Close();
            factory.Close();
        }

        public void Scenario6_EventHubReceiveFromPartition(string eventHubEntity, string partitionId)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);
            EventHubConsumerGroup group = client.GetDefaultConsumerGroup();

            EventHubReceiver receiver = group.CreateReceiver(partitionId);
            
            while (true)
            {
                EventData data = receiver.Receive();
            }
            
            receiver.Close();
            client.Close();
            factory.Close();
        }

        public void Scenario7_EventHubReceiveFromPartitionOffset(string eventHubEntity, string partitionId, string offset)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubEntity);
            EventHubConsumerGroup group = client.GetDefaultConsumerGroup();
            
            EventHubReceiver receiver = group.CreateReceiver(partitionId, offset);

            while (true)
            {
                EventData data = receiver.Receive();
            }

            receiver.Close();
            client.Close();
            factory.Close();
        }

        public void Scenario8_QueueSend(string queue)
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            QueueClient client = factory.CreateQueueClient(queue);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Body"));
            BrokeredMessage message = new BrokeredMessage(stream);
            message.Properties["time"] = DateTime.UtcNow;

            client.Send(message);

            client.Close();
            factory.Close();
        }

        public void Scenario9_QueueRequestResponse(string queue, string replyTo)
        {
            WorkerThread receiverThread = new WorkerThread(this.Scenario10_QueueReceiver);
            receiverThread.Start(queue);

            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            MessageSender sender = factory.CreateMessageSender(queue);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Request"));
            BrokeredMessage message = new BrokeredMessage(stream);
            message.MessageId = Guid.NewGuid().ToString();
            message.ReplyTo = replyTo;
            message.Properties["time"] = DateTime.UtcNow;

            sender.Send(message);

            MessageReceiver receiver = factory.CreateMessageReceiver(replyTo);

            BrokeredMessage response = receiver.Receive();
            response.Complete();

            sender.Close();
            receiver.Close();
            factory.Close();
        }

        private void Scenario10_QueueReceiver(object obj)
        {
            string queue = (string)obj;

            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            QueueClient client = factory.CreateQueueClient(queue);

            while (true)
            {
                BrokeredMessage request = client.Receive();
                request.Complete();

                MessageSender sender = factory.CreateMessageSender(request.ReplyTo);

                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Response"));
                BrokeredMessage response = new BrokeredMessage(stream);
                response.CorrelationId = request.MessageId;
                response.Properties["time"] = DateTime.UtcNow;

                sender.Send(response);

                sender.Close();
            }
        }

        public void Scenario11_TopicSend(string topic, string subscription1, string subscription2)
        {
            WorkerThread sub1Thread = new WorkerThread(this.Scenario12_SubscriptionReceiver);
            sub1Thread.Start(new Scenarios.TopicSub() { Topic = topic, Sub = subscription1 });

            WorkerThread sub2Thread = new WorkerThread(this.Scenario12_SubscriptionReceiver);
            sub2Thread.Start(new Scenarios.TopicSub() { Topic = topic, Sub = subscription2 });

            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);
            
            TopicClient client = factory.CreateTopicClient(topic);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Body"));
            BrokeredMessage message = new BrokeredMessage(stream);
            message.Properties["time"] = DateTime.UtcNow;

            client.Send(message);

            client.Close();
            factory.Close();
        }

        class TopicSub { public string Topic; public string Sub; }

        public void Scenario12_SubscriptionReceiver(object obj)
        {
            Scenarios.TopicSub topicSub = (Scenarios.TopicSub)obj;

            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(this.ConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(this.ConnectionString);

            SubscriptionClient client = factory.CreateSubscriptionClient(topicSub.Topic, topicSub.Sub);

            while (true)
            {
                BrokeredMessage request = client.Receive();
                request.Complete();
            }
        }
    }
}
