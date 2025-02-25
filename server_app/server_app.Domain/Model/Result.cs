using System.Net;

namespace server_app.Domain.Model;

public struct Result
{
    public Result(bool isSuccesed)
    {
        IsSuccesed = isSuccesed;

        if (isSuccesed)
            HttpCode = (int)HttpStatusCode.OK;
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
    public static Result Ok(object? value) => new Result(true, (int)HttpStatusCode.OK, value);

    public static Result Forbid() => new Result(false, (int)HttpStatusCode.Forbidden);

    public static Result InternalServerError() =>
        new Result(false, (int)HttpStatusCode.InternalServerError, "The Error in Server)");

    public static Result BadRequest(object? value = null) => new(false, (int)HttpStatusCode.BadRequest, value);
    public static Result NotFound(object? value = null) => new(false, (int)HttpStatusCode.NotFound, value);

    public static Result PaymentRequired() => new(false, (int)HttpStatusCode.PaymentRequired);

    //Сука лучше не писать числа, лучше блять юзать уже существующие константы из HttpStatusCode.
    
    //Я 3-4 часа блять убил не понимая хули при создании товара получаю 403, когда в других конечных точках не получаю, что уже означает что у меня все заеб с авторизацией, думал, дело в ClaimsExtension, до этого хуйня была с ним, думал опять. Нет нихуя,
    //дело было в том что я блять в Result классе сделал метод BadRequest котоырй возвращал как раз этот неверный код блтяь, просто пиздец.
}