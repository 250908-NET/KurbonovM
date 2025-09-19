using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("9_ClassesChallenge.Tests")]
namespace _9_ClassesChallenge
{
    internal class Human2
    {

        private string lastName = "Smyth";
        private string firstName = "Pat";
        private string eyeColor;
        private int age;

        public Human2()
        {
        }

        public Human2(string firstName, string lastName, string eyeColor, int age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.eyeColor = eyeColor;
            this.age = age;
        }

        public Human2(string firstName, string lastName, int age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
        }

        public Human2(string firstName, string lastName, string eyeColor)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.eyeColor = eyeColor;
        }

        internal void AboutMe2()
        {
            Console.WriteLine($"My name is {firstName} {lastName}");
            if (age != 0)
            {
                Console.WriteLine($"I am {age} years old");
            }
            if (eyeColor != null)
            {
                Console.WriteLine($"My eye color is {eyeColor}");
            }
        }

        private double weight;
        public double Weight
        {
            get { return weight; }
            set
            {
                if (value < 0 || value > 400)
                {
                    weight = 0;
                }
                else
                {
                    weight = value;
                }
            }
        }
    }
}