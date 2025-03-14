import {
  accessTokenInLocalStorage,
  isCustomerInLocalStorage,
} from "../configs/localStorageItemName";
import { refreshTokenInCookies, userIdInCookies } from "../configs/cookiesName";
import axios from "axios";
import Cookies from "js-cookie";
import { apiUrl } from "../configs/sources";
import { logout } from "../redux/authReducer";
import { tokensUpdate } from "../requests/baseAuthRequests";

export const api = axios.create({
  withCredentials: false,
  baseURL: apiUrl,
});

//Request
api.interceptors.request.use((conf) => {
  conf.headers.Authorization = `Bearer ${localStorage.getItem(
    accessTokenInLocalStorage
  )}`;
  return conf;
});

//Response
api.interceptors.response.use(
  (conf) => {
    return conf;
  },
  async (error) => {
    console.error("Error message: ", error);
    console.log(
      "Refresh token from Cookies: ",
      Cookies.get(refreshTokenInCookies)
    );
    console.log("User Id from Cookies", Cookies.get(userIdInCookies));

    if (
      error.response.status == 401 &&
      Cookies.get(refreshTokenInCookies) != undefined &&
      Cookies.get(userIdInCookies) != undefined
    ) {
      try {
        error.config._isRetry = true;

        let response = await tokensUpdate(
          Cookies.get(refreshTokenInCookies),
          Cookies.get(userIdInCookies)
        );
        console.log(response);

        if (response.status == 200) {
          localStorage.setItem(
            accessTokenInLocalStorage,
            response.data.accessToken
          );
          localStorage.setItem(
            isCustomerInLocalStorage,
            response.data.isCustomer
          );
          Cookies.set(refreshTokenInCookies, response.data.refreshToken);
          Cookies.set(userIdInCookies, response.data.userId);

          return api.request(error.config);
        }
        throw new Error("The Tokens didn't update");
      } catch (er) {
        console.error(er);
        console.error("REMOVE ALL AUTH DATA FROM api.js");
        logout();
      }
    }
    throw error;
  }
);
