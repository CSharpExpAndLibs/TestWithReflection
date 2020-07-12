using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;


namespace TestWithReflection.Tests
{

    [TestClass]
    public class MemberSpecTest
    {
        [TestInitialize]
        public void Setup()
        {
            // Runs before each test. (Optional)
        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [TestMethod]
        /*
         * 正常に構築されることをテストする
         */
        public void MemberSpecTest_ConstructNormally()
        {
            MemberSpec m = new MemberSpec(
                "Sakamoto Shigeru",
                57
                );

            // Name = "Sakamoto Shigeru"
            // Age = 57
            Assert.AreEqual("Sakamoto Shigeru", m.Name);
            Assert.AreEqual(57, m.Age);

            /*
             * private firstName, lastNameが正しく設定されている
             * ことを確認する。
             * private fieldはReflectionで取得する
             */

            // mインスタンスのタイプ取得
            Type m_type = m.GetType();

            // firstName FieldInfoを取得
            FieldInfo finf = m_type.GetField(
                "firstName",
                BindingFlags.NonPublic |
                BindingFlags.Instance);
            Assert.AreEqual("Sakamoto", finf.GetValue(m));

            // lastName FieldInfoを取得
            finf = m_type.GetField(
                "lastName",
                BindingFlags.NonPublic |
                BindingFlags.Instance);
            Assert.AreEqual("Shigeru", finf.GetValue(m));
        }

        [TestMethod]
        /*
         * NameSetterの動作をテストする
         */
        public void NameSetterTest()
        {
            MemberSpec m = new MemberSpec(
                "Sakamoto Shigeru",
                57
                );

            // Name = "Sakamoto Shigeru"
            // Age = 57
            Assert.AreEqual("Sakamoto Shigeru", m.Name);
            Assert.AreEqual(57, m.Age);

            // 名前を変更する。ついでに日本語対応もチェック
            m.Name = "坂本 茂";
            Assert.AreEqual("坂本 茂", m.Name);

            // null例外の確認
            try
            {
                m.Name = null;
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is NullReferenceException);
            }

            // lastNameがない場合はFormatExceptionが起きる
            try
            {
                m.Name = "HOGEHOGE";
                Assert.IsTrue(false, "FormatException should occur!");
            }
            catch (Exception e)
            {
                Assert.IsTrue((e is FormatException), "Exception should be a FormatException");
                Assert.AreEqual("Name must include at least 2 elements divided by space",
                    e.Message);
            }

            // 3つ以上のエレメントがあったら、lastNameは最後のエレメントと等しい
            // あと、Empty stringが抑制されていることも同時に確認
            m.Name = "  Elvis christ Presley  ";
            Assert.AreEqual("Elvis Presley", m.Name);
        }

        [TestMethod]
        /*
         * private method GetLength()をテストする
         */
        public void GetLengthTest_Normal()
        {
            MemberSpec m = new MemberSpec(
                "Sakamoto Shigeru",
                57
                );

            // methodinfoを取得する
            MethodInfo method = m.GetType().
                GetMethod(
                    "GetLength",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.InvokeMethod
                );
            // 実行する
            string testStr = "AsssJk  ooooa ass122334";
            int len = (int)method.Invoke(
                m,
                new object[1] { testStr }
                );
            Assert.AreEqual(testStr.Length, len);
        }

        [TestMethod]
        /*
         * GetLengthOf(First|Last)Name()
         * GetTotalLengthOfSpec()
         * をテストする
         */
        public void GetLengthOfNameTest()
        {
            MemberSpec m = new MemberSpec(
                "Sakamoto Shigeruuuuuuuuuuu",
                57
                );

            Assert.AreEqual("Sakamoto".Length, m.GetLengthOfFirstName());
            Assert.AreEqual("Shigeruuuuuuuuuuu".Length, m.GetLengthOfLastName());
            Assert.AreEqual("Sakamoto Shigeruuuuuuuuuuu".Length + 57, m.GetTotalLengthOfSpec());
        }

    }
}
