namespace WebMVC.Data
{
    public interface IWebAPIExecuter
    {
        Task<T?> InvokeGet<T>(string relativeUrl);
    }
}