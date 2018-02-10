using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays
{
    public interface ITextDisplay
    {
        TextDisplayConfig DisplayConfig { get; }

        void WriteLine(string text, byte lineNumber);

        void Clear();

        void ClearLine(byte lineNumber);

        void SetBrightness(float brightness = 0.75f);
    }
}
