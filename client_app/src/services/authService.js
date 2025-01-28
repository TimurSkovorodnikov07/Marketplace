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
  //https://stackoverflow.com/questions/154059/how-do-i-check-for-an-empty-undefined-null-string-in-javascript
  //АХАХАХАА, Сука, хуею с js
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
    //Я беру сразу из куки и локал стора тк блять если я сохраню будет какая то залупа, серваку нахуй твой старый токен не сдался к хуям собачим, потому 401 нахуй.
    //Лучше блять не сохранять тут в константе а сразу блять ьрать и мне поебать что это мега тупое решение.
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
