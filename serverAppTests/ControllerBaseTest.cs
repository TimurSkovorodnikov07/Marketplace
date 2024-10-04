namespace serverAppTests;

public class ControllerBaseTest
{
    protected void CodeVerify(HttpResponseMessage response, int requiredCode)
    {
        Assert.That((int)response.StatusCode, Is.EqualTo(requiredCode), "Unexpected status code.");
    }
}