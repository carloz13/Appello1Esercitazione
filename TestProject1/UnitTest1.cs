using System.Security.Cryptography.X509Certificates;
using ExtensionMethod;

namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void SanityCheck()
        {
            Func<int, int> ff = x => x;
            var source = new[] { ff };
            var result = source.IntersectOn(source, 0);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Test1()
        {
            var source = new Func<int, bool>[] { x => true, x => x%2==0, x => false };
            var other = new Func<int, bool>[] { x => x%3==0, x => x < 10, x => x%5==0 };
            var result = source.IntersectOn(other, 6);

            Assert.That(result, Is.EqualTo(new bool[] {true, true, true}));
        }

        [Test]
        public void Test2()
        {
            var size = 20;
            var source = new Func<int, int>[size];
            var other = new Func<int, int>[size];
            int[] sourceCalls = new int[size];
            int[] otherCalls = new int[size];

            for (int i = 0; i < size; i++)
            {
                var my_i = i;
                source[i] = x =>
                {
                    sourceCalls[my_i]++;
                    return my_i + 1;
                };

                other[i] = x =>
                {
                    otherCalls[my_i]++;
                    return x * (my_i + 1);
                };
            }

            var result = source.IntersectOn(other, 42).Count();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < size; i++)
                {
                    Assert.That(sourceCalls[i], Is.EqualTo(1));
                    Assert.That(otherCalls[i], Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void Test3()
        {
            var source = new Func<int, int>[] {x => x};
            var other = Other();

            Assert.That(() => source.IntersectOn(other, 42).Count(), Throws.TypeOf<ArgumentException>());


            IEnumerable<Func<int, int>> Other()
            {
                while (true) yield return x => x;
            }
        }

        [Test]
        public void Test4()
        {
            var source = new Func<int, bool>[] { x => true, x => (x % 2 == 0)? throw new ArgumentException(): true, x => x < 7 };
            var other = new Func<int, bool>[] { x => false, x => x < 10, x => x % 5 == 0 };
            var result = source.IntersectOn(other, 666);

            Assert.That(result, Is.EqualTo(new bool?[] {false, null, true}));

        }

    }
}