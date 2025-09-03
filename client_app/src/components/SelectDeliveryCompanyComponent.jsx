import { useEffect, useState } from "react";
import { InputComponent } from "./InputComponent";
import { getCompanies } from "../requests/deliveryCompanyRequests";

export function SelectDeliveryCompanyComponent({
  selectedCompany,
  selectFun,
  labelText,
  showInvalidText,
  defaultCompanyId,
  onChanged,
  showCompaniesAtTheBegin = false,
}) {
  const [name, setName] = useState("");
  const [companies, setCompanies] = useState([]);
  const [showCompanies, setShowCompanies] = useState(showCompaniesAtTheBegin);

  useEffect(() => {
    if (showCompanies == false) return;
    
    const asyncFun = async () => {
      try {
        const response = await getCompanies(name);
        const companiesFromResponse = [...response.data];
        if (response.status === 200) setCompanies(companiesFromResponse);
        if (defaultCompanyId != undefined) {
          const defCompany = companiesFromResponse.find(
            (x) => x.id == defaultCompanyId
          );
          selectFun(defCompany);
        }
      } catch (error) {
        console.error(error);
      }
    };
    asyncFun();
  }, [name, showCompanies]);

  return (
    <>
      <p className="give-attention-text">{labelText}</p>
      {selectedCompany ? (
        <div className="selected-delivery-company-wrapper">
          <button
            className="selected-delivery-company-button"
            onClick={(e) => {
              selectFun(undefined);
              onChanged?.(e);
            }}
          >
            <div className="selected-delivery-company-name">
              {selectedCompany.name}
            </div>
          </button>
        </div>
      ) : (
        <>
          <div className="delivery-company-select-wrapper">
            <InputComponent
              id="deliveryCompanySearch"
              type="text"
              beforeValidationFun={(e) => {
                setName(e.target.value);
              }}
              onFocus={() => {
                setShowCompanies(true);
              }}
              inputOtherProps={{
                placeholder: "search...",
                className: `delivery-company-select-search ${
                  companies.length > 0
                    ? " delivery-company-select-search-show-companies"
                    : ""
                }`,
              }}
            />
            <ul className="found-delivery-companies">
              {companies.map((x) => (
                <li key={x.id}>
                  <button
                    className="found-delivery-company-button"
                    onClick={(e) => {
                      selectFun(x);
                      onChanged?.(e);
                    }}
                  >
                    {x.name}
                  </button>
                </li>
              ))}
            </ul>
          </div>
          {showInvalidText === true && !selectedCompany ? (
            <p className="error-text">{"Choose a delivery company"}</p>
          ) : (
            <></>
          )}
        </>
      )}
    </>
  );
}
