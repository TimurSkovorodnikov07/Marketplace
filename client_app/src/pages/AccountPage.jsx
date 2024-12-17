import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { UserInfoForOwnerComponent } from "../components/UserInfoForOwnerComponent";
import Cookies from "js-cookie";
import { accessTokenInLocalStorage } from "../configs/localStorageItemName";
import { goToLogin } from "../services/navigateService";

export function AccountPage() {
  const auth = useSelector((state) => state.auth);
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.isCustomer === false) {
      navigate(`/sellers/${auth.userId}`);
    }

    //AuthenticationMiddleware может не сразу закинуть в стейт контейнер, потому так лучше:
    if (!localStorage.getItem(accessTokenInLocalStorage)) {
      goToLogin(navigate);
    }
  }, [auth.isCustomer, auth.userId]);

  console.log(`Is customer: ${auth.isCustomer}`);

  return auth.isCustomer === true ? (
    <div className="account-info-container">
      <UserInfoForOwnerComponent />
    </div>
  ) : auth.isCustomer === false ? (
    <div>Loading...</div>
  ) : (
    <></>
  );

  // return auth.isCustomer === true ? (
  //   <div>Customer info</div>
  // ) : (
  //   <Navigate to={`/sellers/${auth.userId}`} />
  // ); Раньше был такой, useSelector auth.isCustomer снчало был false а после сатновился true
}
