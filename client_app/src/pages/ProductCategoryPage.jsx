import { useEffect, useState } from "react";
import { ProductCategoryInfoComponent } from "../components/ProductCategoryInfoComponent";
import { getCategory } from "../requests/productCategoriesRequests";
import { useParams } from "react-router-dom";
import { useResponseCode } from "../hooks/customHooks";
import { stringToBool } from "../other/converter";
import { getShoppingCartProductFromSessionById } from "../services/shopppingCartProductFromSessionService";

export function ProductCategoryPage() {
  const paramsFromRoute = useParams();
  const [codeAndText, setCodeAndText] = useResponseCode();
  const [isForOwner, setIsForOwner] = useState(false);
  const [responseCategory, setResponseCategory] = useState(undefined);
  const [isBought, setIsBought] = useState(false);
  const [categoryInfoFromShoppingCart, setCategoryInfoFromShoppingCart] =
    useState(false);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getCategory(paramsFromRoute.id);
        if (response.status === 200) {
          const isForOwnerFromResponse = stringToBool(
            response.headers["x-is-for-owner"]
          );
          const isBought = stringToBool(response.headers["x-is-bought"]);

          setIsBought(isBought);
          setResponseCategory(response.data);
          setIsForOwner(isForOwnerFromResponse);
          setCategoryInfoFromShoppingCart(
            getShoppingCartProductFromSessionById(paramsFromRoute.id)
          );
        }

        setCodeAndText({ code: response.status, text: response.statusText });
      } catch (error) {
        console.error(error);
        setCodeAndText({
          code: error.response.status,
          text: error.response.data,
        });
      }
    };
    asyncFun();
  }, []);

  function render() {
    switch (codeAndText.code) {
      case 200:
        return (
          <ProductCategoryInfoComponent
            category={responseCategory}
            isForOwner={isForOwner}
            haveInShoppingCart={categoryInfoFromShoppingCart != undefined}
            defaultNumberOfPurchases={
              categoryInfoFromShoppingCart?.NumberOfPurchases ?? 1
            }
            isBought={isBought}
          />
        );
      case 404:
        return <div className="error-text">Product category is not found</div>;
      default:
        return <div>Loading...</div>;
    }
  }
  return render();
}
