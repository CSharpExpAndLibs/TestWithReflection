using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using TestWithReflection;
using System.IO;


namespace TestWithReflection.Tests
{
    /// <summary>
    /// UnitTest1 の概要の説明
    /// </summary>
    [TestClass]
    public class ProgramTest
    {

        Program program;

        public ProgramTest()
        {
            //
            // TODO: コンストラクター ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize()]
        public void Setup()
        {
        }

        [TestCleanup()]
        public void Teardown()
        {
        }

        [TestMethod]
        /*
         * Main()をargs付きで呼び出してテストする。
         */
        public void MainTest_WithArgs()
        {
            // Progrma ClassのTypeを取得する
            Program program = new Program();
            Type prg = program.GetType();

            // Main()のMethodInfoを取得する
            MethodInfo main = prg.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic);

            // 渡す引数定義
            string[] args = new string[]
            {
                // Idx=0
                "Sakamoto Shigeru",
                "57",

                // Idx=1
                "Niinuma Kenji",
                "120",
            };

            // 標準入力をリダイレクトしてMain()がIdx入力待ちになったらすぐに
            // 終了するように細工する
            string buffer = "exit\n";
            StringReader rdr = new StringReader(buffer);
            Console.SetIn(rdr);

            // Main()を実行する
            main.Invoke(null, new object[] { args });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            rdr.Dispose();

            // membersの内容を検証
            FieldInfo m = prg.GetField("members", 
                BindingFlags.NonPublic | BindingFlags.Static);
            MemberSpec[] membersArray = (MemberSpec[])m.GetValue(null);

            int numOfMembers = Program.NumOfMembers;
            Assert.AreEqual(args.Length / 2, numOfMembers);

            int idx = 0;
            foreach(MemberSpec s in membersArray)
            {
                Assert.AreEqual(args[idx++], s.Name);
                Assert.AreEqual(args[idx++], s.Age.ToString());
            }
        }

        [TestMethod]
        /*
         * Main()をargs無しで呼び出してテストする。
         */
        public void MainTest_WitOutArgs()
        {
            // Progrma ClassのTypeを取得する
            Program program = new Program();
            Type prg = program.GetType();

            // Main()のMethodInfoを取得する
            MethodInfo main = prg.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic);

            //---- 標準入力に与える入力をエミュレート ----
            // メンバー情報
            string[] args = new string[]
            {
                // Idx=0
                "Sakamoto Shigeru",
                "57",

                // Idx=1
                "Niinuma Kenji",
                "120",
            };
            StringBuilder sb = new StringBuilder();
            StringWriter wrt = new StringWriter(sb);
            // メンバー数
            wrt.WriteLine((args.Length / 2).ToString());
            // メンバー情報
            foreach(string s in args)
            {
                wrt.WriteLine(s);
            }
            // メンバー出力を出すためのIdx
            wrt.WriteLine(0.ToString());
            wrt.WriteLine(1.ToString());
            // Exit
            wrt.WriteLine("exit");
            StringReader rdr = new StringReader(sb.ToString());
            Console.SetIn(rdr);

            //----- 標準出力もリダイレクトする -----
            wrt.Dispose();
            sb = new StringBuilder();
            wrt = new StringWriter(sb);
            Console.SetOut(wrt);

            // Main()呼び出し
            main.Invoke(null, new object[] { null });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            rdr.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            wrt.Dispose();

            // 標準出力を確認する
            rdr = new StringReader(sb.ToString());
            string line;
            // member:0
            while (rdr.ReadLine() !=
                "出力したいメンバーIDを入力して下さい。終了したいときはexitを入力") ;
            Assert.AreEqual($"Name={args[0]}, Age={args[1]}", rdr.ReadLine());
            // member:1
            while (rdr.ReadLine() !=
                "出力したいメンバーIDを入力して下さい。終了したいときはexitを入力") ;
            Assert.AreEqual($"Name={args[2]}, Age={args[3]}", rdr.ReadLine());
            rdr.Dispose();
        }
    }
}
