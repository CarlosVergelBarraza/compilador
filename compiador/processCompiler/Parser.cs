using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using compiador.clases;
using compiador.Typos;

namespace compiador.processCompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;
        private Dictionary<string, KeyValuePair<TokenType, dynamic>> _variables;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _variables = new Dictionary<string, KeyValuePair<TokenType, dynamic>>();
        }
        public Dictionary<string, KeyValuePair<TokenType, dynamic>> Variables
        {
            get { return _variables; }
        }
        public void ParseProgram()
        {
            var result = Match(TokenType.FinDeLinea);
            while (!result)
            {
                ParseStatement();
                result = Match(TokenType.FinDeLinea);

            }
        }

        private void ParseStatement()
        {
            if (Match(TokenType.Identificador))
            {
                string variableName = GetToken().Value;
                Consume(TokenType.Igual);

                dynamic value = ParseExpression();
                Consume(TokenType.FinDeLinea);

                if (_variables.ContainsKey(variableName))
                {
                    _variables[variableName] = new KeyValuePair<TokenType, dynamic>(_variables[variableName].Key, value);
                }
                else
                {
                    throw new Exception($"Variable no declarada: {variableName}");
                }
            }
            else
            {
                ParseExpression();
                //Consume(TokenType.FinDeLinea);
            }
        }

        private dynamic ParseExpression()
        {
            return ParseAddition();
        }

        private dynamic ParseAddition()
        {
            dynamic leftValue = ParseMultiplication();

            while (Match(TokenType.Suma))
            {
                Token op = GetToken();
                dynamic rightValue = ParseMultiplication();

                if (op.IsStringLiteral || rightValue is string)
                {
                    leftValue = leftValue.ToString() + rightValue.ToString();  // Concatenar cadenas
                }
                else
                {
                    leftValue += rightValue;  // Sumar enteros
                }
            }

            return leftValue;
        }

        private dynamic ParseMultiplication()
        {
            dynamic leftValue = ParsePrimary();

            while (Match(TokenType.Multiplicacion))
            {
                Token op = GetToken();
                dynamic rightValue = ParsePrimary();

                leftValue *= rightValue;  // Multiplicar enteros
            }

            return leftValue;
        }

        private dynamic ParsePrimary()
        {
            Token current = GetToken();

            switch (current.Type)
            {
                case TokenType.Entero:
                    return int.Parse(current.Value);
                case TokenType.Cadena:
                    return current.Value;
                case TokenType.ParentesisIzquierdo:
                    dynamic result = ParseExpression();
                    Consume(TokenType.ParentesisDerecho);
                    return result;
                case TokenType.Identificador:
                    if (_variables.ContainsKey(current.Value))
                    {
                        return _variables[current.Value].Value;
                    }
                    else
                    {
                        throw new Exception($"Variable no declarada: {current.Value}");
                    }
                case TokenType.TipoDato:
                    TokenType tipoDato = current.Type;

                    string variableName = GetToken(true).Value;
                    Consume(TokenType.Identificador);
                    var typoEsperado = current.typeIdentificator;
                    Consume(TokenType.Igual);
                    dynamic value = GetToken();
                    if (current.typeIdentificator is not null)
                        comparate(value.Type, current.typeIdentificator);
                    Consume(TokenType.PuntoComa);
                    if (_variables.ContainsKey(variableName))
                        throw new Exception($"ya exite una variable con el nombre {variableName}");
                    _variables.Add(variableName, new KeyValuePair<TokenType, dynamic>(tipoDato, value.Value));

                    return value;
                default:
                    throw new Exception($"Token inesperado: {current.Type}");
            }
        }

        private bool Match(TokenType expectedType)
        {
            if (_position < _tokens.Count && _tokens[_position].Type == expectedType)
            {
                return true;
            }
            if (expectedType == TokenType.FinDeLinea && _position >= _tokens.Count) return true;
            return false;
        }
        private bool comparate(TokenType evalueType, TokenType? comparatedType)
        {
            if (comparatedType is null) throw new ArgumentNullException("necesito dos elemento a commparar");
            if (evalueType.Equals(comparatedType))
            {
                return true;
            }
            throw new Exception($"El Tipo de dato {comparatedType.ToString()}, no puede ser declarado por {evalueType.ToString()}");
        }

        private Token GetToken(bool? sum = false)
        {
            if (sum is true)
            {
                if (_position < _tokens.Count)
                {
                    return _tokens[_position];
                }
            }
            else
            {
                if (_position < _tokens.Count)
                {
                    return _tokens[_position++];
                }

            }
            throw new Exception("Se alcanzó el final de la lista de tokens inesperadamente.");
        }

        private void Consume(TokenType expectedType)
        {
            //if (expectedType == TokenType.FinDeLinea)
            //{
            //    _position++;
            //    return;
            //}
            if (Match(expectedType))
            {
                _position++;
            }
            else
            {
                var token = _tokens[_position];
                throw new Exception($"Se esperaba {expectedType}, pero se encontró {token.Type}, mira por {token}");
            }
        }
    }
}
