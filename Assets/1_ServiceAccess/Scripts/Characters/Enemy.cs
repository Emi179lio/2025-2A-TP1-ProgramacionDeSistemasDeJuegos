using System;
using UnityEngine;

namespace Excercise1
{
    public class Enemy : Character
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private string playerId = "Player";
        private ICharacter _player;
        private string _logTag;

        private void Reset()
            => id = nameof(Enemy);

        private void Awake()
            => _logTag = $"{name}({nameof(Enemy).Colored("#555555")}):";

        protected override void OnEnable()
        {
            base.OnEnable();
            
            //Try get player, if not found log error
            CharacterService charService = CharacterService.instance;
            if(!(charService && CharacterService.instance.TryGetCharacter(playerId, out _player)))
                Debug.LogError($"{_logTag} Player not found!");
        }

        private void Update()
        {
            if (_player == null)
                return;
            var direction = _player.transform.position - transform.position;
            transform.position += direction.normalized * (speed * Time.deltaTime);
        }
    }
}