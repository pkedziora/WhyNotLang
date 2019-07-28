using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ArrayAssignmentTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public ArrayAssignmentTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public async Task ExecutesArrayAssignementWithNumberExpression()
        {
            _executor.Initialise(@"
                var x[10]
                x[0] := 1
            ");
            
            await _executor.ExecuteAll();

            var actual = _programState.CurrentScope.Arrays["x"][0].Value;

            Assert.Equal(1, actual);
        }
        
        [Fact]
        public async Task Executes2ArrayAssignementsWithComplexExpressions()
        {
            _executor.Initialise(@"
                var x[10]
                x[0] := 1
                x[1] := (10 * (4/4) + (2 - 1))
            ");
            
            await _executor.ExecuteAll();

            var actual0 = _programState.CurrentScope.Arrays["x"][0].Value;
            var actual1 = _programState.CurrentScope.Arrays["x"][1].Value;

            Assert.Equal(1, actual0);
            Assert.Equal(11, actual1);
        }

        [Fact]
        public async Task ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise(@"
                var abc:=100
                abc := (10 * (4/4) + (2 - 1))");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ThrowsDuringArrayAssignementWhenUndeclared()
        {
            _executor.Initialise(@"
                x[0] := 2
            ");

            await Assert.ThrowsAsync<ArgumentException>(async () => await _executor.ExecuteAll());
        }
        
        [Fact]
        public async Task ThrowsDuringArrayAssignementWhenOutOfRange()
        {
            _executor.Initialise(@"
                var x[10]
                x[10] := 2
            ");

            await Assert.ThrowsAsync<ArgumentException>(async () => await _executor.ExecuteAll());
        }
        
        [Fact]
        public async Task ArrayValueCanBeRetrieved()
        {
            _executor.Initialise(@"
                var x[10]
                x[1] := 2
                var y:= x[1] 
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ArrayValueUsedInExpressionWithHighPrecedenceOnTheRight()
        {
            _executor.Initialise(@"
                var x[10]
                x[1] := 2
                var y:= 1 + x[1] * 3 
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(7, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ArrayCanBePassedByReferenceToFunctionAndModified()
        {
            _executor.Initialise(@"
                function foo(array)
                begin
                    array[0] := 100
                end
                var bar[10]
                foo(bar)         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetArrayItem("bar", 0);

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task GlobalArrayValueAssignedInFunction()
        {
            _executor.Initialise(@"
                global x[10]

                function foo()
                begin
                    x[1] := 2
                end

                foo()
                var y:= 1 + x[1] * 3 
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(7, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task LocalArrayHidesGlobalArrayInFunction()
        {
            _executor.Initialise(@"
                global x[10]

                function foo()
                begin
                    var x[20]
                    x[1] := 2
                end
                x[1] := 1
                foo()
                var y:= x[1] 
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}