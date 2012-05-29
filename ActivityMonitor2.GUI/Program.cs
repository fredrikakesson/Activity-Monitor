using System;
using System.Windows.Forms;
using ActivityMonitor2.Doman;
using ActivityMonitor2.Doman.Aktivitetsdetektor;
using ActivityMonitor2.Doman.Ikongenerator;
using ActivityMonitor2.GUI.Formular;

namespace ActivityMonitor2.GUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            const string connectionString =
                @"Data Source=c:\temp\ActivityMonitor.db;Pooling=true;FailIfMissing=false;Version=3";

            var detektor = new Aktivitetsdetektor(new SystemTimer(10), new Användaraktivitet(120), new Strömspararkontroll()); // 10,120
            var lagring = new SqliteLagring(connectionString);
            var ikongenerator = new Ikongenerator();
            using (var o = new Övervakare(detektor, lagring))
            {

                Vyer.Veckoöversikt = new Veckopresentatör(new Veckoformulär(), lagring);
                Vyer.Gantt = new GanttPresentatör(new Ganttformulär(), lagring);
                Vyer.Huvud = new Presentatör(new Huvudformulär(ikongenerator), detektor, lagring, new SystemTimer(10));
                    // 10
                Vyer.Huvud.VisaGanttschema += (s, e) => Vyer.Gantt.VisaGränssnitt();
                Vyer.Huvud.VisaVeckoöversikt += (s, e) => Vyer.Veckoöversikt.VisaGränssnitt();

                Vyer.Huvud.VisaGränssnitt();
            }
        }
    }

    internal class Vyer
    {
        public static Presentatör Huvud;
        public static GanttPresentatör Gantt;
        public static Veckopresentatör Veckoöversikt;
    }
}