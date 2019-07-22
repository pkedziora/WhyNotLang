using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class FunctionDeclarationExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IProgramState _programState;

        public FunctionDeclarationExecutor(IStatementIterator statementIterator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _programState = programState;
        }
        
        public void Execute()
        {
            var functionDeclaration = _statementIterator.CurrentStatement as FunctionDeclarationStatement;
            var functionName = functionDeclaration.Name.Value;
            _programState.DeclareFunction(functionName, functionDeclaration);
        }
    }
}