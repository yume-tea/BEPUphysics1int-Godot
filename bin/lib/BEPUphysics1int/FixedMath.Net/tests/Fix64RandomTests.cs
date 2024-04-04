using Xunit;
using Xunit.Abstractions;

namespace FixMath.NET
{
	public class Fix64RandomTests
    {
        private readonly ITestOutputHelper output;

        public Fix64RandomTests(ITestOutputHelper output)
        {
            if (output == null)
                output = new ConsoleTestOutputHelper();
            this.output = output;
        }

        private void TestIntRange(int maxValue, int seed)
        {
            bool[] valueReceived = new bool[maxValue];
            Fix64Random rand = new Fix64Random(seed);

            for (int i = 0; i < maxValue * 100; i++)
            {
                Fix64 value = rand.NextInt(maxValue);
                Assert.True(value >= Fix64.Zero && value < maxValue, string.Format("NextInt({0}) = expected 0 <= result < {0} but got {1}", maxValue, value));
                valueReceived[(int)value] = true;
            }

            for (int i = 0; i < maxValue; i++)
            {
                Assert.True(valueReceived[i], string.Format("NextInt({0}) = expected to receive {1} but never got it", maxValue, i));
            }
        }

        [Fact]
        public void IntRange10()
        {
            TestIntRange(10, 2351);            
        }

        [Fact]
        public void IntRange100()
        {
            TestIntRange(100, 25381);
        }

        [Fact]
        public void IntRange234()
        {
            TestIntRange(234, 1334);
        }

        [Fact]
        public void IntRange1000()
        {
            TestIntRange(1000, 2422281);
        }


        [Fact]
        public void IntSequence()
        {
            Fix64Random rand = new Fix64Random(1234);

            for (int i = 0; i < 100; i++)
            {
                Fix64 value = rand.NextInt(1337);

                Fix64 expected = Fix64RandomExpectedValues.ExpectedInts[i];
                Assert.True(value == expected, string.Format("NextInt() sequence {0} = expected {1} but got {2}", i, expected, value));
            }
        }

        [Fact]
        public void DecimalSequence()
        {
            Fix64Random rand = new Fix64Random(1234);

            for (int i = 0; i < 100; i++)
            {
                Fix64 value = rand.Next();

                Fix64 expected = Fix64RandomExpectedValues.ExpectedDecimals[i];
                Assert.True(value == expected, string.Format("Next() sequence {0} = expected {1} but got {2}", i, expected, value));
            }
        }

        [Fact]
        public void DecimalRange()
        {
            Fix64Random rand = new Fix64Random(123413587);

            const int buckets = 1000;
            bool[] valueReceived = new bool[buckets];

            for (int i = 0; i < 100000; i++)
            {
                Fix64 value = rand.Next();
                Assert.True(value >= Fix64.Zero && value <= Fix64.One, string.Format("Next() = expected 0 <= result <= 1 but got {0}", value));
                valueReceived[(int)(value*buckets)] = true;
            }

            for (int i = 0; i < buckets; i++)
            {
                Assert.True(valueReceived[i], string.Format("Next() = expected to receive {0} but never got it", ((Fix64)i)/buckets));
            }
        }
    }
}
