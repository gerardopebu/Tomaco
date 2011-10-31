using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TomacoServer.ServiceContract;

namespace TomacoServer.Server
{
    public class TomacoServer:IDisposable
    {
        ServiceHost host=null;
        Uri _address = null;

        public TomacoServer(Uri address)
        {
            _address = address;
        }

        public void OpenServer()
        {
            if (host == null || host.State == CommunicationState.Faulted)
            {
                host = new ServiceHost(typeof(TomacoService), _address);
                host.AddServiceEndpoint(typeof(ITomacoService), new NetTcpBinding(SecurityMode.None), "");
            }
            if(host.State!=CommunicationState.Opened && host.State!=CommunicationState.Opening)
                host.Open();
            Console.WriteLine(String.Format("Server open in {0}", _address));
        }

        ~TomacoServer()
        {
            CloseServer();
        }

        public void Dispose()
        {
            CloseServer();
        }

        public void CloseServer()
        {
            if (host != null && (host.State == CommunicationState.Opened || host.State == CommunicationState.Opening))
                host.Close();
        }

        public void StartPomodoro()
        {
            throw new NotImplementedException();
        }
    }
}
