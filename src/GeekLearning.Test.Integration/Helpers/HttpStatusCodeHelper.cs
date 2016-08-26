namespace System.Net
{
    public static class HttpStatusCodeHelper
    {
        public static bool IsErrorStatus(this HttpStatusCode statusCode)
        {
            return (int)statusCode >= 400;
        }
    }
}
