#nullable disable
using Newtonsoft.Json.Serialization;

namespace Disorder.Helpers.Serialization; 

public class KnownTypesBinder : ISerializationBinder {
    public IList<Type> KnownTypes { get; set; }

    public Type BindToType(string assemblyName, string typeName) {
        return KnownTypes.SingleOrDefault(t => t.Name == typeName);
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName) {
        assemblyName = null;
        typeName = serializedType.Name;
    }
}