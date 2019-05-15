using NUnit.Framework;

namespace Wokarol.MessageSystem.Tests
{
    public class MessengerTest
    {
        Messenger messenger;

        int callCount = 0;
        int unusedCallCount = 0;
        void IncrementOnTestMessage(TestMessage m) {
            callCount += 1;
        }
        void IncrementOnUnusedTestMessage(UnusedTestMessage m) {
            callCount += 1;
        }

        private struct TestMessage
        {
        }

        private struct UnusedTestMessage
        {
        }

        private class ClassMessage
        {
        }

        [SetUp]
        public void BeforeTest() {
            callCount = 0;
            messenger = new Messenger();
        }

        [Test]
        public void _0_Messenger_Can_Be_Created () {
            Assert.NotNull(messenger);
        }

        [Test]
        public void _1_Messenger_Call_Method_Once() {
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);

            messenger.SendMessage(new TestMessage());

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void _2_Messenger_Calls_Multiple_Methods() {
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);

            messenger.SendMessage(new TestMessage());

            Assert.AreEqual(3, callCount);
        }

        [Test]
        public void _3_Messenger_Clears_Methods() {
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);

            messenger.SendMessage(new TestMessage());
            Assert.AreEqual(2, callCount);

            messenger.RemoveAllListenersFor(this);

            messenger.SendMessage(new TestMessage());

            Assert.AreEqual(2, callCount);
        }

        [Test]
        public void _4_Messenger_Does_not_Crosscall_Methods() {
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<UnusedTestMessage>(IncrementOnUnusedTestMessage);
            messenger.AddListener<UnusedTestMessage>(IncrementOnUnusedTestMessage);

            messenger.SendMessage(new TestMessage());

            Assert.AreEqual(2, callCount);
            Assert.AreEqual(0, unusedCallCount);
        }

        [Test]
        public void _5_Messenger_Throws_NullException() {
            messenger.AddListener<ClassMessage>(e => { });
            Assert.Throws(typeof(System.ArgumentNullException), () => messenger.SendMessage<ClassMessage>(null));
        }

        [Test]
        public void _6_Messenger_Can_Remove_Given_Listener()
        {
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);
            messenger.AddListener<TestMessage>(IncrementOnTestMessage);

            messenger.SendMessage(new TestMessage());
            Assert.AreEqual(2, callCount);

            messenger.RemoveListener<TestMessage>(IncrementOnTestMessage);

            messenger.SendMessage(new TestMessage());

            Assert.AreEqual(3, callCount);
        }
    }
}
