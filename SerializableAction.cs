using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SerializableAction : ISerializable
{
    private readonly byte[] _targetData;
    private readonly string _methodName;

    public SerializableAction(Delegate action)
    {
        _methodName = action.Method.Name;
        _targetData = SerializeTarget(action.Target);
    }

    protected SerializableAction(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException(nameof(info));

        _methodName = info.GetString("MethodName");
        _targetData = (byte[])info.GetValue("TargetData", typeof(byte[]));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue("MethodName", _methodName);
        info.AddValue("TargetData", _targetData);
    }

    public void Invoke(params object[] args)
    {
        object target = DeserializeTarget(_targetData);
        MethodInfo method = target.GetType().GetMethod(_methodName);
        // MethodInfo genericMethod = method.MakeGenericMethod(args.Select(a => a.GetType()).ToArray());
        // genericMethod.Invoke(target, args);
        method.Invoke(target, args);
    }

    private static byte[] SerializeTarget(object target)
    {
        if (target == null)
            return null;

        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, target);
            return stream.ToArray();
        }
    }

    private static object DeserializeTarget(byte[] data)
    {
        if (data == null)
            return null;

        using (MemoryStream stream = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
    }
}
