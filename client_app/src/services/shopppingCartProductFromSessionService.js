import { purchasedProductsInSessionStorage } from "../configs/sessionStorageItemNames";

export function getShoppingCartProductsFromSession() {
  const productsJson = sessionStorage.getItem(
    purchasedProductsInSessionStorage
  );
  if (!productsJson) {
    return undefined;
  }

  const result = JSON.parse(productsJson);
  return Array.isArray(result) ? result : undefined;
}

export function addProductFromSession(newProduct) {
  const products = getShoppingCartProductsFromSession();

  if (Array.isArray(products)) {
    sessionStorage.setItem(
      purchasedProductsInSessionStorage,
      JSON.stringify([...products, newProduct])
    );
  } else {
    sessionStorage.removeItem(purchasedProductsInSessionStorage);
    sessionStorage.setItem(
      purchasedProductsInSessionStorage,
      JSON.stringify([newProduct])
    );
  }
}

export function removeFromShoppingCartFromSession(id) {
  const products = getShoppingCartProductsFromSession();

  if (Array.isArray(products)) {
    const otherProducts = products.filter((x) => x.PurchasedCategoryId !== id);
    console.log(otherProducts);
    Array.isArray(otherProducts) === true && otherProducts.length > 0
      ? sessionStorage.setItem(
          purchasedProductsInSessionStorage,
          JSON.stringify(otherProducts)
        )
      : sessionStorage.removeItem(purchasedProductsInSessionStorage);
  }
}

export function getShoppingCartProductFromSessionById(id) {
  const products = getShoppingCartProductsFromSession();

  if (Array.isArray(products) && products.length > 0) {
    return products.find((x) => x.PurchasedCategoryId === id);
  }

  return undefined;
}
