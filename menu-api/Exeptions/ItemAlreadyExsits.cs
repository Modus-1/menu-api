using System.Runtime.Serialization;

namespace menu_api
{
    [Serializable]
    public class ItemAlreadyExsits : Exception
    {
        public ItemAlreadyExsits()
        {
        }

        public ItemAlreadyExsits(string? message) : base(message)
        {
        }

        public ItemAlreadyExsits(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ItemAlreadyExsits(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}