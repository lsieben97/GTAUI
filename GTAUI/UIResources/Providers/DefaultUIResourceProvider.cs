using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.UIResources.Providers
{
    internal class DefaultUIResourceProvider : UIResourceProvider
    {
        /// <inheritdoc/>
        public override object GetEmbeddedResource(string resourceName, Stream uiResourceStream, UIResourceManifestEntry manifestEntry)
        {
            using (StreamReader reader = new StreamReader(uiResourceStream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <inheritdoc/>
        public override object GetFileResource(string resourcePath, UIResourceManifestEntry manifestEntry)
        {
            using (StreamReader reader = new StreamReader(new FileStream(resourcePath, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
