using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace GameServer {
    class Server {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;

        public static void Start(int maxPlayers, int port) {
            MaxPlayers = maxPlayers;
            Port = port;

            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Server started on port {Port}!");
        } 

        private static void TCPConnectCallback(IAsyncResult result) {
            TcpClient client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++) {
                if (clients[i].tcp.socket == null) {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server is full!");
        }

        private static void InitializeServerData() {
            for (int i = 1; i <= MaxPlayers; i++) {
                clients.Add(i, new Client(i));
            }
        }
    }
}