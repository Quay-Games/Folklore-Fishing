using System;

[AttributeUsage(AttributeTargets.Class)]
//you can thank codex for this little bit of software engineering, but basically this
//is for labeling for data what nodes they are associated with, so that we dont have to do it manually
public class NodeDataTypeAttribute : Attribute
{
    public Type DataType { get; }

    public NodeDataTypeAttribute(Type dataType)
    {
        DataType = dataType;
    }
}