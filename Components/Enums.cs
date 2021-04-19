using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    [Leskovar]
    public enum LineEndings
    {
        LF,
        CR,
        CRLF,
    }

    [Leskovar]
    public enum BufferType
    {
        Immediate,
        Lazy
    }

    [Leskovar]
    public enum EncodingType
    {
        UTF7,
        UTF8,
        UTF16LE,
        UTF16BE,
        UTF32LE,
        UTF32BE,
    }
}
