import { useEffect, useState } from "react";
import { userInfo } from "../requests/baseAuthRequests";
import { useResponseCode } from "../hooks/customHooks";
import { LogoutModalWindow } from "./LogoutModalWindow";
import { useNavigate } from "react-router-dom";
import { goToLogin } from "../services/navigateService";
import { PurchasedProductsPage } from "../pages/PurchasedProductsPage";

export function UserInfoForOwnerComponent() {
  const navigate = useNavigate();
  const [codeAndText, setCodeAndText] = useResponseCode();
  const [info, setInfo] = useState(undefined);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await userInfo();

        setCodeAndText({ code: response.status, text: response.statusText });
        if (response.status === 200) setInfo(response.data);
      } catch (error) {
        console.error(error);
        setCodeAndText({
          code: error.response.status,
          text: error.response.data,
        });
        goToLogin(navigate);
      }
    };
    asyncFun();
  }, []);

  function baseInfo() {
    return (
      <div>
        <div className="account-info-element">
          <div className="account-info-label">Name: </div> {info.name}
        </div>
        <div className="account-info-element">
          <div className="account-info-label">Email: </div> {info.email}
        </div>
        {info.isCustomer === false && (
          <div className="account-info-element">
            <div className="account-info-label">Description: </div>{" "}
            {info.description}
          </div>
        )}
      </div>
    );
  }

  if (codeAndText.code === 200) {
    return (
      (info.isCustomer === true || info.isCustomer === false) && (
        <>
          <div className="account-info-wrapper">
            <div className="account-info">
              <div>{baseInfo()}</div>

              <div className="toend">
                <LogoutModalWindow />
              </div>
            </div>
          </div>
          <div className="account-partial-products-wrapper">
            <PurchasedProductsPage />
          </div>
        </>
      )
    );
  } else {
    return <div>Loading...</div>;
  }
}
