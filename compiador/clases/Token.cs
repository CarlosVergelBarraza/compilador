using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using compiador.Typos;

namespace compiador.clases
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public bool IsStringLiteral { get; }
        public TokenType? typeIdentificator { get; }

        public Token(TokenType type, string value, bool isStringLiteral = false, TokenType? typeIdentificator = null)
        {
            Type = type;
            Value = value;
            IsStringLiteral = isStringLiteral;
            this.typeIdentificator = typeIdentificator;
        }
    }
}
