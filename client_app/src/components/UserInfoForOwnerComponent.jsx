import { useEffect, useState } from "react";
import { userInfo } from "../requests/baseAuthRequests";
import { useResponseCode } from "../hooks/customHooks";
import { LogoutModalWindow } from "./LogoutModalWindow";
import { useNavigate } from "react-router-dom";
import { goToLogin } from "../services/navigateService";
import { PurchasedProductsPage } from "../pages/PurchasedProductsPage";
import { maxWidthForSmartphones } from "../configs/sizeNumbers";

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
          <div className="account-info-label">Name: </div>
          <h2>{info.name}</h2>
        </div>
        <div className="account-info-element">
          <div className="account-info-label">Email: </div>{" "}
          <div className="small-text">{info.email}</div>
        </div>
        {info.isCustomer === false && (
          <div className="account-info-element">
            <div className="account-info-label">Description: </div>
            <div className="small-text">{info.description}</div>
          </div>
        )}
        <div className="line-on-bottom"></div>
      </div>
    );
  }

  if (codeAndText.code === 200) {
    return (
      (info.isCustomer === true || info.isCustomer === false) && (
        <>
          <div className="account-info-wrapper">
            <div className="account-info mb-16">
              <div>{baseInfo()}</div>

              {window.innerWidth >= maxWidthForSmartphones && (
                <LogoutModalWindow />
              )}
            </div>
          </div>
          <div className="account-partial-products-wrapper">
            <PurchasedProductsPage />
          </div>

          {window.innerWidth < maxWidthForSmartphones && (
            <div className="mt-16">
              <LogoutModalWindow />
            </div>
          )}
        </>
      )
    );
  } else {
    return <div>Loading...</div>;
  }
}
