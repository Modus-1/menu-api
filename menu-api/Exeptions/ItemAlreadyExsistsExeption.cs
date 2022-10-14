using System.Runtime.Serialization;

namespace menu_api.Exeptions
{
    [Serializable]
    public class ItemAlreadyExsistsExeption : Exception
    {
        public ItemAlreadyExsistsExeption()
        {
        }

    }
}