import { api } from "./api";

const controllerName = "images";

export async function getImagesByCategoryId(categoryId) {
  return api.get(`/${controllerName}/by-category-id/${categoryId}`).then();
}
