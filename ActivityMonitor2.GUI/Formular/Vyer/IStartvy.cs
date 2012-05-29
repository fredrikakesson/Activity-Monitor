using System;

namespace ActivityMonitor2.GUI.Formular.Vyer
{
    public interface IStartvy
    {
        void VisaGränssnittFörAnvändaren();
        void VisaSomAktiv();
        void VisaSomInaktiv();
        event EventHandler VisaDagsöversikt;
        event EventHandler VisaVeckoöversikt;
        void VisaAktivDel(double procent, bool användarenÄrAktiv);
    }
}