using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Demo_core.Resources
{
  
        public interface IResource
        {
            string GetString(string name);
            CultureInfo CurrentCulture { set; }
        }

        public interface IResourceTask
        {
            void ChangeLanguage(string language);
            string GetString(string name);
            event EventHandler LanguageChanged;
        }
        public class ResourceTask : IResourceTask
        {
            public ResourceTask()
            {
                System.Data.DataTable _dt = new System.Data.DataTable();

            }
            [ImportMany]
            public IResource[] Resources { get; set; }

            public void ChangeLanguage(string language)
            {
                CultureInfo culture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                Resources.Apply(item => item.CurrentCulture = culture);

                IEventAggregator eventAggregator = IoC.Get<IEventAggregator>();
                eventAggregator.PublishOnCurrentThreadAsync(new LanguageChangedMessage());

                if (LanguageChanged != null)
                {
                    LanguageChanged(this, null);
                }
            }
            public event EventHandler LanguageChanged;

            public string GetString(string name)
            {
                string str = null;

                foreach (var resource in Resources)
                {
                    str = resource.GetString(name);
                    if (str != null)
                    {
                        break;
                    }
                }
                return str;
            }
        }

        public interface IMessage
        {

        }
        public class LanguageChangedMessage : IMessage
        {

        }
    
}
