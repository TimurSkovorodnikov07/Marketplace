import { useRef } from "react";

export function NumberInputComponent({
  id,
  value,
  setValue,
  min = 0,
  max = 1000000,
  inputOtherProps,
}) {
  const ref = useRef();

  function setValueEverywhere(v) {
    ref.current.value = v;
    setValue(v);
  }

  return (
    <div className="number-input-wrapper">
      <button
        onClick={() => setValueEverywhere(value - 1)}
        className={`small-text ${
          min && value <= min
            ? "number-input-left-arrow-innactive"
            : "number-input-left-arrow"
        }`}
      >
        {"-"}
      </button>
      <div className="number-input">
        <input
          id={id}
          type="number"
          onChange={(e) => {
            const newValue = parseFloat(e.target.value);
            if (
              typeof newValue === "number" &&
              newValue >= min &&
              newValue <= max
            ) {
              setValueEverywhere(newValue);
            }
          }}
          value={value}
          ref={ref}
          {...inputOtherProps}
        />
      </div>
      <button
        onClick={() => setValueEverywhere(value + 1)}
        className={`small-text ${
          max && value >= max
            ? "number-input-right-arrow-innactive"
            : "number-input-right-arrow"
        }`}
      >
        {"+"}
      </button>
    </div>
  );
}
