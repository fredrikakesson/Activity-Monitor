using System;
using System.Collections.Generic;
using ActivityMonitor2.Doman.Entiteter;

namespace ActivityMonitor2.Doman
{
    public interface ILagring
    {
        void Spara(AktivPeriod period);
        IList<AktivPeriod> HämtaAllaFörEnVissDag(DateTime dag);
        IList<AktivPeriod> HämtaSenasteVeckansData();
        IList<AktivPeriod> HämtaAllaPerioder();
    }
}