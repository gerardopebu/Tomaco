using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomacoDomain;
using TomacoClientSocket;
using TomacoServerSocket;

namespace TomacoTests
{
    [TestClass]
    public class TomacoBehaviors
    {
        [TestMethod]
        public void puedo_crear_un_servidor_y_un_cliente_y_puedo_iniciarlos_y_sincronizarlos_obteniendo_el_resultado_del_tomaco()
        {
            ServerSocket sS = new ServerSocket();
            ClientSocket cS = new ClientSocket("localhost");
            sS.CrearServer();
            System.Threading.Thread.Sleep(1000);//tenemos que esperar ya que al ser hilos se llega hasta el final antes de que se hagan el primer envío.            
            cS.CrearCliente();
            System.Threading.Thread.Sleep(1000);
            sS.ComenzarEnvios();
            System.Threading.Thread.Sleep(1000);//tenemos que esperar ya que al ser hilos se llega hasta el final antes de que se hagan el primer envío.
            var tiempo = cS.Valor;
            Assert.IsFalse(string.IsNullOrEmpty(tiempo));
            sS.CerrarServidor();
            cS.CerrarCliente();            

        }
        [TestMethod]
        public void se_realizan_los_cambios_de_estado_segun_la_configuracion()
        {//Simulamos el proceso de 2 pomodoros de 0 segundos llevan a un short y después un long según lo configurado
            TomacoClock t = new TomacoClock(4,0,15,0);
            Assert.AreEqual(EstadosTomaco.EnPomodoro, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnShort, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnPomodoro, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnShort, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnPomodoro, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnShort, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnPomodoro, t.Estado);
            t.ActualizarTiempoRestante();
            Assert.AreEqual(EstadosTomaco.EnLong, t.Estado);
        }
    }
}
