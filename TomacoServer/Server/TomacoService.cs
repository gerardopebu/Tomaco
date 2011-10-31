using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TomacoServer.ServiceContract;

namespace TomacoServer.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class TomacoService : ITomacoService
    {
        private TomacoDomain.TomacoClock tomacoClock;
        public TomacoService() { }
        public TomacoService(TomacoDomain.TomacoClock tomacoClock)
        {
            this.tomacoClock = tomacoClock;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void IniciarPomodoro()
        {
            throw new NotImplementedException();
        }
    }
}
