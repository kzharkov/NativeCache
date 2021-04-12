using System;

namespace AlgorithmsDataStructures
{
    public class NativeCache<T>
    {
        public int size;
        public string[] slots;
        public T[] values;
        public int[] hits;
        public int step = 3;

        public NativeCache(int size)
        {
            this.size = size;
            slots = new string[size];
            values = new T[size];
            hits = new int[size];
        }

        public int HashFun(string key)
        {
            int sum = 0;
            Array.ForEach(key.ToCharArray(), delegate (char i) { sum += i; });
            return sum % size;
        }

        public int SeekSlot(string value)
        {
            int slot = HashFun(value);
            int startSlot = slot;
            while (slots[slot] != null && slots[slot] != value)
            {
                slot += step;
                slot %= size;
                if (slot == startSlot) return -1;
            }
            return slot;
        }

        public void Put(string key, T value)
        {
            int slot = SeekSlot(key);
            if (slot == -1)
            {
                RemoveLeastHitSlot();
                slot = SeekSlot(key);
            }
            hits[slot] = 0;
            slots[slot] = key;
            values[slot] = value;
        }

        public int RemoveLeastHitSlot()
        {
            int minSlot = 0;
            for (int i = 1; i < size; i++)
            {
                if (hits[i] < hits[minSlot])
                {
                    minSlot = i;
                }
            }
            hits[minSlot] = 0;
            slots[minSlot] = null;
            values[minSlot] = default;
            return minSlot;
        }

        public T Get(string key)
        {
            int slot = HashFun(key);
            int startSlot = slot;
            while (slots[slot] != null)
            {
                if (slots[slot] == key)
                {
                    hits[slot]++;
                    return values[slot];
                }
                slot += step;
                slot %= size;
                if (slot == startSlot) break;
            }
            return default;
        }
    }
}
