import { useState } from "react";
import { addCard } from "../requests/customerRequests";
import {
  creditCardMoneyValidator,
  creditCardNumberValidator,
  creditCardValidator,
  maxMoneyNumber,
} from "../validators/creditCardValidator";
import { InputComponent } from "./InputComponent";
import { SelectComponent } from "./SelectComponent";
import { NumberInputComponent } from "./NumberInputComponent";

export function AddCreditCardComponent({ onAdded }) {
  const [number, setNumber] = useState("");
  const [type, setType] = useState("");
  const [money, setMoney] = useState(0);
  const [isAllValid, setIsAllValid] = useState(true);

  async function add() {
    try {
      const response = addCard(number, type, money);

      if (response.status === 200) {
        onAdded();
      }
    } catch (error) {
      console.error(error);
    }
  }
  function line() {
    return <div className="line-on-bottom"></div>;
  }

  return (
    <div className="add-credit-card-wrapper">
      <div className="add-credit-card-element">
        <InputComponent
          id="cardNumberInput"
          type="tel"
          invalidText={"Invalid number"}
          validatorFun={creditCardNumberValidator}
          invalidatedFun={() => setNumber(undefined)}
          afterValidationFun={(e) => setNumber(e.target.value)}
          inputOtherProps={{
            placeholder: "xxxx xxxx xxxx xxxx",
            required: true,
          }}
          labelText={"Card number: "}
          showInvalidText={isAllValid === false}
        />
      </div>
      <div className="add-credit-card-element">
        <SelectComponent
          id="typeSelect"
          onSelect={(value) => setType(value)}
          textAndValue={[
            { text: "Master Card", value: "MasterCard" },
            { text: "Visa", value: "VisaCard" },
          ]}
          showInvalidText={isAllValid === false}
        />
      </div>
      <div className="add-credit-card-element">
        <InputComponent
          id="moneyInput"
          type="number"
          invalidText={
            "The amount of money must be greater than 0 and must not exceed 100,000,000"
          }
          validatorFun={(value) => {
            const floatValue = parseFloat(value);
            console.log(floatValue);
            if (floatValue != isNaN)
              return creditCardMoneyValidator(floatValue);

            return false;
          }}
          invalidatedFun={() => setMoney(undefined)}
          afterValidationFun={(e) => {
            setMoney(parseFloat(e.target.value));
          }}
          inputOtherProps={{
            required: true,
            min: 0,
            max: maxMoneyNumber,
          }}
          showInvalidText={isAllValid === false}
          value={money}
          labelText={"Money in card: "}
        />
      </div>
      {line()}
      <button
        onClick={async () => {
          const isValid = creditCardValidator(number, type, money);
          setIsAllValid(isValid);
          if (isValid === true) await add();
        }}
      >
        Add card
      </button>
    </div>
  );
}
