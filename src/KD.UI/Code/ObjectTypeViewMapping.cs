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


public class TimerPlus : System.Timers.Timer
{
    private DateTime m_dueTime;

    public TimerPlus() : base() => this.Elapsed += this.ElapsedAction;
    public TimerPlus(TimeSpan interval) : base(interval) => this.Elapsed += this.ElapsedAction;

    protected new void Dispose()
    {
        this.Elapsed -= this.ElapsedAction;
        base.Dispose();
    }

    public TimeSpan TimeLeft => (this.m_dueTime - DateTime.Now);
    public new void Start()
    {
        this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
        base.Start();
    }

    private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (this.AutoReset)
            this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
    }
}