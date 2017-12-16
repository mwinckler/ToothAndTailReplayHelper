using System;
using System.Collections.Generic;

namespace ToothAndTailReplayHelper.Model
{
    internal interface IFilenameTokenParser
    {
        List<Tuple<FilenameToken, string>> ParseTokens(string fileNamingPattern);
    }
}