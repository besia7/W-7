using System;
using System.Collections.Generic;

public class Repository<T>
{
    private List<T> items = new List<T>();

    // Делегат для критерію
    public delegate bool Criteria<T>(T item);

    // Метод для додавання елемента до репозиторію
    public void Add(T item)
    {
        items.Add(item);
    }

    // Метод для пошуку елементів, які задовольняють критерій
    public List<T> Find(Criteria<T> criteria)
    {
        List<T> result = new List<T>();
        foreach (T item in items)
        {
            if (criteria(item))
            {
                result.Add(item);
            }
        }
        return result;
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання дженеричного репозиторію з критеріями

        Repository<int> intRepository = new Repository<int>();
        intRepository.Add(10);
        intRepository.Add(20);
        intRepository.Add(30);
        intRepository.Add(40);

        // Визначення критерію для пошуку парних чисел
        Repository<int>.Criteria<int> isEven = x => x % 2 == 0;

        List<int> evenNumbers = intRepository.Find(isEven);
        Console.WriteLine("Парні числа:");
        foreach (int number in evenNumbers)
        {
            Console.WriteLine(number);
        }

        Repository<string> stringRepository = new Repository<string>();
        stringRepository.Add("apple");
        stringRepository.Add("banana");
        stringRepository.Add("cherry");
        stringRepository.Add("date");

        // Визначення критерію для пошуку слов, що починаються з букви 'b'
        Repository<string>.Criteria<string> startsWithB = s => s.StartsWith("b", StringComparison.OrdinalIgnoreCase);

        List<string> wordsStartingWithB = stringRepository.Find(startsWithB);
        Console.WriteLine("Слова, що починаються з 'b':");
        foreach (string word in wordsStartingWithB)
        {
            Console.WriteLine(word);
        }
    }
}
