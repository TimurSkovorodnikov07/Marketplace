namespace server_app.Domain.Model;

public struct Result
{
    public Result(bool isSuccesed)
    {
        IsSuccesed = isSuccesed;

        if (isSuccesed)
            HttpCode = 200;
    }

    public Result(bool isSuccesed, int code)
    {
        IsSuccesed = isSuccesed;
        HttpCode = code;
    }

    public Result(bool isSuccesed, int code, object? value)
    {
        IsSuccesed = isSuccesed;
        HttpCode = code;
        Value = value;
    }

    public int HttpCode { get; set; }
    public object? Value { get; set; }
    public bool IsSuccesed { get; set; }

    public static explicit operator bool(Result result) => result.IsSuccesed;


    public static Result Ok() => new Result(true);
    public static Result Ok(object? value) => new Result(true, 200, value);

    public static Result Forbid() => new Result(false, 403);
    public static Result InternalServerError() => new Result(false, 500, "The Error in Server)");
    public static Result BadRequest(object? value = null) => new(false, 403, value);
    public static Result NotFound(object? value = null) => new() { IsSuccesed = false, Value = value, };

    public static Result PaymentRequired() => new(false, 402);


    //Жаль шарп не позволяет указать вроде new(string) в where, в таком случаи я бы передавал value(object?) что удобнее чем ActionResultWithObjectT
    // private static Result ResultReturn<ActionResultT, ActionResultWithObjectT>(bool isSuc,
    //     ActionResultWithObjectT? actionResult = null)
    //     where ActionResultT : StatusCodeResult, new()
    //     where ActionResultWithObjectT : ActionResult =>
    //     new Result(isSuc, actionResult is null
    //         ? new ActionResultT()
    //         : actionResult);
}
