using System;

// 定义一个抽象类 Animal
public abstract class Animal
{
    // 定义一个抽象方法 MakeSound
    public abstract void MakeSound();
}

// 定义一个派生类 Cat，继承自 Animal
public class Cat : Animal
{
    // 重写父类的抽象方法 MakeSound
    public override void MakeSound()
    {
        Console.WriteLine("Meow");
    }
}

// 定义一个派生类 Dog，继承自 Animal
public class Dog : Animal
{
    // 重写父类的抽象方法 MakeSound
    public override void MakeSound()
    {
        Console.WriteLine("Woof");
    }
}

// 在主程序中创建不同类型的对象，并调用它们的方法
class Program
{
    static void Main(string[] args)
    {
        // 创建一个 Cat 对象，并赋值给一个 Animal 类型的变量 a1
        Animal a1 = new Cat();
        // 调用 a1 的 MakeSound 方法，实际上调用的是 Cat 类中重写的方法
        a1.MakeSound(); // 输出 Meow

        // 创建一个 Dog 对象，并赋值给一个 Animal 类型的变量 a2
        Animal a2 = new Dog();
        // 调用 a2 的 MakeSound 方法，实际上调用的是 Dog 类中重写的方法
        a2.MakeSound(); // 输出 Woof

        Console.ReadKey();
    }
}
//委托的示例代码：

// 定义一个委托类型，表示接受两个 int 参数并返回 int 的方法引用 
public delegate int Calculator(int x, int y);

// 定义两个静态方法，分别实现加法和乘法运算，并符合委托类型的签名和返回类型 
public static class MathOperations
{
    public static int Add(int x, int y)
    {
        return x + y;
    }

    public static int Multiply(int x, int y)
    {
        return x * y;
    }
}

// 在主程序中创建委托对象，并将它们指向不同的方法 
class Program
{
    static void Main(string[] args)
    {
        // 创建一个 Calculator 委托对象 calc1，并将它指向 MathOperations 类中的 Add 方法 
        Calculator calc1 = new Calculator(MathOperations.Add);
        // 调用 calc1 委托对象，相当于调用 Add 方法，并传入两个参数 3 和 4 
        int result1 = calc1(3, 4); // 返回 7

        // 创建另一个 Calculator 委托对象 calc2，并将它指向 MathOperations 类中的 Multiply 方法 
        Calculator calc2 = new Calculator(MathOperations.Multiply);
        // 调用 calc2 委托对象，相当于调用 Multiply 方法，并传入两个参数 3 和 4 
        int result2 = calc2(3, 4); // 返回 12

        Console.WriteLine(result1);
        Console.WriteLine(result2);

        Console.ReadKey();
    }
}