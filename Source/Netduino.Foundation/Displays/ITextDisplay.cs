using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays
{
    public interface ITextDisplay
    {
        TextDisplayConfig DisplayConfig { get; }

        void WriteLine(ushort lineNumber, string text);

        void ClearLine(ushort lineNumber);
    }
}
