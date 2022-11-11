using FluentAssertions;
using NUnit.Framework;

namespace GameOfLife;

public class Tests
{
    [Test]
    public void Test1()
    {
        var currentState = new bool[1, 1] {{true}};

        var nextState = Game.GetNextState(currentState);
        var expectingState = new bool[1, 1] {{false}};

        Assert.AreEqual(expectingState, nextState);
        // Альтернативный способ с помощью библиотеки FluentAssertions
        nextState.Should().BeEquivalentTo(expectingState);
    }
}