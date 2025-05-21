using System;
using UnityEngine;

namespace Excercise1
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] protected string id;

        protected virtual void OnEnable()
        {
            //acceder con singleton
            CharacterService charService = CharacterService.instance;
            if(charService) charService.TryAddCharacter(id, this);
            else
            {
                string _logTag = $"{name}({nameof(Character).Colored("#555555")}):";
                Debug.LogError($"{_logTag} CharacterService not found!");
            }
            //TODO: Add to CharacterService. The id should be the given serialized field. 
        }

        protected virtual void OnDisable()
        {
            //acceder con singleton
            CharacterService charService = CharacterService.instance;
            if(charService) charService.TryRemoveCharacter(id);
            else
            {
                string _logTag = $"{name}({nameof(Character).Colored("#555555")}):";
                Debug.LogError($"{_logTag} id not found!");
            }
        }
    }
}