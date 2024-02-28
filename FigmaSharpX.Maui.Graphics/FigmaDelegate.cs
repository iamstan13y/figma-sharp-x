using FigmaSharpX.Converters;
using FigmaSharpX.Maui.Graphics.Converters;
using FigmaSharpX.PropertyConfigure;
using FigmaSharpX.Views;
using System.Reflection;
using IImage = FigmaSharpX.Views.IImage;
using IView = FigmaSharpX.Views.IView;

namespace FigmaSharpX.Maui.Graphics
{
    public class FigmaDelegate : IFigmaDelegate
    {
        public bool IsVerticalAxisFlipped => false;

        public void BeginInvoke(Action handler)
        {
            throw new NotImplementedException();
        }

        public IView CreateEmptyView()
        {
            throw new NotImplementedException();
        }

        public CodePropertyConfigureBase GetCodePropertyConfigure()
        {
            throw new NotImplementedException();
        }

        public NodeConverter[] GetFigmaConverters()
        {
            return new NodeConverter[]{
                new ElipseConverter(),
                new FrameConverter(),  
                new ImageConverter(),
                new LineConverter(),
                new PolygonConverter(),
                new RectangleConverter(),
                new TextConverter()
            };
        }

        public IImage GetImage(string url)
        {
            throw new NotImplementedException();
        }

        public IImage GetImageFromFilePath(string filePath)
        {
            throw new NotImplementedException();
        }

        public IImage GetImageFromManifest(Assembly assembly, string imageRef)
        {
            throw new NotImplementedException();
        }

        public IImageView GetImageView(IImage image)
        {
            throw new NotImplementedException();
        }

        public string GetManifestResource(Assembly assembly, string file)
        {
            throw new NotImplementedException();
        }

        public string GetSvgData(string url)
        {
            throw new NotImplementedException();
        }

        public ViewPropertyConfigureBase GetViewPropertyConfigure()
        {
            throw new NotImplementedException();
        }
    }
}
