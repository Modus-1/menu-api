using System.Runtime.Serialization;

namespace menu_api.Exeptions
{
    [Serializable]
    public class ItemDoesNotExistExeption : Exception
    {
        public ItemDoesNotExistExeption()
        {
        }

        public ItemDoesNotExistExeption(string? message) : base(message)
        {
        }
    }
}