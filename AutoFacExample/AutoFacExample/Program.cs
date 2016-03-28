using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;


/*
 *  FROM: http://autofac.readthedocs.org/en/latest/getting-started/index.html
 *
 */





namespace AutoFacExample
{
    public interface IOutput
    {
        void Write(string content);
    }

    public class ConsoleOutput : IOutput
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }



    public interface IDateWriter
    {
        void WriteDate();
    }

    public class TodayWriter : IDateWriter
    {
        private IOutput _output;
        public TodayWriter(IOutput output)
        {
            this._output = output;
        }

        public void WriteDate()
        {
            this._output.Write(DateTime.Today.ToShortDateString());
        }
    }



    class Program
    {

        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();
            }

            Console.ReadKey();
        }

    }



    //builder.Register(c => new LoggerConfiguration().WriteTo.ColoredConsole().CreateLogger());
    //builder.RegisterType<SerilogOutput>().As<IOutput>();


    public class SerilogOutput : IOutput
    {
        private ILogger _ilogger;

        public SerilogOutput(ILogger logger)
        {
            _ilogger = logger;
        }

        public void Write(string content)
        {
            _ilogger.Information(content);
        }
    }


}
