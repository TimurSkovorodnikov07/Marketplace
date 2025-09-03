import { loginType, logoutType } from "../redux/authReducer";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";
import Cookies from "js-cookie";
import { stringToBool } from "../other/converter";

export function authService(dispath, authData) {
  function IsCustomerToBool() {
    return stringToBool(localStorage.getItem(isCustomerInLocalStorage));
  }
  console.log(authData);
  console.log({
    accessToken: localStorage.getItem(accessTokenInLocalStorage),
    refreshToken: Cookies.get(refreshTokenInCookies),
    userId: Cookies.get(userIdInCookies),
    isCustomer: localStorage.getItem(isCustomerInLocalStorage),
    isCustomerToBool: IsCustomerToBool(),
  });
  if (
    localStorage.getItem(accessTokenInLocalStorage) &&
    Cookies.get(refreshTokenInCookies) &&
    Cookies.get(userIdInCookies) &&
    IsCustomerToBool() !== undefined
  ) {
    const payload = {
      userId: Cookies.get(userIdInCookies),
      refreshToken: Cookies.get(refreshTokenInCookies),
      accessToken: localStorage.getItem(accessTokenInLocalStorage),
      isCustomer: IsCustomerToBool(),
    };

    dispath({
      type: loginType,
      payload: payload,
    });
  } else {
    dispath?.({
      type: logoutType,
      payload: {},
    });
    console.error("Remove all auth data from Authentication service");
  }
}
