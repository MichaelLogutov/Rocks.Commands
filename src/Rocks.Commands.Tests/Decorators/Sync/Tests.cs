using System;
using FluentAssertions;
using Rocks.Commands.Extensions;
using Rocks.Commands.Tests.Decorators.Sync.Commands;
using SimpleInjector;
using Xunit;

namespace Rocks.Commands.Tests.Decorators.Sync
{
    public class Tests
    {
        [Fact]
        public void RegisterCommandsDecorator_Always_RegistersAndUsesOpenGenericDecorators ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>), container, lifestyle);

            var command = new TestDecoratableCommand { Number = 1 };


            // act
            var result = CommandsLibrary.CommandsProcessor.Execute (command);


            // assert
            command.Number.Should ().Be (3);
            result.Should ().Be (2);
        }


        [Fact]
        public void RegisterCommandsDecorator_DoesNotAppliesDecoratorToNotApplicableCommands ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>), container, lifestyle);

            var command = new TestNotDecoratableCommand { Number = 1 };


            // act
            var result = CommandsLibrary.CommandsProcessor.Execute (command);


            // assert
            command.Number.Should ().Be (2);
            result.Should ().Be (2);
        }


        [Fact]
        public void RegisterCommandsDecorator_TwoRegistrations_Verifies ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>), container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>), container, lifestyle);


            // act
            Action act = () => container.Verify ();


            // assert
            act.Should().NotThrow ();
        }


        [Fact]
        public void RegisterCommandsDecorator_TwoRegistrations_Singleton_Verifies ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestSingletonDecorator<,>), container, Lifestyle.Singleton);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestSingletonDecorator<,>), container, Lifestyle.Singleton);


            // act
            Action act = () => container.Verify ();


            // assert
            act.Should().NotThrow ();
        }


        [Fact]
        public void GetAllDecorators_NoDecorators_ReturnsNothing ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);


            // act
            var result = CommandsLibrary.CommandsProcessor.GetAllDecorators<TestDecoratableCommand> ();


            // assert
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void GetAllDecoratorsGenericTypes_OneDecorator_ReturnsIt ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };
            var lifestyle = Lifestyle.Transient;

            CommandsLibrary.Setup (container, lifestyle);
            CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>), container, lifestyle);


            // act
            var result = CommandsLibrary.CommandsProcessor.GetAllDecoratorsGenericTypes<TestDecoratableCommand> ();


            // assert
            result.Should ().Equal (typeof (TestDecorator<,>));
        }
    }
}