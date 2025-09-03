import { ProductCategoryPartialInfoComponent } from "./ProductCategoryPartialInfoComponent";

export function ProductCategoryFromShoppingCart({
  category,
  numberOfPurchases,
}) {
  return (
    <>
      <ProductCategoryPartialInfoComponent category={category} />
      <div>
        There are already {numberOfPurchases} such items in the shopping cart
      </div>
    </>
  );
}
