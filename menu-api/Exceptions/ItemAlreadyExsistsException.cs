using System.Runtime.Serialization;

namespace menu_api.Exeptions
{
    [Serializable]
    public class ItemAlreadyExsistsException : Exception
    {
        public ItemAlreadyExsistsException()
        {
        }

        protected ItemAlreadyExsistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}