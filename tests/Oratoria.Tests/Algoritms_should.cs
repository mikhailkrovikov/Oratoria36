namespace Oratoria.Tests
{
    [TestFixture]
    public class Algoritms_should
    {
        private SimpleTestAlgorithm testAlgorithm = new();

        [TestCase(true, true, true, true, false, "Completed")]
        [TestCase(true, false, true, true, false, "Failed")]
        [TestCase(false, true, true, true, false, "Blocked")]
        [TestCase(true, true, true, true, true, "Canceled")]
        public async Task Test(
            bool canStart,
            bool operation1,
            bool operation2,
            bool operation3,
            bool isCancelled,
            string expected)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            if (isCancelled) 
            { 
                cts.Cancel(); 
            }
            var result = await testAlgorithm.Start(canStart, operation1, operation2, operation3, isCancelled, token);
            Assert.That(expected, Is.EqualTo(result.ToString()));
        }
    }
}
