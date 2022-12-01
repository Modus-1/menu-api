namespace menu_api.Exceptions
{
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