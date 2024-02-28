﻿using FigmaSharpX.Converters;
using FigmaSharpX.Maui.Graphics.Extensions;
using FigmaSharpX.Models;
using FigmaSharpX.Services;
using System.Globalization;
using System.Text;

namespace FigmaSharpX.Maui.Graphics.Converters
{
    internal class RectangleConverter : RectangleVectorConverterBase
    {
        public override string ConvertToCode(CodeNode currentNode, CodeNode parentNode, ICodeRenderService rendererService)
        {
            if (currentNode.Node is not RectangleVector rectangleVector)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("canvas.SaveState();");

            var bounds = rectangleVector.absoluteBoundingBox;
            float[] rectangleCornerRadii = rectangleVector.rectangleCornerRadii;

            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            if (rectangleVector.HasFills)
            {
                var backgroundPaint = rectangleVector.fills.FirstOrDefault();

                if (backgroundPaint != null && backgroundPaint.visible)
                {
                    if (backgroundPaint.color != null)
                    {
                        builder.AppendLine($"canvas.FillColor  = {backgroundPaint.color.ToCodeString()};");

                        builder.AppendLine($"canvas.Alpha  = {backgroundPaint.color.A};");
                    }

                    if (backgroundPaint.gradientStops != null)
                    {
                        if (backgroundPaint.type.Equals("GRADIENT_LINEAR", StringComparison.CurrentCultureIgnoreCase))
                            builder.AppendLine($"canvas.SetFillPaint({backgroundPaint.gradientStops.ToLinearGradientPaint()}, new RectF({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f));");

                        if (backgroundPaint.type.Equals("GRADIENT_RADIAL", StringComparison.CurrentCultureIgnoreCase))
                            builder.AppendLine($"canvas.SetFillPaint({backgroundPaint.gradientStops.ToRadialGradientPaint()}, new RectF({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f));");
                    }

                    if (backgroundPaint.imageRef != null)
                        builder.AppendLine($"canvas.FillColor  = Colors.White;");

                    if (rectangleCornerRadii != null)
                        builder.AppendLine(string.Format($"canvas.FillRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {rectangleCornerRadii[0].ToString(nfi)}f, {rectangleCornerRadii[1].ToString(nfi)}f, {rectangleCornerRadii[2].ToString(nfi)}f, {rectangleCornerRadii[3].ToString(nfi)}f);"));
                    else
                    {
                        var cornerRadius = rectangleVector.cornerRadius;
                        builder.AppendLine(string.Format($"canvas.FillRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {cornerRadius.ToString(nfi)}f);"));
                    }
                }
            }

            if (rectangleVector.HasStrokes)
            {
                var strokePaint = rectangleVector.strokes.FirstOrDefault();

                if (strokePaint != null && strokePaint.visible)
                {
                    if (strokePaint.color != null)
                    {
                        builder.AppendLine($"canvas.StrokeColor  = {strokePaint.color.ToCodeString()};");

                        builder.AppendLine($"canvas.Alpha  = {strokePaint.color.A};");
                    }

                    if (strokePaint.gradientStops != null)
                    {
                        if (strokePaint.type.Equals("GRADIENT_LINEAR", StringComparison.CurrentCultureIgnoreCase))
                            builder.AppendLine($"canvas.SetFillPaint({strokePaint.gradientStops.ToLinearGradientPaint()}, new RectF({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f));");

                        if (strokePaint.type.Equals("GRADIENT_RADIAL", StringComparison.CurrentCultureIgnoreCase))
                            builder.AppendLine($"canvas.SetFillPaint({strokePaint.gradientStops.ToRadialGradientPaint()}, new RectF({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f));");
                    }

                    if (strokePaint.imageRef != null)
                        builder.AppendLine($"canvas.StrokeColor  = Colors.White;");

                    var strokeSize = rectangleVector.strokeWeight;
                    builder.AppendLine($"canvas.StrokeSize  = {strokeSize};");

                    if (rectangleCornerRadii != null)
                        builder.AppendLine(string.Format($"canvas.DrawRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {rectangleCornerRadii[0].ToString(nfi)}f, {rectangleCornerRadii[1].ToString(nfi)}f, {rectangleCornerRadii[2].ToString(nfi)}f, {rectangleCornerRadii[3].ToString(nfi)}f);"));
                    else
                    {
                        var cornerRadius = rectangleVector.cornerRadius;
                        builder.AppendLine(string.Format($"canvas.DrawRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {cornerRadius.ToString(nfi)}f);"));
                    }
                }
            }

            builder.AppendLine("canvas.RestoreState();");

            return builder.ToString();
        }

        public override Views.IView ConvertToView(FigmaNode currentNode, ViewNode parent, ViewRenderService rendererService)
        {
            throw new NotImplementedException();
        }

        public override Type GetControlType(FigmaNode currentNode)    
            => typeof(View);
    }
}
