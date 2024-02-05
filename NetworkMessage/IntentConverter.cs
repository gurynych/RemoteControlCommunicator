using NetworkMessage.Intents;
using System.Reflection;

namespace NetworkMessage
{
    internal static class IntentConverter
    {
        private static readonly Dictionary<string, Type> intents;

        static IntentConverter()
        {
            string namespaceName = typeof(BaseIntent).Namespace;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> namespaceTypes = types.Where(type => type.Namespace == namespaceName);
            intents = namespaceTypes.ToDictionary(type => type.Name, type => type);
        }

        public static Type GetType(string intentType) =>
            intents.GetValueOrDefault(intentType);
    }
}
