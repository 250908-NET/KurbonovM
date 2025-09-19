using System;

namespace _9_ClassesChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Human human = new Human();
            Human human1 = new Human("John", "Doe");
            human.AboutMe();
            human1.AboutMe();
            Console.WriteLine("--------------------");

            Human2 human2_1 = new Human2("Jane", "Doe", "Blue");
            Human2 human2_2 = new Human2("Sam", "Smith", 30);
            Human2 human2_3 = new Human2("Alice", "Johnson", "Green", 25);
            human2_1.AboutMe2();
            human2_2.AboutMe2();
            human2_3.AboutMe2();
            Console.WriteLine("--------------------");

            Human2 human2_4 = new Human2();
            human2_4.Weight = 150;
            Console.WriteLine($"Human2's initial Weight: {human2_4.Weight}"); // Output: Human2's initial Weight: 150
            human2_4.Weight = -10; // Invalid weight, should reset to 0

            Console.WriteLine($"Human2 Weight: {human2_4.Weight}"); // Output: Human2 Weight: 0
        }
    }
} 
