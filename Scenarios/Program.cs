using Amqp;
using System;
using System.Threading;

#if NETMF
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
#else
using System.Diagnostics;
#endif

namespace IoTVenice
{
#if NETMF
    class Program
    {
        static string SB_NAMESPACE = "sb://ppatiernoeventhubs.servicebus.windows.net";
        static string SB_CONNECTION_STRING = "Endpoint=sb://ppatiernoeventhubs.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zyrR1I/Nl6zWpNZJob/BUsIDTXanbyoDfuyaFqlkpgk=";

        static string EVENT_HUB_NAME = "ppatiernoeventhub";
        static string EVENT_HUB_PARTITION_ID = "0";
        static string EVENT_HUB_PARTITION_KEY = "mykey";
        static string EVENT_HUB_PARTITION_OFFSET = "10000";
        static string EVENT_HUB_PUBLISHER_NAME = "device1";

        static string QUEUE_SEND = "q1";
        static string QUEUE_REPLYTO = "q2";
        static string TOPIC_SEND = "t1";
        static string SUBSCRIPTION_ONE = "s1";
        static string SUBSCRIPTION_TWO = "s2";

        static string SHARED_ACCESS_KEY_NAME = "EventSendKeyName";
        static string SHARED_ACCESS_KEY = "6yBMrTiHeMBXhrYHixtu2Y6u4/mRfiNvRH3TttqXW/w=";

        /*
        static string SB_NAMESPACE = "[SB_NAMESPACE]";
        static string SB_CONNECTION_STRING = "[SB_CONNECTION_STRING]";

        static string EVENT_HUB_NAME = "[EVENT_HUB_NAME]";
        static string EVENT_HUB_PARTITION_ID = "[EVENT_HUB_PARTITION_ID]";
        static string EVENT_HUB_PARTITION_KEY = "[EVENT_HUB_PARTITION_KEY]";
        static string EVENT_HUB_PARTITION_OFFSET = "[EVENT_HUB_PARTITION_OFFSET]";
        static string EVENT_HUB_PUBLISHER_NAME = "[EVENT_HUB_PUBLISHER_NAME]";

        static string QUEUE_SEND = "[QUEUE_SEND]";
        static string QUEUE_REPLYTO = "[QUEUE_REPLYTO]";
        static string TOPIC_SEND = "[TOPIC_SEND]";
        static string SUBSCRIPTION_ONE = "[SUBSCRIPTION_ONE]";
        static string SUBSCRIPTION_TWO = "[SUBSCRIPTION_TWO]";

        static string SHARED_ACCESS_KEY_NAME = "[SHARED_ACCESS_KEY_NAME]";
        static string SHARED_ACCESS_KEY = "[SHARED_ACCESS_KEY]";
        */

        static AutoResetEvent networkAvailableEvent = new AutoResetEvent(false);
        static AutoResetEvent networkAddressChangedEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Amqp.Trace.TraceLevel = Amqp.TraceLevel.Frame | Amqp.TraceLevel.Verbose;
            Amqp.Trace.TraceListener = (f, a) => Debug.Print(DateTime.Now.ToString("[hh:ss.fff]") + " " + Fx.Format(f, a));

            Microsoft.SPOT.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            Microsoft.SPOT.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            
            networkAvailableEvent.WaitOne();
            Debug.Print("link is up!");
            networkAddressChangedEvent.WaitOne();
            Debug.Print("address acquired: " + Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);

            Debug.Print("\r\n*** GET NETWORK INTERFACE SETTINGS ***");
            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface[] networkInterfaces = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            Debug.Print("Found " + networkInterfaces.Length + " network interfaces.");
            
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
            
            scenarios.Scenario8_QueueSend(QUEUE_SEND);
            
            scenarios.Scenario9_QueueRequestResponse(
                QUEUE_SEND, 
                QUEUE_REPLYTO);
            
            scenarios.Scenario11_TopicSend(
                TOPIC_SEND, 
                SUBSCRIPTION_ONE, 
                SUBSCRIPTION_TWO);

            Thread.Sleep(Timeout.Infinite);
        }

        static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("NetworkAddressChanged");
            networkAddressChangedEvent.Set();
        }

        static void NetworkChange_NetworkAvailabilityChanged(object sender, Microsoft.SPOT.Net.NetworkInformation.NetworkAvailabilityEventArgs e)
        {
            Debug.Print("NetworkAvailabilityChanged " + e.IsAvailable);
            if (e.IsAvailable)
            {
                networkAvailableEvent.Set();
            }
        }
    }

#else
    class Program
    {
        static string SB_NAMESPACE = "sb://ppatiernoeventhubs.servicebus.windows.net";
        static string SB_CONNECTION_STRING = "Endpoint=sb://ppatiernoeventhubs.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zyrR1I/Nl6zWpNZJob/BUsIDTXanbyoDfuyaFqlkpgk=";

        static string EVENT_HUB_NAME = "ppatiernoeventhub";
        static string EVENT_HUB_PARTITION_ID = "0";
        static string EVENT_HUB_PARTITION_KEY = "mykey";
        static string EVENT_HUB_PARTITION_OFFSET = "10000";
        static string EVENT_HUB_PUBLISHER_NAME = "device1";

        static string QUEUE_SEND = "q1";
        static string QUEUE_REPLYTO = "q2";
        static string TOPIC_SEND = "t1";
        static string SUBSCRIPTION_ONE = "s1";
        static string SUBSCRIPTION_TWO = "s2";

        static string SHARED_ACCESS_KEY_NAME = "EventSendKeyName";
        static string SHARED_ACCESS_KEY = "6yBMrTiHeMBXhrYHixtu2Y6u4/mRfiNvRH3TttqXW/w=";

        /*
        static string SB_NAMESPACE = "[SB_NAMESPACE]";
        static string SB_CONNECTION_STRING = "[SB_CONNECTION_STRING]";

        static string EVENT_HUB_NAME = "[EVENT_HUB_NAME]";
        static string EVENT_HUB_PARTITION_ID = "[EVENT_HUB_PARTITION_ID]";
        static string EVENT_HUB_PARTITION_KEY = "[EVENT_HUB_PARTITION_KEY]";
        static string EVENT_HUB_PARTITION_OFFSET = "[EVENT_HUB_PARTITION_OFFSET]";
        static string EVENT_HUB_PUBLISHER_NAME = "[EVENT_HUB_PUBLISHER_NAME]";

        static string QUEUE_SEND = "[QUEUE_SEND]";
        static string QUEUE_REPLYTO = "[QUEUE_REPLYTO]";
        static string TOPIC_SEND = "[TOPIC_SEND]";
        static string SUBSCRIPTION_ONE = "[SUBSCRIPTION_ONE]";
        static string SUBSCRIPTION_TWO = "[SUBSCRIPTION_TWO]";

        static string SHARED_ACCESS_KEY_NAME = "[SHARED_ACCESS_KEY_NAME]";
        static string SHARED_ACCESS_KEY = "[SHARED_ACCESS_KEY]";
        */

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

            scenarios.Scenario8_QueueSend(QUEUE_SEND);

            scenarios.Scenario9_QueueRequestResponse(
                QUEUE_SEND,
                QUEUE_REPLYTO);

            scenarios.Scenario11_TopicSend(
                TOPIC_SEND,
                SUBSCRIPTION_ONE,
                SUBSCRIPTION_TWO);

            Console.ReadLine();
        }
    }
#endif
}
