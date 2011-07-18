using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace NCI.Messaging
{
    public class MSMQSender
    {
        private String _queueName;

        public MSMQSender(String queueName)
        {
            _queueName = queueName;
            MessageQueue.EnableConnectionCache = false;
        }

        /// <summary>
        /// Adds a new object to the current Message Queue as the Body propery of a Message object.
        /// </summary>
        /// <param name="nciMessage"></param>
        public void AddToQueue(Object body)
        {
            String label = "";
            SendMessage(body, label);
        }

        /// <summary>
        /// Adds a new object to the current Message Queue as the Body propery of a Message object.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="label"></param>
        public void AddToQueue(Object body, String label)
        {
            SendMessage(body, label);
        }

        private void SendMessage(Object body, String label)
        {
            try
            {
                using (MessageQueue messageQueue = new MessageQueue(_queueName))
                {
                    //add later if using complex types
                    //_types = new Type[1];
                    //_types[0] = typeof(NCIMessage);
                    //Message message = new Message(body, new XmlMessageFormatter(_types));
                    Message message = new Message(body);
                    message.Label = label;
                    message.Recoverable = true;
                    try
                    {
                        messageQueue.Send(message);
                    }
                    catch (MessageQueueException mex)
                    {
                        throw new NCIMessagingException("Could not send", mex);
                    }
                }
            }
            catch (ArgumentException argex)
            {
                throw new NCIMessagingException("ArgumentException", argex);
            }
        }
    }
}
