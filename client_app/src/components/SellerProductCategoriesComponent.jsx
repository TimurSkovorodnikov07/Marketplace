import { useEffect, useState } from "react";
import { useResponseCode } from "../hooks/customHooks";
import { paginationInterval } from "../configs/pagination";
import { getCategories } from "../requests/productCategoriesRequests";
import { ProductCategoryPartialInfoComponent } from "../components/ProductCategoryPartialInfoComponent";
import { InputComponent } from "./InputComponent";
import { stringToBool } from "../other/converter";

export function SellerProductCategoriesComponent({
  sellerId,
  search,
  priceNoMoreThenOrEqual,
}) {
  //useEffect dependencies: From, To, Search, PriceNoMoreTheOrEqual
  //Pagination
  //Response codes
  const [codeAndText, setCodeAndText] = useResponseCode();
  const [categories, setCategories] = useState([]);
  const [isForOwner, setIsForOwner] = useState(false);

  const [from, setFrom] = useState(0);
  const [prevValues, setPrevValues] = useState({
    from: 0,
    search: "",
    priceNoMoreThenOrEqual: 0,
  });
  const [maxNumber, setMaxNumber] = useState(paginationInterval);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getCategories(
          from,
          from + paginationInterval,
          search,
          priceNoMoreThenOrEqual,
          sellerId
        );
        setCodeAndText({ code: response.status, text: response.statusText });

        if (response.status === 200) {
          if (
            prevValues.search === search &&
            prevValues.priceNoMoreThenOrEqual === priceNoMoreThenOrEqual
          ) {
            setCategories([...categories, ...response.data]);

            setPrevValues({
              from: from,
              search: search,
              priceNoMoreThenOrEqual: priceNoMoreThenOrEqual,
            });
          } else {
            setCategories([...response.data]);

            setFrom(0);
            setPrevValues({
              from: 0,
              search: search,
              priceNoMoreThenOrEqual: priceNoMoreThenOrEqual,
            });
          }

          setIsForOwner(stringToBool(response.headers["x-is-for-owner"]));
          setMaxNumber(parseInt(response.headers["x-categories-max-number"]));
        }
      } catch (e) {
        console.error(e);
        setCodeAndText({ code: e.status, text: e.message });
      }
    };
    asyncFun();
  }, [from, search, priceNoMoreThenOrEqual]);

  function categoriesRender() {
    switch (codeAndText.code) {
      case 200:
        return (
          <div className="partial-products-container">
            {categories.length > 0 ? (
              <>
                {categories.map((category) => (
                  <div className="partial-product-category-wrapper" key={category.id}>
                    <div className="partial-product-category">
                      <ProductCategoryPartialInfoComponent
                        category={category}
                        isForOwner={isForOwner}
                        onRemove={(removedCategory) => {
                          setCategories(
                            categories.filter(
                              (c) => c.id !== removedCategory.id
                            )
                          );
                        }}
                      />
                    </div>
                  </div>
                ))}
              </>
            ) : (
              <div className="mt-24">No product categories</div>
            )}
          </div>
        );
    }
  }

  return (
    <>
      <div>{categoriesRender()}</div>
      {from + paginationInterval < maxNumber && (
        <button
          onClick={() => {
            setFrom(from + paginationInterval);
          }}
        >
          Load more
        </button>
      )}
    </>
  );
}
