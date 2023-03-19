using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.UIResources.Providers
{
    internal class SpriteResourceProvider : UIResourceProvider
    {
        public override object GetEmbeddedResource(string resourceName, Stream uiResourceStream, UIResourceManifestEntry manifestEntry)
        {
            string extractedPath = ExtractResource(uiResourceStream, resourceName);
            int width = manifestEntry.GetProperty<int>("Width");
            int height = manifestEntry.GetProperty<int>("Height");

            return new SpriteFile(extractedPath, width, height);
        }

        public override object GetFileResource(string resourceName, UIResourceManifestEntry manifestEntry)
        {
            int width = manifestEntry.GetProperty<int>("Width");
            int height = manifestEntry.GetProperty<int>("Height");

            return new SpriteFile(resourceName, width, height);
        }
    }
}
