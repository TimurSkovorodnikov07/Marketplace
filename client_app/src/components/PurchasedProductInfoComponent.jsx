import moment from "moment";
import { ImageComponent } from "./ImageComponent";

export function PurchasedProductInfoComponent({ product }) {
  const format = "DD/MM/YYYY";

  return (
    <>
      <div>{product.name}</div>
      <div>{product.description}</div>
      <div>{product.totalSum}</div>
      <div>{product.purchasedQuantity}</div>
      <div>{moment(product.purchasedDate).format(format)}</div>
      <div>{moment(product.mustDeliveredBefore).format(format)}</div>
      {product.deliveredDate != undefined ? (
        <div>{moment(product.deliveredDate).format(format)}</div>
      ) : (
        <></>
      )}

      <ImageComponent
        imageId={product.imagesIdentifiers[0]}
        linkTo={`/products/${product.categoryId}`}
        imageClass="category-image"
      />
    </>
  );
}
