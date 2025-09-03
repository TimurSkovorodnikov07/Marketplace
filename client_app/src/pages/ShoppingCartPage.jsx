import { useEffect, useState } from "react";
import { getCategory } from "../requests/productCategoriesRequests";
import { getShoppingCartProductsFromSession } from "../services/shopppingCartProductFromSessionService";
import { ProductCategoryFromShoppingCart } from "../components/ProductCategoryFromShoppingCart";
import { BuyModalWindow } from "../components/BuyModalWindow";
import { useSelector } from "react-redux";
import { goToLogin } from "../services/navigateService";
import { useNavigate } from "react-router-dom";

export function ShopppingCartPage() {
  const [categories, setCategories] = useState([]);
  const auth = useSelector((state) => state.auth);
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.isCustomer !== true || auth.isAuth !== true) {
      goToLogin(navigate);
    }

    const productIdentifiersFromStorage = getShoppingCartProductsFromSession();

    if (Array.isArray(productIdentifiersFromStorage)) {
      const asyncFun = async () => {
        const resultArray = [];

        for (let p of productIdentifiersFromStorage) {
          try {
            const response = await getCategory(p.PurchasedCategoryId);

            if (response.status === 200) {
              const category = {
                ...response.data,
                numberOfPurchases: p.NumberOfPurchases,
              };
              resultArray.push(category);
            }
          } catch (error) {
            console.error(error);
          }
        }
        setCategories([...resultArray]);
      };
      asyncFun();
    } else {
      console.error("No products");
    }
  }, []);

  return Array.isArray(categories) && categories.length > 0 ? (
    <>
      {categories.map((x) => (
        <div key={x.id}>
          <ProductCategoryFromShoppingCart
            category={x}
            numberOfPurchases={x.numberOfPurchases}
            haveInShoppingCart={true}
          />
        </div>
      ))}

      <BuyModalWindow list={categories} />
    </>
  ) : (
    <>
      <div>you shopping cart is empty</div>
    </>
  );
}
