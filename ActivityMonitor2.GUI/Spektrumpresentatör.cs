using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;

namespace ActivityMonitor2.GUI
{
    class Spektrumpresentatör
    {
        private readonly ISpektrumVy _vy;
        private readonly ILagring _lagring;

        public Spektrumpresentatör(ISpektrumVy vy, ILagring lagring)
        {
            _vy = vy;
            _lagring = lagring;
        }

        public void VisaGränssnitt()
        {
            var perioder = _lagring.HämtaSenasteMånadensData();
            if (perioder.Any())
                _vy.VisaData(perioder);
            else
                _vy.VisaDataSaknas();
            _vy.VisaGränssnitt(); 
        }
    }
}
