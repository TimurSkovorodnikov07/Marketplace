import { useEffect } from "react";
import { authService } from "../services/authService";
import { useDispatch, useSelector } from "react-redux";
import { loginType, logoutType } from "../redux/authReducer";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";
import Cookies from "js-cookie";
import { stringToBool } from "../other/converter";
import { useLocation } from "react-router-dom";

export default function MiddlewareForPages({ children }) {
  const dispath = useDispatch();
  const authData = useSelector((x) => x);
  const location = useLocation();

  useEffect(() => {
    console.log(`PathName changed to: ${location.pathname}`);

    const middleware = () => {
      authService(dispath, authData);
    };
    middleware();
  }, [location.pathname]);

  return children;
}
