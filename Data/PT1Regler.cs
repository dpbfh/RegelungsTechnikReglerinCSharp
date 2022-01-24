using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Regler.Data
{
    public class PT1Regler : IRegler
    {
        public List<double> SignalValuesU { get; }

        double K1 { get; }
        double alpha { get; }

        private List<double> uk { get; }
        private List<double> yk { get; }
        /// <summary>
        /// Regler inizialisieren.
        /// </summary>
        /// <param name="KR"></param>
        /// <param name="TR"></param>
        /// <param name="Ts"></param>
        /// <param name="n">Höchste Zahl k -n default 1 das k-1 machbar ist</param>
        public PT1Regler(double K1, double alpha, int n = 1)
        {
            SignalValuesU = new List<double>();
            this.K1 = K1;
            this.alpha = alpha;

            this.uk = Enumerable.Repeat(0.0, n + 1).ToList();
            this.yk = Enumerable.Repeat(0.0, n + 1).ToList();

        }

        public void read_from_hardware()
        {
            this.uk[0] = 1;
            // Sprung, weil gesucht ist die Sprungantwort

        }

        public void process()
        {
            this.yk[0] = (1 - this.alpha) * this.yk[^1] + this.K1 * this.alpha * this.uk[0];
            this.uk[^1] = this.uk[0];
            this.yk[^1] = this.yk[0];
        }

        public void write_to_hardware()
        {
            SignalValuesU.Add(this.yk[0]);
        }

    }
}
