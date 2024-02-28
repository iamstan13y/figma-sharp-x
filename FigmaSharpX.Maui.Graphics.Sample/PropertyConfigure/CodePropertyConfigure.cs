using FigmaSharpX.Converters;
using FigmaSharpX.PropertyConfigure;
using FigmaSharpX.Services;

namespace FigmaSharpX.Maui.Graphics.Sample.PropertyConfigure
{
    public class CodePropertyConfigure : CodePropertyConfigureBase
    {
        public override string ConvertToCode(string propertyName, CodeNode currentNode, CodeNode parentNode, NodeConverter converter, CodeRenderService rendererService)
        {
            return string.Empty;
        }
    }
}
