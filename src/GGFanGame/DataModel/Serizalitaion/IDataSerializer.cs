namespace GGFanGame.DataModel.Serizalitaion
{
    internal interface IDataSerializer<T> where T : DataModel<T>
    {
        T FromString(string data);
        
        string ToString(DataModel<T> dataModel);
    }
}
