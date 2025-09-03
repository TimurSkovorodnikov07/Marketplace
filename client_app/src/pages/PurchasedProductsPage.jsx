import { useEffect, useState } from "react";
import { getPurchasedProducts } from "../requests/productCategoriesRequests";
import { paginationInterval } from "../configs/pagination";
import { useResponseCode } from "../hooks/customHooks";
import { PurchasedProductInfoComponent } from "../components/PurchasedProductInfoComponent";

export function PurchasedProductsPage() {
  const [from, setFrom] = useState(0);
  const [maxCount, setMaxCount] = useState(0);
  const [codeAndText, setCodeAndText] = useResponseCode();
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getPurchasedProducts(
          from,
          from + paginationInterval
        );
        if (response.status === 200) {
          console.log(response.data);
          setProducts(
            response.data.sort((a, b) => {
              return new Date(b.purchasedDate) - new Date(a.purchasedDate);
            })
          );
          setMaxCount(
            parseInt(response.headers["x-purchased-products-max-number"])
          );
        }
        setCodeAndText({ code: response.status, text: response.statusText });
      } catch (error) {
        setCodeAndText({
          code: error.response.status,
          text: error.response.data,
        });
      }
    };
    asyncFun();
  }, [from]);

  function purchasedProductsRender() {
    switch (codeAndText.code) {
      case 200:
        return (
          <>
            {products.length > 0 ? (
              <div className="purchased-product-images">
                {products.map((x) => (
                  <div className="category purchased-product" key={x.purchasedDate}>
                    <PurchasedProductInfoComponent product={x} />
                  </div>
                ))}
              </div>
            ) : (
              <div className="mt-24">No purchased product</div>
            )}
          </>
        );
    }
  }

  return (
    <>
      <div>{purchasedProductsRender()}</div>
      {from + paginationInterval < maxCount ? (
        <button
          onClick={() => {
            setFrom(from + paginationInterval);
          }}
        >
          Load more
        </button>
      ) : (
        <></>
      )}
    </>
  );
}
