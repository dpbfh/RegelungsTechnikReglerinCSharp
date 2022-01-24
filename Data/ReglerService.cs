using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Regler.Data
{
    public class ReglerService
    {

        public (List<double>, List<double>) GetPIRegler()
        {

            var Ts = 0.01;

            var KR = 5;

            var TR = 1;

            var controller = new PIRegler(KR: KR, TR: TR, Ts: Ts);
            if (!controller.SignalValuesU.Any())
            {

                var sim_time = 5;
                var sim_time_start = DateTimeOffset.Now.ToUnixTimeSeconds();
                while (DateTimeOffset.Now.ToUnixTimeSeconds() - sim_time_start <= sim_time)
                {

                    controller.read_from_hardware();
                    controller.process();
                    controller.write_to_hardware();
                    Thread.Sleep((int)(Ts * 1000));
                }


                System.Console.WriteLine(controller.SignalValuesU.Count);
            }
            var timevalues = new List<double>();
            for (int i = 0; i < controller.SignalValuesU.Count; i++)
            {
                timevalues.Add(i * Ts);
            }
            return (controller.SignalValuesU, timevalues);
        }

        public (List<double>, List<double>) GetPT1Regler()
        {

            var Ts = 0.01;

            var K1 = 1;
            var T1 = 1;

            var controller = new PT1Regler(K1, 1 / (1 + T1 / Ts));
            if (!controller.SignalValuesU.Any())
            {

                var sim_time = 5;
                var sim_time_start = DateTimeOffset.Now.ToUnixTimeSeconds();
                while (DateTimeOffset.Now.ToUnixTimeSeconds() - sim_time_start <= sim_time)
                {

                    controller.read_from_hardware();
                    controller.process();
                    controller.write_to_hardware();
                    Thread.Sleep((int)(Ts * 1000));
                }

            }
            var timevalues = new List<double>();
            for (int i = 0; i < controller.SignalValuesU.Count; i++)
            {
                timevalues.Add(i * Ts);
            }
            return (controller.SignalValuesU, timevalues);
        }

         public string GetZForm(string Laplace)
        {

          string s = "((1/Ts)*(1-z^(-1)))";
          return Laplace.Replace("*s", s);
        }
    }
}
