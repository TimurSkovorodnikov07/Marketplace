using Microsoft.AspNetCore.Mvc;

public struct Result
{
    public Result(bool isSuccesed)
    {
        IsSuccesed = isSuccesed;

        if (isSuccesed)
            ActionResult = new OkResult();
    }

    public Result(bool isSuccesed, IActionResult actionResult)
    {
        IsSuccesed = isSuccesed;
        ActionResult = actionResult;
    }

    public Result(bool isSuccesed, IActionResult actionResult, string errorMessage)
    {
        IsSuccesed = isSuccesed;
        ActionResult = actionResult;
        ErrorMessage = errorMessage;
    }

    public IActionResult ActionResult { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsSuccesed { get; set; }

    public static explicit operator bool(Result result) => result.IsSuccesed;


    public static Result Ok() => new Result(true);

    public static Result BadRequest(object? value = null) =>
        ResultReturn<BadRequestResult, BadRequestObjectResult>(false, new BadRequestObjectResult(value));

    public static Result NotFound(object? value = null) =>
        ResultReturn<NotFoundResult, NotFoundObjectResult>(false, new NotFoundObjectResult(value));

    public static Result PaymentRequired() => new Result(false, new StatusCodeResult(402));


    //Жаль шарп не позволяет указать вроде new(string) в where, в таком случаи я бы передавал value(object?) что удобнее чем ActionResultWithObjectT
    private static Result ResultReturn<ActionResultT, ActionResultWithObjectT>(bool isSuc,
        ActionResultWithObjectT? actionResult = null)
        where ActionResultT : StatusCodeResult, new()
        where ActionResultWithObjectT : ActionResult =>
        new Result(isSuc, actionResult is null
            ? new ActionResultT()
            : actionResult);
}