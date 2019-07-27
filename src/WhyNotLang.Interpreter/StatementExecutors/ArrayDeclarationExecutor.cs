using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class ArrayDeclarationExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public ArrayDeclarationExecutor(IStatementIterator statementIterator,
            IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public ExpressionValue Execute()
        {
            var arrayDeclaration = _statementIterator.CurrentStatement as ArrayDeclarationStatement;
            var arrayName = arrayDeclaration.ArrayName.Value;
            var arraySize = _expressionEvaluator.Eval(arrayDeclaration.IndexExpression);
            
            _programState.DeclareArray(arrayName, (int)arraySize.Value, arrayDeclaration.IsGlobal);
            
            return ExpressionValue.Empty;
        }
    }
}