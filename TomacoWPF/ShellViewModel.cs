using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using TomacoDomain;
using TomacoClientSocket;
using TomacoServerSocket;

namespace TomacoWPF
{
    public class ShellViewModel: PropertyChangedBase,IHandle<IMessageCambioValor>,IHandle<IMessageCambioEstado>
    {      
        private EventAggregator _eventAggregator;
        private ServerSocket _serverSock;
        private ClientSocket _clientSock;
        public string TiempoYEstado { get; set; }
        private string _tiempoRestante;
        public string TiempoRestante 
        { 
            get {                 
                    return _tiempoRestante;                                
                }
            set {
                _tiempoRestante = value;
                }
        }
        public string Estado { get; set; }
        public string IPServer { get; set; }
        public ShellViewModel() 
        {
            
        }
        public void CrearServidor() 
        {
            _serverSock = new ServerSocket();
            _serverSock.CrearServer();
            _serverSock.ComenzarEnvios();            
        }
        public void CrearCliente()
        {
            _clientSock = new ClientSocket(IPServer);
            _eventAggregator = _clientSock.EventAgregator;
            _eventAggregator.Subscribe(this);
            _clientSock.CrearCliente();            
        }
        public void Parar() 
        {
            _serverSock.PararTomaco();
        }

        public void Continuar()
        {
            _serverSock.ContinuarTomaco();
        }
        public void EmpezarPomodoro()
        {
            _serverSock.EmpezarPomodoro();
        }
        public void EmpezarShort()
        {
            _serverSock.EmpezarShort();
        }
        public void EmpezarLong()
        {
            _serverSock.EmpezarLong();
        }
        public void ReiniciarTomaco()
        {
            _serverSock.ReiniciarTomaco();
        }
        public void Handle(IMessageCambioValor message)
        {
            TiempoRestante = message.ValorCambio;
            NotifyOfPropertyChange(() => TiempoRestante);
            TiempoYEstado = TiempoRestante + " " + Estado;
            NotifyOfPropertyChange(() => TiempoYEstado);
        }

        public void Handle(IMessageCambioEstado message)
        {
            Estado = message.ValorCambioEstado;
            NotifyOfPropertyChange(() => Estado);
            var mplayer = new System.Windows.Media.MediaPlayer();
            mplayer.Open(new Uri("beep.mp3", UriKind.Relative));
            mplayer.Play();
        }
        public void CerrarServidorYCliente()
        {
            if (_clientSock!=null)
                _clientSock.CerrarCliente();

            if (_serverSock != null)
            {
                _serverSock.CerrarServidor();
            }
        }
    }
}
