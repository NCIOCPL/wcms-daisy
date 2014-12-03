using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Messaging;

namespace NCI.Messaging
{
    public class MSMQReceiver : IDisposable
    {
        public delegate void MSMQMessageProcessor(Object message);

        private String _queueName;
        private MSMQMessageProcessor _processor = null;
        private MessageQueue _messageQueue;
        private int _retryWaitSeconds = 5;
        private bool _isConnected = false;
        private bool _isDisposed = false;
        private bool _isProcessing = false;
        private bool _ended = false;

        public MSMQMessageProcessor Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }

        public MSMQReceiver(String queueName, int retryWaitSeconds, MSMQMessageProcessor processor)
        {
            MessageQueue.EnableConnectionCache = false;
            _queueName = queueName;
            _retryWaitSeconds = retryWaitSeconds;
            Processor = processor;
        }

        public void BeginReceiving()
        {
            if (_isConnected)
                throw new NCIMessagingException("Queue is already connected");
            OpenQueueAndReceive();
            _ended = false;
        }

        //To be implemented later. For now call object.Dispose()
        //public void EndReceiving()
        //{
        //    if (!_ended)
        //    {
        //        CloseQueue();
        //        _ended = true;
        //    }
        //}


        /// <summary>
        /// Instantiates a new MessageQueue instance.
        /// </summary>
        private void OpenQueueAndReceive()
        {
            try
            {
                _messageQueue = new MessageQueue(_queueName);
                _isConnected = true;
                _messageQueue.MessageReadPropertyFilter.SetAll();
                _messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(ReceiveCompletedEvent);
                _messageQueue.BeginReceive();

                //add later if using complex types
                //_messageQueue.Formatter = new XmlMessageFormatter(_types);
            }
            catch (MessageQueueException mex)
            {
                if ((mex.MessageQueueErrorCode == MessageQueueErrorCode.MachineNotFound)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.NoResponseFromObjectServer)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.ObjectServerNotAvailable)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.QueueNotAvailable)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.RemoteMachineNotAvailable)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.ServiceNotAvailable)
                    || (mex.MessageQueueErrorCode == MessageQueueErrorCode.WriteNotAllowed)
                    )
                {

                    if ((!_isDisposed) && (!_ended))
                    {
                        CloseQueue();
                        Thread.Sleep(_retryWaitSeconds * 1000);
                        OpenQueueAndReceive();
                    }
                }
                else
                {
                    throw new NCIMessagingException("Fatal MessageQueueException Error:" + mex.MessageQueueErrorCode, mex);
                }
            }
            catch (Exception ex)
            {
                throw new NCIMessagingException("Check queue permissions OR...", ex);
            }
        }

        /// <summary>
        /// Handles the Receive completed event of MessageQueue instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveCompletedEvent(object sender, ReceiveCompletedEventArgs e)
        {
            Message message = null;

            try
            {
                if (e != null)
                {
                    message = e.Message;

                }
                if (message != null)
                {
                    try
                    {
                        _isProcessing = true;
                        Processor(message);
                        _isProcessing = false;
                    }
                    catch
                    {
                        _isProcessing = false;
                        throw new NCIMessagingException("Processor is null");
                    }
                    if (_messageQueue != null)
                    {
                        _messageQueue.EndReceive(e.AsyncResult);
                        _messageQueue.BeginReceive();
                    }
                }
            }
            catch (MessageQueueException mex)
            {
                //Could not communicate with queue,
                //sleep then close and reopen queue
                if ((!_isDisposed) && (!_ended))
                {
                    CloseQueue();
                    Thread.Sleep(_retryWaitSeconds * 1000);
                    OpenQueueAndReceive();
                }

            }
            catch (Exception ex)
            {
                throw new NCIMessagingException("The processor broke.", ex);
                //tear down connection?
            }
        }



        /// <summary>
        /// Closes the current MessageQueue instance, frees the memory, and nullifies the member.
        /// </summary>
        private void CloseQueue()
        {
            if (_messageQueue != null)
            {
                _messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(ReceiveCompletedEvent);
                try
                {
                    //if (_isConnected)
                    //{
                    //    _messageQueue.Close();
                    //    _isConnected = false;
                    //}
                    _messageQueue.Dispose();
                    _messageQueue = null;
                }
                catch (Exception ex)
                {
                    throw new NCIMessagingException("Queue closing failed.", ex);
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._isDisposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    if (_messageQueue != null)
                        _messageQueue.Dispose();

                    _messageQueue = null;
                }

                // Note disposing has been done.
                _isDisposed = true;
            }
        }

        #endregion
    }
}
