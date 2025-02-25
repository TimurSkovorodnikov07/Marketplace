namespace server_app.Domain.Entities.ProductCategories.ValueObjects;

public class WebSiteValueObject
{
    public string WebSiteValue { get; set; }

    public static WebSiteValueObject? Create(string webSite)
    {
        //UriKind.Absolute vs UriKind.Releative:
        //https://stackoverflow.com/questions/20699572/what-is-difference-between-urikind-relative-and-urikind-absolute

        return Uri.TryCreate(webSite, UriKind.Absolute, out var result)
            ? new WebSiteValueObject { WebSiteValue = webSite }
            : null;
    }
}