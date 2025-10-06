using System;
using System.Collections.Generic;

namespace Bitstream
{
    class UIComponent
    {
        // UI 시작점
        public readonly int StartX;
        public readonly int StartY;

        // UI 크기
        public readonly int Width;
        public readonly int Height;

        public UIComponent(int x, int y, int width, int height)
        {
            StartX = x;
            StartY = y;
            Width = width;
            Height = height;
        }
    }
}
