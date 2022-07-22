// Ishan Pranav's REBUS: ZoneVisualizer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Rebus.Client
{
    public class ZoneVisualizer : SKDrawable
    {
        private readonly IEnumerable<ZoneResult> _zones;
        private readonly int _playerId;
        private readonly Layout _layout;

        public ZoneVisualizer(IEnumerable<ZoneResult> zones, int playerId, Layout layout)
        {
            _zones = zones;
            _playerId = playerId;
            _layout = layout;
        }

        protected override void OnDraw(SKCanvas canvas)
        {
            canvas.Clear(new SKColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue));

            Dictionary<HexPoint, SKPath> hexagons = new Dictionary<HexPoint, SKPath>();

            using (SKFont font = new SKFont(SKTypeface.Default, size: (_layout.X + _layout.Y) * 0.05f))
            using (SKPaint paint = new SKPaint(font)
            {
                StrokeJoin = SKStrokeJoin.Bevel,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center
            })
            {
                foreach (ZoneResult zone in _zones)
                {
                    byte red;
                    byte green;
                    byte blue;
                    const byte mid = (byte)sbyte.MaxValue;

                    switch (zone.Depth)
                    {
                        case Depth.Star:
                            red = byte.MaxValue;
                            green = byte.MaxValue;
                            blue = byte.MaxValue;
                            break;

                        case Depth.Planet:
                            red = byte.MinValue;
                            green = mid;
                            blue = byte.MinValue;
                            break;

                        default:
                            red = byte.MinValue;
                            green = byte.MinValue;
                            blue = byte.MinValue;
                            break;
                    }

                    if (zone.Value.Units.Count == 0)
                    {
                        paint.Color = new SKColor(red, green, blue, mid);
                    }
                    else
                    {
                        paint.Color = new SKColor(red, green, blue);
                    }

                    SKPath path = _layout.GetHexagon(zone.Value.Location);

                    hexagons.Add(zone.Value.Location, path);

                    canvas.DrawPath(path, paint);
                }

                float strokeWidth = Math.Clamp((_layout.X + _layout.Y) * 0.005f, min: 0, max: 10);

                foreach (ZoneResult zone in _zones)
                {
                    if (zone.Value.Units.Count > 0)
                    {
                        paint.StrokeWidth = strokeWidth;
                        paint.Style = SKPaintStyle.Stroke;

                        if (zone.Value.PlayerId == _playerId)
                        {
                            paint.Color = SKColors.DodgerBlue;
                        }
                        else
                        {
                            paint.Color = SKColors.Red;
                        }

                        canvas.DrawPath(hexagons[zone.Value.Location], paint);

                        paint.Style = SKPaintStyle.Fill;

                        string text = zone.Value.Units.Count.ToString();
                        SKPoint point = _layout.GetCenter(zone.Value.Location);

                        point.Y += font.Size * 0.5f;

                        canvas.DrawText(text, point, paint);

                        paint.Color = SKColors.White;
                        paint.StrokeWidth = 0;
                        paint.Style = SKPaintStyle.Stroke;

                        canvas.DrawText(text, point, paint);
                    }
                }
            }
        }
    }
}
