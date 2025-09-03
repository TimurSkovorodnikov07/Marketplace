import objectToFormMapper from "../mappers/objectToFormMapper";
import { queryWithStringsCreate } from "../other/queryWithStringsCreate";
import { api } from "./api";

const controllerName = "reviews";

export async function getReviews(categoryId, from, to) {
  return api
    .get(
      queryWithStringsCreate(`/${controllerName}/categories`, [
        {
          type: "From",
          value: from,
        },
        {
          type: "To",
          value: to,
        },
        {
          type: "CategoryId",
          value: categoryId,
        },
      ])
    )
    .then();
}
export async function getMyReview(categoryId) {
  return api.get(`/${controllerName}/${categoryId}`).then();
}

export async function addReview(text, estimation, categoryId) {
  const data = {
    Text: text,
    Estimation: estimation,
    CategoryId: categoryId,
  };
  const formData = objectToFormMapper(data);

  return api
    .post(`/${controllerName}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
    .then();
}

export async function updateReview(newText, newEstimation, updatedId) {
  const data = {
    NewText: newText,
    NewEstimation: newEstimation,
    Id: updatedId,
  };
  const formData = objectToFormMapper(data);

  return api
    .put(`/${controllerName}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
    .then();
}

export async function removeReview(id) {
  return api.delete(`/${controllerName}/${id}`).then();
}
