using System.Runtime.Serialization;

namespace menu_api.Exeptions
{
    [Serializable]
    public class ItemAlreadyExsistsException : Exception
    {
        public ItemAlreadyExsistsException()
        {
        }

    }
}