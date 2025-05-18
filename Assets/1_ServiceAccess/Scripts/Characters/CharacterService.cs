using System.Collections.Generic;
using UnityEngine;
using Universal.Singletons;

namespace Excercise1
{
    public class CharacterService : Singleton<CharacterService>
    {
        private readonly Dictionary<string, ICharacter> _charactersById = new();
        public bool TryAddCharacter(string id, ICharacter character)
            => _charactersById.TryAdd(id, character);
        public bool TryGetCharacter(string id, out ICharacter character)
            => _charactersById.TryGetValue(id, out character);
        public bool TryRemoveCharacter(string id)
            => _charactersById.Remove(id);
    }
}