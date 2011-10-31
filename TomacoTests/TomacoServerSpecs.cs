using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomacoServer;

namespace TomacoTests
{
    [TestClass]
    public class TomacoServerSpecs
    {
        Uri uri = new Uri("net.tcp://localhost/tomaco");

        [TestMethod]
        public void puedo_iniciar_el_tomaco_server()
        {
            using (var tomacoServer = new TomacoServer.Server.TomacoServer(uri))
            {
                tomacoServer.OpenServer();
                var channelFactory = new ChannelFactory<TomacoServer.ServiceContract.ITomacoService>(new NetTcpBinding(SecurityMode.None),
                    new EndpointAddress(uri));
                var canal = channelFactory.CreateChannel();
                canal.GetData(1);
                channelFactory.Close();

                tomacoServer.CloseServer();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void puedo_parar_el_tomaco_server()
        {
            using (var tomacoServer = new TomacoServer.Server.TomacoServer(uri))
            {
                tomacoServer.OpenServer();
                tomacoServer.CloseServer();

                 var channelFactory = new ChannelFactory<TomacoServer.ServiceContract.ITomacoService>(new NetTcpBinding(SecurityMode.None),
                    new EndpointAddress(uri));
                var canal = channelFactory.CreateChannel();

                canal.GetData(1);
            }
        }

        [TestMethod]
        public void puedo_abrir_un_servidor_ya_abierto_sin_provocar_un_fallo()
        {
            using (var tomacoServer = new TomacoServer.Server.TomacoServer(uri))
            {
                tomacoServer.OpenServer();
                tomacoServer.OpenServer();

            }
        }
    }
}
