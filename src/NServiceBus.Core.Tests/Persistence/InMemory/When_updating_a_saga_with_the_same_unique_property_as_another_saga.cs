namespace NServiceBus.SagaPersisters.InMemory.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    class When_updating_a_saga_with_the_same_unique_property_as_another_saga:InMemorySagaPersistenceFixture
    {
        public When_updating_a_saga_with_the_same_unique_property_as_another_saga()
        {
            RegisterSaga<SagaWithUniqueProperty>();

            RegisterSaga<SagaWithTwoUniqueProperties>();
        }
        [Test]
        public void It_should_persist_successfully()
        {
            var saga1 = new SagaWithUniquePropertyData {Id = Guid.NewGuid(), UniqueString = "whatever1"};
            var saga2 = new SagaWithUniquePropertyData {Id = Guid.NewGuid(), UniqueString = "whatever"};
              
            persister.Save(saga1);
            persister.Save(saga2);

            Assert.Throws<InvalidOperationException>(() => 
            {
                var saga = persister.Get<SagaWithUniquePropertyData>(saga2.Id);
                saga.UniqueString = "whatever1";
                persister.Update(saga);
            });
        }

        [Test]
        public void It_should_persist_successfully_for_two_unique_properties()
        {
            var saga1 = new SagaWithTwoUniquePropertiesData { Id = Guid.NewGuid(), UniqueString = "whatever1", UniqueInt = 5};
            var saga2 = new SagaWithTwoUniquePropertiesData { Id = Guid.NewGuid(), UniqueString = "whatever", UniqueInt = 37};
            
            persister.Save(saga1);
            persister.Save(saga2);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var saga = persister.Get<SagaWithTwoUniquePropertiesData>(saga2.Id);
                saga.UniqueInt = 5;
                persister.Update(saga);
            });
        }
    }
}