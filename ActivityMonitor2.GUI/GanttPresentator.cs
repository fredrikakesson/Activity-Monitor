using ActivityMonitor2.Doman;
using ActivityMonitor2.GUI.Formular.Vyer;

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
            _ganttVy.VisaData(_lagring.HämtaAllaPerioder());
            _ganttVy.VisaGränssnitt();
        }
    }
}