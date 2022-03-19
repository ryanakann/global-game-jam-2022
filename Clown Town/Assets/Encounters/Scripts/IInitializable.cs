namespace Encounters
{
    public interface IInitializable<T>
    {
        public void Init(T info);
    }
}