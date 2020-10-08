using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Demo_core.ExtendViews.ExtendViewModels
{
    class ExtendWindowViewModel : Screen, INotifyPropertyChanged, IHandle<string>
    {
        private readonly IEventAggregator _eventAggregator;
        public ExtendWindowViewModel()
        {
            _eventAggregator = IoC.Get<IEventAggregator>();
            _eventAggregator.SubscribeOnPublishedThread(this);
        }
        private string message;
      public string Message
        {
            get
            {

                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange(() => Message
);
            }
        }
        public Task HandleAsync(string message, CancellationToken cancellationToken)
        {

            Message = message;
            return Task.FromResult(Message);
        }
    }
}
