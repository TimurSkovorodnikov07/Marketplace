import {
  emailInvalidText,
  passwordInvalidText,
} from "../../configs/TextsDuringInvalidity";
import emailValidator from "../../validators/emailValidator";
import passwordValidator from "../../validators/passwordValidator";
import { useRef, useState } from "react";
import { InputComponent } from "../../components/InputComponent";
import { Link, useNavigate } from "react-router-dom";
import { EmailVerify, saveAuthDates } from "../../components/EmailVerify";
import { login } from "../../requests/baseAuthRequests";
import { useResponseCode } from "../../hooks/customHooks";
import { useDispatch } from "react-redux";
import { stringToBool } from "../../other/converter";

export default function LoginPage() {
  //For NOT confirmed
  const [responseData, setResponseData] = useState({
    userId: "",
    codeDiedAfterSeconds: 0,
    codeLength: 0,
  });

  //Generic
  const navigate = useNavigate();
  const dispath = useDispatch();
  const [codeAndText, setCodeAndText] = useResponseCode();
  const [accountIsConfirmed, setAccountIsConfirmed] = useState(false);

  const emailRef = useRef(null);
  const pasRef = useRef(null);

  const [emailIsValid, setEmailIsValid] = useState(false);
  const [pasIsValid, setPasIsValid] = useState(false);
  const [sent, setSent] = useState(false); //Чтобы юзер не отправил дважды запрос

  async function onSubmit() {
    setSent(true);

    try {
      const response = await login(
        emailRef?.current?.value,
        pasRef?.current?.value
      );

      setCodeAndText({
        code: response.status,
        text: response.statusText,
      });

      if (response.status === 200) {
        const headerValue = response.headers["x-account-is-confirmed"];
        const isCon = stringToBool(headerValue);
        const userId = response.data.userId;

        if (isCon === true) {
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
        } else if (isCon === false) {
          setResponseData({
            userId: userId,
            codeLength: response.data.codeLength,
            codeDiedAfterSeconds: response.data.codeDiedAfterSeconds,
          });
        }
        setAccountIsConfirmed(isCon);
      }
    } catch (error) {
      console.error(error);
      setSent(false);
      setCodeAndText({
        code: error.response.status,
        text: error.response.data,
      });
    }
  }
  const ren = () => {
    switch (codeAndText.code) {
      case 200:
        if (accountIsConfirmed === true) {
          return <></>;
        } else {
          return (
            <EmailVerify
              userId={responseData.userId}
              codeDiedAfterSeconds={responseData.codeDiedAfterSeconds}
              codeLength={responseData.codeLength}
            />
          );
        }
      default:
        return (
          <div className="login-page">
            <div className="min-h-[70vh] mb-16">
              <div className="mb-16">
                <h2>Log in</h2> to your account to use our application
              </div>
              <div className="mb-8">
                <InputComponent
                  id="emailInput"
                  type="email"
                  invalidText={emailInvalidText}
                  validatorFun={emailValidator}
                  afterValidationFun={(e) => setEmailIsValid(true)}
                  invalidatedFun={() => setEmailIsValid(false)}
                  ref={emailRef}
                  labelText={"Email: "}
                  inputOtherProps={{ placeholder: "example@mail.abc..." }}
                />
              </div>
              <div className="mb-8">
                <InputComponent
                  id="passwordInput"
                  type="password"
                  invalidText={passwordInvalidText}
                  validatorFun={passwordValidator}
                  afterValidationFun={(e) => setPasIsValid(true)}
                  invalidatedFun={() => setPasIsValid(false)}
                  ref={pasRef}
                  labelText={"Password: "}
                  inputOtherProps={{ placeholder: "MegaPasw03r+dD..." }}
                />
              </div>
            </div>
            <div>
              {codeAndText.code === 404 ||
                (codeAndText.code === 400 && (
                  <div className="error-text">{codeAndText.text}</div>
                ))}

              <input
                type="submit"
                onClick={async () => {
                  if (
                    emailIsValid === true &&
                    pasIsValid === true &&
                    sent === false
                  )
                    await onSubmit();
                }}
              />
              <div>
                <p>
                  If you don't have an account,{" "}
                  <Link to={"/registration"}>create</Link> one
                </p>
              </div>
            </div>
          </div>
        );
    }
  };
  return <>{ren()}</>;
}
