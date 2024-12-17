import { accessTokenInLocalStorage } from "../configs/localStorageItemName";
import { refreshTokenInCookies } from "../configs/cookiesName";
import axios from "axios";
import Cookies from "js-cookie";
import { apiUrl } from "../configs/sources";
import { logout } from "../redux/authReducer";
import { tokensUpdate } from "../requests/baseAuthRequests";

export const api = axios.create({
  withCredentials: false,
  baseURL: apiUrl,
});

//ТУТ БЛЯТЬ КОНСТАТНЫ НЕ ПИСАТЬ!!!!!!!!!!!!!!

//Request
api.interceptors.request.use((conf) => {
  //Беру сразу тут тк блять, ранее кидал в константу и получал отказ когда хотел взять и залогиниться, почему?
  //Потому что блять он тут лежал, адже когда я logout делал, все ровно он в конст лежал БЛЯТЬ!!!
  conf.headers.Authorization = `Bearer ${localStorage.getItem(
    accessTokenInLocalStorage
  )}`;
  return conf;
});

//Response
//Первый парраметр действия если запрос успешный, второй если нет:
api.interceptors.response.use(
  (conf) => {
    return conf;
  },
  async (error) => {
    const originalReq = error.config;
    if (
      error.response.status === 401 &&
      Cookies(refreshTokenInCookies) !== undefined &&
      error.config._isRetry === false
    ) {
      try {
        //Нужно isRetry проверка чтобы не сделать бесконечный цикл где хочешь избавиться от 401 но в итоге опять его получаешь(если сервак писал даун)
        originalReq._isRetry = true;

        const response = await tokensUpdate();

        if (response.status == 200) {
          localStorage.setItem(
            accessTokenInLocalStorage,
            response.data.accessToken
          );
          Cookies.set(refreshTokenInCookies, response.data.refreshToken);
          return api.request(originalReq);
        }
        throw new Error("");
      } catch (er) {
        console.error(er);
        console.error("REMOVE ALL AUTH DATA FROM api.js");
        logout();
      }
    }
    throw error;
  }
);
