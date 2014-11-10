
namespace Game.Common
{
    /*------------------------------------------------------------------
      Template    : Singleton<T>
      Description : 单例模式实现模板。
      Usage       : Test instance = Singleton<Test>.Instance();
      Tips        : where T : A     - 表示类型T继承于A或A本身
                    where T : new() - 指明创建T实例时应该使用无参构造函数
    --------------------------------------------------------------------*/
    public class Singleton<T> where T : new()
    {
        public static T Instance()
        {
            return SingletonCreator._instance;
        }

        class SingletonCreator
        {
            static SingletonCreator()
            { // 类静态构造函数 : 程序域中至多执行一次。执行条件：创建类的实例或引用类的任何静态成员
                _instance = new T();                                    // 第一次访问才实例化
            }

            // internal   : 只有在同一程序集的文件中才可访问
            // default(T) : T为引用类型返回null，T为数值类型返回0
            internal static T _instance = default(T);
        }
    }
}
