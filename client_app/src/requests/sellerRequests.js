import objectToFormMapper from "../mappers/objectToFormMapper";
import { api } from "./api";

const controllerName = "seller-controller";
export async function sellerAccountCreate(name, email, password, description) {
  return api
    .post(
      `/${controllerName}/accountcreate`,
      objectToFormMapper({
        Name: name,
        Email: email,
        Password: password,
        Description: description,
      })
    )
    .then();
}

export async function getSellerInfo(id) {
  return api.get(`${controllerName}/${id || undefined}`).then();
}
