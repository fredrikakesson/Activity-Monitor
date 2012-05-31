using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;
using System.Linq;

namespace ActivityMonitor2.GUI
{
    public class GanttPresentatör
    {
        private readonly IGanttVy _ganttVy;
        private readonly ILagring _lagring;

        public GanttPresentatör(IGanttVy ganttVy, ILagring lagring)
        {
            _ganttVy = ganttVy;
            _lagring = lagring;
        }

        public void VisaGränssnitt()
        {
            var perioder = _lagring.HämtaAllaPerioder();

            if (perioder.Any())
                _ganttVy.VisaData(perioder);
            else
                _ganttVy.VisaDataSaknas();
            _ganttVy.VisaGränssnitt();
        }
    }
}