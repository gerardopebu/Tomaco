using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Threading;
using TomacoDomain;

namespace TomacoServerSocket
{
    public class ServerSocket
    {
        TomacoClock _tomacoReloj;
        Thread pServer = null;

        public ServerSocket() { ListaSocketsClientes = new List<Socket>(); }
        IList<Socket> ListaSocketsClientes{get;set;}
        Socket _sock;

        public void CrearServer() 
        {
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 5656);
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _sock.Bind(ipEnd);
            EsperarConexiones();
        }

        private void EsperarConexiones()
        {
            pServer = new Thread(new ThreadStart(esperarConexion));
            pServer.Start();
        }
        private void esperarConexion() 
        {
            while (true)
            {
                _sock.Listen(100);
                Socket clientSock = null;
                clientSock = _sock.Accept();
                ListaSocketsClientes.Add(clientSock);
            }
        }

        public void CerrarServidor()
        {
            if (pServer != null)
            {
                _sock.Close();
                pServer.Abort();
                
            }
        }
        public void ComenzarEnvios() 
        {
            pServer = new Thread(new ThreadStart(comenzarEnvio));
            pServer.Start();
        }
        public void PararTomaco ()
        {
            _tomacoReloj.PararTomaco();
        }
        public void ContinuarTomaco()
        {
            _tomacoReloj.IniciarTrasParada();
        }
        public void EmpezarPomodoro()
        {
            _tomacoReloj.Estado= EstadosTomaco.EnPomodoro;
        }
        public void EmpezarShort()
        {
            _tomacoReloj.Estado = EstadosTomaco.EnShort;
        }
        public void EmpezarLong()
        {
            _tomacoReloj.Estado = EstadosTomaco.EnLong;
        }
        public void ReiniciarTomaco()
        {
            _tomacoReloj = new TomacoClock(4, 5, 15, 25);
        }
        private void comenzarEnvio()
        {            
            ReiniciarTomaco();
            //for (int i = 0; i < 4; i++)
            //string clientDataInString = "";
            EstadosTomaco estadoAnterior = EstadosTomaco.SinIniciar;            
            while (true)
            {




                //byte[] clientData = new byte[1024];
                //int receivedBytesLen = clientSock.Receive(clientData);
                //clientDataInString = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                //Console.WriteLine("Received Data {0}", clientDataInString);
                foreach (var clientSock in ListaSocketsClientes)
                {

                    string clientStr = _tomacoReloj.TiempoRestante;
                    byte[] sendData = new byte[1024];
                    sendData = Encoding.ASCII.GetBytes("Tiempo:" + clientStr);
                    if (clientSock.Connected)
                        try
                        {
                            clientSock.Send(sendData);
                        }
                        catch (Exception e) { 
                            //TODO:Eliminar El cliente de la lista
                            //ListaSocketsClientes.Remove(clientSock); 
                        }
                    if (_tomacoReloj.Estado != estadoAnterior)
                    {
                        string estado = _tomacoReloj.Estado.ToString();
                        byte[] sendDataEstado = new byte[1024];
                        sendDataEstado = Encoding.ASCII.GetBytes("Estado:" + estado);
                        clientSock.Send(sendDataEstado);
                        
                    }
                }
                estadoAnterior = _tomacoReloj.Estado;
                System.Threading.Thread.Sleep(1000);
            }
            foreach (var clientSock in ListaSocketsClientes)
            {
                clientSock.Close();
            }
            System.Console.WriteLine("Aviso Servidor {0}!", "Fin server");
        }
    }
}
