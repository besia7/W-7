using System;

public class Calculator<T>
{
    // Делегат для операції додавання
    public delegate T AddDelegate(T a, T b);

    // Делегат для операції віднімання
    public delegate T SubtractDelegate(T a, T b);

    // Делегат для операції множення
    public delegate T MultiplyDelegate(T a, T b);

    // Делегат для операції ділення
    public delegate T DivideDelegate(T a, T b);

    // Поля для збереження екземплярів делегатів
    private AddDelegate add;
    private SubtractDelegate subtract;
    private MultiplyDelegate multiply;
    private DivideDelegate divide;

    // Конструктор, де передаються делегати для арифметичних операцій
    public Calculator(AddDelegate add, SubtractDelegate subtract, MultiplyDelegate multiply, DivideDelegate divide)
    {
        this.add = add;
        this.subtract = subtract;
        this.multiply = multiply;
        this.divide = divide;
    }

    // Методи для виконання арифметичних операцій
    public T Add(T a, T b)
    {
        return add(a, b);
    }

    public T Subtract(T a, T b)
    {
        return subtract(a, b);
    }

    public T Multiply(T a, T b)
    {
        return multiply(a, b);
    }

    public T Divide(T a, T b)
    {
        if (b.Equals(default(T))) // Перевірка на ділення на нуль
        {
            throw new DivideByZeroException("Ділення на нуль.");
        }
        return divide(a, b);
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання класу Calculator для різних типів даних

        // Для цілочисельних типів (int)
        Calculator<int> intCalculator = new Calculator<int>(
            (a, b) => a + b,
            (a, b) => a - b,
            (a, b) => a * b,
            (a, b) => a / b);

        int result = intCalculator.Add(10, 5);
        Console.WriteLine("Додавання: " + result);

        // Для типів з плаваючою точкою (double)
        Calculator<double> doubleCalculator = new Calculator<double>(
            (a, b) => a + b,
            (a, b) => a - b,
            (a, b) => a * b,
            (a, b) => a / b);

        double doubleResult = doubleCalculator.Multiply(3.5, 2.0);
        Console.WriteLine("Множення: " + doubleResult);
    }
}
