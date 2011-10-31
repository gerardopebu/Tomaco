using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Caliburn.Micro;

namespace TomacoClientSocket
{   
    public class ClientSocket
    {
        public EventAggregator EventAgregator;
        Socket _clientSock;
        private string _valor;
        public string Valor 
        {             
            get{return _valor;} 
            private set{ _valor = value;
            EventAgregator.Publish(new MessageCambioValor() { ValorCambio = Valor });
            } 
        }
        private string _estadoTomaco;
        public string EstadoTomaco
        {
            get { return _estadoTomaco; }
            private set
            {
                if (_estadoTomaco!=value)
                    EventAgregator.Publish(new MessageCambioEstado() { ValorCambioEstado = value });
                _estadoTomaco = value;
                
            }
        }

        Thread pClient = null;
        string HostServer;
        public ClientSocket(string hostServer) 
        {
            EventAgregator = new EventAggregator();
            HostServer = hostServer;
        }

        public void CrearCliente() 
        {
            pClient = new Thread(new ThreadStart(comenzar));
            pClient.Start();
        }
        //Quitar Tomaco de aquí!
        private void comenzar() 
        {
            IPEndPoint ipEnd;
            IPAddress[] ipAddress = Dns.GetHostAddresses(HostServer);
            if (ipAddress.Count() == 1)
                ipEnd = new IPEndPoint(ipAddress[0], 5656);
            else
                ipEnd = new IPEndPoint(ipAddress[1], 5656);

            _clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _clientSock.Connect(ipEnd);



            //string leidoTeclado = "";
            //var leidoTeclado=Console.ReadLine();
            while (true)
            {
                try
                {
                    //string strData = "Message from client end.";
                    //byte[] clientData = new byte[1024];
                    //clientData = Encoding.ASCII.GetBytes(strData);
                    //leidoTeclado = Console.ReadLine();
                    //clientSock.Send(clientData);

                    byte[] serverData = new byte[1024];

                    int len = _clientSock.Receive(serverData);
                    //System.Console.Clear();
                    //System.Console.WriteLine("Cliente Recive: " + Encoding.ASCII.GetString(serverData, 0, len));
                    if (_clientSock.Connected)
                    {
                        var datosRecibidos = Encoding.ASCII.GetString(serverData, 0, len);
                        if (datosRecibidos.StartsWith("Tiempo:"))
                            Valor = datosRecibidos.Substring("Tiempo:".Length);
                        if (datosRecibidos.StartsWith("Estado:")) 
                        {
                            EstadoTomaco = datosRecibidos.Substring("Estado:".Length);
                        }
                    }   
                }
                catch { System.Console.WriteLine("Error Cliente {0}!", "ERROR"); }
            }
            _clientSock.Close();
            System.Console.WriteLine("Aviso Cliente {0}!", "Fin Cliente");



        }
        public void CerrarCliente()
        {
            if (pClient != null) 
            {
                _clientSock.Close();
                pClient.Abort();
            }
            
        }

    }
    public interface IMessageCambioValor
    {
        string ValorCambio{get;set;}
    }
    public class MessageCambioValor : IMessageCambioValor 
    {
        public string ValorCambio{get;set;}
    }
    public interface IMessageCambioEstado
    {
        string ValorCambioEstado { get; set; }
    }
    public class MessageCambioEstado : IMessageCambioEstado
    {
        public string ValorCambioEstado { get; set; }
    }
}
