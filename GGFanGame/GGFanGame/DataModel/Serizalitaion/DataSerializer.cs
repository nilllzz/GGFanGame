namespace GGFanGame.DataModel.Serizalitaion
{
    internal interface DataSerializer<T> where T : DataModel<T>
    {
        T fromString(string data);
        
        string toString(DataModel<T> dataModel);
    }
}
