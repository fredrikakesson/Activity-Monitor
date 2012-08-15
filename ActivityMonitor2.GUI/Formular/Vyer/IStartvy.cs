using System;

namespace ActivityMonitor2.GUI.Formular.Vyer
{
    public interface IStartvy
    {
        void VisaGränssnittFörAnvändaren();
        event EventHandler VisaDagsöversikt;
        event EventHandler VisaVeckoöversikt;
        void VisaAktivDel(double procent, bool användarenAktiv);
    }
}