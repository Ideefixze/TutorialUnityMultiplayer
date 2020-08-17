using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockNet.GameState.Commands
{
/// <summary>
    /// All classes that will be serialized and unserialized should have their type contained in string so it helps unserializing them by their type.
    /// </summary>
    [Serializable]
    public class SerializableClass
    {
        [SerializeField]
        private string serializedClassName;

        public SerializableClass()
        {
            serializedClassName = this.GetType().ToString();
        }

        public string GetClassName()
        {
            return serializedClassName;
        }

    }
}