import { useEffect, useState } from "react";
import { InputComponent } from "./InputComponent";
import { useNavigate } from "react-router-dom";
import { emailVerify, codeResend } from "../requests/baseAuthRequests";
import { useDispatch } from "react-redux";
import { loginType } from "../redux/authReducer";
import { refreshTokenInCookies } from "../configs/cookiesName";
import { stringToBool } from "../other/converter";

export function saveAuthDates(data, dispath) {
  try {
    const payload = {
      accessToken: data.accessToken,
      refreshToken: data.refreshToken,
      userId: data.userId,
      isCustomer: data.isCustomer,
    };

    if (typeof data.isCustomer != "boolean")
      throw new Error("IsCutomer NOT boolean");

    dispath({
      type: loginType,
      payload: payload,
    });
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export function EmailVerify({ userId, codeDiedAfterSeconds, codeLength }) {
  const navigate = useNavigate();
  const dispath = useDispatch();

  const [time, setTime] = useState(codeDiedAfterSeconds);
  const [errorText, setErrorText] = useState("");

  useEffect(() => {
    //setTimeout позволяет вызвать функцию один раз через определённый интервал времени.
    //Например проходит 3 сек, работает фукнция, все, дальше он не будет работать.

    //setInterval позволяет вызывать функцию регулярно, повторяя вызов через определённый интервал времени.
    //Он же работать будет каждые 3 секунды
    const timer = setInterval(() => {
      setTime((prevTime) => {
        if (prevTime <= 0) {
          clearInterval(prevTime);
          return 0;
        }
        return prevTime - 1;
      });
    }, 1000);
    return () => clearInterval(timer);
  }, []);

  async function emailVer(code) {
    try {
      const response = await emailVerify(userId, code);

      if (response.status == 200) {
        saveAuthDates(
          {
            userId: response.data.userId,
            accessToken: response.data.accessToken,
            refreshToken: response.data.refreshToken,
            isCustomer: stringToBool(response.data.isCustomer),
          },
          dispath
        );
        navigate("/");
      }
    } catch (error) {
      setErrorText(error.response.data);
    }
  }

  async function resend() {
    try {
      const response = await codeResend(userId);

      if (response.status === 200) {
        console.log(response);
        setErrorText("");
        setTime(response.data.codeDiedAfterSeconds);
      }
    } catch (e) {
      console.log(e);
      if (e.status === 404)
        console.error(`userId такого на серваке нету, мб userId = ""`);

      console.error(`Code: ${e.status}, Text: ${e.data}`);
    }
  }

  return (
    <>
      <div>
        {time > 0 ? (
          <>
            <p>The code will become invalid after: {time} seconds</p>
          </>
        ) : (
          <p>Time is up, send the code again, and write a new code below</p>
        )}
      </div>
      <div>
        <InputComponent
          id="codeInput"
          type="text"
          validatorFun={(value) => {
            console.log(value);
            console.log(codeLength);
            return value.length === codeLength;
          }}
          invalidatedFun={() =>
            setErrorText(`The code must be ${codeLength} characters long`)
          }
          afterValidationFun={(e) => emailVer(e.target.value)}
          inputOtherProps={{
            placeholder: "Code...",
          }}
        />
      </div>
      <div>
        <p id="error-text" className="error-text">
          {errorText}
        </p>
        <button onClick={async () => await resend()}>
          Send the code again
        </button>
        <p>
          If the code has not yet been sent to your email address, check the
          quality The Internet and whether you entered your email address
          accurately and whether our message does not end up in spam
        </p>
      </div>
    </>
  );
}
