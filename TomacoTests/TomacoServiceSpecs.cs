using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TomacoDomain;
using TomacoServer.Server;

namespace TomacoTests
{
    [TestClass]
    public class TomacoServiceSpecs
    {
        [TestMethod]
        public void puedo_iniciar_un_pomodoro()
        {
            var tomacoService = new TomacoService(new TomacoClock(4,5,15,25));
            tomacoService.IniciarPomodoro();
        }
    }
}
