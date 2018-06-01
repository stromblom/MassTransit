﻿namespace MassTransit.DapperIntegration.Tests
{
    using System;
    using System.Threading.Tasks;
    using MassTransit.Tests.Saga;
    using MassTransit.Tests.Saga.Messages;
    using NUnit.Framework;
    using Saga;
    using Shouldly;
    using TestFramework;
    using Testing;


    [TestFixture, Category("Integration")]
    public class LocatingExistingSagag :
        InMemoryTestFixture
    {
        [Test]
        public async Task A_correlated_message_should_find_the_correct_saga()
        {
            Guid sagaId = NewId.NextGuid();
            var message = new InitiateSimpleSaga(sagaId);

            await this.InputQueueSendEndpoint.Send(message);

            Guid? foundId = await this._sagaRepository.Value.ShouldContainSaga(message.CorrelationId, this.TestTimeout);

            foundId.HasValue.ShouldBe(true);

            var nextMessage = new CompleteSimpleSaga { CorrelationId = sagaId };

            await this.InputQueueSendEndpoint.Send(nextMessage);

            foundId = await this._sagaRepository.Value.ShouldContainSaga(x => x.CorrelationId == sagaId && x.Completed, this.TestTimeout);

            foundId.HasValue.ShouldBe(true);
        }

        [Test]
        public async Task An_initiating_message_should_start_the_saga()
        {
            Guid sagaId = NewId.NextGuid();
            var message = new InitiateSimpleSaga(sagaId);

            await this.InputQueueSendEndpoint.Send(message);

            Guid? foundId = await this._sagaRepository.Value.ShouldContainSaga(message.CorrelationId, this.TestTimeout);

            foundId.HasValue.ShouldBe(true);
        }

        readonly Lazy<ISagaRepository<SimpleSaga>> _sagaRepository;

        public LocatingExistingSagag()
        {
            var connectionString = LocalDbConnectionStringProvider.GetLocalDbConnectionString();
            this._sagaRepository = new Lazy<ISagaRepository<SimpleSaga>>(() => new DapperSagaRepository<SimpleSaga>(connectionString));
        }

        protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
        {
            configurator.Saga(this._sagaRepository.Value);
        }
    }
}
