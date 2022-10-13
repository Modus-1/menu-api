using System.Runtime.Serialization;

namespace menu_api
{
    [Serializable]
    public class ItemAlreadyExsists : Exception
    {
        public ItemAlreadyExsists()
        {
        }

    }
}