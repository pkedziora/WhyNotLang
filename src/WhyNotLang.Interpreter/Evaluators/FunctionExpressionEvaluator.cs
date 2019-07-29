using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class FunctionExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionEvaluator _mainEvaluator;
        private readonly IBuiltinFunctionEvaluator _builtinEvaluator;
        private readonly IExecutor _mainExecutor;

        public FunctionExpressionEvaluator(IExpressionEvaluator mainEvaluator, IBuiltinFunctionEvaluator builtinEvaluator, IExecutor mainExecutor)
        {
            _mainEvaluator = mainEvaluator;
            _builtinEvaluator = builtinEvaluator;
            _mainExecutor = mainExecutor;
        }
        
        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Function)
            {
                throw new ArgumentException("FunctionExpression expected");
            }
            
            var functionExpression = expression as FunctionExpression;
            var functionDeclaration = _mainExecutor.ProgramState.GetFunction(functionExpression.Name.Value);
            var argumentsValues = await EvaluateArguments(functionExpression.Parameters.Where(p => p.Type != ExpressionType.Empty).ToList());

            if (functionDeclaration.IsBuiltin)
            {
                return await _builtinEvaluator.Eval(functionExpression.Name.Value, argumentsValues);
            }

            _mainExecutor.ProgramState.AddScope(functionDeclaration.Name.Value, true);
            InitialiseParameterVariables(argumentsValues, functionDeclaration.Parameters);
            
            _mainExecutor.CreateNewContext(functionDeclaration.Body.ChildStatements);
            var returnValue = await _mainExecutor.ExecuteAll();
            _mainExecutor.LeaveContext();
            _mainExecutor.ProgramState.RemoveScope();

            return returnValue;
        }

        private async Task<List<ExpressionValue>> EvaluateArguments(List<IExpression> arguments)
        {
            var result = new List<ExpressionValue>();
            foreach (var parameter in arguments)
            {
                var valueExpression = parameter as ValueExpression;
                var isArray = parameter.Type == ExpressionType.Value &&
                              valueExpression.Token.Type == TokenType.Identifier &&
                              _mainExecutor.ProgramState.IsArrayDefined(valueExpression.Token.Value);
                result.Add(isArray
                    ? _mainExecutor.ProgramState.GetArrayReference(valueExpression.Token.Value)
                    : await _mainEvaluator.Eval(parameter));
            }

            return result;
        }

        private void InitialiseParameterVariables(List<ExpressionValue> argumentsValues, List<Token> parameters)
        {
            if (argumentsValues.Count != parameters.Count)
            {
                throw new ArgumentException($"Function called with {argumentsValues.Count} arguments, expected {parameters.Count}");
            }

            for (int i = 0; i < argumentsValues.Count; i++)
            {
                if (argumentsValues[i].Type == ExpressionValueTypes.ArrayReference)
                {
                    _mainExecutor.ProgramState.DeclareArrayByReference(parameters[i].Value, argumentsValues[i]);
                }
                else
                {
                    _mainExecutor.ProgramState.DeclareVariable(parameters[i].Value, argumentsValues[i]);
                }
            }
        }
    }
}