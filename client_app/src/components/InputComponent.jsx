import { useState, forwardRef, useEffect } from "react";
import "../styles/index.css";

//forwardRef позволяет сзодать ссылку в первом комп и юзать ее в втором комп.
//Стоит заметить, ебанный typescript выебываеться, потому при создании ссылки у нее в джинериках должен быьть тип HTMLElement-а, в какой тип элемента ссылаться
//Вобще это можно было сделать и с помощью useState, но мне просто стало интересно хули реакт не может работать с ref за пределами компонента.
export const InputComponent = forwardRef((params, ref) => {
  const [isValid, setIsValid] = useState(false);
  const [inputValue, setInputValue] = useState(undefined);

  function validCheck(value) {
    return !params?.validatorFun || params.validatorFun(value) === true;
  }
  useEffect(() => {
    if (params.defaultValue != undefined) {
      if (validCheck(params.defaultValue)) {
        setIsValid(true);
        setInputValue(params.defaultValue);
      } else {
        console.error(
          "Разраб долбоеб, закинул дефолтное не валидное значение "
        );
      }
    }
  }, [params.defaultValue]);

  useEffect(() => {
    if (params?.needClear != undefined && params.needClear === true) {
      const valueAfterClear = params?.valueAfterClear;
      setInputValue(valueAfterClear != undefined ? valueAfterClear : "");
      setIsValid(false);
    }
  }, [params?.needClear]);

  function onChangeFun(e) {
    const value = e.target.value;

    setInputValue(value);
    params.beforeValidationFun?.(e);

    if (validCheck(value)) {
      params.afterValidationFun?.(e);
      setIsValid(true);
    } else {
      setIsValid(false);
      params.invalidatedFun?.();
    }
  }
  return (
    <>
      <div>
        <label
          htmlFor={params.id}
          className="give-attention-text"
          {...params.labelOtherProps}
        >
          {params.labelText}
        </label>
      </div>
      <input
        id={params.id}
        type={params.type}
        onChange={(e) => onChangeFun(e)}
        ref={ref}
        value={inputValue}
        {...params.inputOtherProps}
      />
      {isValid === false &&
      (params.showInvalidText == undefined ||
        params.showInvalidText === true) ? (
        <p className="error-text">{params.invalidText}</p>
      ) : (
        <p className="error-text"></p>
      )}
    </>
  );
});
