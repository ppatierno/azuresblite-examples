# azuresblite-examples
Example scenarios to use Azure SB Lite library to access Azure Service Bus (Event Hubs, Queues and Topics/Subscription).

Current examples are for :

* .Net Framework (even if you can use official Service Bus Nuget package)
* .Net Micro Framework
* WinRT

Simple scenarios covered are :

* Simple send to Event Hub 
* Send to Event Hub to a specific partition 
* Send to Event Hub to a publisher 
* Send to Event Hub to a publisher with token 
* Send to Event Hub with a partition key 
* Receive from Event Hub from specific partition 
* Receive from Event Hub from specific partition with offset
* Receive from Event Hub from specific partition with date/time offset
* Simple send to a Queue 
* Send to a Queue and receive from a ReplyTo Queue 
* Send to a Topic and receive from Sunscriptions

To build these examples you need following libraries :

* *AMQP SB Lite* : http://azuresblite.codeplex.com/ (you need to clone from here)

* *AMQP .Net Lite* : http://amqpnetlite.codeplex.com/ (related Nuget packages are already referenced)

The .Net Micro Framework project is build for running on Netduino 3 Wi-Fi.

The IoTEventHubProcessor project is an example for receiving event data sent using event hub following above scenarios.
