using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Resources;
using System.Text;

namespace Demo_core.Resources
{
    [Export(typeof(IResource))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class Resource_Language : IResource
    {
        private ResourceManager stringResource;
        private CultureInfo culture = new CultureInfo("zh-cn");
        public CultureInfo CurrentCulture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value;
            }
        }

       

        public Resource_Language()
        {
            stringResource = new ResourceManager("Demo_core.Resources.Language-CH", typeof(Resource_Language).Assembly);
        }


        public string GetString(string name)
        {
            return stringResource.GetString(name, culture);
        }

    }
}
