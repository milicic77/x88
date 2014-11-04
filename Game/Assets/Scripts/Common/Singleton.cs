
namespace Game.Common
{
    /**
     *   Use:
     *   
     *      Response.Write(Singleton<Test>.Instance().Time);  ...
     */
    public class Singleton<T> where T : new()
    {
        public Singleton() { }

        public static T Instance()
        {
            return SingletonCreator.instance;
        }

        class SingletonCreator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonCreator() { }
            internal static T instance = new T();
        }
    }
}