
namespace Game.Common
{
    /*------------------------------------------------------------------
      Template    : Singleton<T>
      Description : 单例模式实现模板。
      Usage       : Test instance = Singleton<Test>.Instance();
      Tips        : where T : A     - 表示类型T继承于A或A本身
                    where T : new() - 指明创建T实例时应该使用的构造函数
    --------------------------------------------------------------------*/
    public class Singleton<T> where T : new()
    {
        public Singleton()
        {
        }

        public static T Instance()
        {
            return SingletonCreator._instance;
        }

        class SingletonCreator
        {
            // 类静态构造函数在给定程序域中至多执行一次，只有创建类的实例或引用类的任何静态成员，才激发静态构造函数
            static SingletonCreator()
            {
            }

            internal static T _instance = new T();
        }
    }
}
