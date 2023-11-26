namespace WebMVC.Data
{
    public interface IWebAPIExecuter
    {
        Task InvokeDelete(string relativeUrl);
        Task<T?> InvokeGet<T>(string relativeUrl);
        Task<T?> InvokePost<T>(string relativeUrl, T obj);
        Task InvokePut<T>(string relativeUrl, T obj);
    }
}