using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ToothAndTailReplayHelper.Model
{
    internal sealed class FilenameTokenParser : IFilenameTokenParser
    {
        private const string PlayerTokenIdentifier = "Players";
        private const string DateTokenIdentifier   = "Date:";

        public List<Tuple<FilenameToken, string>> ParseTokens(string fileNamingPattern)
        {
            var tokens = new List<Tuple<FilenameToken, string>>();
            var tokenRegex = new Regex(@"\{[^}]+\}");
            var searchIndex = 0;

            foreach (Match match in tokenRegex.Matches(fileNamingPattern))
            {
                if (match.Index > searchIndex)
                {
                    tokens.Add(Tuple.Create(FilenameToken.StringLiteral, fileNamingPattern.Substring(searchIndex, match.Index - searchIndex)));
                    searchIndex = match.Index;
                }

                var strippedValue = match.Value.TrimStart('{').TrimEnd('}');

                if (strippedValue.StartsWith(PlayerTokenIdentifier))
                {
                    tokens.Add(Tuple.Create(FilenameToken.PlayerNames, string.Empty));
                }
                else if (strippedValue.StartsWith(DateTokenIdentifier))
                {
                    tokens.Add(Tuple.Create(FilenameToken.DateTime, strippedValue.Substring(DateTokenIdentifier.Length)));
                }
                else
                {
                    tokens.Add(Tuple.Create(FilenameToken.StringLiteral, match.Value));
                }

                searchIndex = match.Index + match.Length;
            }

            if (searchIndex < fileNamingPattern.Length)
            {
                tokens.Add(Tuple.Create(
                    FilenameToken.StringLiteral,
                    fileNamingPattern.Substring(searchIndex, fileNamingPattern.Length - searchIndex)
                ));
            }

            return tokens;
        }
    }
}