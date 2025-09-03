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
  const [sent, setSent] = useState(false);
  const [isAllValid, setIsAllValid] = useState(true);

  async function onSubmit() {
    try {
      const response = await login(
        emailRef.current.value,
        pasRef.current.value
      );

      if (response.status === 200) {
        setCodeAndText({
          code: response.status,
          text: response.statusText,
        });
        setSent(true);

        const headerValue = response.headers["x-account-is-confirmed"];
        let isConfirmed = stringToBool(headerValue);
        const userId = response.data.userId;

        if (isConfirmed === true) {
          saveAuthDates(
            {
              userId: response.data.userId,
              accessToken: response.data.accessToken,
              refreshToken: response.data.refreshToken,
              isCustomer: response.data.isCustomer,
            },
            dispath
          );
          navigate("/");
        } else if (isConfirmed === false) {
          setResponseData({
            userId: userId,
            codeLength: response.data.codeLength,
            codeDiedAfterSeconds: response.data.codeDiedAfterSeconds,
          });
        }
        setAccountIsConfirmed(isConfirmed);
      }
    } catch (error) {
      console.error(error);
      console.log(error);
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
      case 404:
        <div>Idi nahui!</div>;
      default:
        return (
          <div className="login-page">
            <div className="mb-16">
              <h2>Log in</h2> to your account to use our application
            </div>

            <div className="login-page-inputs min-h-[70vh] mb-16">
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
                showInvalidText={isAllValid === false}
              />
              <div className="mb-8"></div>

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
                showInvalidText={isAllValid === false}
              />
            </div>

            <div>
              <input
                type="submit"
                onClick={async () => {
                  let isVal =
                    emailIsValid === true &&
                    pasIsValid === true &&
                    sent === false;

                  setIsAllValid(isVal);
                  if (isVal === true) await onSubmit();
                }}
              />
              {codeAndText.code == 404 && (
                <div className="error-big-text">The User doesn't exist</div>
              )}
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
