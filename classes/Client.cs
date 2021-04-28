using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer {
    class Client {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;

        public Client(int clientId) {
            id = clientId;
            tcp = new TCP(id);
        }

        public class TCP {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id) {
                id = _id;
            }

            public void Connect(TcpClient _socket) {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                //TODO: sendo welcome packet
            }

            private void ReceiveCallback(IAsyncResult result) {
                try {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0) {
                        //TODO: disconnect
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    //TODO: handle data
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch(Exception ex) {
                    Console.WriteLine($"Error receiving TCP data {ex}");
                    //TODO: disconnect
                }
            }
        }
    }
}