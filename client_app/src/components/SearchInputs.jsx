import { InputComponent } from "./InputComponent";

export function SearchInputs({
  setSearch,
  setPriceNoMoreThenOrEqual,
  priceNoMoreThenOrEqual,
}) {
  return (
    <div>
      <div>
        <InputComponent
          id="searchInput"
          type="text"
          afterValidationFun={(e) => {
            setSearch(e.target.value);
          }}
          labelText="Search: "
          inputOtherProps={{ placeholder: "search..." }}
        />
      </div>
      <div>
        <InputComponent
          id="priceNoMoreThenOrEqualInput"
          validatorFun={(value) => value > 0}
          afterValidationFun={(e) => {
            setPriceNoMoreThenOrEqual(e.target.value);
          }}
          type="number"
          labelText="Max Price: "
          inputOtherProps={{ placeholder: "1000", min: 1 }}
          setValue={setPriceNoMoreThenOrEqual}
          value={priceNoMoreThenOrEqual}
        />
      </div>
    </div>
  );
}
