﻿using FigmaSharpX.Converters;
using FigmaSharpX.Maui.Graphics.Extensions;
using FigmaSharpX.Models;
using FigmaSharpX.Services;
using System.Globalization;
using System.Text;
using IView = FigmaSharpX.Views.IView;

namespace FigmaSharpX.Maui.Graphics.Converters
{
    public class FrameConverter : FrameConverterBase
    {
        public override string ConvertToCode(CodeNode currentNode, CodeNode parentNode, ICodeRenderService rendererService)
        {
            if (currentNode.Node is not FigmaFrame frameNode)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("canvas.SaveState();");

            var bounds = frameNode.absoluteBoundingBox;
            var cornerRadius = frameNode.cornerRadius;

            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            if (frameNode.HasFills)
            {
                var backgroundPaint = frameNode.fills.FirstOrDefault();

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

                    builder.AppendLine(string.Format($"canvas.FillRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {cornerRadius.ToString(nfi)}f);"));
                }
            }

            if (frameNode.HasStrokes)
            {
                var strokePaint = frameNode.fills.FirstOrDefault();

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
                }

                var strokeSize = frameNode.strokeWeight;
                builder.AppendLine($"canvas.StrokeSize  = {strokeSize};");

                builder.AppendLine(string.Format($"canvas.DrawRoundedRectangle({bounds.X.ToString(nfi)}f, {bounds.Y.ToString(nfi)}f, {bounds.Width.ToString(nfi)}f, {bounds.Height.ToString(nfi)}f, {cornerRadius.ToString(nfi)}f);"));
            }

            builder.AppendLine("canvas.RestoreState();");

            return builder.ToString();
        }

        public override IView ConvertToView(FigmaNode currentNode, ViewNode parent, ViewRenderService rendererService)
        {
            throw new NotImplementedException();
        }

        public override Type GetControlType(FigmaNode currentNode)
            => typeof(View);

        public override bool ScanChildren(FigmaNode currentNode)
            => true;
    }
}
