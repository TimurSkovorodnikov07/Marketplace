import { useEffect, useState } from "react";
import {
  getRecommendationCategories,
  getRecommendationCategoriesByTag,
} from "../requests/productCategoriesRequests";
import { useResponseCode } from "../hooks/customHooks";
import { ProductCategoryPartialInfoComponent } from "../components/ProductCategoryPartialInfoComponent";
import { useLocation } from "react-router-dom";

export function HomePage() {
  const location = useLocation();
  const query = new URLSearchParams(location.search);

  const [codeAndText, setCodeAndText] = useResponseCode();
  const [categories, setCategories] = useState([]);

  async function get(tag) {
    try {
      const response =
        tag != undefined && tag.length > 0
          ? await getRecommendationCategoriesByTag(tag)
          : await getRecommendationCategories();

      if (response.status === 200) {
        setCategories(response.data);
      }
      setCodeAndText({
        code: response.status,
        text: response.statusText,
      });
    } catch (error) {
      console.error(error);
      setCodeAndText({
        code: error.response.status,
        text: error.response.data,
      });
    }
  }
  useEffect(() => {
    let tag = query.get("search") || "";

    const asyncFun = async () => await get(tag);
    asyncFun();
  }, []);

  function categoriesRender() {
    switch (codeAndText.code) {
      case 200:
        return (
          <div className="partial-products-container">
            {categories.length > 0 ? (
              <>
                {categories.map((x) => (
                  <div key={x.id} className="partial-product-category">
                    <ProductCategoryPartialInfoComponent category={x} />
                  </div>
                ))}
              </>
            ) : (
              <div>No categories</div>
            )}
          </div>
        );
      default:
        return <></>;
    }
  }
  return categoriesRender();
}
