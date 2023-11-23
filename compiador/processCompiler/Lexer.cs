using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using compiador.clases;
using compiador.Typos;

namespace compiador.processCompiler
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }

        public List<Token> GetTokens()
        {
            List<Token> tokens = new List<Token>();

            while (_position < _input.Length)
            {
                char currentChar = _input[_position];

                if (char.IsDigit(currentChar))
                {
                    tokens.Add(ScanEntero());
                }
                else if (char.IsLetter(currentChar))
                {
                    tokens.Add(ScanPalabraClaveOIdentificador());
                }
                else if (currentChar == '"')
                {
                    tokens.Add(ScanCadena());
                }
                else if (currentChar == '+')
                {
                    tokens.Add(new Token(TokenType.Suma, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == '-')
                {
                    tokens.Add(new Token(TokenType.Resta, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == '*')
                {
                    tokens.Add(new Token(TokenType.Multiplicacion, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == '/')
                {
                    tokens.Add(new Token(TokenType.Division, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == '(')
                {
                    tokens.Add(new Token(TokenType.ParentesisIzquierdo, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == ')')
                {
                    tokens.Add(new Token(TokenType.ParentesisDerecho, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == '=')
                {
                    tokens.Add(new Token(TokenType.Igual, currentChar.ToString()));
                    _position++;
                }
                else if (currentChar == ';')
                {
                    tokens.Add(new Token(TokenType.PuntoComa, currentChar.ToString()));
                    _position++;
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    _position++;  // Ignorar espacios en blanco
                }
                else
                {
                    // Carácter no reconocido
                    throw new Exception($"Carácter no reconocido: {currentChar}");
                }
            }

            tokens.Add(new Token(TokenType.FinDeLinea, ""));
            return tokens;
        }

        private Token ScanPalabraClaveOIdentificador()
        {
            string result = "";
            while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
            {
                result += _input[_position];
                _position++;
            }

            switch (result)
            {
                case "int":
                    return new Token(TokenType.TipoDato, result, false, TokenType.Entero);
                case "string":
                    return new Token(TokenType.TipoDato, result, false, TokenType.Cadena);
                default:
                    return new Token(TokenType.Identificador, result);
            }
        }

        private Token ScanCadena()
        {
            string result = "";
            _position++;  // Saltar el primer comillas dobles
            while (_position < _input.Length && _input[_position] != '"')
            {
                result += _input[_position];
                _position++;
            }
            _position++;  // Saltar el último comillas dobles
            return new Token(TokenType.Cadena, result, isStringLiteral: true);
        }

        private Token ScanEntero()
        {
            string result = "";
            while (_position < _input.Length && char.IsDigit(_input[_position]))
            {
                result += _input[_position];
                _position++;
            }

            return new Token(TokenType.Entero, result);
        }
    }
}
