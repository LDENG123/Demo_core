using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Demo_core.ViewModels
{
    class LogViewModel : Screen, INotifyPropertyChanged,IHandle<string>
    {
        #region
        /// <summary>
        /// 通知
        /// </summary>
        private readonly IEventAggregator _eventAggregator;
        /// <summary>
        public LogViewModel()
        {
            _eventAggregator = IoC.Get<IEventAggregator>();
            _eventAggregator.SubscribeOnPublishedThread(this);
        }
        private String message;
        public String Message
        {
            get
            {

                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
        public Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            Message = message;
            return Task.FromResult(Message);
        }
        #endregion
    }
}
