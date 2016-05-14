using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using OcrEngine;

namespace SaveAsOcr
{
    public class UnityConfig
    {
        private readonly IUnityContainer container = new UnityContainer();

        public MainWindow MainWindow { get { return new Lazy<MainWindow>(() => container.Resolve<MainWindow>()).Value; } }

        public void RegisterContainer()
        {
            container.RegisterType<IPdfConverter, PdfConverter>();
            container.RegisterType<IOcrReader, OcrReader>();
            container.RegisterType<IMainController, MainController>();
            container.RegisterType<MainWindow, MainWindow>();
        }
    }
}
