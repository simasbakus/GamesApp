using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesApp
{
    [ContentProperty (nameof(Source))]
    class ImageResourceExt : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExt).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
