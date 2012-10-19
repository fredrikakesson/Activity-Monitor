using System.Collections.Generic;
using ActivityMonitor2.Doman.Entiteter;

namespace ActivityMonitor2.GUI.Formular.Vyer
{
    public interface ISpektrumVy
    {
        void VisaGränssnitt();
        void VisaData(IList<AktivPeriod> perioder);
        void VisaDataSaknas();
    }
}