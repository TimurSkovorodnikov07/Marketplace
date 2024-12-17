import { loginType, logoutType } from "../redux/authReducer";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";
import Cookies from "js-cookie";
import { stringToBool } from "../other/converter";

export function authService(dispath) {
  const refreshToken = Cookies.get(refreshTokenInCookies);
  const userId = Cookies.get(userIdInCookies);

  const accessToken = localStorage.getItem(accessTokenInLocalStorage);
  const isCustomer = localStorage.getItem(isCustomerInLocalStorage);
  const isCustomerToBool = stringToBool(isCustomer);

  //https://stackoverflow.com/questions/154059/how-do-i-check-for-an-empty-undefined-null-string-in-javascript
  //АХАХАХАА, Сука, хуею с js
  if (accessToken && refreshToken && userId && isCustomerToBool !== undefined) {
    const payload = {
      userId: userId,
      refreshToken: refreshToken,
      accessToken: accessToken,
      isCustomer: isCustomerToBool,
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
    console.error("Remove all auth data from Authentication Middleware");
  }
}
