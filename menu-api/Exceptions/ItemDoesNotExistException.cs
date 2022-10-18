using System.Runtime.Serialization;

namespace menu_api.Exeptions
{
    [Serializable]
    public class ItemDoesNotExistException : Exception
    {
        public ItemDoesNotExistException()
        {
        }

        public ItemDoesNotExistException(string? message) : base(message)
        {
        }
    }
}