# Adapters

The Adapter is part of the [Activity Process Stack.](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0&tabs=csharp#the-activity-processing-stack) It's a piece of code that allows the connection between the bot and communication apps. You configure an adapter into a bot to connect it to the communication app you want it to be available on. You can use an adapter to connect your bot with several popular services, such as Slack, Facebook Messenger, Webex and Twilio. 



## How it works

In conversation, the adapter is a component mediator between the communication apps and the bot's logic.

![](C:\Users\BillyDelgado\Desktop\Adapters\media\bot-builder-adapter-work-flow.png)



In the example above, the bot replied to the message activity with another message activity containing the same text message. 

The adapter is the core of the SDK runtime. It receives an HTTP POST request with the message activity from the controller. 

This request is deserialized using the *Process Async* method to create a *Turn Context* object. Then, it passes this object to the bot's logic. 

The *Turn Context* object provides the mechanism for the bot to send outbound activities, most often in response to an inbound activity. To achieve this, the turn context provides *send, update, and delete activity* response methods. 



## **Specific Flavors**

Currently, the BotBuilder SDK provides you with adapters to connect your bot with the next communication apps:

- Slack
- Webex
- Twilio 
- Facebook Messenger

Most channels require that your bot have an account in the communication app client. Then, you must provide some configuration variables to make the integration with your bot.