using System.Runtime.CompilerServices;

namespace StringSplit;

public static class TokeniseExtensions
{
    /// <summary>
    /// Extension method to tokenise a string using ',' as a separator, this will return any empty results i.e. x,,y and will not trim results.
    /// </summary>
    public static Enumerator Tokenise(this ReadOnlySpan<char> span)
    {
        return new Enumerator(span);
    }

    /// <summary>
    /// Extension method to tokenise a string using ',' as a separator, this will not return empty results and will trim values.
    /// </summary>
    public static TrimEnumerator TokeniseWithTrim(this ReadOnlySpan<char> span)
    {
        return new TrimEnumerator(span);
    }

    /// <summary>
    /// Simple enumerator that tokenises the string using ',' but does not removed empty entries or trim results.
    /// </summary>
    /// <remarks>This is typically not required when using TryParse as this should handle empty values and trimming</remarks>
    public ref struct Enumerator
    {
        private ReadOnlySpan<char> _span;
        private ReadOnlySpan<char> _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(ReadOnlySpan<char> span)
        {
            _span = span;
            _current = ReadOnlySpan<char>.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerator GetEnumerator() => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            // There is nothing left to process
            if (_span.Length == 0)
            {
                _current = ReadOnlySpan<char>.Empty;
                return false;
            }

            // Find the next comma
            int index = _span.IndexOf(',');

            // The final part
            if (index == -1)
            {
                _current = _span;
                _span = ReadOnlySpan<char>.Empty;
                return true;
            }

            // Current is before and after is for next time
            _current = _span[..index];
            _span = _span[(index + 1)..];
            return true;
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public readonly ReadOnlySpan<char> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _current;
        }
    }

    /// <summary>
    /// Move advanced enumerator that tokenises the string using ',' and removes empty entries and trims results.
    /// </summary>
    public ref struct TrimEnumerator
    {
        private ReadOnlySpan<char> _span;
        private ReadOnlySpan<char> _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal TrimEnumerator(ReadOnlySpan<char> span)
        {
            _span = span;
            _current = ReadOnlySpan<char>.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TrimEnumerator GetEnumerator() => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            // There is nothing left to process
            if (_span.Length == 0)
            {
                _current = ReadOnlySpan<char>.Empty;
                return false;
            }

            // Loop around until we find a valid entry or run out of string
            while (true)
            {
                int index = _span.IndexOf(',');

                // The final part
                if (index == -1)
                {
                    _current = _span.Trim();
                    _span = ReadOnlySpan<char>.Empty;

                    // Handle a single/final empty entry
                    return _current.Length != 0;
                }

                _current = _span[..index].Trim();
                _span = _span[(index + 1)..];

                // After trimming we could be left with an empty string
                if (_current.Length != 0)
                {
                    return true;
                }
            }
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public readonly ReadOnlySpan<char> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_current.IsEmpty)
                {
                    throw new InvalidOperationException();
                }

                return _current;
            }
        }
    }
}