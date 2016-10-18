namespace GGFanGame.DataModel.Serizalitaion
{
    internal interface DataSerializer<T> where T : DataModel<T>
    {
        T FromString(string data);
        
        string ToString(DataModel<T> dataModel);
    }
}
