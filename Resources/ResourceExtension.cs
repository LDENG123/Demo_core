using Caliburn.Micro;
using Demo_core.Resources;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Demo_core
{
    class ResourceExtension : MarkupExtension, INotifyPropertyChanged, IHandle<LanguageChangedMessage>
    {
        public string Key
        {
            get;
            set;
        }
        public string Value
        {
            get
            {
                if (Key == null)
                {
                    return null;
                }
                IResourceTask result = IoC.Get<IResourceTask>();
                string s = result.GetString(Key);
                return s;
            }
        }
        public ResourceExtension()
        {

            if (!Execute.InDesignMode)
            {
                IoC.Get<IEventAggregator>().SubscribeOnUIThread(this);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async Task PropertyChanted()
        {
            if (PropertyChanged != null)
            {
             PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            Binding binding = new Binding("Value") { Source = this, Mode = BindingMode.OneWay };
            return binding.ProvideValue(serviceProvider) as BindingExpression;
        }

       

        public async Task HandleAsync(LanguageChangedMessage message, CancellationToken cancellationToken)
        {
          await  PropertyChanted();
        }
    }
}
