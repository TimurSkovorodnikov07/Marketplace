import { api } from "./api";
import { queryWithStringsCreate } from "../other/queryWithStringsCreate";
import objectToFormMapper from "../mappers/objectToFormMapper";

const controllerName = "products";

export async function getRecommendationCategories() {
  return api.get(`/${controllerName}/recommendation`).then();
}

export async function getRecommendationCategoriesByTag(tag) {
  return api.get(`/${controllerName}/recommendation-by-tag/${tag}`).then();
}

export async function getCategory(guid) {
  return api
    .get(`/${controllerName}/${guid}`, {
      headers: {
        "X-Get-Is-Bought": "true",
      },
    })
    .then();
}

export async function getCategories(
  from,
  to,
  search,
  priceNoMoreThenOrEqual,
  selelrId
) {
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
          type: "Search",
          value: search,
        },
        {
          type: "PriceNoMoreThenOrEqual",
          value: priceNoMoreThenOrEqual,
        },
        {
          type: "SellerId",
          value: selelrId,
        },
      ])
    )
    .then();
}

export async function getPurchasedProducts(from, to) {
  return api
    .get(
      queryWithStringsCreate(`/${controllerName}/purchased-products`, [
        {
          type: "From",
          value: from,
        },
        {
          type: "To",
          value: to,
        },
      ])
    )
    .then();
}
export async function productCategoryCreate(
  name,
  description,
  tags,
  price,
  quantity,
  deliveryCompanyId,
  images
) {
  const data = {
    Name: name,
    Description: description,
    Tags: tags,
    Price: price,
    Quantity: quantity,
    DeliveryCompanyId: deliveryCompanyId,
  };

  const formData = objectToFormMapper(data);

  if (Array.isArray(images)) {
    images.forEach((file) => {
      if (file && file.name) {
        console.log("Adding image:", file);
        formData.append("Images", file, file.name);
      } else {
        console.warn("Invalid file object:", file);
      }
    });
  }
  else
  {
    console.error("Images array are empty")
  }

  return api
    .post(`/${controllerName}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
    .then();
}

export async function productCategoryUpdate(
  id,
  newName,
  newDescription,
  newTags,
  newPrice,
  newQuantity,
  newDeliveryCompanyId
) {
  const data = {
    Id: id,
    NewName: newName,
    NewDescription: newDescription,
    NewTags: newTags,
    NewPrice: newPrice,
    NewQuantity: newQuantity,
    NewDeliveryCompanyId: newDeliveryCompanyId,
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
export async function categoryRemove(guid) {
  return api.delete(`/${controllerName}/${guid}`).then();
}

export async function productBuy(buyedProducts) {
  return api.patch(`/${controllerName}/buy`, [...buyedProducts]).then();
}

export async function categoryNameIsFree(name) {
  return api.get(`/${controllerName}/category-name-is-free/${name}`).then();
}
