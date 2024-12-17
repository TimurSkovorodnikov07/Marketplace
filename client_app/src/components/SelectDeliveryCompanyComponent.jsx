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
}) {
  const [name, setName] = useState("");
  const [companies, setCompanies] = useState([]);

  useEffect(() => {
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
  }, [name]);

  return (
    <>
      <p>{labelText}</p>
      {selectedCompany ? (
        <div>
          <button
            onClick={(e) => {
              selectFun(undefined);
              onChanged?.(e);
            }}
          >
            <div className="">
              <div className="">{selectedCompany.name}</div>
              <div className="">{selectedCompany.description}</div>
            </div>
          </button>
        </div>
      ) : (
        <div>
          <InputComponent
            id="deliveryCompanySearch"
            type="text"
            beforeValidationFun={(e) => {
              setName(e.target.value);
            }}
          />
          <ul>
            {companies.map((x) => (
              <li key={x.id}>
                <button
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
          {showInvalidText === true || !selectedCompany ? (
            <p className="error-text">{"Choose a delivery company"}</p>
          ) : (
            <></>
          )}
        </div>
      )}
    </>
  );
}
