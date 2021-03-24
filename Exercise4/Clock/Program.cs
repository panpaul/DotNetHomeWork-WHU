using System;
using System.Timers;

namespace Clock
{
    internal class Program
    {
        private static int[] _alarmTime;

        private static void Main(string[] args)
        {
            Console.WriteLine("Please Set the Alarm");
            Console.WriteLine("When will the alarm trigger? (in HH:MM:SS)");
            try
            {
                var str = Console.ReadLine()?.Trim().Split(':');
                var num = Array.ConvertAll(str, int.Parse);
                if (num.Length != 3) throw new Exception("Wrong Input");
                _alarmTime = num;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var tick = new Timer(1000);
            tick.Elapsed += Alarm;
            tick.Elapsed += Tick;
            tick.AutoReset = true;

            tick.Start();

            Console.ReadLine();

            tick.Stop();
            tick.Dispose();
        }

        private static void Tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Tick: {e.SignalTime}");
        }

        private static void Alarm(object sender, ElapsedEventArgs e)
        {
            if (e.SignalTime.Hour == _alarmTime[0] &&
                e.SignalTime.Minute == _alarmTime[1] &&
                e.SignalTime.Second == _alarmTime[2])
                Console.WriteLine($"Alarm: {e.SignalTime}");
        }
    }
}