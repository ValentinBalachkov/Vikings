namespace SecondChanceSystem.SaveSystem
{
    public interface IData //реализуется у ScriptableObject, которые должны быть сохранены
    {
        void Save(); //Метод сохранения
        void Load(); //Метод сохранения
    }
}