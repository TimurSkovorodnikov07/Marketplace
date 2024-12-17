import Cookies from "js-cookie";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";

export const loginType = "LOGIN_TYPE_WITH_SAVE";
export const logoutType = "LOGOUT";

function login(payload) {
  console.log("AUTH DATA SAVE!");

  localStorage.setItem(accessTokenInLocalStorage, payload.accessToken);
  localStorage.setItem(isCustomerInLocalStorage, payload.isCustomer);
  Cookies.set(refreshTokenInCookies, payload.refreshToken);
  Cookies.set(userIdInCookies, payload.userId);

  return {
    userId: payload.userId,
    accessToken: payload.accessToken,
    refreshToken: payload.refreshToken,
    isCustomer: payload.isCustomer,
    isAuth: true,
  };
}
const defaultValue = {
  userId: undefined,
  accessToken: undefined,
  refreshToken: undefined,
  isCustomer: undefined,
  isAuth: false,
};

export function logout() {
  localStorage.removeItem(accessTokenInLocalStorage);
  localStorage.removeItem(isCustomerInLocalStorage);
  Cookies.remove(refreshTokenInCookies);
  Cookies.remove(userIdInCookies);
}

export function authReducer(state = { ...defaultValue }, action) {
  switch (action.type) {
    case loginType:
      return login(action.payload);
    case logoutType:
      logout();
      return { ...defaultValue };
    default:
      return state;
  }
}
