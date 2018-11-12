using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.NCS.CourseSearchService.Sql.Gateways
{
    public class SearchQuery
    {
        #region Properties

        private readonly char[] _modifiers = { '!', '-', '+' };

        private readonly string[] _stopWords =
        {
            //"", "a", "able", "about", "across", "after", "all", "almost", "also", "am", "among",
            //"an", "and", "any", "are", "as", "at", "be", "because", "been", "but", "by", "can",
            //"cannot", "could", "dear", "did", "do", "does", "either", "else", "ever", "every",
            //"for", "from", "get", "got", "had", "has", "have", "he", "her", "hers", "him",
            //"his", "how", "however", "i", "if", "in", "into", "is", "it", "its", "just",
            //"least", "let", "like", "likely", "may", "me", "might", "most", "must", "my",
            //"neither", "no", "nor", "not", "of", "off", "often", "on", "only", "or", "other",
            //"our", "own", "rather", "said", "say", "says", "she", "should", "since", "so",
            //"some", "than", "that", "the", "their", "them", "then", "there", "these", "they",
            //"this", "tis", "to", "too", "twas", "us", "wants", "was", "we", "were", "what",
            //"when", "where", "which", "while", "who", "whom", "why", "will", "with", "would",
            //"yet", "you", "your"
        };

        private readonly char[] _whiteSpace = { ' ', '\r', '\n', '\t' };

        /// <summary>
        ///     List of words/phrases to exclude from the search.
        /// </summary>
        public List<string> Exclude = new List<string>();

        /// <summary>
        ///     List of words/phrases to include in the search.
        /// </summary>
        public List<string> Include = new List<string>();

        /// <summary>
        ///     List of words/phrases to exclude from the exact match search.
        /// </summary>
        public List<string> ExcludeExact = new List<string>();

        /// <summary>
        ///     List of words/phrases to include in the exact match search.
        /// </summary>
        public List<string> IncludeExact = new List<string>();

        /// <summary>
        ///     Search query.
        /// </summary>
        public string Query { get; private set; }

        #endregion

        public SearchQuery(string query)
        {
            Query = query;
            if (!String.IsNullOrWhiteSpace(query))
            {
                Parse();
            }
        }

        private void Parse()
        {
            int p = 0;
            var tType = TokenType.NoOp;
            while (tType != TokenType.Stop)
            {
                string token;
                p = Lex(p, out token, out tType);
                token = token.ToLower();
                if (tType == TokenType.Include)
                {
                    Include.Add(token);
                }
                else if (tType == TokenType.Exclude)
                {
                    Exclude.Add(token);
                }
                else if (tType == TokenType.IncludeExact)
                {
                    IncludeExact.Add(token);
                }
                else if (tType == TokenType.ExcludeExact)
                {
                    ExcludeExact.Add(token);
                }
            }
        }

        private int Lex(int p, out string token, out TokenType tType)
        {
            token = "";
            if (p >= Query.Length)
            {
                tType = TokenType.Stop;
                return p;
            }

            char ch = Query[p];

            while (_whiteSpace.Contains(ch) && p < Query.Length)
            {
                if (++p < Query.Length)
                {
                    ch = Query[p];
                }
            }

            if (p >= Query.Length)
            {
                tType = TokenType.Stop;
                return p;
            }

            tType = (ch == '-' || ch == '!') ? TokenType.Exclude : TokenType.Include;
            while (_modifiers.Contains(ch) && p < Query.Length)
            {
                if (++p < Query.Length)
                {
                    ch = Query[p];
                }
            }

            if (p >= Query.Length)
            {
                tType = TokenType.Stop;
                return p;
            }

            if (ch == '"' || ch == '\'')
            {
                tType = tType == TokenType.Include ? TokenType.IncludeExact : TokenType.ExcludeExact;
                char quote = ch;
                if (++p < Query.Length)
                {
                    ch = Query[p];
                    while (ch != quote && p < Query.Length)
                    {
                        token += ch;
                        if (++p < Query.Length)
                        {
                            ch = Query[p];
                        }
                    }
                    p++;
                }
                if (_stopWords.Contains(token.ToLower()))
                {
                    tType = TokenType.NoOp;
                }
                return p;
            }

            while (!_whiteSpace.Contains(ch) && p < Query.Length)
            {
                token += ch;
                if (++p < Query.Length)
                {
                    ch = Query[p];
                }
            }
            if (_stopWords.Contains(token.ToLower()))
            {
                tType = TokenType.NoOp;
            }
            return p;
        }

        private enum TokenType
        {
            NoOp,
            Include,
            IncludeExact,
            Exclude,
            ExcludeExact,
            Stop
        }
    }
}
