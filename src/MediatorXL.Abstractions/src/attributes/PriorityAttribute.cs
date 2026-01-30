
namespace MediatorXL.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PriorityAttribute(int priority = 0) : Attribute
{
    public int Priority { get; set; } = priority;
}
