import objectToFormMapper from "../mappers/objectToFormMapper";
import { api } from "./api";

const loginControllerName = "baselogincontroller";

export async function login(email, password) {
  return api
    .post(
      `/${loginControllerName}/login`,
      objectToFormMapper({ Email: email, Password: password })
    )
    .then();
}
export async function tokensUpdate(oldRefreshToken, userId) {
  return api
    .put(`/${loginControllerName}/tokensupdate`, {
      OldRefreshToken: oldRefreshToken,
      UserId: userId,
    })
    .then();
}

export async function codeResend(userId) {
  return api.put(`/${loginControllerName}/coderesend/${userId}`).then();
}

export async function userInfo() {
  return api.get(`/${loginControllerName}/userinfo`).then();
}

export async function emailVerify(userId, code) {
  return api
    .post(`/${loginControllerName}/emailverify`, {
      UserId: userId,
      Code: code,
    })
    .then();
}
