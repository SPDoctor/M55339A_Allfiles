namespace ValuesAndReferences
{
    struct MyStruct
    {
        public int Contents;
    }

    class MyClass
    {
        public int Contents = 0;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyStruct struct1 = new MyStruct();
            MyStruct struct2 = struct1;
            struct2.Contents = 100;

            MyClass class1 = new MyClass();
            MyClass class2 = class1;
            class2.Contents = 100;

            Console.WriteLine($"Value types: {struct1.Contents}, {struct2.Contents}");
            Console.WriteLine($"Reference types: {class1.Contents}, {class2.Contents}");
        }
    }
}
