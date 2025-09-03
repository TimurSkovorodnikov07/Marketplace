import { useMemo, useState } from "react";
import { productBuy } from "../requests/productCategoriesRequests";
import ModalWindow from "./ModalWindow";
import { useNavigate } from "react-router-dom";
import { useResponseCode } from "../hooks/customHooks";
import { AddCreditCardComponent } from "./AddCreditCardComponent";
import { purchasedProductsInSessionStorage } from "../configs/sessionStorageItemNames";
import { goToLogin } from "../services/navigateService";

export function BuyModalWindow({ list }) {
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);
  const [codeAndText, setCodeAndText] = useResponseCode();
  const listForQuery = useMemo(
    () =>
      list.map((x) => {
        return {
          PurchasedCategoryId: x.id,
          NumberOfPurchases: x.numberOfPurchases,
        };
      }),
    [list]
  );

  console.log(list);
  async function buy() {
    try {
      const response = await productBuy(listForQuery);
      setCodeAndText({
        code: response.status,
        text: response.statusText,
      });

      if (response.status === 200) {
        sessionStorage.removeItem(purchasedProductsInSessionStorage);
        navigate("/account");
      }
    } catch (error) {
      const errorStatus = error.response.status;
      console.error(error.response.data);
      console.error(error);
      setCodeAndText({
        code: errorStatus,
        text: error.response.data,
      });
      if (errorStatus === 401) goToLogin(navigate);
    }
  }

  function switchFun() {
    switch (codeAndText.code) {
      case 400:
        return (
          <AddCreditCardComponent
            OnAdded={() =>
              setCodeAndText({
                code: 0,
                text: "",
              })
            }
          />
        );
      case 402:
        return <div>Do you have enough money for this product</div>;
      default:
        return (
          <>
            <div className="mb-8">
              You sure want to buy:
              <ol>
                {list.map((x) => (
                  <li key={x.id}>{`${x.name}(${x.numberOfPurchases})`}</li>
                ))}
              </ol>
            </div>
            <button
              onClick={async () => {
                await buy();
              }}
            >
              Yes!
            </button>
          </>
        );
    }
  }

  return (
    <>
      <div>
        <button
          onClick={() => {
            const result = true;

            listForQuery.map((x) => {
              if (x.numberOfPurchases <= 0) {
                result = false;
                console.error(x.numberOfPurchases);
              }
            });
            if (result === true) {
              setIsOpen(true);
            }
          }}
        >
          Buy
        </button>
      </div>
      {isOpen === true && (
        <ModalWindow isOpen={isOpen} onClosed={() => setIsOpen(false)}>
          {switchFun()}
        </ModalWindow>
      )}
    </>
  );
}
