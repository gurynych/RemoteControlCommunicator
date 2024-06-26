﻿using NetworkMessage.Intents.ConcreteIntents;
using System.Reflection;

namespace NetworkMessage.Intents
{
    internal static class IntentConverter
    {
        private static readonly Dictionary<string, Type> intents;

        static IntentConverter()
        {
            string namespaceName = typeof(AmountOfRAMIntent).Namespace;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> namespaceTypes = types.Where(type => type.Namespace == namespaceName);
            intents = namespaceTypes.ToDictionary(type => type.Name, type => type);
        }

        public static Type GetType(string intentType) =>
            intents.GetValueOrDefault(intentType);
    }
}
