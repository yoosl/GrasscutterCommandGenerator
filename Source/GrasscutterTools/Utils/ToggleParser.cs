﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GrasscutterTools.Utils
{
    /// <summary>
    /// Simple command line toggles parser: 
    ///  - toggles are identified with (any number of) '-' prefixes
    ///  - toggle can be with or without associated value 
    ///  - toggles are case-insensitive
    /// 
    /// <example>--toggle_without_value -toggle value</example>
    /// <see cref="https://gist.github.com/jchapuis/64b5adf9d0f3062e6a72dded110a6028"/>
    /// </summary>
    internal class ToggleParser
    {
        private readonly Dictionary<string, string> toggles;

        public ToggleParser(string[] args)
        {
            toggles =
                args.Zip(args.Skip(1).Concat(new[] { string.Empty }), (first, second) => new { first, second })
                    .Where(pair => IsToggle(pair.first))
                    .ToDictionary(pair => RemovePrefix(pair.first).ToLowerInvariant(), g => IsToggle(g.second) ? string.Empty : g.second);
        }

        private static string RemovePrefix(string toggle)
            => new string(toggle.SkipWhile(c => c == '-').ToArray());

        private static bool IsToggle(string arg)
            => arg.StartsWith("-", StringComparison.InvariantCulture);

        public bool HasToggle(string toggle)
            => toggles.ContainsKey(toggle.ToLowerInvariant());

        public string GetToggleValueOrDefault(string toggle, string defaultValue)
            => toggles.TryGetValue(toggle.ToLowerInvariant(), out var value) ? value : defaultValue;

        public bool IsEmpty => toggles.Count == 0;
    }
}
