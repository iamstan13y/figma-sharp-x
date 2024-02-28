using FigmaSharpX.Converters;
using FigmaSharpX.Models;
using FigmaSharpX.Services;
using System.Text;
using IView = FigmaSharpX.Views.IView;

namespace FigmaSharpX.Maui.Graphics.Converters
{
    internal class PolygonConverter : RegularPolygonConverterBase
    {
        public override string ConvertToCode(CodeNode currentNode, CodeNode parentNode, ICodeRenderService rendererService)
        {
            if (currentNode.Node is not FigmaRegularPolygon figmaPolygon)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("canvas.SaveState();");

            // TODO: Render Polygon

            builder.AppendLine("canvas.RestoreState();");

            return builder.ToString();
        }

        public override IView ConvertToView(FigmaNode currentNode, ViewNode parent, ViewRenderService rendererService)
        {
            throw new NotImplementedException();
        }

        public override Type GetControlType(FigmaNode currentNode)
            => typeof(View);
    }
}
