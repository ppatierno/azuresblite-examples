using Amqp;
using System;
using System.Diagnostics;

namespace AzureSBLite.Examples
{
    class Program
    {
        static string SB_NAMESPACE = "[SB_NAMESPACE]";
        static string SB_CONNECTION_STRING = "[SB_CONNECTION_STRING]";

        static string EVENT_HUB_NAME = "[EVENT_HUB_NAME]";
        static string EVENT_HUB_PARTITION_ID = "[EVENT_HUB_PARTITION_ID]";
        static string EVENT_HUB_PARTITION_KEY = "[EVENT_HUB_PARTITION_KEY]";
        static string EVENT_HUB_PARTITION_OFFSET = "[EVENT_HUB_PARTITION_OFFSET]";
        static DateTime EVENT_HUB_PARTITION_DATE_TIME_OFFSET;
        static string EVENT_HUB_PUBLISHER_NAME = "[EVENT_HUB_PUBLISHER_NAME]";

        static string QUEUE_SEND = "[QUEUE_SEND]";
        static string QUEUE_REPLYTO = "[QUEUE_REPLYTO]";
        static string TOPIC_SEND = "[TOPIC_SEND]";
        static string SUBSCRIPTION_ONE = "[SUBSCRIPTION_ONE]";
        static string SUBSCRIPTION_TWO = "[SUBSCRIPTION_TWO]";

        static string SHARED_ACCESS_KEY_NAME = "[SHARED_ACCESS_KEY_NAME]";
        static string SHARED_ACCESS_KEY = "[SHARED_ACCESS_KEY]";
        
        static void Main(string[] args)
        {
            Amqp.Trace.TraceLevel = Amqp.TraceLevel.Frame | Amqp.TraceLevel.Verbose;
            Amqp.Trace.TraceListener = (f, a) => Debug.Print(DateTime.Now.ToString("[hh:ss.fff]") + " " + Fx.Format(f, a));

            Scenarios scenarios = new Scenarios();
            scenarios.ConnectionString = SB_CONNECTION_STRING;

            scenarios.Scenario1_EventHubSend(EVENT_HUB_NAME);

            scenarios.Scenario2_EventHubSendToPartition(
                EVENT_HUB_NAME, 
                EVENT_HUB_PARTITION_ID);
            
            scenarios.Scenario3_EventHubSendToPublisher(
                EVENT_HUB_NAME, 
                EVENT_HUB_PUBLISHER_NAME);
            
            scenarios.Scenario4_EventHubSendToPublisherWithToken(
                SB_NAMESPACE, 
                EVENT_HUB_NAME, 
                EVENT_HUB_PUBLISHER_NAME, 
                SHARED_ACCESS_KEY_NAME,
                SHARED_ACCESS_KEY);
            
            scenarios.Scenario5_EventHubSendToPartitionKey(
                EVENT_HUB_NAME, 
                EVENT_HUB_PARTITION_KEY);
            
            scenarios.Scenario6_EventHubReceiveFromPartition(
                EVENT_HUB_NAME, 
                EVENT_HUB_PARTITION_ID);
            
            scenarios.Scenario7_EventHubReceiveFromPartitionOffset(
                EVENT_HUB_NAME, 
                EVENT_HUB_PARTITION_ID, 
                EVENT_HUB_PARTITION_OFFSET);

            scenarios.Scenario8_EventHubReceiveFromPartitionDateTimeOffset(
                EVENT_HUB_NAME,
                EVENT_HUB_PARTITION_ID,
                EVENT_HUB_PARTITION_DATE_TIME_OFFSET);
            
            scenarios.Scenario9_QueueSend(QUEUE_SEND);
            
            scenarios.Scenario10_QueueRequestResponse(
                QUEUE_SEND, 
                QUEUE_REPLYTO);
            
            scenarios.Scenario12_TopicSend(
                TOPIC_SEND, 
                SUBSCRIPTION_ONE, 
                SUBSCRIPTION_TWO);

            Console.ReadKey();
        }
    }
}
