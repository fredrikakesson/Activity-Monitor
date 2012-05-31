using System.Linq;
using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;

namespace ActivityMonitor2.GUI
{
    public class Veckopresentatör
    {
        private readonly IVeckoöversiktVy _vy;
        private readonly ILagring _lagring;

        public Veckopresentatör(IVeckoöversiktVy veckoöversiktVy, ILagring lagring)
        {
            _vy = veckoöversiktVy;
            _lagring = lagring;
        }

        public void VisaGränssnitt()
        {
            var perioder = _lagring.HämtaSenasteVeckansData();
            if (perioder.Any())
                _vy.VisaData(_lagring.HämtaSenasteVeckansData());
            else
                _vy.VisaDataSaknas();
            _vy.VisaGränssnitt(); 
        }
    }
}
