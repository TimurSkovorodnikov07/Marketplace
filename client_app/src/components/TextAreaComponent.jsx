import { useEffect, useState } from "react";

export function TextAreaComponent(props) {
  const [isValid, setIsValid] = useState(false);
  const [value, setValue] = useState(undefined);

  function validCheck(value) {
    return !props?.validatorFun || props.validatorFun(value) === true;
  }
  useEffect(() => {
    if (props.defaultValue != undefined) {
      if (validCheck(props.defaultValue)) {
        setIsValid(true);
        setValue(props.defaultValue);
      } else {
        console.error(
          "Разраб долбоеб, закинул дефолтное не валидное значение "
        );
      }
    }
  }, [props?.defaultValue]);

  useEffect(() => {
    if (props?.needClear != undefined && props.needClear === true) {
      const valueAfterClear = props?.valueAfterClear;
      setValue(valueAfterClear != undefined ? valueAfterClear : "");
      setIsValid(false);
    }
  }, [props?.needClear]);

  function onChangeFun(e) {
    setValue(e.target.value);
    props.beforeValidationFun?.(e);

    if (validCheck(e.target.value)) {
      props.afterValidationFun?.(e);
      setIsValid(true);
    } else {
      setIsValid(false);
      props.invalidatedFun?.();
    }
  }

  return (
    <>
      <div>
        <label
          htmlFor={props.id}
          className="give-attention-text"
          {...props.labelOtherProps}
        >
          {props.labelText}
        </label>
      </div>
      <textarea
        id={props.id}
        onChange={(e) => {
          console.log(validCheck(e.target.value));
          onChangeFun(e);

          e.target.style.height = "auto";
          let scrollHeight = e.target.scrollHeight;
          e.target.style.height = `${scrollHeight}px`;
        }}
        value={value}
        {...props.textareaOtherProps}
      ></textarea>
      {isValid === false &&
      (props.showInvalidText == undefined || props.showInvalidText === true) ? (
        <p className="error-text">{props.invalidText}</p>
      ) : (
        <p className="error-text"></p>
      )}
    </>
  );
}
