import objectToFormMapper from "../mappers/objectToFormMapper";
import { api } from "./api";

const controllerName = "customer-controller";

export async function customerAccountCreate(name, email, password) {
  return api
    .post(
      `/${controllerName}/accountcreate`,
      objectToFormMapper({
        Name: name,
        Email: email,
        Password: password,
      })
    )
    .then();
}

export async function addCard(number, type, money) {
  return api
    .patch(`/${controllerName}/addcard`, {
      Number: number,
      Many: money,
      Type: type,
    })
    .then();
}
