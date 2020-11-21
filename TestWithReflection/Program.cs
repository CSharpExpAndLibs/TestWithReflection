using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWithReflection
{

    public class Program
    {
        private static MemberSpec[] members;
        public static int NumOfMembers;

        static void Main(string[] args)
        {
            if(args != null && args.Length != 0)
            {
                NumOfMembers = args.Length / 2;
                members = new MemberSpec[NumOfMembers];

                for (int i = 0; i < NumOfMembers; i++)
                {
                    members[i] = new MemberSpec(args[2 * i], Convert.ToInt32(args[2 * i + 1]));
                }
            }
            else
            {
                Console.WriteLine("メンバー数を入力して下さい");
                NumOfMembers = Convert.ToInt32(Console.ReadLine());
                members = new MemberSpec[NumOfMembers];

                for (int i = 0; i < NumOfMembers; i++)
                {
                    Console.WriteLine("メンバー{0}:名前を入力して下さい", i);
                    string name = Console.ReadLine();
                    Console.WriteLine("メンバー{0}:年齢を入力して下さい", i);
                    int age = Convert.ToInt32(Console.ReadLine());

                    members[i] = new MemberSpec(name, age);
                }
            }

            string buff = "";
            while(true)
            {
                Console.WriteLine("出力したいメンバーIDを入力して下さい。終了したいときはexitを入力");
                buff = Console.ReadLine();
                if (buff.ToUpper() == "EXIT")
                    break;

                int id = Convert.ToInt32(buff);
                Console.WriteLine("Name={0}, Age={1}", members[id].Name, members[id].Age);
            }

        }
    }
}
