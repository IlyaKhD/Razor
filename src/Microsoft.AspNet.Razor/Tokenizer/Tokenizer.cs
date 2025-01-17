// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if DEBUG
using System.Globalization;
#endif
using System.Linq;
using System.Text;
using Microsoft.AspNet.Razor.Text;
using Microsoft.AspNet.Razor.Tokenizer.Symbols;

namespace Microsoft.AspNet.Razor.Tokenizer
{
    public abstract partial class Tokenizer<TSymbol, TSymbolType> : StateMachine<TSymbol>, ITokenizer
        where TSymbolType : struct
        where TSymbol : SymbolBase<TSymbolType>
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "TextDocumentReader does not require disposal")]
        protected Tokenizer(ITextDocument source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Source = new TextDocumentReader(source);
            Buffer = new StringBuilder();
            CurrentErrors = new List<RazorError>();
            StartSymbol();
        }

        public TextDocumentReader Source { get; private set; }

        protected StringBuilder Buffer { get; private set; }

        protected bool EndOfFile
        {
            get { return Source.Peek() == -1; }
        }

        protected IList<RazorError> CurrentErrors { get; private set; }

        public abstract TSymbolType RazorCommentStarType { get; }
        public abstract TSymbolType RazorCommentType { get; }
        public abstract TSymbolType RazorCommentTransitionType { get; }

        protected bool HaveContent
        {
            get { return Buffer.Length > 0; }
        }

        protected char CurrentCharacter
        {
            get
            {
                var peek = Source.Peek();
                return peek == -1 ? '\0' : (char)peek;
            }
        }

        protected SourceLocation CurrentLocation
        {
            get { return Source.Location; }
        }

        protected SourceLocation CurrentStart { get; private set; }

        public virtual TSymbol NextSymbol()
        {
            // Post-Condition: Buffer should be empty at the start of Next()
            Debug.Assert(Buffer.Length == 0);
            StartSymbol();

            if (EndOfFile)
            {
                return null;
            }
            var sym = Turn();

            // Post-Condition: Buffer should be empty at the end of Next()
            Debug.Assert(Buffer.Length == 0);

            return sym;
        }

        public void Reset()
        {
            CurrentState = StartState;
        }

        protected abstract TSymbol CreateSymbol(SourceLocation start, string content, TSymbolType type, IEnumerable<RazorError> errors);

        protected TSymbol Single(TSymbolType type)
        {
            TakeCurrent();
            return EndSymbol(type);
        }

        protected void StartSymbol()
        {
            Buffer.Clear();
            CurrentStart = CurrentLocation;
            CurrentErrors.Clear();
        }

        protected TSymbol EndSymbol(TSymbolType type)
        {
            return EndSymbol(CurrentStart, type);
        }

        protected TSymbol EndSymbol(SourceLocation start, TSymbolType type)
        {
            TSymbol sym = null;
            if (HaveContent)
            {
                sym = CreateSymbol(start, Buffer.ToString(), type, CurrentErrors.ToArray());
            }
            StartSymbol();
            return sym;
        }

        protected bool TakeUntil(Func<char, bool> predicate)
        {
            // Take all the characters up to the end character
            while (!EndOfFile && !predicate(CurrentCharacter))
            {
                TakeCurrent();
            }

            // Why did we end?
            return !EndOfFile;
        }

        protected void TakeCurrent()
        {
            if (EndOfFile)
            {
                return;
            } // No-op
            Buffer.Append(CurrentCharacter);
            MoveNext();
        }

        protected void MoveNext()
        {
#if DEBUG
            _read.Append(CurrentCharacter);
#endif
            Source.Read();
        }

        protected bool TakeAll(string expected, bool caseSensitive)
        {
            return Lookahead(expected, takeIfMatch: true, caseSensitive: caseSensitive);
        }

        protected char Peek()
        {
            using (LookaheadToken lookahead = Source.BeginLookahead())
            {
                MoveNext();
                return CurrentCharacter;
            }
        }

        protected StateResult AfterRazorCommentTransition()
        {
            if (CurrentCharacter != '*')
            {
                // We've been moved since last time we were asked for a symbol... reset the state
                return Transition(StartState);
            }
            AssertCurrent('*');
            TakeCurrent();
            return Transition(EndSymbol(RazorCommentStarType), RazorCommentBody);
        }

        protected StateResult RazorCommentBody()
        {
            TakeUntil(c => c == '*');
            if (CurrentCharacter == '*')
            {
                var star = CurrentCharacter;
                var start = CurrentLocation;
                MoveNext();
                if (!EndOfFile && CurrentCharacter == '@')
                {
                    State next = () =>
                    {
                        Buffer.Append(star);
                        return Transition(EndSymbol(start, RazorCommentStarType), () =>
                        {
                            if (CurrentCharacter != '@')
                            {
                                // We've been moved since last time we were asked for a symbol... reset the state
                                return Transition(StartState);
                            }
                            TakeCurrent();
                            return Transition(EndSymbol(RazorCommentTransitionType), StartState);
                        });
                    };

                    if (HaveContent)
                    {
                        return Transition(EndSymbol(RazorCommentType), next);
                    }
                    else
                    {
                        return Transition(next);
                    }
                }
                else
                {
                    Buffer.Append(star);
                    return Stay();
                }
            }
            return Transition(EndSymbol(RazorCommentType), StartState);
        }

        /// <summary>
        /// Internal for unit testing
        /// </summary>
        internal bool Lookahead(string expected, bool takeIfMatch, bool caseSensitive)
        {
            Func<char, char> filter = c => c;
            if (!caseSensitive)
            {
                filter = Char.ToLowerInvariant;
            }

            if (expected.Length == 0 || filter(CurrentCharacter) != filter(expected[0]))
            {
                return false;
            }

            // Capture the current buffer content in case we have to backtrack
            string oldBuffer = null;
            if (takeIfMatch)
            {
                oldBuffer = Buffer.ToString();
            }

            using (LookaheadToken lookahead = Source.BeginLookahead())
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (filter(CurrentCharacter) != filter(expected[i]))
                    {
                        if (takeIfMatch)
                        {
                            // Clear the buffer and put the old buffer text back
                            Buffer.Clear();
                            Buffer.Append(oldBuffer);
                        }
                        // Return without accepting lookahead (thus rejecting it)
                        return false;
                    }
                    if (takeIfMatch)
                    {
                        TakeCurrent();
                    }
                    else
                    {
                        MoveNext();
                    }
                }
                if (takeIfMatch)
                {
                    lookahead.Accept();
                }
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This only occurs in Release builds, where this method is empty by design")]
        [Conditional("DEBUG")]
        internal void AssertCurrent(char current)
        {
#if NET45
            // No Debug.Assert with this many arguments in CoreCLR

            Debug.Assert(CurrentCharacter == current, "CurrentCharacter Assumption violated", "Assumed that the current character would be {0}, but it is actually {1}", current, CurrentCharacter);
#else
            Debug.Assert(CurrentCharacter == current, string.Format("CurrentCharacter Assumption violated.  Assumed that the current character would be {0}, but it is actually {1}", current, CurrentCharacter));
#endif
        }

        ISymbol ITokenizer.NextSymbol()
        {
            return (ISymbol)NextSymbol();
        }
    }

#if DEBUG
    [DebuggerDisplay("{DebugDisplay}")]
    public partial class Tokenizer<TSymbol, TSymbolType>
    {
        private StringBuilder _read = new StringBuilder();

        public string DebugDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "[{0}] [{1}] [{2}]", _read.ToString(), CurrentCharacter, Remaining); }
        }

        public string Remaining
        {
            get
            {
                var remaining = Source.ReadToEnd();
                Source.Seek(-remaining.Length);
                return remaining;
            }
        }
    }
#endif
}
