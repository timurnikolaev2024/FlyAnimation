using System;

namespace Game.Scripts
{
    public class CollectibleCounter
    {
        public event Action<int> Changed;
        public int Value { get; private set; }

        public void Add(int delta)
        {
            Value += delta;
            Changed?.Invoke(Value);
        }
    }
}