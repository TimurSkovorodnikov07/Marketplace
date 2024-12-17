import {
  emailInvalidText,
  nameInvalidText,
  passwordInvalidText,
  descriptionInvalidText,
} from "../../configs/TextsDuringInvalidity";
import emailValidator from "../../validators/emailValidator";
import passwordValidator from "../../validators/passwordValidator";
import userNameValidator from "../../validators/userNameValidator";
import { descriptionValidator } from "../../validators/descriptionValidator";

import { useRef, useState } from "react";
import { InputComponent } from "../../components/InputComponent";
import { Link } from "react-router-dom";
import { EmailVerify } from "../../components/EmailVerify";
import { customerAccountCreate } from "../../requests/customerRequests";
import { sellerAccountCreate } from "../../requests/sellerRequests";
import { useResponseCode } from "../../hooks/customHooks";
import { TextAreaComponent } from "../../components/TextAreaComponent";

export default function RegistrationPage() {
  const [isForCustomer, setIsForCustomer] = useState(true);
  const [codeAndText, setCodeAndText] = useResponseCode();
  //Generic
  const [userId, setUserId] = useState("");
  const [codeDiedAfterSeconds, setCodeDiedAfterSeconds] = useState(0);
  const [codeLength, setCodeLength] = useState(0);

  const nameRef = useRef(null);
  const emailRef = useRef(null);
  const pasRef = useRef(null);

  const [nameIsValid, setNameIsValid] = useState(false);
  const [emailIsValid, setEmailIsValid] = useState(false);
  const [pasIsValid, setPasIsValid] = useState(false);
  const [sent, setSent] = useState(false); //Чтобы юзер не отправил дважды запрос

  //Only for Seller
  const desRef = useRef(null);
  const [descriptionIsValid, setDescriptionNameIsValid] = useState(false);

  async function onSubmit() {
    setSent(true);

    try {
      const response = isForCustomer
        ? await customerAccountCreate(
            nameRef?.current?.value,
            emailRef?.current?.value,
            pasRef?.current?.value
          )
        : await sellerAccountCreate(
            nameRef?.current?.value,
            emailRef?.current?.value,
            pasRef?.current?.value,
            desRef?.current?.value
          );
      if (response.status === 200) {
        setUserId(response.data.userId);
        setCodeLength(parseInt(response.data.codeLength));
        setCodeDiedAfterSeconds(parseInt(response.data.codeDiedAfterSeconds));
      }
      setCodeAndText({ code: response.status, text: response.statusText });
    } catch (error) {
      setSent(false);
      console.log(error);
      setCodeAndText({
        code: error.response.status,
        text: error.response.data,
      });
    }
  }

  const ren = () => {
    switch (codeAndText.code) {
      case 200:
        return (
          <EmailVerify
            userId={userId}
            codeDiedAfterSeconds={codeDiedAfterSeconds}
            codeLength={codeLength}
          />
        );
      case 400:
        return <p>Эксепшены чекни</p>;
      default:
        return (
          <div className="registration-page">
            <div className="min-h-[70vh] mb-16">
              <div className="mb-16">
                {isForCustomer ? (
                  <>
                    <h2>Registration</h2> to your account to use our application
                  </>
                ) : (
                  <>
                    <h2>Registration</h2>
                    to your{" "}
                    <span className="give-attention-text text-[#f38ba8]">
                      seller
                    </span>{" "}
                    account to use our application
                  </>
                )}
              </div>
              <div className="mb-8">
                <InputComponent
                  id="nameInput"
                  type="text"
                  invalidText={nameInvalidText}
                  validatorFun={userNameValidator}
                  afterValidationFun={(e) => setNameIsValid(true)}
                  invalidatedFun={() => setNameIsValid(false)}
                  ref={nameRef}
                  inputOtherProps={{
                    placeholder: "Name...",
                  }}
                  labelText={"Name: "}
                />
              </div>
              {isForCustomer ? (
                <></>
              ) : (
                <div className="mb-8">
                  <TextAreaComponent
                    id="descriptionInput"
                    invalidText={descriptionInvalidText}
                    validatorFun={descriptionValidator}
                    afterValidationFun={(e) => {
                      setDescriptionNameIsValid(true);
                    }}
                    invalidatedFun={() => {
                      setDescriptionNameIsValid(false);
                    }}
                    labelText={"Description: "}
                    ref={desRef}
                    inputOtherProps={{
                      placeholder: "Description...",
                    }}
                  />
                </div>
              )}
              <div className="mb-8">
                <InputComponent
                  id="emailInput"
                  type="email"
                  invalidText={emailInvalidText}
                  validatorFun={emailValidator}
                  afterValidationFun={(e) => setEmailIsValid(true)}
                  invalidatedFun={() => setEmailIsValid(false)}
                  ref={emailRef}
                  inputOtherProps={{
                    placeholder: "example@mail.abc...",
                  }}
                  labelText={"Email: "}
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
                  inputOtherProps={{
                    placeholder: "megaPasw03r+dD...",
                  }}
                  labelText={"Password: "}
                />
              </div>
            </div>

            <div>
              <input
                type="submit"
                onClick={async () => {
                  if (
                    nameIsValid === true &&
                    (descriptionIsValid === true || isForCustomer === true) &&
                    emailIsValid === true &&
                    pasIsValid === true &&
                    sent === false
                  )
                    await onSubmit();
                }}
              />
              {codeAndText.code == 409 && (
                <div>
                  <h2 className="error-text">{codeAndText.text}</h2>
                </div>
              )}
              <div>
                <p>
                  If you have an account, <Link to={"/login"}>log in</Link> to
                  it
                </p>
              </div>
              <div className="mt-16">
                {isForCustomer ? (
                  <button
                    className="registration-as-input"
                    onClick={() => setIsForCustomer(false)}
                  >
                    Registration as a seller
                  </button>
                ) : (
                  <button
                    className="registration-as-input"
                    onClick={() => setIsForCustomer(true)}
                  >
                    Registration as a customer
                  </button>
                )}
              </div>
            </div>
          </div>
        );
    }
  };
  return <>{ren()}</>;
}
