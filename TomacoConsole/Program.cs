using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using TomacoDomain;
using TomacoClientSocket;
using TomacoServerSocket;

namespace TomacoConsole
{
    
    class Program
    {
        static void Main(string[] args)
        {             
            ServerSocket ServerSock=null;
            ClientSocket ClienteSock=null;
            
            string hostServer;

            if (args[0] == "server")
            {
                ServerSock = new ServerSocket();
                ServerSock.CrearServer();                                  
                ServerSock.ComenzarEnvios();
            }

            if (args[0] == "client")
            {
                hostServer = args[1];
                ClienteSock = new ClientSocket(hostServer);
                ClienteSock.CrearCliente();
            }

            Resultado R = new Resultado(ClienteSock);
            Thread pMostrar;
            pMostrar = new Thread(new ThreadStart(R.Mostrar));
            pMostrar.Start();

            string comando = "salir";
            while (comando != Console.ReadLine())
            {
                if (ClienteSock!=null)
                    System.Console.WriteLine(ClienteSock.Valor);
            }

            if (ClienteSock!=null)
                ClienteSock.CerrarCliente();

            if (ServerSock != null)
            {
                ServerSock.CerrarServidor();
            }
            System.Console.WriteLine("Aviso Programa {0}!", "Fin Program");


        }
        public class Resultado 
        {
            ClientSocket ClientSock;
            public Resultado(ClientSocket c)
            {
                ClientSock = c;
            }
            public void Mostrar()
            {
                while (true)
                {
                    System.Console.Clear();
                    System.Console.WriteLine(ClientSock.Valor);
                }                
            }
        }
    }
}
