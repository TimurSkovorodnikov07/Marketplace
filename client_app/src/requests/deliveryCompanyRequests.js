import { apiUrl } from "../configs/sources";
import { queryWithStringsCreate } from "../other/queryWithStringsCreate";
import { api } from "./api";

const controllerName = `delivery-company`;

export async function getCompanyInfo(id) {
  return api.get(`/${controllerName}/${id}`).then();
}
export async function getCompanies(name) {
  return api
    .get(
      queryWithStringsCreate(`/${controllerName}/companies`, [
        {
          type: "CompanyName",
          value: name,
        },
      ])
    )
    .then();
}
