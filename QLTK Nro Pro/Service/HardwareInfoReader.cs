using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QLTK_Nro_Pro.Service
{
    internal class ProcessResourceMonitor
    {
        private readonly Dictionary<int, TimeSpan> _lastCpu = new();
        private readonly int _core = Environment.ProcessorCount;
        private const int INTERVAL = 500;

        private readonly PerformanceCounter _cpuTotal =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

        private readonly PerformanceCounter _ramAvail =
            new PerformanceCounter("Memory", "Available MBytes");

        private readonly float _ramTotal =
            new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1024f / 1024f;

        public string GetProcessUsage(Dictionary<int, int> tabs)
        {
            float cpuHeThong = _cpuTotal.NextValue();
            float ramHeThong = ((_ramTotal - _ramAvail.NextValue()) / _ramTotal) * 100;

            if (cpuHeThong < 0.3f) cpuHeThong = 0;
            if (ramHeThong < 0.3f) ramHeThong = 0;

            double cpuTabTong = 0;
            double ramTabTong = 0;

            StringBuilder sb = new StringBuilder(256);

            sb.Append($"HỆ THỐNG | CPU {cpuHeThong:F0}% | RAM {ramHeThong:F0}%\n");
            sb.Append("TAB GAME | ");

            foreach (var kv in tabs)
            {
                int id = kv.Key;
                int pid = kv.Value;

                try
                {
                    var p = Process.GetProcessById(pid);

                    double ramMb = p.WorkingSet64 / 1024d / 1024d;
                    ramTabTong += ramMb;

                    double cpu = 0;
                    var cur = p.TotalProcessorTime;

                    if (_lastCpu.TryGetValue(pid, out var last))
                    {
                        cpu = (cur - last).TotalMilliseconds / (INTERVAL * _core) * 100;
                        if (cpu < 0.2) cpu = 0;
                        cpuTabTong += cpu;
                    }

                    _lastCpu[pid] = cur;

                    sb.Append($"Tab{id} {cpu:F0}% {ramMb:F0}MB | ");
                }
                catch { }
            }

            sb.Append($"\nTỔNG TAB | CPU {cpuTabTong:F0}% | RAM {ramTabTong:F0}MB");

            return sb.ToString();
        }
    }
}
