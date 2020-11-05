namespace LalaSlots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LootTable
    {
        private readonly List<Loot> _lootTable;
        private readonly Dictionary<Loot, int> _cachedLoot;
        private readonly Random _rngInstance;
        private bool _isRebuildRequired;

        public LootTable()
        {
            _rngInstance = new Random();
            _cachedLoot  = new Dictionary<Loot, int>();
            _lootTable   = new List<Loot>();

            this.AddLoot(500, Loot.None);
            this.AddLoot(400, Loot.Two);
            this.AddLoot(100, Loot.Three);
            this.Build();
        }

        private void AddLoot(int probability, Loot name)
        {
            if (!_cachedLoot.ContainsKey(name))
            {
                this._cachedLoot.Add(name, probability);
                _isRebuildRequired = true;
            }
        }

        private void Build()
        {
            _lootTable.Clear();
            foreach (KeyValuePair<Loot, int> pair in _cachedLoot)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    _lootTable.Add(pair.Key);
                }
            }

            _isRebuildRequired = false;
        }

        public Loot NextRandomItem()
        {
            if (_isRebuildRequired) this.Build();
            
            return _lootTable[_rngInstance.Next(_lootTable.Count)];
        }
    }

    public enum Loot
    {
        None  = 0,
        Two   = 1,
        Three = 2,
    }
}
