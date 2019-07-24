using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class FunctionExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionEvaluator _mainEvaluator;
        private readonly IBuiltinFunctionEvaluator _builtinEvaluator;
        private readonly IProgramState _programState;

        public FunctionExpressionEvaluator(IExpressionEvaluator mainEvaluator, IBuiltinFunctionEvaluator builtinEvaluator, IProgramState programState)
        {
            _mainEvaluator = mainEvaluator;
            _builtinEvaluator = builtinEvaluator;
            _programState = programState;
        }
        
        public ExpressionValue Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Function)
            {
                throw new ArgumentException("FunctionExpression expected");
            }
            
            var functionExpression = expression as FunctionExpression;
            var functionDeclaration = _programState.GetFunction(functionExpression.Name.Value);
            var parameterValues = EvaluateParameters(functionExpression.Parameters.Where(p => p.Type != ExpressionType.Empty).ToList());

            if (functionDeclaration.IsBuiltin)
            {
                return _builtinEvaluator.Eval(functionExpression.Name.Value, parameterValues);
            }

            _programState.AddScope(functionDeclaration.Name.Value, true);
            InitialiseParameterVariables(parameterValues, functionDeclaration.Parameters);
            
            var statementExecutor =
                Executor.CreateExecutor(functionDeclaration.Body.ChildStatements, _programState);
            
            var returnValue = statementExecutor.ExecuteAll();
            _programState.RemoveScope();

            return returnValue;
        }

        private List<ExpressionValue> EvaluateParameters(List<IExpression> parameters)
        {
            var result = new List<ExpressionValue>();
            foreach (var parameter in parameters)
            {
                result.Add(_mainEvaluator.Eval(parameter));
            }

            return result;
        }

        private void InitialiseParameterVariables(List<ExpressionValue> values, List<Token> parameters)
        {
            if (values.Count != parameters.Count)
            {
                throw new ArgumentException($"Function called with {values.Count} arguments, expected {parameters.Count}");
            }

            for (int i = 0; i < values.Count; i++)
            {
                _programState.DeclareVariable(parameters[i].Value, values[i]);
            }
        }
    }
}