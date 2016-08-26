namespace System.Net
{
    public static class HttpStatusCodeHelper
    {
        public static bool IsErrorCode(this HttpStatusCode statusCode)
        {
            return (int)statusCode >= 400;
        }
    }
}
