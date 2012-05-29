using System.Collections.Generic;

namespace ActivityMonitor2.GUI.Formular.Vyer
{
    public interface IVeckoöversiktVy
    {
        void VisaGränssnitt();

        void VisaData(IList<Doman.Entiteter.AktivPeriod> iList);
    }
}
