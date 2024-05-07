using Newtonsoft.Json;

[Serializable]
public class MyClass
{
    public SerializableAction MyAction { get; set; }
    public SerializableAction MyAction1 { get; set; }
    public SerializableAction MyAction2 { get; set; }
    public SerializableAction MyAction3 { get; set; }
    public string Desc { get; set; } = "This is the test case for serialization";
    public void MyMethod<T>(T message)
    {
        Console.WriteLine(message);
    }
    public void MyFunc1(string message)
    {
        Console.WriteLine(message);
    }
    public void MyFunc2()
    {
        Console.WriteLine("This is MyFunc2.");
    }
    public void MyFunc3(int i, string message)
    {
        Console.WriteLine($"[{i}] : {message}");
    }
}

class TestCase
{
    static void Main()
    {
        var obj = new MyClass();
        obj.MyAction = new SerializableAction(new Action<string>(obj.MyMethod<string>));
        obj.MyAction1 = new SerializableAction(new Action<string>(obj.MyFunc1));
        obj.MyAction2 = new SerializableAction(new Action(obj.MyFunc2));
        obj.MyAction3 = new SerializableAction(new Action<int, string>(obj.MyFunc3));

        var serialized = JsonConvert.SerializeObject(obj);
        Console.WriteLine("Serialized:");
        Console.WriteLine(serialized);

        var deserialized = JsonConvert.DeserializeObject<MyClass>(serialized);
        deserialized.MyAction.Invoke("Hello, Method");
        deserialized.MyAction1.Invoke("Hello, Func");
        deserialized.MyAction2.Invoke();
        deserialized.MyAction3.Invoke(999, "AAA");
    }
}
