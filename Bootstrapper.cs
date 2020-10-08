using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Demo_core.Resources;

namespace Demo_core
{
    public interface IShell
    {

    }
    class Bootstrapper: BootstrapperBase
    {
       private CompositionContainer container;
        public Bootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {


            AggregateCatalog _catalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());
            container = new CompositionContainer(_catalog);
            CompositionBatch _batch = new CompositionBatch();
            _batch.AddExportedValue<IWindowManager>(new WindowManager());
            _batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            _batch.AddExportedValue<IResourceTask>(new ResourceTask());
            _batch.AddExportedValue(container);
            _batch.AddExportedValue(_catalog);

            container.Compose(_batch);
            container.ComposeParts(container.GetExportedValue<IResourceTask>());
            Coroutine.Completed += (s, e) =>
            {
                if (e.Error != null)
                    MessageBox.Show(e.Error.Message);
            };
            MessageBinder.SpecialValues.Add("$MySender", Ctx =>
            {
                var _Ui = Ctx.Source as UIElement;
                return _Ui;
            });
        }
        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }
        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }
        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
