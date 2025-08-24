using KD.Infrastructure.k8s;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Reflection;

namespace KD.UI.Code;

public record ObjectTypes(Type? View, Type? Property);

public static class ObjectTypeViewMapping
{
    public static ReadOnlyDictionary<string, ObjectTypes> Map;

    static ObjectTypeViewMapping()
    {
        var map = new Dictionary<string, ObjectTypes>();

        var constants = typeof(ObjectType)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
               .Select(fi => (string)fi.GetRawConstantValue()!)
               .ToList();

        var allComponents = Assembly
                .GetExecutingAssembly()
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)));

        foreach (var c in constants)
        {
            var view = allComponents.SingleOrDefault(x => x.Name.Equals($"{c}View", StringComparison.InvariantCultureIgnoreCase));
            var property = allComponents.SingleOrDefault(x => x.Name.Equals($"{c}Property", StringComparison.InvariantCultureIgnoreCase));

            map.Add(c, new ObjectTypes(view, property));
        }

        Map = map.AsReadOnly();
    }
}

public record ConditionViewModel(string Type, string Status, string Reason, string Message);