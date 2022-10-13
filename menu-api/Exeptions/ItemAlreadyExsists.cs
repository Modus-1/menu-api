using System.Runtime.Serialization;

namespace menu_api
{
    [Serializable]
    public class ItemAlreadyExsists : Exception
    {
        public ItemAlreadyExsists()
        {
        }

        public ItemAlreadyExsists(string? message) : base(message)
        {
        }

        public ItemAlreadyExsists(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ItemAlreadyExsists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}