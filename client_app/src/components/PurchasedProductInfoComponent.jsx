import moment from "moment";
import { ImageComponent } from "./ImageComponent";

export function PurchasedProductInfoComponent({ product }) {
  const format = "DD/MM/YYYY";
  console.log("Purchased product: ", product);

  return (
    <>
      <h1>{product.name}</h1>
      <div>Sum: {product.totalSum}$</div>
      <div>Quantity: {product.purchasedQuantity}</div>
      <div>Purchased Date: {moment(product.purchasedDate).format(format)}</div>
      {product.deliveredDate != undefined ? (
        <div>
          Delivered Date: {moment(product.deliveredDate).format(format)}
        </div>
      ) : (
        <div>
          Must Delivered Before:{" "}
          {moment(product.mustDeliveredBefore).format(format)}
        </div>
      )}
      <div className="mt-8">
        <ImageComponent
          imageId={product.mainImageId}
          linkTo={`/products/${product.categoryId}`}
          imageWrapperClass="purchased-product-image-wrapper"
          imageClass="purchased-product-image"
        />
      </div>
    </>
  );
}
