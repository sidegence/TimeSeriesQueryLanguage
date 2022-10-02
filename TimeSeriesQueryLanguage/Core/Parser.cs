using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesQueryLanguage.Enums;
using TimeSeriesQueryLanguage.Functions;

namespace TimeSeriesQueryLanguage.Core
{
    public class TimeSeriesQueryLanguageParser<TAggFn, TAggCl> where TAggFn : Enum where TAggCl : Enum
    {
        Tokenizer<TAggFn, TAggCl>? _tokenizer;

        public TimeSeriesQueryLanguageParser()
        {
        }

        public TimeSeriesQueryLanguageParser(string text)
        {
            Set(text);
        }

        public TimeSeriesQueryLanguageParser<TAggFn, TAggCl> Set(string text)
        {
            _tokenizer = new Tokenizer<TAggFn, TAggCl>(text);
            return this;
        }

        public AbstractNode? Parse()
        {
            switch (_tokenizer?.Token)
            {
                case TokenEnum.Agg:
                case TokenEnum.Tid:
                case TokenEnum.FId:
                case TokenEnum.Number:
                    return ParseLeafNode();
                case TokenEnum.Mult:
                case TokenEnum.Div:
                case TokenEnum.Add:
                case TokenEnum.And:
                case TokenEnum.Or:
                case TokenEnum.V1mV2:
                case TokenEnum.V1lV2:
                case TokenEnum.Scale:
                case TokenEnum.V1inV2V3:
                    return ParseFunctionNode(_tokenizer.Token);
                default:
                    break;
            }
            _tokenizer?.NextToken();
            return _tokenizer?.Token == TokenEnum.EOF ? null : Parse();
        }

        public AbstractNode ParseLeafNode()
        {
            switch (_tokenizer?.Token)
            {
                case TokenEnum.Agg:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == TokenEnum.OpenParens)
                    {
                        _tokenizer.NextToken();
                        TAggFn? aggFn = default; TAggCl? aggCl = default; AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0; AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M1;
                        while (_tokenizer.Token != TokenEnum.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case TokenEnum.Comma: break;
                                case TokenEnum.AggFn: aggFn = _tokenizer.AggFn; break;
                                case TokenEnum.AggCl: aggCl = _tokenizer.AggCl; break;
                                case TokenEnum.AggTsSlideTo: aggTsSlideTo = _tokenizer.AggTsSlideTo; break;
                                case TokenEnum.AggTsFrame: aggTsFrame = _tokenizer.AggTsFrame; break;
                                default:
                                    throw new Exception($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new AggNode<TAggFn, TAggCl>(aggFn, aggCl, aggTsSlideTo, aggTsFrame);
                    }
                    else
                    {
                        throw new Exception("ParseLeafNode Token.Agg no open parans");
                    }
                case TokenEnum.Tid:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == TokenEnum.OpenParens)
                    {
                        _tokenizer.NextToken();
                        TAggFn? aggFn = default; decimal tid = 0; AggTimeIntervalEnum aggTsSlideTo = AggTimeIntervalEnum.M0; AggTimeIntervalEnum aggTsFrame = AggTimeIntervalEnum.M1;
                        while (_tokenizer.Token != TokenEnum.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case TokenEnum.Comma: break;
                                case TokenEnum.AggFn: aggFn = _tokenizer.AggFn; break;
                                case TokenEnum.Number: tid = _tokenizer.Number; break;
                                case TokenEnum.AggTsSlideTo: aggTsSlideTo = _tokenizer.AggTsSlideTo; break;
                                case TokenEnum.AggTsFrame: aggTsFrame = _tokenizer.AggTsFrame; break;
                                default:
                                    throw new Exception($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new TidNode<TAggFn, TAggCl>(aggFn, tid, aggTsSlideTo, aggTsFrame);
                    }
                    else
                    {
                        throw new Exception("ParseLeafNode Token.Agg no open parans");
                    }
                case TokenEnum.FId:
                    _tokenizer.NextToken();
                    if (_tokenizer.Token == TokenEnum.OpenParens)
                    {
                        _tokenizer.NextToken();
                        decimal i = 0;
                        while (_tokenizer.Token != TokenEnum.CloseParens)
                        {
                            switch (_tokenizer.Token)
                            {
                                case TokenEnum.Number: i = _tokenizer.Number; break;
                                default:
                                    throw new Exception($"ParseLeafNode not valid arg: {_tokenizer.Token}");
                            }
                            _tokenizer.NextToken();
                        }
                        return new FIdNode<TAggFn, TAggCl>(i);
                    }
                    else
                    {
                        throw new Exception("ParseLeafNode Token.Agg no open parans");
                    }
                case TokenEnum.Number:
                    return new NumberNode(_tokenizer.Number);
            }
            throw new Exception($"ParseLeafNode _tokenizer.Token{_tokenizer?.Token}: Invalid syntax");
        }

        public AbstractNode ParseFunctionNode(TokenEnum token)
        {
            _tokenizer?.NextToken();
            if (_tokenizer?.Token == TokenEnum.OpenParens)
            {
                _tokenizer.NextToken();
                List<AbstractNode> args = new List<AbstractNode>();
                while (_tokenizer.Token != TokenEnum.CloseParens)
                {
                    switch (_tokenizer.Token)
                    {
                        case TokenEnum.Agg:
                        case TokenEnum.Tid:
                        case TokenEnum.FId:
                        case TokenEnum.Number:
                            args.Add(ParseLeafNode());
                            break;
                        case TokenEnum.Comma:
                            break;
                        case TokenEnum.Mult:
                        case TokenEnum.Div:
                        case TokenEnum.Add:
                        case TokenEnum.And:
                        case TokenEnum.Or:
                        case TokenEnum.V1mV2:
                        case TokenEnum.V1lV2:
                        case TokenEnum.Scale:
                        case TokenEnum.V1inV2V3:
                            args.Add(ParseFunctionNode(_tokenizer.Token));
                            break;
                        default:
                            throw new Exception($"ParseFunctionNode unexpected token: {_tokenizer.Token}");
                    }
                    _tokenizer.NextToken();
                }
                switch (token)
                {
                    case TokenEnum.Mult: return new MultiplicationNode(args);
                    case TokenEnum.Div: return new DivisionNode(args);
                    case TokenEnum.Add: return new AddNode(args);
                    case TokenEnum.And: return new AndNode(args);
                    case TokenEnum.Or: return new OrNode(args);
                    case TokenEnum.V1mV2: return new V1mV2Node(args);
                    case TokenEnum.V1lV2: return new V1lV2Node(args);
                    case TokenEnum.Scale: return new ScaleNode(args);
                    case TokenEnum.V1inV2V3: return new V1inV2V3Node(args);
                }
                throw new Exception("ParseFunctionNode not a valid token");
            }
            else
            {
                throw new Exception("ParseFunctionNode no open parens");
            }
        }
    }
}
