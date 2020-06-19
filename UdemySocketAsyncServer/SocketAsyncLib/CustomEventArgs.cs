using System;

namespace SocketAsyncLib
{
    public class ConnectedEventArgs : EventArgs
    {
        public string NewClient { get; set; }

        public ConnectedEventArgs(string newClient)
        {
            NewClient = newClient;
        }

    }

    public class TextReceivedEventArgs : EventArgs
    {
        public string ClientWhoSentText { get; set; }

        public string TextReceived { get; set; }

        public TextReceivedEventArgs(string clientWhoSentText, string textReceived)
        {
            ClientWhoSentText = clientWhoSentText;
            TextReceived = textReceived;
        }
    }

    public class DisconnectedEventArgs : EventArgs
    {
        public string DisconnectedPeer { get; set; }

        public DisconnectedEventArgs(string disconnectedPeer)
        {
            DisconnectedPeer = disconnectedPeer;
        }

    }

}