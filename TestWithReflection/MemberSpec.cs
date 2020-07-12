using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWithReflection
{
    public class MemberSpec
    {
        private string firstName;
        private string lastName;

        public string Name
        {
            get => firstName + " " + lastName;
            set
            {
                string[] s = value.Split(new char[] { ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                if (s.Length <= 1)
                    throw new FormatException("Name must include at least 2 " +
                        "elements divided by space");
                firstName = s[0];
                lastName = s[s.Length - 1];
            }
        }
        public int Age;

        public MemberSpec(string name, int age)
        {
            Name = name;
            Age = age;
        }

        private int GetLength(string s)
        {
            return s.Length;
        }

        public int GetLengthOfFirstName()
        {
            return GetLength(firstName);
        }

        public int GetLengthOfLastName()
        {
            return GetLength(lastName);
        }

        public int GetTotalLengthOfSpec()
        {
            return GetLength(Name) + Age;
        }
    }
}
