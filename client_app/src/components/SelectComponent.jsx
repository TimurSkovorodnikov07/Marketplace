import { useState } from "react";

export function SelectComponent({
  id,
  className,
  onSelect,
  textAndValue,
  showInvalidText,
}) {
  const [isSelected, setIsSelected] = useState(false);

  return (
    <>
      <div>
        <label className="give-attention-text" htmlFor={id}>
          Your card type:{" "}
        </label>
      </div>
      <select
        id={id}
        onChange={(e) => {
          setIsSelected(true);
          onSelect(e.target.value);
        }}
        className={className == undefined ? `select` : `select ${className}`}
        defaultValue={"DEFAULT_VALUE"}
      >
        <option value={"DEFAULT_VALUE"} disabled>
          Null
        </option>
        {textAndValue.map((x) => (
          <option key={x.value} value={x.value}>
            {x.text}
          </option>
        ))}
      </select>
      {isSelected === false &&
      (showInvalidText == undefined || showInvalidText === true) ? (
        <p className="error-text">Not selected</p>
      ) : (
        <></>
      )}
    </>
  );
}
