using System;

namespace TomacoDomain
{   
    public enum EstadosTomaco { SinIniciar,EnPomodoro,EnShort,EnLong,EnPausa}
    public class TomacoClock
    {

        private int _numPomodorosRealizadosSinLong;

        //Todo:Clase Config
        private int numPomodorosToLongConfig = 4;
        private int minutosShortConfig = 5;
        private int minutoslongConfig = 15;
        private int minutosPomodoroConfig = 25;

        private EstadosTomaco _estadoAnteriorAntesDeParada;
        private EstadosTomaco _estado;
        public EstadosTomaco Estado
        {
            get
            { 
                return _estado;
            }
            set//De momento no es privado ya que permitimos el paso de un estado a otro sin seguir la configuracion
            {
                _estado = value;
                switch (value) 
                {
                    case EstadosTomaco.EnLong:
                        _numPomodorosRealizadosSinLong=0;
                        _inicio = DateTime.Now.AddMinutes(minutoslongConfig);
                        break;
                    case EstadosTomaco.EnShort:
                        //_numPomodorosRealizadosSinLong++;
                        _inicio = DateTime.Now.AddMinutes(minutosShortConfig);
                        break;
                    case EstadosTomaco.EnPomodoro:
                        _numPomodorosRealizadosSinLong++;
                        _inicio = DateTime.Now.AddMinutes(minutosPomodoroConfig);
                        break;
                }
            }
        }

        private DateTime _inicio;
        private int _minutosRestantes;
        private int _segundosRestantes;
        private int _minutosParada;
        private int _segundosParada;
        public bool Parado { get; private set; }
        public string TiempoRestante 
        { 
            get 
            { 
                ActualizarTiempoRestante();
                return _minutosRestantes.ToString("00:").Replace('-', ' ') + _segundosRestantes.ToString("00").Trim('-');
             } 
        }

        public TomacoClock(int numPomodorosToLongConfig, int minutosShortConfig, int minutoslongConfig, int minutosPomodoroConfig)
        {
            this.numPomodorosToLongConfig= numPomodorosToLongConfig;
            this.minutosShortConfig = minutosShortConfig;
            this.minutoslongConfig = minutoslongConfig;
            this.minutosPomodoroConfig = minutosPomodoroConfig;
            Estado = EstadosTomaco.SinIniciar;
            IniciarTomaco();    
        }
        public void IniciarTomaco()
        {
            Estado = EstadosTomaco.EnPomodoro;
            _inicio = DateTime.Now.AddMinutes(minutosPomodoroConfig);
        }
        public void IniciarTrasParada() 
        {
            Estado = _estadoAnteriorAntesDeParada;
            _inicio = DateTime.Now.AddMinutes(_minutosParada).AddSeconds(_segundosParada);
        }
        public void PararTomaco()
        {
            _estadoAnteriorAntesDeParada = Estado;
            Estado = EstadosTomaco.EnPausa;
            guardarTiempoParada();
        }
        private void guardarTiempoParada() 
        {
            _minutosParada = -1*_minutosRestantes;
            _segundosParada = -1*_segundosRestantes;
        }
        public void ActualizarTiempoRestante()
        {
            if (Estado!=EstadosTomaco.EnPausa)
            {

                DateTime actual;
                TimeSpan duracion;
                double segundosTotales;
                double minutosTotales;
                actual = DateTime.Now;
                duracion = actual - _inicio;

                segundosTotales = duracion.TotalSeconds;
                minutosTotales = duracion.TotalMinutes;
                _minutosRestantes = duracion.Minutes;
                _segundosRestantes = duracion.Seconds;
                if (_segundosRestantes > 60) _segundosRestantes = _segundosRestantes / 60;

                controlarEstado();

            }
        }
        private void controlarEstado()
        {
            if (_segundosRestantes == 0 && _minutosRestantes == 0) 
            {
                cambiarASiguienteEstado();
            //Enviar Mensaje y Pasar a siguiente estado
            }
        }
        private void cambiarASiguienteEstado() 
        { 
            switch (Estado)
            {
                case EstadosTomaco.EnLong:
                    Estado = EstadosTomaco.EnPomodoro;
                    break;
                case EstadosTomaco.EnShort:
                    Estado = EstadosTomaco.EnPomodoro;
                    break;
                case EstadosTomaco.EnPomodoro:
                    if (_numPomodorosRealizadosSinLong == numPomodorosToLongConfig)
                    {
                        Estado = EstadosTomaco.EnLong;
                    }
                    else Estado = EstadosTomaco.EnShort;
                    break;
            }
        }
    }
    

}
