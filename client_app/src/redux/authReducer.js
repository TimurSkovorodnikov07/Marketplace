import Cookies from "js-cookie";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";

export const loginType = "LOGIN_TYPE_WITH_SAVE";
export const logoutType = "LOGOUT";

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
      console.log("AUTH DATA SAVE TO REDUX STORE!");
      localStorage.setItem(
        accessTokenInLocalStorage,
        action.payload.accessToken
      );
      localStorage.setItem(isCustomerInLocalStorage, action.payload.isCustomer);
      Cookies.set(refreshTokenInCookies, action.payload.refreshToken);
      Cookies.set(userIdInCookies, action.payload.userId);

      const newState = {
        userId: action.payload.userId,
        accessToken: action.payload.accessToken,
        refreshToken: action.payload.refreshToken,
        isCustomer: action.payload.isCustomer,
        isAuth: true,
      };

      console.log(newState);
      return newState;
    case logoutType:
      logout();
      return { ...defaultValue };
    default:
      return state;
  }
}
