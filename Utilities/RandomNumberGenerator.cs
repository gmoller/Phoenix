using System;

namespace Utilities
{
    public sealed class RandomNumberGenerator
    {
        private static readonly Lazy<RandomNumberGenerator> Lazy = new Lazy<RandomNumberGenerator>(() => new RandomNumberGenerator());

        private Random _random;

        public static RandomNumberGenerator Instance => Lazy.Value;

        private RandomNumberGenerator()
        {
            _random = new Random();
        }

        public int GetRandomInt(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber + 1);
        }

        public float GetRandomFloat(float minNumber, float maxNumber)
        {
            return (float)_random.NextDouble() * (maxNumber - minNumber) + minNumber;
        }

        public void SetNewRandomSeed(int seed)
        {
            _random = new Random(seed);
        }
    }
}