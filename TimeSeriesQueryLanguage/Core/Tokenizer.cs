using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;

namespace TimeSeriesQueryLanguage.Core
{
    public class Tokenizer<TAggFn,TAggCl> where TAggFn : Enum where TAggCl : Enum
    {
        char[] _MathQualifiers = new char[] { '-', '.' };
        char[] _Operations = new char[] { '*', '/', '+', '<', '>', '.', '&', '|' };

        readonly string _Command;
        char _CurrentChar;
        int _CommandCharIndex = 0;

        public Tokenizer(string command)
        {
            _Command = command;
            NextChar();
            NextToken();
        }

        public string GetCommand()
        {
            return _Command;
        }

        public TokenEnum Token;
        public decimal Number = 0;
        public TAggFn? AggFn = default;
        public TAggCl? AggCl = default;
        public AggTimeIntervalEnum AggTsFr;
        public AggTimeIntervalEnum AggTsTo;

        private string[] _TAggFnEnumNames = Enum.GetNames(typeof(TAggFn));
        private string[] _TAggClEnumNames = Enum.GetNames(typeof(TAggCl));
        private string[] _AggTimeIntervalEnumNames = Enum.GetNames(typeof(AggTimeIntervalEnum));
        
        public void NextChar()
        {
            _CurrentChar = _CommandCharIndex < _Command.Length ? _Command[_CommandCharIndex] : '\0';
            _CommandCharIndex++;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(_CurrentChar))
            {
                NextChar();
            }

            if (_CurrentChar == '\0')
            {
                Token = TokenEnum.EOF;
            }
            else if (_CurrentChar == '(')
            {
                Token = TokenEnum.OpenParens;
                NextChar();
            }
            else if (_CurrentChar == ')')
            {
                Token = TokenEnum.CloseParens;
                NextChar();
            }
            else if (_CurrentChar == ',')
            {
                Token = TokenEnum.Comma;
                NextChar();
            }
            else if (char.IsDigit(_CurrentChar) || _MathQualifiers.Contains(_CurrentChar))
            {
                var sb = new StringBuilder();
                while (char.IsDigit(_CurrentChar) || _MathQualifiers.Contains(_CurrentChar))
                {
                    sb.Append(_CurrentChar);
                    NextChar();
                }
                Token = TokenEnum.Number; Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            }
            else if (char.IsLetter(_CurrentChar) || _Operations.Contains(_CurrentChar))
            {
                var sb = new StringBuilder();
                while (char.IsLetterOrDigit(_CurrentChar) || _Operations.Contains(_CurrentChar))
                {
                    sb.Append(_CurrentChar);
                    NextChar();
                }
                var currentWord = sb.ToString();
                var dot = currentWord.IndexOf(".");
                if (currentWord == "*")
                {
                    Token = TokenEnum.Mult;
                }
                else if (currentWord == "/")
                {
                    Token = TokenEnum.Div;
                }
                else if (currentWord == "+")
                {
                    Token = TokenEnum.Add;
                }
                else if (currentWord == "&")
                {
                    Token = TokenEnum.And;
                }
                else if (currentWord == "|")
                {
                    Token = TokenEnum.Or;
                }
                else if (currentWord == "ag")
                {
                    Token = TokenEnum.Agg;
                }
                else if (currentWord == "fid")
                {
                    Token = TokenEnum.FId;
                }
                else if (currentWord == ">")
                {
                    Token = TokenEnum.V1mV2;
                }
                else if (currentWord == "<")
                {
                    Token = TokenEnum.V1lV2;
                }
                else if (currentWord == "sc")
                {
                    Token = TokenEnum.Scale;
                }
                else if (currentWord == "in")
                {
                    Token = TokenEnum.V1inV2V3;
                }
                else if (_TAggFnEnumNames.Contains(currentWord))
                {
                    Token = TokenEnum.AggFn; 
                    AggFn = (TAggFn)Enum.Parse(typeof(TAggFn), currentWord);
                }
                else if (_TAggClEnumNames.Contains(currentWord))
                {
                    Token = TokenEnum.AggCl; 
                    AggCl = (TAggCl)Enum.Parse(typeof(TAggCl), currentWord);
                }
                else if (currentWord.StartsWith("Fr.") && dot > 0)
                {
                    Token = TokenEnum.AggTsFr;
                    var currentWord2ndpart = currentWord.Substring(dot + 1);
                    if (!_AggTimeIntervalEnumNames.Contains(currentWord2ndpart)) throw new Exception($"ERR: failed to find AggTimeIntervalEnum:'{currentWord}' in '{_Command}'");
                    AggTsFr = (AggTimeIntervalEnum)Enum.Parse(typeof(AggTimeIntervalEnum), currentWord2ndpart);
                }
                else if (currentWord.StartsWith("To.") && dot > 0)
                {
                    Token = TokenEnum.AggTsTo;
                    var currentWord2ndpart = currentWord.Substring(dot + 1);
                    if (!_AggTimeIntervalEnumNames.Contains(currentWord2ndpart)) throw new Exception($"ERR: failed to find AggTimeIntervalEnum:'{currentWord}' in '{_Command}'");
                    AggTsTo = (AggTimeIntervalEnum)Enum.Parse(typeof(AggTimeIntervalEnum), currentWord2ndpart);
                }
                else
                    throw new Exception($"ERR: '{currentWord}' in '{_Command}'");
            }
        }
    }
}
