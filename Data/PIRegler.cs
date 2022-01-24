using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Regler.Data
{
    public class PIRegler : IRegler
    {
        public List<double> SignalValuesU { get; }

        double KR { get; }
        double TR { get; }
        double Ts { get; }

        private List<double> ek { get; }
        private List<double> uk { get; }
        /// <summary>
        /// Regler inizialisieren.
        /// </summary>
        /// <param name="KR"></param>
        /// <param name="TR"></param>
        /// <param name="Ts"></param>
        /// <param name="n">Höchste Zahl k -n default 1 das k-1 machbar ist</param>
        public PIRegler(double KR, double TR, double Ts, int n = 1)
        {
            SignalValuesU = new List<double>();
            this.KR = KR;
            this.TR = TR;
            this.Ts = Ts;
            this.uk = Enumerable.Repeat(0.0, n + 1).ToList();
            this.ek = Enumerable.Repeat(0.0, n + 1).ToList();

        }

        public void read_from_hardware()
        {
            this.ek[0] = 1;
            // Sprung, weil gesucht ist die Sprungantwort

        }

        public void process()
        {
            this.uk[0] = this.uk[^1] + this.KR * ((this.TR + this.Ts) * this.ek[0] - this.TR * this.ek[^1]);
            this.uk[^1] = this.uk[0];
            this.ek[^1] = this.ek[0];
        }

        public void write_to_hardware()
        {
            SignalValuesU.Add(this.uk[0]);
        }

    }
}
