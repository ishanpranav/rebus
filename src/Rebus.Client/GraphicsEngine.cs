// Ishan Pranav's REBUS: GraphicsEngine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using SkiaSharp;

namespace Rebus.Client
{
    public class GraphicsEngine
    {
        private readonly int _width;
        private readonly int _height;

        public GraphicsEngine(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Draw(SKWStream output, SKEncodedImageFormat format, SKDrawable drawable)
        {
            using (SKBitmap bitmap = new SKBitmap(_width, _height))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear();

                    drawable.Draw(canvas, x: 0, y: 0);
                }

                bitmap.Encode(output, format, quality: 100);
            }
        }
    }
}
